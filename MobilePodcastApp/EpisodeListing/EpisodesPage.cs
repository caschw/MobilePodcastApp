using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Forms;

// ReSharper disable once CheckNamespace
namespace MobilePodcastApp.EpisodeListing
{
	public class EpisodesView : ContentPage
	{
	    private readonly ObservableCollection<string> _episodeTitles = new ObservableCollection<string>();

	    public EpisodesView()
	    {
	        var episodeListView = new ListView
	        {
	            ItemsSource = _episodeTitles
	        };
	        Content = episodeListView;

	        Task.Run(async () => await LoadEpisodeList());
	    }

	    private async Task LoadEpisodeList()
	    {
            var episodes = await GetEpisodes();

	        foreach (var title in episodes.Select(x => x.Title))
	        {
	            _episodeTitles.Add(title);
	        }
	    }

	    private async Task<IEnumerable<FeedItem>> GetEpisodes()
        {
            var http = new HttpClient();
            var rss = await http.GetStringAsync(App.AppSettings.EpisodeRssFeed);

	        var doc = XDocument.Parse(rss);
            var items = (from item in doc.Element("rss").Element("channel").Elements("item")
             select new FeedItem
             {
                 Title = item.Element("title").Value,
                 Link = item.Element("link").Value,
                 Description = item.Element("description").Value,
                 PublicationDate = DateTime.Parse(item.Element("pubDate").Value),
                 GUID = item.Element("guid").Value
             }).ToList();

	        return items;
        }
	}
}
