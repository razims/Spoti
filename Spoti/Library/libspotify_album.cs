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
        public enum sp_albumtype
        {
            SP_ALBUMTYPE_ALBUM = 0,
            SP_ALBUMTYPE_SINGLE = 1,
            SP_ALBUMTYPE_COMPILATION = 2,
            SP_ALBUMTYPE_UNKNOWN = 3
        }

        [DllImport("libspotify")]
        public static extern bool sp_album_is_loaded(IntPtr albumPtr);

        [DllImport("libspotify")]
        public static extern bool sp_album_is_available(IntPtr albumPtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_album_artist(IntPtr albumPtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_album_cover(IntPtr albumPtr, sp_image_size size);

        [DllImport("libspotify")]
        public static extern IntPtr sp_album_name(IntPtr albumPtr);

        [DllImport("libspotify")]
        public static extern int sp_album_year(IntPtr albumPtr);

        [DllImport("libspotify")]
        public static extern sp_albumtype sp_album_type(IntPtr albumPtr);

        [DllImport("libspotify")]
        public static extern sp_error sp_album_add_ref(IntPtr albumPtr);

        [DllImport("libspotify")]
        public static extern sp_error sp_album_release(IntPtr albumPtr);
    }
}