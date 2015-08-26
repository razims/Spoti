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
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Spoti.Helper;
using albumbrowse_complete_cb_delegate = Spoti.Library.albumbrowse_complete_cb_delegate;
using libspotify = Spoti.Library.libspotify;

namespace Spoti.Entity
{
    public class Album : IModule, IDisposable
    {
        private string ModuleId = "Album";


        public libspotify.sp_albumtype Type { get; protected set; }
        public string Name { get; protected set; }
        public string Artist { get; private set; }
        public int Year { get; protected set; }
        public string Cover { get; protected set; }


        public bool IsAvailable
        {
            get { return libspotify.sp_album_is_available(Handle); }
        }

        public bool IsLoaded
        {
            get { return libspotify.sp_album_is_loaded(Handle); }
        }

        public bool IsBrowseComplete { get; protected set; }


        public List<Track> Tracks { get; protected set; }


        protected SessionBase _session;
        protected IntPtr _browsePtr;
        protected albumbrowse_complete_cb_delegate _autobrowseCompleteDelegate;


        public void Init(bool browse = false)
        {
            if (Handle == IntPtr.Zero)
                throw new InvalidOperationException("Album handle is null.");

            this.Type = libspotify.sp_album_type(Handle);
            this.Name = Functions.PtrToString(libspotify.sp_album_name(Handle));
            this.Year = libspotify.sp_album_year(Handle);
            this.Tracks = new List<Track>();

            #region Cover

            IntPtr linkPtr = libspotify.sp_link_create_from_album_cover(Handle,
                libspotify.sp_image_size.SP_IMAGE_SIZE_LARGE);
            try
            {
                if (linkPtr != IntPtr.Zero)
                    Cover = Functions.LinkPtrToString(linkPtr);
            }
            catch (Exception ex)
            {
                _session.Log.Warning(ModuleId, String.Format("No link could be created for album cover. {0}", ex.Message));
            }
            finally
            {
                if (linkPtr != IntPtr.Zero)
                    libspotify.sp_link_release(linkPtr);
            }

            #endregion

            if (browse)
            {
                EndBrowse();
                BeginBrowse();
            }
        }


        public Album(IntPtr albumPtr, SessionBase session, bool browse = false)
        {
            this._session = session;
            this.Handle = albumPtr;
            Init(browse);
        }

        public Album(Link link, SessionBase session, bool browse = false)
        {
            this._session = session;
            this.Handle = libspotify.sp_link_as_album(link.Handle);
            Init(browse);
        }

        /*public Album(string link, SessionBase session, bool browse = false)
        {
            
            var linkPtr = Functions.StringToLinkPtr(link);
            try
            {
                _session = session;
                Handle = libspotify.sp_link_as_album(  linkPtr );
                Init(browse);
            }
            finally
            {
                if (linkPtr != IntPtr.Zero)
                    libspotify.sp_link_release(linkPtr);
            }
            
        }*/

        #region IDisposable Members

        private bool _disposed;

        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Album()
        {
            dispose(false);
        }

        private void dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    EndBrowse();
                    libspotify.sp_album_release(Handle);
                }

                _disposed = true;
            }
        }

        #endregion IDisposable Members

        #region Browse

        private void _AlbumbrowseComplete(IntPtr result, IntPtr userDataPtr)
        {
            try
            {
                libspotify.sp_error error = libspotify.sp_albumbrowse_error(result);

                if (error != libspotify.sp_error.OK)
                {
                    _session.Log.Error(ModuleId, String.Format("Album browse failed: {0}", libspotify.sp_error_message(error)));
                    return;
                }

                if (Tracks == null)
                    Tracks = new List<Track>();

                else
                {
                    if (Tracks != null)
                        foreach (var track in Tracks)
                            track.Dispose();

                    Tracks.Clear();
                }

                int numtracks = libspotify.sp_albumbrowse_num_tracks(_browsePtr);
                for (int i = 0; i < libspotify.sp_albumbrowse_num_tracks(_browsePtr); i++)
                {
                    var trackPtr = libspotify.sp_albumbrowse_track(_browsePtr, i);
                    var track = new Track(trackPtr, _session);
                    Tracks.Add(track);
                }
                this.IsBrowseComplete = true;
            }
            finally
            {
                EndBrowse();
            }
        }

        public bool BeginBrowse()
        {
            try
            {
                IsBrowseComplete = false;
                _autobrowseCompleteDelegate = _AlbumbrowseComplete;
                IntPtr callbackPtr = Marshal.GetFunctionPointerForDelegate(_autobrowseCompleteDelegate);
                _browsePtr = libspotify.sp_albumbrowse_create(_session.Handle, Handle, callbackPtr, IntPtr.Zero);

                return true;
            }
            catch (Exception ex)
            {
                _session.Log.Warning(ModuleId, String.Format("Album.BeginBrowse() failed: {0}", ex.Message));
                return false;
            }
        }

        public void EndBrowse()
        {
            if (this._browsePtr != IntPtr.Zero)
            {
                try
                {
                    // necessary metadata is destroyed if the browse is released here...
                    // libspotify.sp_albumbrowse_release(_browsePtr);
                }
                catch
                {
                }
            }
        }

        #endregion
    }
}