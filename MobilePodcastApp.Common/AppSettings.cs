using System;
using System.Collections.Generic;

namespace MobilePodcastApp.Common
{
    public class AppSettings
    {
        public string EpisodeRssFeed
        {
            get { return "http://msdevshow.libsyn.com/rss"; }
        }

        public string PodcastDescription
        {
            get
            {
                return
                    "A NEW podcast for Microsoft developers covering topics such as Azure/cloud, Windows, Windows Phone, .NET, Visual Studio, and more! Hosted by Jason Young and Carl Schweitzer.";
            }
        }

        public IDictionary<string, Uri> MainScreenWebPages()
        {
            return new Dictionary<string, Uri>
            {
                { "Twitter", new Uri("http://twitter.com/msdevshow") }
            };
        }
    }
}
