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
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Spoti.Entity;
using Spoti.Helper;
using Spoti.Interface;
using libspotify = Spoti.Library.libspotify;

namespace Spoti
{
    public abstract class SessionBase : IModule, IDisposable
    {
        public string ModuleId = "SessionBase";

        #region Delegate Declarations

        public delegate void ConnectionErrorDelegate(IntPtr sessionPtr, libspotify.sp_error error);

        public delegate void EndOfTrackDelegate(IntPtr sessionPtr);

        public delegate void GetAudioBufferStatsDelegate(IntPtr sessionPtr, IntPtr statsPtr);

        public delegate void LogMessageDelegate(IntPtr sessionPtr, string message);

        public delegate void LoggedInDelegate(IntPtr sessionPtr, libspotify.sp_error error);

        public delegate void LoggedOutDelegate(IntPtr sessionPtr);

        public delegate void MessageToUserDelegate(IntPtr sessionPtr, string message);

        public delegate void MetadataUpdatedDelegate(IntPtr sessionPtr);

        public delegate int MusicDeliveryDelegate(IntPtr sessionPtr, IntPtr formatPtr, IntPtr framesPtr, int num_frames);

        public delegate void NotifyMainThreadDelegate(IntPtr sessionPtr);

        public delegate void OfflineStatusUpdatedDelegate(IntPtr sessionPtr);

        public delegate void PlayTokenLostDelegate(IntPtr sessionPtr);

        public delegate void StartPlaybackDelegate(IntPtr sessionPtr);

        public delegate void StopPlaybackDelegate(IntPtr sessionPtr);

        public delegate void StreamingErrorDelegate(IntPtr sessionPtr, libspotify.sp_error error);

        public delegate void UserinfoUpdatedDelegate(IntPtr sessionPtr);

        public delegate void ConnectionStateUpdatedDelegate(IntPtr sessionPtr);

        public delegate void CredentialsBlobUpdatedDelegate(IntPtr sessionPtr, string blob);

        public delegate void OfflineErrorDelegate(IntPtr sessionPtr, libspotify.sp_error error);

        public delegate void PrivateSessionModeChangedDelegate(IntPtr sessionPtr, bool IsPrivate);

        public delegate void ScobbleErrorDelegate(IntPtr sessionPtr, libspotify.sp_error error);

        #endregion

        #region Events Declarations

        public event ConnectionErrorDelegate OnConnectionError;
        public event EndOfTrackDelegate OnEndOfTrack;
        public event GetAudioBufferStatsDelegate OnGetAudioBufferStats;
        public event LogMessageDelegate OnLogMessage;
        public event LoggedInDelegate OnLoggedIn;
        public event LoggedOutDelegate OnLoggedOut;
        public event MessageToUserDelegate OnMessageToUser;
        public event MetadataUpdatedDelegate OnMetadataUpdated;
        public event MusicDeliveryDelegate OnMusicDelivery;
        public event NotifyMainThreadDelegate OnNotifyMainThread;
        public event OfflineStatusUpdatedDelegate OnOfflineStatusUpdated;
        public event PlayTokenLostDelegate OnPlayTokenLost;
        public event StartPlaybackDelegate OnStartPlayback;
        public event StopPlaybackDelegate OnStopPlayback;
        public event StreamingErrorDelegate OnStreamingError;
        public event UserinfoUpdatedDelegate OnUserinfoUpdated;
        public event ConnectionStateUpdatedDelegate OnConnectionStateUpdated;
        public event CredentialsBlobUpdatedDelegate OnCredentialsBlobUpdated;
        public event OfflineErrorDelegate OnOfflineError;
        public event PrivateSessionModeChangedDelegate OnPrivateSessionModeChanged;
        public event ScobbleErrorDelegate OnScobbleError;

        #endregion

        #region main

        protected byte[] AppKey;
        protected string AppId;
        protected string CacheLocation;
        protected libspotify.sp_session_callbacks Callbacks;

        public ILog Log;

        public bool IsLoggedIn { get; protected set; } = false;
        public bool IsInterrupted { get; protected set; } = false;

        public SessionBase(byte[] appKey, string appId, ILog log = null, string cacheLocation = null)
        {
            this.AppKey = appKey;
            this.AppId = appId;
            this.CacheLocation = cacheLocation;
            this.Log = log ?? new DummyLogger();

            StartMainWorker();
            Init();
        }

        protected virtual libspotify.sp_error Init()
        {
            // first add our handlers
            OnNotifyMainThread += OnOnNotifyMainThread;
            OnLoggedIn += OnOnLoggedIn;
            OnLoggedOut += OnOnLoggedOut;
            OnPlayTokenLost += OnOnPlayTokenLost;

            // then custom ones
            InitEvents();

            Callbacks = new libspotify.sp_session_callbacks();
            if (OnNotifyMainThread != null)
                Callbacks.notify_main_thread = Marshal.GetFunctionPointerForDelegate(OnNotifyMainThread);
            if (OnConnectionError != null)
                Callbacks.connection_error = Marshal.GetFunctionPointerForDelegate(OnConnectionError);
            if (OnEndOfTrack != null) Callbacks.end_of_track = Marshal.GetFunctionPointerForDelegate(OnEndOfTrack);
            if (OnGetAudioBufferStats != null)
                Callbacks.get_audio_buffer_stats = Marshal.GetFunctionPointerForDelegate(OnGetAudioBufferStats);
            if (OnLogMessage != null) Callbacks.log_message = Marshal.GetFunctionPointerForDelegate(OnLogMessage);
            if (OnLoggedIn != null) Callbacks.logged_in = Marshal.GetFunctionPointerForDelegate(OnLoggedIn);
            if (OnLoggedOut != null) Callbacks.logged_out = Marshal.GetFunctionPointerForDelegate(OnLoggedOut);
            if (OnMessageToUser != null)
                Callbacks.message_to_user = Marshal.GetFunctionPointerForDelegate(OnMessageToUser);
            if (OnMetadataUpdated != null)
                Callbacks.metadata_updated = Marshal.GetFunctionPointerForDelegate(OnMetadataUpdated);
            if (OnMusicDelivery != null)
                Callbacks.music_delivery = Marshal.GetFunctionPointerForDelegate(OnMusicDelivery);
            if (OnOfflineStatusUpdated != null)
                Callbacks.offline_status_updated = Marshal.GetFunctionPointerForDelegate(OnOfflineStatusUpdated);
            if (OnPlayTokenLost != null)
                Callbacks.play_token_lost = Marshal.GetFunctionPointerForDelegate(OnPlayTokenLost);
            if (OnStartPlayback != null)
                Callbacks.start_playback = Marshal.GetFunctionPointerForDelegate(OnStartPlayback);
            if (OnStopPlayback != null)
                Callbacks.stop_playback = Marshal.GetFunctionPointerForDelegate(OnStopPlayback);
            if (OnStreamingError != null)
                Callbacks.streaming_error = Marshal.GetFunctionPointerForDelegate(OnStreamingError);
            if (OnUserinfoUpdated != null)
                Callbacks.userinfo_updated = Marshal.GetFunctionPointerForDelegate(OnUserinfoUpdated);
            if (OnConnectionStateUpdated != null)
                Callbacks.connectionstate_updated = Marshal.GetFunctionPointerForDelegate(OnConnectionStateUpdated);
            if (OnCredentialsBlobUpdated != null)
                Callbacks.credentials_blob_updated = Marshal.GetFunctionPointerForDelegate(OnCredentialsBlobUpdated);
            if (OnOfflineError != null)
                Callbacks.offline_error = Marshal.GetFunctionPointerForDelegate(OnOfflineError);
            if (OnPrivateSessionModeChanged != null)
                Callbacks.private_session_mode_changed =
                    Marshal.GetFunctionPointerForDelegate(OnPrivateSessionModeChanged);
            if (OnScobbleError != null)
                Callbacks.scrobble_error = Marshal.GetFunctionPointerForDelegate(OnScobbleError);


            IntPtr callbacksPtr = Marshal.AllocHGlobal(Marshal.SizeOf(Callbacks));
            Marshal.StructureToPtr(Callbacks, callbacksPtr, true);

            if (String.IsNullOrEmpty(CacheLocation))
                CacheLocation = Path.Combine(Path.GetTempPath(),
                    "spotify_api_temp" + Path.DirectorySeparatorChar + AppId);

            var config = new libspotify.sp_session_config
            {
                api_version = libspotify.SPOTIFY_API_VERSION,
                user_agent = "Spoti",
                application_key_size = AppKey.Length,
                application_key = Marshal.AllocHGlobal(AppKey.Length),
                cache_location = CacheLocation, // Path.Combine(Path.GetTempPath(), "spotify_api_temp\\" + _appId),
                settings_location = CacheLocation,
                // Path.Combine(Path.GetTempPath(), "spotify_api_temp\\" + _appId) : ,
                callbacks = callbacksPtr,
                compress_playlists = true,
                dont_save_metadata_for_playlists = false,
                initially_unload_playlists = false
            };

            Log.Debug(ModuleId, String.Format("api_version={0}", config.api_version));
            Log.Debug(ModuleId, String.Format("application_key_size={0}", config.application_key_size));
            Log.Debug(ModuleId, String.Format("cache_location={0}", config.cache_location));
            Log.Debug(ModuleId, String.Format("settings_location={0}", config.settings_location));

            Marshal.Copy(AppKey, 0, config.application_key, AppKey.Length);

            IntPtr sessionPtr;
            libspotify.sp_error err = libspotify.sp_session_create(ref config, out sessionPtr);

            if (err == libspotify.sp_error.OK)
            {
                Handle = sessionPtr;
                libspotify.sp_session_set_connection_type(sessionPtr,
                    libspotify.sp_connection_type.SP_CONNECTION_TYPE_WIRED);
            }

            var preferredBitrate = libspotify.sp_session_preferred_bitrate(sessionPtr,
                libspotify.sp_bitrate.BITRATE_320k);
            if (preferredBitrate == libspotify.sp_error.OK)
                Log.Debug(ModuleId, "Session Bitrate set to 320 Kbp/s");
            else
                Log.Warning(ModuleId, "Failed to set session bitrate to 320 Kbp/s");


            // _workerSemafore.Set();

            return err;
        }


        protected virtual void InitEvents()
        {
            // throw new NotImplementedException("InitEventMapping method should be implemented");
        }

        #endregion

        #region Methods

        public virtual bool LogIn(string username, string password)
        {
            if (IsLoggedIn)
                return true;

            libspotify.sp_session_login(Handle, username, password, false, null);
            _workerSemafore.Set();
            _programSemafore.WaitOne();

            return IsLoggedIn;
        }

        public virtual void LogOut()
        {
            if (!IsLoggedIn)
                return;

            try
            {
                libspotify.sp_session_logout(Handle);
            }
            catch (Exception)
            {
                // ignore, by now
            }
            finally
            {
                _workerSemafore.Set();
                _programSemafore.WaitOne();
            }
        }

        public libspotify.sp_error LoadPlayer(Track track)
        {
            return libspotify.sp_session_player_load(Handle, track.Handle);
        }

        public void Play()
        {
            IsInterrupted = false;
            libspotify.sp_session_player_play(Handle, true);
        }

        public void Pause()
        {
            // IsInterrupted = false;
            libspotify.sp_session_player_play(Handle, false);
        }

        public void UnloadPlayer()
        {
            libspotify.sp_session_player_unload(Handle);
        }

        public libspotify.sp_error FlushCache()
        {
            return libspotify.sp_session_flush_caches(Handle);
        }

        #endregion

        #region Event Callbacks

        private void OnOnLoggedIn(IntPtr sessionPtr, libspotify.sp_error error)
        {
            if (error == libspotify.sp_error.OK)
                IsLoggedIn = true;

            Log.Debug(ModuleId, String.Format("Session Logged IN: {0}", error));

            if (_programSemafore != null)
            {
                _programSemafore.Set();
            }
        }

        private void OnOnLoggedOut(IntPtr sessionPtr)
        {
            Log.Debug(ModuleId, String.Format("Session Logged OUT"));

            IsLoggedIn = false;

            if (_programSemafore != null)
            {
                _programSemafore.Set();
            }
        }

        private void OnOnPlayTokenLost(IntPtr sessionPtr)
        {
            IsInterrupted = true;
        }

        #endregion

        #region Worker Thread

        public bool IsRunning { get; protected set; }

        private BackgroundWorker _mainWorker;
        private object _sync = new object();
        private AutoResetEvent _programSemafore;
        private AutoResetEvent _workerSemafore;

        [HandleProcessCorruptedStateExceptions]
        private void StartMainWorker()
        {
            lock (_sync)
            {
                _programSemafore = new AutoResetEvent(false);
                _workerSemafore = new AutoResetEvent(false);

                _mainWorker = new BackgroundWorker {WorkerSupportsCancellation = true};
                _mainWorker.DoWork += MainWorkerOnDoWork;

                _mainWorker.RunWorkerAsync(); // 1. runs worker thread and waits for it
                _programSemafore.WaitOne();
            }
        }

        private void OnOnNotifyMainThread(IntPtr sessionPtr)
        {
            if (_workerSemafore != null)
                _workerSemafore.Set(); // 4. libspotify reports back and allows worker thread to go further

            // Log.Debug(ModuleId, "libspotify Notifies");
        }

        private void MainWorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            var worker = sender as BackgroundWorker;
            IsRunning = true;
            // _workerSemafore = new AutoResetEvent(false);

            _programSemafore.Set(); // 2. worker thread allows program thread to run

            while (true)
            {
                if (worker.CancellationPending)
                    break;

                _workerSemafore.WaitOne(); // 3. worker thread waits for further notification

                if (worker.CancellationPending)
                    break;

                lock (_sync)
                {
                    if (Handle != IntPtr.Zero)
                    {
                        var timeout = Timeout.Infinite;
                        do
                        {
                            try
                            {
                                // if (IsLoggedIn)
                                libspotify.sp_session_process_events(Handle, out timeout);
                            }
                            catch (Exception ex)
                            {
                                Log.Error(ModuleId, ex.Message);
                            }
                        } while (worker.CancellationPending && timeout == 0);
                    }
                }
            }

            IsRunning = false;
        }

        #endregion

        #region Dispose

        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        // Protected implementation of Dispose pattern. 
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here. 
                //

                if (IsLoggedIn)
                    LogOut();
                _mainWorker.CancelAsync();
                _workerSemafore.Set();
                while (IsRunning)
                {
                    Thread.Sleep(10);
                }
                libspotify.sp_session_release(Handle);
            }

            // Free any unmanaged objects here. 
            //
            _disposed = true;
        }

        ~SessionBase()
        {
            Dispose(false);
        }

        #endregion
    }
}