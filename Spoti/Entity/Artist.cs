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
using System.Threading.Tasks;
using Spoti.Helper;
using artistbrowse_complete_cb_delegate = Spoti.Library.artistbrowse_complete_cb_delegate;
using libspotify = Spoti.Library.libspotify;

namespace Spoti.Entity
{
    public class Artist : IModule, IDisposable
    {
        private string ModuleId = "Artist";

        public libspotify.sp_albumtype Type { get; protected set; }
        public string Name { get; private set; }
        public string Portrait { get; private set; }
        public string Biography { get; private set; }


        public bool IsLoaded
        {
            get { return libspotify.sp_artist_is_loaded(Handle); }
        }

        public bool IsBrowseComplete { get; protected set; }


        public List<Album> Albums { get; protected set; }

        protected SessionBase _session;
        protected IntPtr _browsePtr;
        protected artistbrowse_complete_cb_delegate _autobrowseCompleteDelegate;

        public Artist(IntPtr artistPtr, SessionBase session, bool browse = false)
        {
            this.Handle = artistPtr;
            this._session = session;

            Init(browse);
        }

        public Artist(Link link, SessionBase session, bool browse = false)
        {
            this.Handle = libspotify.sp_link_as_artist(link.Handle);
            this._session = session;

            Init(browse);
        }

        private void Init(bool browse = false)
        {
            if (Handle == IntPtr.Zero)
                throw new InvalidOperationException("Artist pointer is null.");

            this.Name = Functions.PtrToString(libspotify.sp_artist_name(Handle));
            this.Albums = new List<Album>();

            #region Portrait

            IntPtr linkPtr = libspotify.sp_link_create_from_artist_portrait(Handle,
                libspotify.sp_image_size.SP_IMAGE_SIZE_LARGE);
            try
            {
                if (linkPtr != IntPtr.Zero)
                    Portrait = Functions.LinkPtrToString(linkPtr);
            }
            catch (Exception ex)
            {
                _session.Log.Warning(ModuleId, String.Format("No link could be created for artist portrait. {0}", ex.Message));
            }
            finally
            {
                if (linkPtr != IntPtr.Zero)
                    libspotify.sp_link_release(linkPtr);
            }

            #endregion
        }

        #region IDisposable Members

        private bool _disposed;

        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Artist()
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
                    libspotify.sp_artist_release(Handle);
                }

                _disposed = true;
            }
        }

        #endregion IDisposable Members

        #region Browse

        private void _ArtistbrowseComplete(IntPtr result, IntPtr userDataPtr)
        {
            try
            {
                libspotify.sp_error error = libspotify.sp_albumbrowse_error(result);

                if (error != libspotify.sp_error.OK)
                {
                    _session.Log.Error(ModuleId, String.Format("Artist browse failed: {0}", libspotify.sp_error_message(error)));
                    return;
                }

                int numalbums = libspotify.sp_artistbrowse_num_albums(_browsePtr);

                if (this.Albums == null)
                    this.Albums = new List<Album>();

                for (int i = 0; i < libspotify.sp_artistbrowse_num_albums(_browsePtr); i++)
                {
                    IntPtr albumPtr = libspotify.sp_artistbrowse_album(_browsePtr, i);

                    // excluding singles, compilations, and unknowns
                    if (libspotify.sp_album_type(albumPtr) == libspotify.sp_albumtype.SP_ALBUMTYPE_ALBUM
                        && libspotify.sp_album_is_available(albumPtr))
                        this.Albums.Add(new Album(albumPtr, _session));
                }

                this.Biography = Functions.PtrToString(libspotify.sp_artistbrowse_biography(_browsePtr));

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
                _autobrowseCompleteDelegate = _ArtistbrowseComplete;
                IntPtr callbackPtr = Marshal.GetFunctionPointerForDelegate(_autobrowseCompleteDelegate);
                _browsePtr = libspotify.sp_artistbrowse_create(_session.Handle, Handle,
                    libspotify.sp_artistbrowse_type.SP_ARTISTBROWSE_NO_ALBUMS, callbackPtr, IntPtr.Zero);

                return true;
            }
            catch (Exception ex)
            {
                _session.Log.Warning(ModuleId, String.Format("Artist.BeginBrowse() failed: {0}", ex.Message));
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