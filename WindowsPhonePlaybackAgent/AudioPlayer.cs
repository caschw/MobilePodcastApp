using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Microsoft.Phone.BackgroundAudio;
using MobilePodcastApp.Common;

namespace WindowsPhonePlaybackAgent
{
    public class AudioPlayer : AudioPlayerAgent
    {
        private static readonly List<AudioTrack> Tracks = new List<AudioTrack>();
        private static AudioTrack _startTrack;

        static AudioPlayer()
        {
            // Subscribe to the managed exception handler
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
            });

            try
            {
                var playlist = PlaylistCache.Load().Result;
                PopulateTrackList(playlist);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// Code to execute on Unhandled Exceptions
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        //Url, Title
        private static void PopulateTrackList(PlaylistCache playlist)
        {
            if (Tracks.Count > 0)
            {
                //Might make sense to update the track list here
                return;
            }

            var albumArt = new Uri("http://static.libsyn.com/p/assets/1/6/1/9/16195f20fda67595/iTunes_Cover_Art.png");

            foreach (var episode in playlist.Episodes)
            {
                Uri audioUri;
                try
                {
                    audioUri = new Uri(episode.Mp3Url);
                }
                catch (UriFormatException)
                {
                    Debug.WriteLine("Couldn't parse {0} as a URI", episode.Mp3Url);
                    continue;
                }

                //TODO: take out hard coded values
                Tracks.Add(new AudioTrack(audioUri, episode.EpisodeTitle, "Jason Young & Carl Schweitzer",
                    "MS Dev Show", albumArt));
            }

            if (playlist.UserSelectedEpisode != null)
            {
                _startTrack = Tracks.Single(x => x.Title == playlist.UserSelectedEpisode.EpisodeTitle);
            }
            else
            {
                _startTrack = Tracks.Last();
            }
        }

        /// <summary>
        /// Called when the playstate changes, except for the Error state (see OnError)
        /// </summary>
        /// <param name="player">The BackgroundAudioPlayer</param>
        /// <param name="track">The track playing at the time the playstate changed</param>
        /// <param name="playState">The new playstate of the player</param>
        /// <remarks>
        /// Play State changes cannot be cancelled. They are raised even if the application
        /// caused the state change itself, assuming the application has opted-in to the callback.
        ///
        /// Notable playstate events:
        /// (a) TrackEnded: invoked when the player has no current track. The agent can set the next track.
        /// (b) TrackReady: an audio track has been set and it is now ready for playack.
        ///
        /// Call NotifyComplete() only once, after the agent request has been completed, including async callbacks.
        /// </remarks>
        protected override void OnPlayStateChanged(BackgroundAudioPlayer player, AudioTrack track, PlayState playState)
        {
            switch (playState)
            {
                case PlayState.TrackEnded:
                    break;
                case PlayState.TrackReady:
                    player.Play();
                    break;
                case PlayState.Shutdown:
                    // TODO: Handle the shutdown state here (e.g. save state)
                    break;
                case PlayState.Unknown:
                    break;
                case PlayState.Stopped:
                    break;
                case PlayState.Paused:
                    break;
                case PlayState.Playing:
                    break;
                case PlayState.BufferingStarted:
                    break;
                case PlayState.BufferingStopped:
                    break;
                case PlayState.Rewinding:
                    break;
                case PlayState.FastForwarding:
                    break;
            }

            NotifyComplete();
        }

        /// <summary>
        /// Called when the user requests an action using application/system provided UI
        /// </summary>
        /// <param name="player">The BackgroundAudioPlayer</param>
        /// <param name="track">The track playing at the time of the user action</param>
        /// <param name="action">The action the user has requested</param>
        /// <param name="param">The data associated with the requested action.
        /// In the current version this parameter is only for use with the Seek action,
        /// to indicate the requested position of an audio track</param>
        /// <remarks>
        /// User actions do not automatically make any changes in system state; the agent is responsible
        /// for carrying out the user actions if they are supported.
        ///
        /// Call NotifyComplete() only once, after the agent request has been completed, including async callbacks.
        /// </remarks>
        protected override void OnUserAction(BackgroundAudioPlayer player, AudioTrack track, UserAction action, object param)
        {
            switch (action)
            {
                case UserAction.Play:
                    if (player.PlayerState != PlayState.Playing)
                    {
                        if (player.Track == null)
                        {
                            player.Track = _startTrack;
                        }

                        player.Play();
                    }
                    break;
                case UserAction.Stop:
                    player.Stop();
                    break;
                case UserAction.Pause:
                    player.Pause();
                    break;
                case UserAction.FastForward:
                    player.FastForward();
                    break;
                case UserAction.Rewind:
                    player.Rewind();
                    break;
                case UserAction.Seek:
                    player.Position = (TimeSpan)param;
                    break;
                case UserAction.SkipNext:
                    player.Track = GetNextTrack(player.Track);
                    break;
                case UserAction.SkipPrevious:
                    player.Track = GetPreviousTrack(player.Track);
                    break;
            }

            NotifyComplete();
        }

        /// <summary>
        /// Implements the logic to get the next AudioTrack instance.
        /// In a playlist, the source can be from a file, a web request, etc.
        /// </summary>
        /// <remarks>
        /// The AudioTrack URI determines the source, which can be:
        /// (a) Isolated-storage file (Relative URI, represents path in the isolated storage)
        /// (b) HTTP URL (absolute URI)
        /// (c) MediaStreamSource (null)
        /// </remarks>
        /// <returns>an instance of AudioTrack, or null if the playback is completed</returns>
        private AudioTrack GetNextTrack(AudioTrack currentTrack)
        {
            var index = Tracks.FindIndex(x => x.Source == currentTrack.Source);

            if (index == Tracks.Count - 1)
            {
                //Last track, play the first
                return Tracks.First();
            }

            return Tracks[index + 1];
        }

        /// <summary>
        /// Implements the logic to get the previous AudioTrack instance.
        /// </summary>
        /// <remarks>
        /// The AudioTrack URI determines the source, which can be:
        /// (a) Isolated-storage file (Relative URI, represents path in the isolated storage)
        /// (b) HTTP URL (absolute URI)
        /// (c) MediaStreamSource (null)
        /// </remarks>
        /// <returns>an instance of AudioTrack, or null if previous track is not allowed</returns>
        private AudioTrack GetPreviousTrack(AudioTrack currentTrack)
        {
            var index = Tracks.FindIndex(x => x.Source == currentTrack.Source);

            if (index == 0)
            {
                //Frist track, play the last
                return Tracks.Last();
            }

            return Tracks[index - 1];
        }

        /// <summary>
        /// Called whenever there is an error with playback, such as an AudioTrack not downloading correctly
        /// </summary>
        /// <param name="player">The BackgroundAudioPlayer</param>
        /// <param name="track">The track that had the error</param>
        /// <param name="error">The error that occured</param>
        /// <param name="isFatal">If true, playback cannot continue and playback of the track will stop</param>
        /// <remarks>
        /// This method is not guaranteed to be called in all cases. For example, if the background agent
        /// itself has an unhandled exception, it won't get called back to handle its own errors.
        /// </remarks>
        protected override void OnError(BackgroundAudioPlayer player, AudioTrack track, Exception error, bool isFatal)
        {
            if (isFatal)
            {
                Abort();
            }
            else
            {
                NotifyComplete();
            }

        }

        /// <summary>
        /// Called when the agent request is getting cancelled
        /// </summary>
        /// <remarks>
        /// Once the request is Cancelled, the agent gets 5 seconds to finish its work,
        /// by calling NotifyComplete()/Abort().
        /// </remarks>
        protected override void OnCancel()
        {
        }
    }
}