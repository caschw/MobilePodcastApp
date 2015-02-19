using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Forms;
using DataTemplate = Xamarin.Forms.DataTemplate;
using Thickness = Xamarin.Forms.Thickness;

#if WINDOWS_PHONE
using WindowsPhonePlaybackAgent;
#endif

// ReSharper disable once CheckNamespace
namespace MobilePodcastApp.EpisodeListing
{
	public class EpisodesView : ContentPage
	{
        private readonly ObservableCollection<FeedItem> _episodes = new ObservableCollection<FeedItem>();
	    private readonly ListView _episodeListView;

	    public EpisodesView()
	    {
	        _episodeListView = new ListView
	        {
	            ItemsSource = _episodes,
	            ItemTemplate = new DataTemplate(() =>
	            {
	                var titleLabel = new Label{FontSize = 26, FontAttributes = FontAttributes.Bold};
                    titleLabel.SetBinding(Label.TextProperty, "Title");

	                var publishDateLabel = new Label();
                    publishDateLabel.SetBinding(Label.TextProperty, new Binding("PublicationDate", stringFormat:"{0:d}"));

	                return new ViewCell
	                {
	                    View = new StackLayout
	                    {
	                        Padding = new Thickness(0, 0, 0, 20),
	                        Orientation = StackOrientation.Vertical,
	                        Children =
	                        {
	                            titleLabel,
	                            new StackLayout
	                            {
	                                Orientation = StackOrientation.Horizontal,
	                                Children = {publishDateLabel}
	                            }
	                        }
	                    }
	                };
	            })
	        };

            _episodeListView.ItemTapped += EpisodeListViewOnItemSelected;

            Content = _episodeListView;

	        Task.Run(async () => await LoadEpisodeList());
	    }

	    protected override void OnAppearing()
	    {
            //This is needed so that when the user returns the episode list,
            //they can select the same item again
	        _episodeListView.SelectedItem = null;
	        base.OnAppearing();
	    }

	    private void EpisodeListViewOnItemSelected(object sender, ItemTappedEventArgs eventArgs)
	    {
	        var selected = (FeedItem) eventArgs.Item;
	        Navigation.PushAsync(new EpisodeDetail(selected));
	    }

	    private async Task LoadEpisodeList()
	    {
            var episodes = await GetEpisodes();
	        episodes = episodes.ToList();

            //Get the tracks ready to play before displaying
            var tracks = episodes.OrderBy(x => x.PublicationDate)
                .Select(x => new Episode { EpisodeTitle = x.Title, Mp3Url = x.EnclosureUrl })
                .ToList();

	        var playlist = new PlaylistCache
	        {
	            Episodes = tracks
	        };
            PlaylistCache.Save(playlist);

	        foreach (var episode in episodes)
	        {
                _episodes.Add(episode);
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
                 GUID = item.Element("guid").Value,
                 EnclosureUrl = item.Element("enclosure").Attribute("url").Value,
                 //Duration = TimeSpan.Parse(item.Element("duration").Value)
             }).ToList();

	        return items;
        }
	}
}
