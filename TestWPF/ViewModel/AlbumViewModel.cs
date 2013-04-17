using GoogleMusicAPI;
using System.Collections.ObjectModel;
using System.Linq;
using TestWPF.MVVM;

namespace TestWPF.ViewModel
{
    public class AlbumViewModel : ViewModelBase
    {
        private string _album;
        private ObservableCollection<SongViewModel> _songs;
        private string _artist;
        private string _albumArtURL;

        public string Artist
        {
            get { return _artist; }
        }

        public string Album
        {
            get { return _album; }
        }

        public int TrackCount
        {
            get { return _songs.Count; }
        }

        public string AlbumArtURL
        {
            get
            {
                if (string.IsNullOrEmpty(_albumArtURL))
                {
                    return "../Images/vinyl-icon.png";
                }
                else
                {
                    return _albumArtURL;
                }
            }
        }

        public ObservableCollection<SongViewModel> Songs
        {
            get { return _songs; }
        }

        public AlbumViewModel(string album, IGrouping<string, GoogleMusicSong> songs)
        {
            _album = album;
            _songs = new ObservableCollection<SongViewModel>(from song in songs orderby song.Track orderby song.Disc select new SongViewModel(song));
            if (_songs.Any())
            {
                _artist = _songs.First().Artist;
                _albumArtURL = _songs.First().ArtURL;
            }
        }
    }
}
