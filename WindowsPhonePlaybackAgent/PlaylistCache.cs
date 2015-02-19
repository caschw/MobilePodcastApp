using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using Newtonsoft.Json;

namespace WindowsPhonePlaybackAgent
{
    public class PlaylistCache
    {
        public List<Episode> Episodes { get; set; }

        //This is what the user last selected to play
        public Episode UserSelectedEpisode { get; set; }

        private const string PlaylistFileName = "Playlist.json";

        public static void Save(PlaylistCache playlist)
        {
            var serialized = JsonConvert.SerializeObject(playlist);
            var storage = IsolatedStorageFile.GetUserStoreForApplication();

            if (!storage.FileExists(PlaylistFileName))
            {
                storage.CreateFile(PlaylistFileName);
            }

            using (var fileStream = storage.OpenFile(PlaylistFileName, FileMode.Truncate))
            {
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write(serialized);
                }
            }
        }

        public static PlaylistCache Load()
        {
            var storage = IsolatedStorageFile.GetUserStoreForApplication();

            using (var fileStream = storage.OpenFile(PlaylistFileName, FileMode.Open))
            {
                using (var sr = new StreamReader(fileStream))
                {
                    var serialized = sr.ReadToEnd();

                    return JsonConvert.DeserializeObject<PlaylistCache>(serialized);
                }
            }
        }
    }
}
