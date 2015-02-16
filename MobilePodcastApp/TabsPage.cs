using Xamarin.Forms;

// ReSharper disable once CheckNamespace
namespace MobilePodcastApp
{
	public class TabsPage : TabbedPage
	{
		public TabsPage ()
		{
            Children.Add(new MobilePodcastApp.EpisodeListing.EpisodesView { Title = "Episodes" });
            Children.Add(new AboutPage { Title = "About" });
		}
	}
}
