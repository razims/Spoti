#region License

// Distributed under GNU General Public License v2.0
// 
// Copyright (C) 2015  Razim Saidov
// 
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License along
// with this program; if not, write to the Free Software Foundation, Inc.,
// 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
// 

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Spoti.Entity;
using Spoti.Helper;
using Spoti.Interface;
using libspotify = Spoti.Library.libspotify;

namespace Spoti
{
    public class DownloadSession : SessionBase
    {
        public string ModuleId = "Session";


        protected bool _completed = false;
        protected string _filename;
        protected ulong _bytesTotal = 0;
        protected Track _track;
        protected bool _forceDownload = false;
        protected CancellationTokenSource _cts;

        public DownloadSession(byte[] appKey, string appId, ILog log = null, string cacheLocation = null, bool forceDownload = false)
            : base(appKey, appId, log, cacheLocation)
        {
            _forceDownload = forceDownload;
            this.Reset();
        }


        protected void Reset()
        {
            _completed = false;
            _bytesTotal = 0;
        }

        public Task TrackToFileAsync(Track track, string filename, CancellationTokenSource cts)
        {
            _filename = filename;
            _track = track;
            _cts = cts;
            // _track = new Track(trackLink, this);
            // track.Album.BeginBrowse();
            // while (!track.Album.IsBrowseComplete)

            if (!track.IsLoaded)
                Thread.Sleep(1000);

            var status = LoadPlayer(track);
            if (status == libspotify.sp_error.OK)
            {
                this.Reset();
                Play();
                return Task.Run(() =>
                {
                    while (!_completed)
                    {
                        Thread.Sleep(10);
                    }
                }, cts.Token);
                // libspotify.sp_link_release(trackPtr);
                // libspotify.sp_link_release(linkPtr);
            }
            else
            {
                return null;
            }
        }

        #region events

        public event Action<int> OnReportProgress;

        protected override void InitEvents()
        {
            this.OnMusicDelivery += OnOnMusicDelivery;
            this.OnEndOfTrack += OnOnEndOfTrack;
            this.OnPlayTokenLost += OnOnPlayTokenLost;
            this.OnMessageToUser += OnOnMessageToUser;
        }

        private void OnOnMessageToUser(IntPtr sessionPtr, string message)
        {
            Log.Debug(ModuleId, message);
        }

        private void OnOnPlayTokenLost(IntPtr sessionPtr)
        {
            if (_forceDownload)
            {
                _completed = false;
                Play();
            }
            else
            {
                Pause();
                _completed = true;
                UnloadPlayer();
            }
        }

        private void OnOnEndOfTrack(IntPtr sessionPtr)
        {
            Pause();
            _completed = true;
            UnloadPlayer();
        }

        private int OnOnMusicDelivery(IntPtr sessionPtr, IntPtr formatPtr, IntPtr framesPtr, int numFrames)
        {
            if (_completed)
                return 0;

            if (numFrames == 0)
                return 0;

            var format =
                (libspotify.sp_audioformat) Marshal.PtrToStructure(formatPtr, typeof (libspotify.sp_audioformat));
            var buffer = new byte[numFrames*sizeof (Int16)*format.channels];
            Marshal.Copy(framesPtr, buffer, 0, buffer.Length);

            using (var stream = new FileStream(_filename, FileMode.Append))
            {
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
            }

            /*
            var chunkFormat = new Format()
                {
                    KBitsPerSecond = Bitrate, // ie 320
                    BitsPerSample = 16,
                    SamplesPerSecond = format.sample_rate, // ie 44100
                    Channels = format.channels // ie 2
                };

            // Log.Debug(ModuleId, String.Format("{0} Bytes and {1} Frames received", buffer.Length, numFrames));
            */
            _bytesTotal += (ulong) buffer.LongLength;
            if (OnReportProgress != null)
            {
                var calculatedTotalSize = (double) (format.sample_rate*format.channels*16*_track.Seconds)*0.125;

                var progress = (int) Math.Floor((_bytesTotal/calculatedTotalSize)*100);

                OnReportProgress(progress);
            }


            if (_cts.IsCancellationRequested || (this.IsInterrupted && !_forceDownload))
            {
                this.OnOnEndOfTrack(sessionPtr);
            }


            return numFrames;
        }

        #endregion
    }
}