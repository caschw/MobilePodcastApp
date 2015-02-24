using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PCLStorage;

namespace MobilePodcastApp.Common
{
    public class PlaylistCache
    {
        public List<Episode> Episodes { get; set; }

        //This is what the user last selected to play
        public Episode UserSelectedEpisode { get; set; }

        private const string PlaylistFileName = "Playlist.json";

        private static readonly Mutex _mutex = new Mutex(false);

        public static async Task Save(PlaylistCache playlist)
        {
            _mutex.WaitOne(1000);
            var serialized = JsonConvert.SerializeObject(playlist);
            var file = await FileSystem.Current.LocalStorage.CreateFileAsync(PlaylistFileName, CreationCollisionOption.ReplaceExisting);
            await file.WriteAllTextAsync(serialized);
            _mutex.ReleaseMutex();
        }
        
        public static async Task<PlaylistCache> Load()
        {
            _mutex.WaitOne(1000);
            try
            {
                var file = await FileSystem.Current.LocalStorage.GetFileAsync(PlaylistFileName);
                var serialized = await file.ReadAllTextAsync();

                return JsonConvert.DeserializeObject<PlaylistCache>(serialized);
            }
            finally 
            {
                _mutex.ReleaseMutex();
            }
        }
    }
}
