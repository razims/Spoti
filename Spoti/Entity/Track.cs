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
using System.Threading;
using Spoti.Helper;
using libspotify = Spoti.Library.libspotify;

namespace Spoti.Entity
{
    public class Track : IModule, IDisposable
    {
        public string ModuleId = "Track";


        public string Name { get; private set; }
        public int TrackNumber { get; private set; }
        public int DiskNumber { get; private set; }
        public decimal Seconds { get; private set; }
        public int Popularity { get; private set; }
        public Album Album { get; private set; }
        public List<Artist> Artists { get; private set; }

        public bool IsAvailable
        {
            get
            {
                return ((libspotify.sp_track_get_availability(_session.Handle, Handle)) ==
                        libspotify.sp_availability.SP_TRACK_AVAILABILITY_AVAILABLE);
            }
        }

        public bool IsLoaded
        {
            get { return libspotify.sp_track_is_loaded(Handle); }
        }

        public bool IsLocal
        {
            get { return libspotify.sp_track_is_local(_session.Handle, Handle); }
        }

        public bool IsPlaceholder
        {
            get { return libspotify.sp_track_is_placeholder(Handle); }
        }

        public bool IsStarred
        {
            get { return libspotify.sp_track_is_starred(_session.Handle, Handle); }
        }

        public libspotify.sp_availability Availability
        {
            get { return libspotify.sp_track_get_availability(_session.Handle, Handle); }
        }

        public libspotify.sp_track_offline_status OfflineStatus
        {
            get { return libspotify.sp_track_offline_get_status(Handle); }
        }

        protected SessionBase _session;

        /*public Track(string link, SessionBase session)
        {
            IntPtr linkPtr = Functions.StringToLinkPtr(link);
            try
            {
                _session = session;
                this.Handle = libspotify.sp_link_as_track(linkPtr);
                Init();
            }
            finally
            {
                if (linkPtr != IntPtr.Zero)
                    libspotify.sp_link_release(linkPtr);
            }
        }*/

        public Track(Link link, SessionBase session)
        {
            _session = session;
            this.Handle = libspotify.sp_link_as_track(link.Handle);
            Init();
        }

        public Track(IntPtr trackPtr, SessionBase session)
        {
            _session = session;
            this.Handle = trackPtr;
            Init();
        }

        private void Init()
        {
            /*if (!this.IsLoaded)
                throw new InvalidOperationException("Track is not loaded.");*/
            while (!this.IsLoaded)
            {
                Thread.Sleep(1);
            }

            this.Name = Functions.PtrToString(libspotify.sp_track_name(Handle));
            this.TrackNumber = libspotify.sp_track_index(Handle);
            this.Seconds = (decimal) libspotify.sp_track_duration(Handle)/1000M;
            this.DiskNumber = libspotify.sp_track_disc(Handle);
            this.Popularity = libspotify.sp_track_popularity(Handle);


            var albumPtr = libspotify.sp_track_album(Handle);
            if (albumPtr != IntPtr.Zero)
                this.Album = new Album(albumPtr, _session);

            this.Artists = new List<Artist>();
            for (int i = 0; i < libspotify.sp_track_num_artists(Handle); i++)
            {
                IntPtr artistPtr = libspotify.sp_track_artist(Handle, i);
                Artists.Add(new Artist(artistPtr, _session));
            }

            /*for (int i = 0; i < libspotify.sp_track_num_artists(trackPtr); i++)
            {
                IntPtr artistPtr = libspotify.sp_track_artist(trackPtr, i);
                if (artistPtr != IntPtr.Zero)
                    _artists.Add(Functions.PtrToString(libspotify.sp_artist_name(artistPtr)));
            }*/
        }

        #region IDisposable Members

        private bool _disposed;

        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Track()
        {
            dispose(false);
        }

        private void dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    libspotify.sp_track_release(Handle);
                }

                _disposed = true;
            }
        }

        #endregion IDisposable Members
    }
}