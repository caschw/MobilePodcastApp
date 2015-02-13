using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

// ReSharper disable once CheckNamespace
namespace MobilePodcastApp
{
	public class TabsPage : TabbedPage
	{
		public TabsPage ()
		{
            Children.Add(new MobilePodcastApp.EpisodeListing.EpisodesView { Title = "Episodes" });
            Children.Add(new TestPage { Title = "About" });
		}
	}
}
