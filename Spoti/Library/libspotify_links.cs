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
        public enum sp_linktype
        {
            SP_LINKTYPE_INVALID = 0,
            SP_LINKTYPE_TRACK = 1,
            SP_LINKTYPE_ALBUM = 2,
            SP_LINKTYPE_ARTIST = 3,
            SP_LINKTYPE_SEARCH = 4,
            SP_LINKTYPE_PLAYLIST = 5,
            SP_LINKTYPE_PROFILE = 6,
            SP_LINKTYPE_STARRED = 7,
            SP_LINKTYPE_LOCALTRACK = 8,
            SP_LINKTYPE_IMAGE = 9
        }

        [DllImport("libspotify")]
        public static extern IntPtr sp_link_create_from_string(IntPtr linkString);

        [DllImport("libspotify")]
        public static extern IntPtr sp_link_create_from_track(IntPtr trackPtr, int offset);

        [DllImport("libspotify")]
        public static extern IntPtr sp_link_create_from_album(IntPtr albumPtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_link_create_from_album_cover(IntPtr albumPtr, sp_image_size size);

        [DllImport("libspotify")]
        public static extern IntPtr sp_link_create_from_artist(IntPtr artistPtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_link_create_from_artist_portrait(IntPtr artistPtr, sp_image_size size);

        [DllImport("libspotify")]
        public static extern IntPtr sp_link_create_from_artistbrowse_portrait(IntPtr artistBrowsePtr, int index);

        [DllImport("libspotify")]
        public static extern IntPtr sp_link_create_from_search(IntPtr searchPtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_link_create_from_playlist(IntPtr playlistPtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_link_create_from_user(IntPtr userPtr);

        [DllImport("libspotify")]
        public static extern int sp_link_as_string(IntPtr linkPtr, IntPtr bufferPtr, int buffer_size);

        [DllImport("libspotify")]
        public static extern sp_linktype sp_link_type(IntPtr linkPtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_link_as_track(IntPtr linkPtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_link_as_track_and_offset(IntPtr linkPtr, out IntPtr offsetPtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_link_as_album(IntPtr linkPtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_link_as_artist(IntPtr linkPtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_link_as_user(IntPtr linkPtr);

        [DllImport("libspotify")]
        public static extern sp_error sp_link_add_ref(IntPtr linkPtr);

        [DllImport("libspotify")]
        public static extern sp_error sp_link_release(IntPtr linkPtr);
    }
}