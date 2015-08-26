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
    public delegate void image_loaded_cb_delegate(IntPtr imagePtr, IntPtr userDataPtr);

    public static partial class libspotify
    {
        public enum sp_imageformat
        {
            SP_IMAGE_FORMAT_UNKNOWN = -1,
            SP_IMAGE_FORMAT_JPEG = 0
        }

        [DllImport("libspotify")]
        public static extern IntPtr sp_image_create(IntPtr sessionPtr, IntPtr idPtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_image_create_from_link(IntPtr sessionPtr, IntPtr linkPtr);

        [DllImport("libspotify")]
        public static extern sp_error sp_image_add_load_callback(IntPtr imagePtr, IntPtr callbackPtr, IntPtr userDataPtr);

        [DllImport("libspotify")]
        public static extern sp_error sp_image_remove_load_callback(IntPtr imagePtr, IntPtr callbackPtr,
            IntPtr userDataPtr);

        [DllImport("libspotify")]
        public static extern bool sp_image_is_loaded(IntPtr imagePtr);

        [DllImport("libspotify")]
        public static extern sp_error sp_image_error(IntPtr imagePtr);

        [DllImport("libspotify")]
        public static extern sp_imageformat sp_image_format(IntPtr imagePtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_image_data(IntPtr imagePtr, out int data_size);

        [DllImport("libspotify")]
        public static extern IntPtr sp_image_image_id(IntPtr imagePtr);

        [DllImport("libspotify")]
        public static extern sp_error sp_image_add_ref(IntPtr imagePtr);

        [DllImport("libspotify")]
        public static extern sp_error sp_image_release(IntPtr imagePtr);
    }
}