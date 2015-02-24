using System;

namespace MobilePodcastApp.Common
{
    public class FeedItem
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public DateTime PublicationDate { get; set; }
        public string GUID { get; set; }
        public string EnclosureUrl { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
