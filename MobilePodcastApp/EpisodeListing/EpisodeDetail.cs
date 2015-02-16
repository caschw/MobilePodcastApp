using Xamarin.Forms;
using Label = Xamarin.Forms.Label;

// ReSharper disable once CheckNamespace
namespace MobilePodcastApp.EpisodeListing
{
	public class EpisodeDetail : ContentPage
	{
		public EpisodeDetail (FeedItem episode)
		{
			Content = new StackLayout {
				Children = {
					new Label { Text = episode.Description }
				}
			};
		}
	}
}

