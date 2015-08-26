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
using System.Runtime.InteropServices;

namespace Spoti.Library
{
    public delegate void albumbrowse_complete_cb_delegate(IntPtr result, IntPtr userDataPtr);

    public static partial class libspotify
    {
        [DllImport("libspotify")]
        public static extern IntPtr sp_albumbrowse_create(IntPtr sessionPtr, IntPtr albumPtr, IntPtr callbackPtr,
            IntPtr userDataPtr);

        [DllImport("libspotify")]
        public static extern bool sp_albumbrowse_is_loaded(IntPtr albumBrowsePtr);

        [DllImport("libspotify")]
        public static extern sp_error sp_albumbrowse_error(IntPtr albumBrowsePtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_albumbrowse_album(IntPtr albumBrowsePtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_albumbrowse_artist(IntPtr albumBrowsePtr);

        [DllImport("libspotify")]
        public static extern int sp_albumbrowse_num_copyrights(IntPtr albumBrowsePtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_albumbrowse_copyright(IntPtr albumBrowsePtr, int index);

        [DllImport("libspotify")]
        public static extern int sp_albumbrowse_num_tracks(IntPtr albumBrowsePtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_albumbrowse_track(IntPtr albumBrowsePtr, int index);

        [DllImport("libspotify")]
        public static extern IntPtr sp_albumbrowse_review(IntPtr albumBrowsePtr);

        [DllImport("libspotify")]
        public static extern int sp_albumbrowse_backend_request_duration(IntPtr albumBrowsePtr);

        [DllImport("libspotify")]
        public static extern sp_error sp_albumbrowse_add_ref(IntPtr albumBrowsePtr);

        [DllImport("libspotify")]
        public static extern sp_error sp_albumbrowse_release(IntPtr albumBrowsePtr);
    }
}