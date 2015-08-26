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
        [DllImport("libspotify")]
        public static extern bool sp_track_is_loaded(IntPtr trackPtr);

        [DllImport("libspotify")]
        public static extern sp_error sp_track_error(IntPtr trackPtr);

        [DllImport("libspotify")]
        public static extern sp_availability sp_track_get_availability(IntPtr sessionPtr, IntPtr trackPtr);

        [DllImport("libspotify")]
        public static extern bool sp_track_is_local(IntPtr sessionPtr, IntPtr trackPtr);

        [DllImport("libspotify")]
        public static extern bool sp_track_is_autolinked(IntPtr sessionPtr, IntPtr trackPtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_track_get_playable(IntPtr sessionPtr, IntPtr trackPtr);

        [DllImport("libspotify")]
        public static extern bool sp_track_is_starred(IntPtr sessionPtr, IntPtr trackPtr);

        [DllImport("libspotify")]
        public static extern bool sp_track_is_placeholder(IntPtr trackPtr);

        [DllImport("libspotify")]
        public static extern sp_error sp_track_set_starred(IntPtr sessionPtr, IntPtr tracksArrayPtr, int num_tracks,
            bool star);

        [DllImport("libspotify")]
        public static extern int sp_track_num_artists(IntPtr trackPtr);

        [DllImport("libspotify")]
        public static extern sp_track_offline_status sp_track_offline_get_status(IntPtr trackPtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_track_artist(IntPtr trackPtr, int index);

        [DllImport("libspotify")]
        public static extern IntPtr sp_track_album(IntPtr trackPtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_track_name(IntPtr trackPtr);

        [DllImport("libspotify")]
        public static extern int sp_track_duration(IntPtr trackPtr);

        [DllImport("libspotify")]
        public static extern int sp_track_popularity(IntPtr trackPtr);

        [DllImport("libspotify")]
        public static extern int sp_track_disc(IntPtr trackPtr);

        [DllImport("libspotify")]
        public static extern int sp_track_index(IntPtr trackPtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_localtrack_create(string artist, string title, string album, int length);

        [DllImport("libspotify")]
        public static extern sp_error sp_track_add_ref(IntPtr trackPtr);

        [DllImport("libspotify")]
        public static extern sp_error sp_track_release(IntPtr trackPtr);
    }
}