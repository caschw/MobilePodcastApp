using WindowsPhonePlaybackAgent;
using Microsoft.Xna.Framework.Media;
using System.Linq;
using System;
using Microsoft.Phone.BackgroundAudio;
using Xamarin.Forms;
using Label = Xamarin.Forms.Label;

#if WINDOWS_PHONE
using Microsoft.Phone.BackgroundAudio;
#endif

// ReSharper disable once CheckNamespace
namespace MobilePodcastApp.EpisodeListing
{
	public class EpisodeDetail : ContentPage
	{
	    private FeedItem _displayedFeedItem;

		public EpisodeDetail (FeedItem episode)
		{
		    _displayedFeedItem = episode;

		    var playButton = new Button();
		    playButton.Text = "Play";
            playButton.Clicked += PlayButtonOnClicked;

			Content = new StackLayout {
				Children = {
					new Label { Text = episode.Description },
                    playButton
				}
			};
		}

	    private void PlayButtonOnClicked(object sender, EventArgs eventArgs)
	    {
	        var playlist = PlaylistCache.Load();
            playlist.UserSelectedEpisode = playlist.Episodes.Single(x => x.Mp3Url == _displayedFeedItem.EnclosureUrl);
            PlaylistCache.Save(playlist);

            var player = BackgroundAudioPlayer.Instance;
            player.Play();
	    }
	}
}

