﻿using System;
using System.Net;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace GoogleMusicAPI
{
    public class API
    {
        #region Members

        private GoogleHTTP _client;
        private List<GoogleMusicSong> _trackContainer;
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
        public void Login(string email, string password, Action<APIResponse<bool>> completeCallback)
        {
            var fields = new Dictionary<string, string>
            {
                {"service", "sj"},
                {"Email",  email},
                {"Passwd", password},
            };

            var form = CreateForm(fields);

            _client.UploadDataAsync(new Uri("https://www.google.com/accounts/ClientLogin"), form.ContentType, form.GetBytes(),
                (data) =>
                {
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
                        completeCallback(new APIResponse<bool> { Exception = data.Exception });
                    }
                });
        }

        /// <summary>
        /// Logins the specified auth token.
        /// </summary>
        /// <param name="authToken">The auth token.</param>
        /// <param name="completeCallback">The complete callback.</param>
        public void Login(string authToken, Action<APIResponse<bool>> completeCallback)
        {
            GoogleHTTP.AuthroizationToken = authToken;
            GetAuthCookies(completeCallback);
        }

        /// <summary>
        /// Gets the auth cookies.
        /// </summary>
        /// <param name="callback">The callback.</param>
        private void GetAuthCookies(Action<APIResponse<bool>> callback)
        {
            _client.UploadDataAsync(new Uri("https://play.google.com/music/listen?hl=en&u=0"), FormBuilder.Empty,
                (data) =>
                {
                    if (data.Exception == null)
                    {
                        GetTotalTracks(data.JsonData);

                        GoogleHTTP.SetCookieData(data.Request.CookieContainer, data.Response.Cookies);

                        callback(new APIResponse<bool> { Data = true });
                    }
                    else
                    {
                        callback(new APIResponse<bool> { Exception = data.Exception });
                    }
                });
        }

        /// <summary>
        /// Gets the total tracks.
        /// </summary>
        /// <param name="httpData">The HTTP data.</param>
        private void GetTotalTracks(string httpData)
        {
            Regex regEx = new Regex(@"window\['TOTAL_TRACKS'\]\s=\s*(?<tracks>[0-9]*)");
            _totalTracks = int.Parse(regEx.Match(httpData).Groups["tracks"].Value);
        }

        #endregion

        #region Song Methods

        /// <summary>
        /// Gets all songs.
        /// </summary>
        /// <param name="completeCallback">The complete callback.</param>
        /// <param name="reportProgressCallback">The report progress callback.</param>
        /// <param name="continuationToken">The continuation token.</param>
        public void GetAllSongs(Action<APIResponse<List<GoogleMusicSong>>> completeCallback, Action<int, int> reportProgressCallback = null, string continuationToken = "")
        {
            List<GoogleMusicSong> library = new List<GoogleMusicSong>();

            if (string.IsNullOrEmpty(continuationToken))
            {
                _trackContainer.Clear();
                _tracksReceived = 0;
            }

            _client.UploadDataAsync(new Uri("https://play.google.com/music/services/loadalltracks"),
                CreateForm(CreateJSONField("{\"continuationToken\":\"" + continuationToken + "\"}")),
                (data) =>
                {
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
                                GetAllSongs(completeCallback, reportProgressCallback, chunk.ContToken);
                            }
                            else
                            {
                                completeCallback(new APIResponse<List<GoogleMusicSong>> { Data = _trackContainer });
                            }
                        }
                        catch (Exception ex)
                        {
                            completeCallback(new APIResponse<List<GoogleMusicSong>> { Exception = ex });
                        }
                    }
                    else
                    {
                        completeCallback(new APIResponse<List<GoogleMusicSong>> { Exception = data.Exception });
                    }
                });
        }

        /// <summary>
        /// Gets the song URL.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="completedCallback">The completed callback.</param>
        public void GetSongURL(string id, Action<APIResponse<GoogleMusicSongUrl>> completedCallback)
        {
            _client.DownloadStringAsync(new Uri(string.Format("https://play.google.com/music/play?u=0&songid={0}&pt=e", id)),
                (data) =>
                {
                    if (data.Exception == null)
                    {
                        try
                        {
                            var url = JSON.DeserializeObject<GoogleMusicSongUrl>(data.JsonData);
                            completedCallback(new APIResponse<GoogleMusicSongUrl> { Data = url });
                        }
                        catch (Exception ex)
                        {
                            completedCallback(new APIResponse<GoogleMusicSongUrl> { Exception = ex });
                        }
                    }
                    else
                    {
                        completedCallback(new APIResponse<GoogleMusicSongUrl> { Exception = data.Exception });
                    }
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
            _client.UploadDataAsync(new Uri("https://play.google.com/music/services/addplaylist"),
                CreateForm(CreateJSONField("{\"title\":\"" + playlistName + "\"}")),
                (data) =>
                {
                    if (data.Exception == null)
                    {
                        try
                        {
                            var resp = JSON.DeserializeObject<AddPlaylistResp>(data.JsonData);
                            completeCallback(new APIResponse<AddPlaylistResp> { Data = resp });
                        }
                        catch (Exception ex)
                        {
                            completeCallback(new APIResponse<AddPlaylistResp> { Exception = ex });
                        }
                    }
                    else
                    {
                        completeCallback(new APIResponse<AddPlaylistResp> { Exception = data.Exception });
                    }
                });
        }

        public void DeletePlaylist(string id, Action<APIResponse<DeletePlaylistResp>> completeCallback)
        {
            _client.UploadDataAsync(new Uri("https://play.google.com/music/services/deleteplaylist"),
                CreateForm(CreateJSONField("{\"id\":\"" + id + "\"}")),
                (data) =>
                {
                    if (data.Exception == null)
                    {
                        try
                        {
                            var resp = JSON.DeserializeObject<DeletePlaylistResp>(data.JsonData);
                            completeCallback(new APIResponse<DeletePlaylistResp> { Data = resp });
                        }
                        catch (Exception ex)
                        {
                            completeCallback(new APIResponse<DeletePlaylistResp> { Exception = ex });
                        }
                    }
                    else
                    {
                        completeCallback(new APIResponse<DeletePlaylistResp> { Exception = data.Exception });
                    }
                });
        }

        /// <summary>
        /// Returns all user and instant playlists
        /// </summary>
        public void GetPlaylist(string playlistID, Action<APIResponse<GoogleMusicPlaylist>> completeCallback)
        {
            _client.UploadDataAsync(new Uri("https://play.google.com/music/services/loadplaylist"),
                CreatePlaylistRequest(playlistID),
                (data) =>
                {
                    if (data.Exception == null)
                    {
                        try
                        {
                            var playlist = JSON.DeserializeObject<GoogleMusicPlaylist>(data.JsonData);
                            completeCallback(new APIResponse<GoogleMusicPlaylist> { Data = playlist });
                        }
                        catch(Exception ex)
                        {
                            completeCallback(new APIResponse<GoogleMusicPlaylist> { Exception = ex });
                        }
                    }
                    else
                    {
                        completeCallback(new APIResponse<GoogleMusicPlaylist> { Exception = data.Exception });
                    }
                });
        }

        /// <summary>
        /// Gets all playlists.
        /// </summary>
        /// <param name="completeCallback">The complete callback.</param>
        public void GetAllPlaylists(Action<APIResponse<GoogleMusicPlaylists>> completeCallback)
        {
            _client.UploadDataAsync(new Uri("https://play.google.com/music/services/loadplaylist"),
                CreatePlaylistRequest("all"),
                (data) =>
                {
                    if (data.Exception == null)
                    {
                        try
                        {
                            var playlists = JSON.DeserializeObject<GoogleMusicPlaylists>(data.JsonData);
                            completeCallback(new APIResponse<GoogleMusicPlaylists> { Data = playlists });
                        }
                        catch(Exception ex)
                        {
                            completeCallback(new APIResponse<GoogleMusicPlaylists> { Exception = ex});
                        }
                    }
                    else
                    {
                        completeCallback(new APIResponse<GoogleMusicPlaylists> { Exception = data.Exception });
                    }
                });
        }

        /// <summary>
        /// Creates the playlist request.
        /// </summary>
        /// <param name="playlistID">The playlist ID.</param>
        /// <returns></returns>
        private static FormBuilder CreatePlaylistRequest(string playlistID)
        {
            return CreateForm(CreateJSONField((playlistID.Equals("all")) ? "{}" : "{\"id\":\"" + playlistID + "\"}"));
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Creates the JSON fields.
        /// </summary>
        /// <param name="jsonString">The json string.</param>
        /// <returns></returns>
        private static Dictionary<string, string> CreateJSONField(string jsonString)
        {
            Dictionary<string, string> fields = new Dictionary<string, string>
            {
               {"json", jsonString}
            };

            return fields;
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

        #endregion
    }
}