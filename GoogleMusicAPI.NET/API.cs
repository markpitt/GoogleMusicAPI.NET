﻿using System;
using System.Net;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace GoogleMusicAPI
{
    public class API
    {
        #region Fields

        private const string loginURL = "https://www.google.com/accounts/ClientLogin";
        private const string baseURL = "https://play.google.com/music/";
        private const string serviceURL = baseURL + "services/";

        #endregion

        #region Members

        private readonly GoogleHTTP _client;
        private readonly List<GoogleMusicSong> _trackContainer;
        private int _totalTracks;
        private int _tracksReceived;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="API"/> class.
        /// </summary>
        public API()
        {
            _client = new GoogleHTTP();
            _trackContainer = new List<GoogleMusicSong>();
        }

        #endregion

        #region Login

        /// <summary>
        /// Logins the specified email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <param name="completeCallback">The complete callback.</param>
        public void Login(string email, string password, Action<APIResponse<GoogleLoginResponse>> completeCallback)
        {
            var form = CreateForm(new Dictionary<string, string>
                                            {
                                                {"service", "sj"},
                                                {"Email",  email},
                                                {"Passwd", password},
                                            });

            _client.UploadDataAsync(new Uri(loginURL), form.ContentType, form.GetBytes(),
                data =>
                {
                    var response = new APIResponse<GoogleLoginResponse>();

                    if (data.Exception == null)
                    {
                        var CountTemplate = @"Auth=(?<AUTH>(.*?))$";
                        var CountRegex = new Regex(CountTemplate, RegexOptions.IgnoreCase);
                        var auth = CountRegex.Match(data.JsonData).Groups["AUTH"].ToString();

                        GoogleHTTP.AuthroizationToken = auth;

                        GetAuthCookies(completeCallback);
                    }
                    else
                    {
                        response.Data = new GoogleLoginResponse { LoggedIn = false };
                        response.Exception = data.Exception;
                        completeCallback(response);
                    }
                });
        }

        /// <summary>
        /// Logins the specified auth token.
        /// </summary>
        /// <param name="authToken">The auth token.</param>
        /// <param name="completeCallback">The complete callback.</param>
        public void Login(string authToken, Action<APIResponse<GoogleLoginResponse>> completeCallback)
        {
            GoogleHTTP.AuthroizationToken = authToken;
            GetAuthCookies(completeCallback);
        }

        /// <summary>
        /// Gets the auth cookies.
        /// </summary>
        /// <param name="completeCallback">The callback.</param>
        private void GetAuthCookies(Action<APIResponse<GoogleLoginResponse>> completeCallback)
        {
            _client.UploadDataAsync(new Uri(baseURL + "listen?hl=en&u=0"), FormBuilder.Empty,
                data =>
                {
                    var response = new APIResponse<GoogleLoginResponse>();

                    if (data.Exception == null)
                    {
                        _totalTracks = GetTotalTracks(data.JsonData);

                        GoogleHTTP.SetCookieData(data.Request.CookieContainer, data.Response.Cookies);

                        response.Data = new GoogleLoginResponse { LoggedIn = true, AuthorizationToken = GoogleHTTP.AuthroizationToken };
                    }
                    else
                    {
                        response.Data = new GoogleLoginResponse { LoggedIn = false };
                        response.Exception = data.Exception;
                    }
                    completeCallback(response);
                });
        }

        #endregion

        #region Song Methods

        /// <summary>
        /// Gets the library songs.
        /// </summary>
        /// <param name="completeCallback">The complete callback.</param>
        /// <param name="reportProgressCallback">The report progress callback.</param>
        /// <param name="continuationToken">The continuation token.</param>
        public void GetLibrarySongs(Action<APIResponse<List<GoogleMusicSong>>> completeCallback, Action<int, int> reportProgressCallback = null, string continuationToken = "")
        {
            if (string.IsNullOrEmpty(continuationToken))
            {
                _trackContainer.Clear();
                _tracksReceived = 0;
            }

            _client.UploadDataAsync(new Uri(serviceURL + "loadalltracks"),
                CreateForm(CreateJSONField(new { continuationToken = continuationToken })),
                data =>
                {
                    var response = new APIResponse<List<GoogleMusicSong>>();

                    if (data.Exception == null)
                    {
                        try
                        {
                            var chunk = JSON.DeserializeObject<GoogleMusicPlaylist>(data.JsonData);

                            _trackContainer.AddRange(chunk.Songs);

                            if (reportProgressCallback != null)
                                reportProgressCallback(_tracksReceived += chunk.Songs.Count, _totalTracks);

                            if (!string.IsNullOrEmpty(chunk.ContToken))
                            {
                                GetLibrarySongs(completeCallback, reportProgressCallback, chunk.ContToken);
                            }
                            else
                            {
                                response.Data = _trackContainer;
                            }
                        }
                        catch (Exception ex)
                        {
                            response.Exception = ex;
                        }
                    }
                    else
                    {
                        response.Exception = data.Exception;
                    }

                    completeCallback(response);
                });
        }


        /// <summary>
        /// Reports the bad song match.
        /// </summary>
        /// <param name="songIds">The song ids.</param>
        /// <param name="completeCallback">The complete callback.</param>
        public void ReportBadSongMatch(List<string> songIds, Action<APIResponse<bool>> completeCallback)
        {

        }

        /// <summary>
        /// Gets the stream URL.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="completedCallback">The completed callback.</param>
        public void GetStreamURL(string id, Action<APIResponse<GoogleMusicSongUrl>> completedCallback)
        {
            _client.DownloadStringAsync(new Uri(string.Format(baseURL + "play?u=0&songid={0}&pt=e", id)),
                data =>
                {
                    completedCallback(CreateAPIResponse<GoogleMusicSongUrl>(data));
                });
        }

        /// <summary>
        /// Changes the song metadata.
        /// </summary>
        /// <param name="songs">The songs.</param>
        /// <param name="completeCallback">The complete callback.</param>
        public void ChangeSongMetadata(GoogleMusicChangeMetadata songs, Action<APIResponse<bool>> completeCallback)
        {
            var t = JSON.Serialize<GoogleMusicChangeMetadata>(songs);
        }

        /// <summary>
        /// Deletes the songs.
        /// </summary>
        /// <param name="songIds">The song ids.</param>
        /// <param name="completeCallback">The complete callback.</param>
        /// <param name="playlistId">The playlist id.</param>
        /// <param name="entryIds">The entry ids.</param>
        public void DeleteSongs(List<string> songIds, Action<APIResponse<bool>> completeCallback, string playlistId = "all", List<string> entryIds = null)
        {

        }

        /// <summary>
        /// Gets the download info.
        /// </summary>
        /// <param name="songIds">The song ids.</param>
        /// <param name="completeCallback">The complete callback.</param>
        public void GetDownloadInfo(List<string> songIds, Action<APIResponse<bool>> completeCallback)
        {
            _client.UploadDataAsync(new Uri(serviceURL + "multidownload"),
                CreateForm(CreateJSONField(new { songIds = songIds })),
                data =>
                {
                    completeCallback(CreateAPIResponse<bool>(data));
                });
        }

        #endregion

        #region Playlist Methods

        /// <summary>
        /// Adds the playlist.
        /// </summary>
        /// <param name="playlistName">Name of the playlist.</param>
        /// <param name="completeCallback">The complete callback.</param>
        public void AddPlaylist(string playlistName, Action<APIResponse<AddPlaylistResp>> completeCallback)
        {
            _client.UploadDataAsync(new Uri(serviceURL + "addplaylist"),
                CreateForm(CreateJSONField(new { title = playlistName })),
                data =>
                {
                    completeCallback(CreateAPIResponse<AddPlaylistResp>(data));
                });
        }

        public void AddToPlaylist(string playlistId, List<string> songIds, Action<APIResponse<bool>> completeCallback)
        {
            _client.UploadDataAsync(new Uri(serviceURL + "addtoplaylist"),
                CreateForm(CreateJSONField(new { id = playlistId })),
                data =>
                {
                    completeCallback(CreateAPIResponse<bool>(data));
                });

        }

        public void DeletePlaylist(string playlistId, Action<APIResponse<DeletePlaylistResp>> completeCallback)
        {
            _client.UploadDataAsync(new Uri(serviceURL + "deleteplaylist"),
                CreateForm(CreateJSONField(new { id = playlistId })),
                data =>
                {
                    completeCallback(CreateAPIResponse<DeletePlaylistResp>(data));
                });
        }

        /// <summary>
        /// Changes the name of the playlist.
        /// </summary>
        /// <param name="playlistId">The playlist id.</param>
        /// <param name="newTitle">The new title.</param>
        /// <param name="completeCallback">The complete callback.</param>
        public void ChangePlaylistName(string playlistId, string newTitle, Action<APIResponse<bool>> completeCallback)
        {

        }

        /// <summary>
        /// Changes the playlist order.
        /// </summary>
        /// <param name="playlistId">The playlist id.</param>
        /// <param name="songIds">The song ids.</param>
        /// <param name="entryIds">The entry ids.</param>
        /// <param name="afterId">The after id.</param>
        /// <param name="beforeId">The before id.</param>
        /// <param name="completeCallback">The complete callback.</param>
        public void ChangePlaylistOrder(string playlistId, List<string> songIds, List<string> entryIds, Action<APIResponse<bool>> completeCallback, string afterId = null, string beforeId = null)
        {

        }

        /// <summary>
        /// Gets the playlist.
        /// </summary>
        /// <param name="playlistId">The playlist ID.</param>
        /// <param name="completeCallback">The complete callback.</param>
        public void GetPlaylistSongs(string playlistId, Action<APIResponse<GoogleMusicPlaylist>> completeCallback)
        {
            _client.UploadDataAsync(new Uri(serviceURL + "loadplaylist"),
                CreateForm(CreateJSONField(new { id = playlistId })),
                data =>
                {
                    completeCallback(CreateAPIResponse<GoogleMusicPlaylist>(data));
                });
        }

        /// <summary>
        /// Gets all playlists.
        /// </summary>
        /// <param name="completeCallback">The complete callback.</param>
        public void GetAllPlaylistSongs(Action<APIResponse<GoogleMusicPlaylists>> completeCallback)
        {
            _client.UploadDataAsync(new Uri(serviceURL + "loadplaylist"),
                CreateForm(CreateJSONField(new { })),
                data =>
                {
                    completeCallback(CreateAPIResponse<GoogleMusicPlaylists>(data));
                });
        }

        #endregion

        /// <summary>
        /// Searches the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="completeCallback">The complete callback.</param>
        public void Search(string query, Action<APIResponse<GoogleMusicSearchResult>> completeCallback)
        {
            _client.UploadDataAsync(new Uri(serviceURL + "search"),
                CreateForm(CreateJSONField(new { q = query })),
                data =>
                {
                    completeCallback(CreateAPIResponse<GoogleMusicSearchResult>(data));
                });
        }

        /// <summary>
        /// Uploads the image.
        /// </summary>
        /// <param name="imageFilepath">The image filepath.</param>
        /// <param name="completeCallback">The complete callback.</param>
        public void UploadImage(string imageFilepath, Action<APIResponse<bool>> completeCallback)
        {
            _client.UploadDataAsync(new Uri(serviceURL + ""),
                CreateForm(CreateJSONField(new { })),
                data =>
                {
                    completeCallback(CreateAPIResponse<bool>(data));
                });
        }

        #region Upload Methods

        /// <summary>
        /// Authenticates the uploader.
        /// </summary>
        /// <param name="uploaderId">The uploader id.</param>
        /// <param name="uploaderFriendlyName">Name of the uploader friendly.</param>
        /// <param name="completeCallback">The complete callback.</param>
        public void AuthenticateUploader(string uploaderId, string uploaderFriendlyName, Action<APIResponse<bool>> completeCallback)
        {

        }

        /// <summary>
        /// Cancels the upload jobs.
        /// </summary>
        /// <param name="uploaderId">The uploader id.</param>
        /// <param name="completeCallback">The complete callback.</param>
        public void CancelUploadJobs(string uploaderId, Action<APIResponse<bool>> completeCallback)
        {

        }

        /// <summary>
        /// Gets the upload jobs.
        /// </summary>
        /// <param name="uploaderId">The uploader id.</param>
        /// <param name="completeCallback">The complete callback.</param>
        public void GetUploadJobs(string uploaderId, Action<APIResponse<bool>> completeCallback)
        {

        }

        /// <summary>
        /// Gets the upload session.
        /// </summary>
        /// <param name="uploaderId">The uploader id.</param>
        /// <param name="numberAlreadyUploaded">The number already uploaded.</param>
        /// <param name="track">The track.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="serverId">The server id.</param>
        /// <param name="completeCallback">The complete callback.</param>
        /// <param name="doNotRematch">if set to <c>true</c> [do not rematch].</param>
        public void GetUploadSession(string uploaderId, int numberAlreadyUploaded, string track, string filePath, string serverId, Action<APIResponse<bool>> completeCallback, bool doNotRematch = false)
        {

        }

        /// <summary>
        /// Updates the state of the upload.
        /// </summary>
        /// <param name="toState">To state.</param>
        /// <param name="uploaderId">The uploader id.</param>
        /// <param name="completeCallback">The complete callback.</param>
        public void UpdateUploadState(string toState, string uploaderId, Action<APIResponse<bool>> completeCallback)
        {

        }

        /// <summary>
        /// Uploads the file.
        /// </summary>
        /// <param name="sessionUrl">The session URL.</param>
        /// <param name="completeCallback">The complete callback.</param>
        public void UploadFile(string sessionUrl, Action<APIResponse<bool>> completeCallback)
        {

        }

        public void UploadMetadata()
        {

        }

        #endregion

        #region Private Helper Methods

        //private static Dictionary<string, string> CreateJSONFields(List<string> values)
        //{


        //    Dictionary<string, string> fields = new Dictionary<string, string>
        //    {
        //       {"json", from value in values select value}
        //    };

        //    return fields;
        //}

        private static Dictionary<string, string> CreateJSONField(object properties)
        {
            return new Dictionary<string, string> { { "json", JObject.FromObject(properties).ToString() } };
        }



        /// <summary>
        /// Creates the form.
        /// </summary>
        /// <param name="fields">The fields.</param>
        /// <returns></returns>
        private static FormBuilder CreateForm(Dictionary<string, string> fields)
        {
            var form = new FormBuilder();
            form.AddFields(fields);
            form.Close();
            return form;
        }

        /// <summary>
        /// Parses the response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        private static APIResponse<T> CreateAPIResponse<T>(GoogleHTTPResponse data)
        {
            var response = new APIResponse<T>();

            if (data.Exception == null)
            {
                try
                {
                    response.Data = JSON.DeserializeObject<T>(data.JsonData);
                }
                catch (Exception ex)
                {
                    response.Exception = ex;
                }
            }
            else
            {
                response.Exception = data.Exception;
            }
            return response;
        }

        /// <summary>
        /// Gets the total tracks.
        /// </summary>
        /// <param name="httpData">The HTTP data.</param>
        private static int GetTotalTracks(string httpData)
        {
            Regex regEx = new Regex(@"window\['TOTAL_TRACKS'\]\s=\s*(?<tracks>[0-9]*)");
            return int.Parse(regEx.Match(httpData).Groups["tracks"].Value);
        }

        #endregion
    }
}