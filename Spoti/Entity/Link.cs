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
using Spoti.Helper;
using libspotify = Spoti.Library.libspotify;

namespace Spoti.Entity
{
    public class Link : IModule, IDisposable
    {
        public string ModuleId = "Link";

        public SessionBase _session { get; protected set; }

        public libspotify.sp_linktype Type { get; protected set; }
        public bool IsLoaded { get; protected set; } = false;

        public Link(string link, SessionBase session)
        {
            _session = session;
            var _link = Functions.ConvertUrlToUri(link);
            IntPtr linkPtr = Functions.StringToLinkPtr(_link);
            try
            {
                if (linkPtr != IntPtr.Zero)
                {
                    this.Handle = linkPtr; // libspotify.sp_link_create_from_string(linkPtr);
                    this.Type = libspotify.sp_link_type(linkPtr);

                    this.IsLoaded = true;
                }
            }
            catch (Exception ex)
            {
                _session.Log.Error(this.ModuleId, String.Format("Could not create Link Object. {0}", ex.Message));
            }
            finally
            {
                /*if (linkPtr != IntPtr.Zero)
                    libspotify.sp_link_release(linkPtr);*/
            }
        }

        #region IDisposable Members

        private bool _disposed;

        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Link()
        {
            dispose(false);
        }

        private void dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    try
                    {
                        if (Handle != IntPtr.Zero)
                            libspotify.sp_link_release(Handle);
                    }
                    catch (Exception ex)
                    {
                        _session.Log.Error(this.ModuleId, String.Format("Could not release Link Object. {0}", ex.Message));
                    }

                }

                _disposed = true;
            }
        }

        #endregion IDisposable Members
    }
}