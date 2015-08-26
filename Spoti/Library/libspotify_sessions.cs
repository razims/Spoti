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
        public const int SPOTIFY_API_VERSION = 12;

        public struct sp_audioformat
        {
            public int sample_type;
            public int sample_rate;
            public int channels;
        }

        public struct sp_audio_buffer_stats
        {
            public int samples;
            public int stutter;
        }

        public struct sp_subscribers
        {
            public uint count;
            public IntPtr subscribers;
        }

        public struct sp_offline_sync_status
        {
            public int queued_tracks;
            public int done_tracks;
            public int copied_tracks;
            public int willnotcopy_tracks;
            public int error_tracks;
            public bool syncing;
        }

        public struct sp_session_callbacks
        {
            public IntPtr logged_in;
            public IntPtr logged_out;
            public IntPtr metadata_updated;
            public IntPtr connection_error;
            public IntPtr message_to_user;
            public IntPtr notify_main_thread;
            public IntPtr music_delivery;
            public IntPtr play_token_lost;
            public IntPtr log_message;
            public IntPtr end_of_track;
            public IntPtr streaming_error;
            public IntPtr userinfo_updated;
            public IntPtr start_playback;
            public IntPtr stop_playback;
            public IntPtr get_audio_buffer_stats;
            public IntPtr offline_status_updated;
            public IntPtr offline_error;
            public IntPtr credentials_blob_updated;
            public IntPtr connectionstate_updated;
            public IntPtr scrobble_error;
            public IntPtr private_session_mode_changed;
        }

        public struct sp_session_config
        {
            public int api_version;
            public string cache_location;
            public string settings_location;
            public IntPtr application_key;
            public int application_key_size;
            public string user_agent;
            public IntPtr callbacks;
            public IntPtr userdata;
            public bool compress_playlists;
            public bool dont_save_metadata_for_playlists;
            public bool initially_unload_playlists;
        }

        public enum sp_connectionstate
        {
            LOGGED_OUT = 0,
            LOGGED_IN = 1,
            DISCONNECTED = 2,
            UNDEFINED = 3,
            OFFLINE = 4
        }

        public enum sp_sampletype
        {
            INT16_NATIVE_ENDIAN = 0
        }

        public enum sp_bitrate
        {
            BITRATE_160k = 0,
            BITRATE_320k = 1,
            BITRATE_96k = 2
        }

        public enum sp_playlist_type
        {
            SP_PLAYLIST_TYPE_PLAYLIST = 0,
            SP_PLAYLIST_TYPE_START_FOLDER = 1,
            SP_PLAYLIST_TYPE_END_FOLDER = 2,
            SP_PLAYLIST_TYPE_PLACEHOLDER = 3
        }

        public enum sp_playlist_offline_status
        {
            SP_PLAYLIST_OFFLINE_STATUS_NO = 0,
            SP_PLAYLIST_OFFLINE_STATUS_YES = 1,
            SP_PLAYLIST_OFFLINE_STATUS_DOWNLOADING = 2,
            SP_PLAYLIST_OFFLINE_STATUS_WAITING = 3
        }

        public enum sp_availability
        {
            SP_TRACK_AVAILABILITY_UNAVAILABLE = 0,
            SP_TRACK_AVAILABILITY_AVAILABLE = 1,
            SP_TRACK_AVAILABILITY_NOT_STREAMABLE = 2,
            SP_TRACK_AVAILABILITY_BANNED_BY_ARTIST = 3
        }

        public enum sp_track_offline_status
        {
            SP_TRACK_OFFLINE_NO = 0,
            SP_TRACK_OFFLINE_WAITING = 1,
            SP_TRACK_OFFLINE_DOWNLOADING = 2,
            SP_TRACK_OFFLINE_DONE = 3,
            SP_TRACK_OFFLINE_ERROR = 4,
            SP_TRACK_OFFLINE_DONE_EXPIRED = 5,
            SP_TRACK_OFFLINE_LIMIT_EXCEEDED = 6,
            SP_TRACK_OFFLINE_DONE_RESYNC = 7
        }

        public enum sp_image_size
        {
            SP_IMAGE_SIZE_NORMAL = 0,
            SP_IMAGE_SIZE_SMALL = 1,
            SP_IMAGE_SIZE_LARGE = 2
        }

        public enum sp_connection_type
        {
            SP_CONNECTION_TYPE_UNKNOWN = 0,
            SP_CONNECTION_TYPE_NONE = 1,
            SP_CONNECTION_TYPE_MOBILE = 2,
            SP_CONNECTION_TYPE_MOBILE_ROAMING = 3,
            SP_CONNECTION_TYPE_WIFI = 4,
            SP_CONNECTION_TYPE_WIRED = 5
        }

        public enum sp_connection_rules
        {
            SP_CONNECTION_RULE_NETWORK = 0x1,
            SP_CONNECTION_RULE_NETWORK_IF_ROAMING = 0x2,
            SP_CONNECTION_RULE_ALLOW_SYNC_OVER_MOBILE = 0x4,
            SP_CONNECTION_RULE_ALLOW_SYNC_OVER_WIFI = 0x8
        }

        public enum sp_social_provider
        {
            SP_SOCIAL_PROVIDER_SPOTIFY,
            SP_SOCIAL_PROVIDER_FACEBOOK,
            SP_SOCIAL_PROVIDER_LASTFM,
        };

        public enum sp_scrobbling_state
        {
            SP_SCROBBLING_STATE_USE_GLOBAL_SETTING = 0,
            SP_SCROBBLING_STATE_LOCAL_ENABLED = 1,
            SP_SCROBBLING_STATE_LOCAL_DISABLED = 2,
            SP_SCROBBLING_STATE_GLOBAL_ENABLED = 3,
            SP_SCROBBLING_STATE_GLOBAL_DISABLED = 4,
        };

        [DllImport("libspotify")]
        public static extern sp_error sp_session_create(ref sp_session_config config, out IntPtr sessionPtr);

        [DllImport("libspotify")]
        public static extern sp_error sp_session_release(IntPtr sessionPtr);

        [DllImport("libspotify")]
        public static extern sp_error sp_session_login(IntPtr sessionPtr, string username, string password,
            bool rememberMe, string blob);

        [DllImport("libspotify")]
        public static extern sp_error sp_session_relogin(IntPtr sessionPtr);

        [DllImport("libspotify")]
        public static extern int sp_session_remembered_user(IntPtr sessionPtr, string buffer, int buffer_size);

        [DllImport("libspotify")]
        public static extern IntPtr sp_session_user_name(IntPtr sessionPtr);

        [DllImport("libspotify")]
        public static extern sp_error sp_session_forget_me(IntPtr sessionPtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_session_user(IntPtr sessionPtr);

        [DllImport("libspotify")]
        public static extern sp_error sp_session_logout(IntPtr sessionPtr);

        [DllImport("libspotify")]
        public static extern sp_error sp_session_flush_caches(IntPtr sessionPtr);

        [DllImport("libspotify")]
        public static extern sp_connectionstate sp_session_connectionstate(IntPtr sessionPtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_session_userdata(IntPtr sessionPtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_session_set_cache_size(IntPtr sessionPtr, int size);

        [DllImport("libspotify")]
        public static extern sp_error sp_session_process_events(IntPtr sessionPtr, out int next_timeout);

        [DllImport("libspotify")]
        public static extern sp_error sp_session_player_load(IntPtr sessionPtr, IntPtr trackPtr);

        [DllImport("libspotify")]
        public static extern sp_error sp_session_player_seek(IntPtr sessionPtr, int offset);

        [DllImport("libspotify")]
        public static extern sp_error sp_session_player_play(IntPtr sessionPtr, bool play);

        [DllImport("libspotify")]
        public static extern sp_error sp_session_player_unload(IntPtr sessionPtr);

        [DllImport("libspotify")]
        public static extern sp_error sp_session_player_prefetch(IntPtr sessionPtr, IntPtr trackPtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_session_playlistcontainer(IntPtr sessionPtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_session_inbox_create(IntPtr sessionPtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_session_starred_create(IntPtr sessionPtr);

        [DllImport("libspotify")]
        public static extern IntPtr sp_session_starred_for_user_create(IntPtr sessionPtr, string canonical_username);

        [DllImport("libspotify")]
        public static extern IntPtr sp_session_publishedcontainer_for_user_create(IntPtr sessionPtr,
            IntPtr canonical_username);

        [DllImport("libspotify")]
        public static extern sp_error sp_session_preferred_bitrate(IntPtr sessionPtr, sp_bitrate bitrate);

        [DllImport("libspotify")]
        public static extern sp_error sp_session_preferred_offline_bitrate(IntPtr sessionPtr, sp_bitrate bitrate,
            bool allow_resync);

        [DllImport("libspotify")]
        public static extern bool sp_session_get_volume_normalization(IntPtr sessionPtr);

        [DllImport("libspotify")]
        public static extern bool sp_session_set_volume_normalization(IntPtr sessionPtr, bool on);

        [DllImport("libspotify")]
        public static extern sp_error sp_session_set_private_session(IntPtr sessionPtr, bool enabled);

        [DllImport("libspotify")]
        public static extern bool sp_session_is_private_session(IntPtr sessionPtr);

        [DllImport("libspotify")]
        public static extern sp_error sp_session_set_scrobbling(IntPtr sessionPtr, sp_social_provider provider,
            sp_scrobbling_state state);

        [DllImport("libspotify")]
        public static extern sp_error sp_session_is_scrobbling(IntPtr sessionPtr, sp_social_provider provider,
            sp_scrobbling_state state);

        [DllImport("libspotify")]
        public static extern sp_error sp_session_is_scrobbling_possible(IntPtr sessionPtr, sp_social_provider provider,
            out bool isPossible);

        [DllImport("libspotify")]
        public static extern sp_error sp_session_set_social_credentials(IntPtr sessionPtr, sp_social_provider provider,
            string username, string password);

        [DllImport("libspotify")]
        public static extern sp_error sp_session_set_connection_type(IntPtr sessionPtr, sp_connection_type type);

        [DllImport("libspotify")]
        public static extern sp_error sp_session_set_connection_rules(IntPtr sessionPtr, sp_connection_rules rules);

        [DllImport("libspotify")]
        public static extern int sp_offline_tracks_to_sync(IntPtr sessionPtr);

        [DllImport("libspotify")]
        public static extern int sp_offline_num_playlists(IntPtr sessionPtr);

        [DllImport("libspotify")]
        public static extern bool sp_offline_sync_get_status(IntPtr sessionPtr, IntPtr status);

        [DllImport("libspotify")]
        public static extern int sp_offline_time_left(IntPtr sessionPtr);

        [DllImport("libspotify")]
        public static extern int sp_session_user_country(IntPtr sessionPtr);
    }
}