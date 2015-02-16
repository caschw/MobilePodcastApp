using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;
using Label = Xamarin.Forms.Label;

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

