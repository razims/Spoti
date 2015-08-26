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
        public enum sp_relation_type
        {
            SP_RELATION_TYPE_UNKNOWN = 0,
            SP_RELATION_TYPE_NONE = 1,
            SP_RELATION_TYPE_UNIDIRECTIONAL = 2,
            SP_RELATION_TYPE_BIDIRECTIONAL = 3
        }

        [DllImport("libspotify")]
        public static extern IntPtr sp_user_canonical_name(IntPtr userPtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_user_display_name(IntPtr userPtr);

        [DllImport("libspotify")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool sp_user_is_loaded(IntPtr userPtr);

        [DllImport("libspotify")]
        public static extern sp_error sp_user_add_ref(IntPtr userPtr);

        [DllImport("libspotify")]
        public static extern sp_error sp_user_release(IntPtr userPtr);
    }
}