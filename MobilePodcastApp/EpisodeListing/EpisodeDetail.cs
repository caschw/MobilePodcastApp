using System.Linq;
using System;
using MobilePodcastApp.Common;
using Xamarin.Forms;
using Label = Xamarin.Forms.Label;

#if WINDOWS_PHONE
using Microsoft.Phone.BackgroundAudio;
#endif
#if ANDROID
using MobilePodcastApp.Droid;
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

	    private async void PlayButtonOnClicked(object sender, EventArgs eventArgs)
	    {
	        var playlist = await PlaylistCache.Load();
            playlist.UserSelectedEpisode = playlist.Episodes.Single(x => x.Mp3Url == _displayedFeedItem.EnclosureUrl);
            await PlaylistCache.Save(playlist);

#if WINDOWS_PHONE
            var player = BackgroundAudioPlayer.Instance;
            player.Play();
#endif
#if ANDROID
            var player = new Audio();
            player.Start();
#endif 
	    }
	}
}

