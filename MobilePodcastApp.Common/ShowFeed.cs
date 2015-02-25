using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MobilePodcastApp.Common
{
    public class ShowFeed
    {
        public static IEnumerable<FeedItem> ParseRssFeed(string rss)
        {
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
                             Duration = GetDuration(item)
                         }).ToList();
            //item.Elements().ToArray()[7].Name.LocalName
            return items;
        }

        private static TimeSpan GetDuration(XElement element)
        {
            var unparsed = element.Elements().Single(x => x.Name.LocalName == "duration").Value;
            if (unparsed.Length == 5)
            {
                unparsed = "00:" + unparsed;
            }
            return TimeSpan.Parse(unparsed);
        }
    }
}
