using System;
using System.Collections.Generic;
using System.Windows.Input;
using GoogleMusicAPI;
using TestWPF.MVVM;
using System.ComponentModel.Composition;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;
using System.Windows;

namespace TestWPF.ViewModel
{
    [Export("MainViewModel", typeof(IViewModel))]
    public class MainViewModel : ViewModelBase
    {
        #region Fields

        private string _email;
        private string _password;
        private bool _loggedOut = true;
        private bool _fetchingTracks;
        private ObservableCollection<AlbumViewModel> _albums;
        private int _totalTracks;
        private int _tracksRemaining;

        private RelayCommand _fetchTracksCommand;
        private RelayCommand _fetchPlaylistsCommand;
        private RelayCommand _createTestPlaylistCommand;
        private RelayCommand _getSongUrlCommand;
        private RelayCommand _deletePlaylistCommand;
        private RelayCommand _getPlaylistSongsCommand;
        private RelayCommand _loginCommand;

        private readonly API api = new API();

        private IMessageBoxService _messageBoxService;
        private IDispatchService _dispatchService;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public MainViewModel(IMessageBoxService messageBoxService, IDispatchService dispatchService)
        {
            _messageBoxService = messageBoxService;
            _dispatchService = dispatchService;
        }

        #endregion

        #region Public

        public string Email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    RaisePropertyChanged(() => Email);
                    LoginCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    RaisePropertyChanged(() => Password);
                    LoginCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public bool LoggedOut
        {
            get { return _loggedOut; }
            private set
            {
                if (_loggedOut != value)
                {
                    _loggedOut = value;
                    RaisePropertyChanged(() => LoggedOut);
                }
            }
        }

        public bool FetchingTracks
        {
            get { return _fetchingTracks; }
            private set
            {
                if (_fetchingTracks != value)
                {
                    _fetchingTracks = value;
                    RaisePropertyChanged(() => FetchingTracks);
                }
            }
        }

        public ObservableCollection<AlbumViewModel> Albums
        {
            get { return _albums; }
            set
            {
                if (_albums != value)
                {
                    _albums = value;
                    RaisePropertyChanged(() => Albums);
                }
            }
        }

        public int TotalTracks
        {
            get { return _totalTracks; }
            set
            {
                if (_totalTracks != value)
                {
                    _totalTracks = value;
                    RaisePropertyChanged(() => TotalTracks);
                    RaisePropertyChanged(() => Progress);
                }
            }
        }

        public int TracksRemaining
        {
            get { return _tracksRemaining; }
            set
            {
                if (_tracksRemaining != value)
                {
                    _tracksRemaining = value;
                    RaisePropertyChanged(() => TracksRemaining);
                    RaisePropertyChanged(() => Progress);
                }
            }
        }

        public string Progress
        {
            get { return string.Format("{0} of {1}", _tracksRemaining, _totalTracks); }
        }


        #endregion

        #region Commands

        #region Login Command and Method

        public RelayCommand LoginCommand
        {
            get
            {
                if (_loginCommand == null)
                {
                    _loginCommand = new RelayCommand(CanLogin, OnLogin);
                }
                return _loginCommand;
            }
        }

        private bool CanLogin(object parameter)
        {
            return !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password);
        }

        private void OnLogin(object parameter)
        {
            api.Login(Email, Password, result =>
            {
                if (result.Data)
                {
                    LoggedOut = false;
                    RaiseCanExecuteChanged();
                }
            });
        }

        #endregion

        #region FetchTracks Command and Method

        public RelayCommand FetchTracksCommand
        {
            get
            {
                if (_fetchTracksCommand == null)
                {
                    _fetchTracksCommand = new RelayCommand(CanFetchTracks, OnFetchTracks);
                }
                return _fetchTracksCommand;
            }
        }

        private bool CanFetchTracks(object parameter)
        {
            return !LoggedOut && !FetchingTracks;
        }

        private void OnFetchTracks(object parameter)
        {
            FetchingTracks = true;
            RaiseCanExecuteChanged();
            api.GetAllSongs(result =>
            {
                Albums = new ObservableCollection<AlbumViewModel>(from song in result.Data
                                                                  orderby song.Album
                                                                  group song by song.Album into album
                                                                  select new AlbumViewModel(album.Key, album));

                FetchingTracks = false;
                RaiseCanExecuteChanged();
            });
        }

        #endregion

        #region FetchPlaylists Command and Method

        public RelayCommand FetchPlaylistsCommand
        {
            get
            {
                if (_fetchPlaylistsCommand == null)
                {
                    _fetchPlaylistsCommand = new RelayCommand(CanFetchPlaylists, OnFetchPlaylists);
                }
                return _fetchPlaylistsCommand;
            }
        }

        private bool CanFetchPlaylists(object parameter)
        {
            return !LoggedOut && !FetchingTracks;
        }

        private void OnFetchPlaylists(object parameter)
        {
            api.GetAllPlaylists(result =>
                {

                });

        }

        #endregion

        #region CreateTestPlaylist Command and Method

        public RelayCommand CreateTestPlaylistCommand
        {
            get
            {
                if (_createTestPlaylistCommand == null)
                {
                    _createTestPlaylistCommand = new RelayCommand(CanCreateTestPlaylist, OnCreateTestPlaylist);
                }
                return _createTestPlaylistCommand;
            }
        }

        private bool CanCreateTestPlaylist(object parameter)
        {
            return !LoggedOut && !FetchingTracks;
        }

        private void OnCreateTestPlaylist(object parameter)
        {
            api.AddPlaylist("Testing", result =>
 {
            });
        }

        #endregion

        #region GetSongURL Command and Method

        public RelayCommand GetSongURLCommand
        {
            get
            {
                if (_getSongUrlCommand == null)
                {
                    _getSongUrlCommand = new RelayCommand(CanGetSongURL, OnGetSongURL);
                }
                return _getSongUrlCommand;
            }
        }

        private bool CanGetSongURL(object parameter)
        {
            return !LoggedOut && !FetchingTracks;
        }

        private void OnGetSongURL(object parameter)
        {

        }

        #endregion

        #region DeletePlaylist Command and Method

        public RelayCommand DeletePlaylistCommand
        {
            get
            {
                if (_deletePlaylistCommand == null)
                {
                    _deletePlaylistCommand = new RelayCommand(CanDeletePlaylist, OnDeletePlaylist);
                }
                return _deletePlaylistCommand;
            }
        }

        private bool CanDeletePlaylist(object parameter)
        {
            return !LoggedOut && !FetchingTracks;
        }

        private void OnDeletePlaylist(object parameter)
        {
        }

        #endregion

        #region GetPlaylist Command and Method

        public RelayCommand GetPlaylistSongsCommand
        {
            get
            {
                if (_getPlaylistSongsCommand == null)
                {
                    _getPlaylistSongsCommand = new RelayCommand(CanGetPlaylistSongs, OnGetPlaylistSongs);
                }
                return _getPlaylistSongsCommand;
            }
        }

        private bool CanGetPlaylistSongs(object parameter)
        {
            return !LoggedOut && !FetchingTracks;
        }

        private void OnGetPlaylistSongs(object parameter)
        {
        }

        #endregion

        #endregion

        #region Private

        private void OnError(Exception e)
        {
            _messageBoxService.Title = "Error";
            _messageBoxService.Message = e.Message;
            _messageBoxService.ShowMessageBox();
        }

        private void RaiseCanExecuteChanged()
        {
            _dispatchService.Invoke(() =>
            {
                FetchTracksCommand.RaiseCanExecuteChanged();
                FetchPlaylistsCommand.RaiseCanExecuteChanged();
                CreateTestPlaylistCommand.RaiseCanExecuteChanged();
                GetSongURLCommand.RaiseCanExecuteChanged();
                DeletePlaylistCommand.RaiseCanExecuteChanged();
                GetPlaylistSongsCommand.RaiseCanExecuteChanged();
            });
        }

        private void OnCreatePlaylistComplete(AddPlaylistResp resp)
        {
            throw new System.NotImplementedException();
        }

        private void OnReportProgress(int tracksRemaining, int totalTracks)
        {
            TracksRemaining = tracksRemaining;
            TotalTracks = totalTracks;
        }


        #endregion

    }
}
