using System.Collections.Generic;
using System.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MobilePodcastApp.Common
{
    [TestClass]
    public class PlaylistCacheTests
    {
        [TestMethod]
        public void SaveLoadPlaylistCache()
        {
            var cache = new PlaylistCache
            {
                Episodes = new List<Episode>
                {
                    new Episode { EpisodeTitle = "episode 1", Mp3Url = "http://1"},
                    new Episode { EpisodeTitle = "episode 2", Mp3Url = "http://2"},
                }
            };
            cache.UserSelectedEpisode = cache.Episodes[0];

            PlaylistCache.Save(cache).Wait();

            var rehydrated = PlaylistCache.Load().Result;

            Assert.AreEqual(2, rehydrated.Episodes.Count);
            Assert.AreEqual("episode 1", rehydrated.UserSelectedEpisode.EpisodeTitle);
        }
    }
}
