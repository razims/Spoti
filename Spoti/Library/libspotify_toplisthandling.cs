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
    public static partial class libspotify
    {
        public enum sp_toplisttype
        {
            SP_TOPLIST_TYPE_ARTISTS = 0,
            SP_TOPLIST_TYPE_ALBUMS = 1,
            SP_TOPLIST_TYPE_TRACKS = 2
        }

        public enum sp_toplistregion
        {
            SP_TOPLIST_REGION_EVERYWHERE = 0,
            SP_TOPLIST_REGION_USER = 1
        }

        [DllImport("libspotify")]
        public static extern IntPtr sp_toplistbrowse_create(IntPtr sessionPtr, sp_toplisttype type, int region,
            IntPtr usernamePtr, IntPtr browseCompleteCb, IntPtr userDataPtr);

        [DllImport("libspotify")]
        public static extern bool sp_toplistbrowse_is_loaded(IntPtr tlb);

        [DllImport("libspotify")]
        public static extern sp_error sp_toplistbrowse_error(IntPtr tlb);

        [DllImport("libspotify")]
        public static extern sp_error sp_toplistbrowse_add_ref(IntPtr tlb);

        [DllImport("libspotify")]
        public static extern sp_error sp_toplistbrowse_release(IntPtr tlb);

        [DllImport("libspotify")]
        public static extern int sp_toplistbrowse_num_artists(IntPtr tlbb);

        [DllImport("libspotify")]
        public static extern IntPtr sp_toplistbrowse_artist(IntPtr tlb, int index);

        [DllImport("libspotify")]
        public static extern int sp_toplistbrowse_num_albums(IntPtr tlb);

        [DllImport("libspotify")]
        public static extern IntPtr sp_toplistbrowse_album(IntPtr tlb, int index);

        [DllImport("libspotify")]
        public static extern int sp_toplistbrowse_num_tracks(IntPtr tlb);

        [DllImport("libspotify")]
        public static extern IntPtr sp_toplistbrowse_track(IntPtr tlb, int index);

        [DllImport("libspotify")]
        public static extern IntPtr sp_toplistbrowse_backend_request_duration(IntPtr tlb);
    }
}