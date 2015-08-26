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
    public delegate void search_complete_cb_delegate(IntPtr searchPtr, IntPtr userDataPtr);

    public enum sp_search_type
    {
        SP_SEARCH_STANDARD = 0,
        SP_SEARCH_SUGGEST = 1,
    }

    public static partial class libspotify
    {
        [DllImport("libspotify")]
        public static extern IntPtr sp_search_create(IntPtr sessionPtr, IntPtr query, int track_offset, int track_count,
            int album_offset, int album_count, int artist_offset, int artist_count,
            int playlist_offset, int playlist_count, sp_search_type search_type,
            IntPtr callbackPtr, IntPtr userDataPtr);

        [DllImport("libspotify")]
        public static extern bool sp_search_is_loaded(IntPtr searchPtr);

        [DllImport("libspotify")]
        public static extern sp_error sp_search_error(IntPtr searchPtr);

        [DllImport("libspotify")]
        public static extern int sp_search_num_tracks(IntPtr searchPtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_search_track(IntPtr searchPtr, int index);

        [DllImport("libspotify")]
        public static extern int sp_search_num_albums(IntPtr searchPtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_search_album(IntPtr searchPtr, int index);

        [DllImport("libspotify")]
        public static extern int sp_search_num_artists(IntPtr searchPtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_search_artist(IntPtr searchPtr, int index);

        [DllImport("libspotify")]
        public static extern IntPtr sp_search_query(IntPtr searchPtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_search_did_you_mean(IntPtr searchPtr);

        [DllImport("libspotify")]
        public static extern int sp_search_total_tracks(IntPtr searchPtr);

        [DllImport("libspotify")]
        public static extern int sp_search_total_albums(IntPtr searchPtr);

        [DllImport("libspotify")]
        public static extern int sp_search_total_artists(IntPtr searchPtr);

        [DllImport("libspotify")]
        public static extern sp_error sp_search_add_ref(IntPtr searchPtr);

        [DllImport("libspotify")]
        public static extern sp_error sp_search_release(IntPtr searchPtr);

        [DllImport("libspotify")]
        public static extern int sp_search_num_playlists(IntPtr searchPtr);

        [DllImport("libspotify")]
        public static extern int sp_search_total_playlists(IntPtr searchPtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_search_playlist(IntPtr searchPtr, int index);

        [DllImport("libspotify")]
        public static extern IntPtr sp_search_playlist_name(IntPtr searchPtr, int index);

        [DllImport("libspotify")]
        public static extern IntPtr sp_search_playlist_uri(IntPtr searchPtr, int index);

        [DllImport("libspotify")]
        public static extern IntPtr sp_search_playlist_image_uri(IntPtr searchPtr, int index);
    }
}