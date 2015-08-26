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
        public enum sp_error
        {
            OK = 0,
            BAD_API_VERSION = 1,
            API_INITIALIZATION_FAILED = 2,
            TRACK_NOT_PLAYABLE = 3,
            APPLICATION_KEY = 5,
            BAD_USERNAME_OR_PASSWORD = 6,
            USER_BANNED = 7,
            UNABLE_TO_CONTACT_SERVER = 8,
            CLIENT_TOO_OLD = 9,
            OTHER_PERMANENT = 10,
            BAD_USER_AGENT = 11,
            MISSING_CALLBACK = 12,
            INVALID_INDATA = 13,
            INDEX_OUT_OF_RANGE = 14,
            USER_NEEDS_PREMIUM = 15,
            OTHER_TRANSIENT = 16,
            IS_LOADING = 17,
            NO_STREAM_AVAILABLE = 18,
            PERMISSION_DENIED = 19,
            INBOX_IS_FULL = 20,
            NO_CACHE = 21,
            NO_SUCH_USER = 22,
            NO_CREDENTIALS = 23,
            NETWORK_DISABLED = 24,
            INVALID_DEVICE_ID = 25,
            CANT_OPEN_TRACE_FILE = 26,
            APPLICATION_BANNED = 27,
            OFFLINE_TOO_MANY_TRACKS = 31,
            OFFLINE_DISK_CACHE = 32,
            OFFLINE_EXPIRED = 33,
            OFFLINE_NOT_ALLOWED = 34,
            OFFLINE_LICENSE_LOST = 35,
            OFFLINE_LICENSE_ERROR = 36
        }

        [DllImport("libspotify")]
        public static extern IntPtr sp_error_message(sp_error error);
    }
}