using System;
using Android.Media;

namespace MobilePodcastApp.Droid
{
    public class Audio
    {
        private static MediaPlayer _player;

        public void Start()
        {
            _player = new MediaPlayer();
            _player.Completion += PlayerOnCompletion;

            _player.SetAudioStreamType(Stream.Music);
            _player.SetDataSource("http://traffic.libsyn.com/msdevshow/msdevshow_0044.mp3");

            _player.Prepare();
            _player.Start();
        }

        private void PlayerOnCompletion(object sender, EventArgs eventArgs)
        {
            _player.Release();
        }
    }
}