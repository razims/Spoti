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
    public delegate void inboxpost_complete_cb(IntPtr result, IntPtr userDataPtr);

    public static partial class libspotify
    {
        [DllImport("libspotify")]
        public static extern IntPtr sp_inbox_post_tracks(IntPtr sessionPtr, string user, IntPtr trackPtr, int num_tracks,
            string message, IntPtr callback, IntPtr userdata);

        [DllImport("libspotify")]
        public static extern sp_error sp_inbox_error(IntPtr inboxPtr);

        [DllImport("libspotify")]
        public static extern sp_error sp_inbox_add_ref(IntPtr inboxPtr);

        [DllImport("libspotify")]
        public static extern sp_error sp_inbox_release(IntPtr inboxPtr);
    }
}