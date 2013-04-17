using GoogleMusicAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWPF.MVVM;

namespace TestWPF.ViewModel
{
    public class SongViewModel : ViewModelBase
    {
        private GoogleMusicSong _song;

        public string Album
        {
            get { return _song.Album; }
        }

        public string Artist
        {
            get { return _song.Artist; }
        }

        public int Track
        {
            get { return _song.Track; }
        }

        public string Title
        {
            get { return _song.Title; }
        }

        public string ArtURL
        {
            get { return _song.ArtURL; }
        }

        public int Disc
        {
            get { return _song.Disc; }
        }

        public string Type
        {
            get { return _song.Type.ToString(); }
        }

        public SongViewModel(GoogleMusicSong song)
        {
            _song = song;
        }
    }
}
