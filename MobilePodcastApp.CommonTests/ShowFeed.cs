using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MobilePodcastApp.Common
{
    [TestClass]
    public class ShowFeedTests
    {
        [TestMethod]
        public void ParseRssFeed_ItemCount()
        {
            var rss = GetEmbeddedSampleRss("msdevshow.xml");
            var items = ShowFeed.ParseRssFeed(rss);

            Assert.AreEqual(44, items.Count());
        }

        [TestMethod]
        public void ParseRssFeed_ItemDetails()
        {
            var rss = GetEmbeddedSampleRss("msdevshow.xml");
            var items = ShowFeed.ParseRssFeed(rss);

            var feedItem = items.First();

            Assert.AreEqual("Soft Skills with John Sonmez", feedItem.Title);
            Assert.AreEqual("http://traffic.libsyn.com/msdevshow/msdevshow_0044.mp3", feedItem.EnclosureUrl);
            Assert.AreEqual("e5c299990328725409374e2953b1c47c", feedItem.GUID);
            Assert.AreEqual("This week we talk to John Sonmez about all the details software developers didn't learn in school. Creating a DMZ in Azure, and the Joel test for Programmers.", feedItem.Description);
            Assert.AreEqual("http://msdevshow.com/2015/02/soft-skills-with-john-sonmez/", feedItem.Link);
            Assert.AreEqual(DateTime.Parse("2/22/2015 4:17:30 PM"), feedItem.PublicationDate);
            Assert.AreEqual(TimeSpan.Parse("01:03:42"), feedItem.Duration);
        }

        public static string GetEmbeddedSampleRss(string name)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(ShowFeedTests),
     "SampleRss." + name);
            using (var sr = new StreamReader(stream))
            {
                var rss = sr.ReadToEnd();
                return rss;
            } 
        }
    }
}
