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
    public delegate void artistbrowse_complete_cb_delegate(IntPtr result, IntPtr userDataPtr);

    public static partial class libspotify
    {
        public enum sp_artistbrowse_type
        {
            [Obsolete("The SP_ARTISTBROWSE_FULL mode has been deprecated and will be removed in a future release.")] SP_ARTISTBROWSE_FULL = 0,
            SP_ARTISTBROWSE_NO_TRACKS = 1,
            SP_ARTISTBROWSE_NO_ALBUMS = 2
        }

        [DllImport("libspotify")]
        public static extern IntPtr sp_artistbrowse_create(IntPtr sessionPtr, IntPtr artistPtr,
            sp_artistbrowse_type type, IntPtr callbackPtr, IntPtr userDataPtr);

        [DllImport("libspotify")]
        public static extern bool sp_artistbrowse_is_loaded(IntPtr artistBrowsePtr);

        [DllImport("libspotify")]
        public static extern sp_error sp_artistbrowse_error(IntPtr artistBrowsePtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_artistbrowse_artist(IntPtr artistBrowsePtr);

        [DllImport("libspotify")]
        public static extern int sp_artistbrowse_num_portraits(IntPtr artistBrowsePtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_artistbrowse_portrait(IntPtr artistBrowsePtr, int index);

        [DllImport("libspotify")]
        public static extern int sp_artistbrowse_num_tracks(IntPtr artistBrowsePtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_artistbrowse_track(IntPtr artistBrowsePtr, int index);

        [DllImport("libspotify")]
        public static extern int sp_artistbrowse_tophit_tracks(IntPtr artistBrowsePtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_artistbrowse_tophit_track(IntPtr artistBrowsePtr, int index);

        [DllImport("libspotify")]
        public static extern int sp_artistbrowse_num_albums(IntPtr artistBrowsePtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_artistbrowse_album(IntPtr artistBrowsePtr, int index);

        [DllImport("libspotify")]
        public static extern int sp_artistbrowse_num_similar_artists(IntPtr artistBrowsePtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_artistbrowse_similar_artist(IntPtr artistBrowsePtr, int index);

        [DllImport("libspotify")]
        public static extern IntPtr sp_artistbrowse_biography(IntPtr artistBrowsePtr);

        [DllImport("libspotify")]
        public static extern int sp_artistbrowse_backend_request_duration(IntPtr artistBrowsePtr);

        [DllImport("libspotify")]
        public static extern sp_error sp_artistbrowse_add_ref(IntPtr artistBrowsePtr);

        [DllImport("libspotify")]
        public static extern sp_error sp_artistbrowse_release(IntPtr artistBrowsePtr);
    }
}