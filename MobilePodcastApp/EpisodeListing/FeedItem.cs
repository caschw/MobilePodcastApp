using System;
using System.Collections.Generic;
using System.Text;

namespace MobilePodcastApp.EpisodeListing
{
    public class FeedItem
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public DateTime PublicationDate { get; set; }
        public string GUID { get; set; }
    }
}
