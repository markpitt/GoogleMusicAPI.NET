using GoogleMusicAPI;
using GoogleMusicAPI.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.QualityTools.Testing.Fakes;

namespace GoogleMusicAPI.Tests
{


    /// <summary>
    ///This is a test class for APITest and is intended
    ///to contain all APITest Unit Tests
    ///</summary>
    [TestClass()]
    public class APITest
    {
        private TestContext testContextInstance;
        private static API target = new API();
        private static bool loggedIn = false;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            //var stub = new StubAPI();

            //stub.Login("email", "password", result =>
            //    {
            //        loggedIn = result.Data.LoggedIn;
            //    });


            //string email = "email";
            //string password = "password";

            //var completion = new ManualResetEvent(false);

            //target.Login(email, password, (result) =>
            //{
            //    loggedIn = result.Data.LoggedIn;
            //    completion.Set();
            //});

            //completion.WaitOne();
            //Assert.IsTrue(loggedIn);
        }
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for AddPlaylist
        ///</summary>
        [TestMethod()]
        public void AddPlaylistTest()
        {
            API target = new API(); // TODO: Initialize to an appropriate value
            string playlistName = string.Empty; // TODO: Initialize to an appropriate value
            Action<APIResponse<AddPlaylistResp>> completeCallback = null; // TODO: Initialize to an appropriate value
            target.AddPlaylist(playlistName, completeCallback);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for DeletePlaylist
        ///</summary>
        [TestMethod()]
        public void DeletePlaylistTest()
        {
            API target = new API(); // TODO: Initialize to an appropriate value
            string id = string.Empty; // TODO: Initialize to an appropriate value
            Action<APIResponse<DeletePlaylistResp>> callback = null; // TODO: Initialize to an appropriate value
            target.DeletePlaylist(id, callback);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for GetAllSongs
        ///</summary>
        [TestMethod()]
        public void GetAllSongsTest()
        {
            API target = new API(); // TODO: Initialize to an appropriate value
            Action<APIResponse<List<GoogleMusicSong>>> completeCallback = null; // TODO: Initialize to an appropriate value
            Action<int, int> reportProgressCallback = null; // TODO: Initialize to an appropriate value
            string continuationToken = string.Empty; // TODO: Initialize to an appropriate value
            target.GetLibrarySongs(completeCallback, reportProgressCallback, continuationToken);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for GetPlaylistSongs
        ///</summary>
        [TestMethod()]
        public void GetPlaylistSongsTest()
        {
            API target = new API(); // TODO: Initialize to an appropriate value
            string playlistID = string.Empty; // TODO: Initialize to an appropriate value
            Action<APIResponse<GoogleMusicPlaylist>> completeCallback = null; // TODO: Initialize to an appropriate value
            target.GetPlaylistSongs(playlistID, completeCallback);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for GetSongURL
        ///</summary>
        [TestMethod()]
        public void GetSongURLTest()
        {
            API target = new API(); // TODO: Initialize to an appropriate value
            string id = string.Empty; // TODO: Initialize to an appropriate value
            Action<APIResponse<GoogleMusicSongUrl>> completedCallback = null; // TODO: Initialize to an appropriate value
            target.GetStreamURL(id, completedCallback);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Login
        ///</summary>
        [TestMethod()]
        public void LoginTest()
        {

            using (ShimsContext.Create())
            {
                var api = new API();
                var complete = new ManualResetEvent(false);
                api.Login("email", "password", result =>
                    {
                        loggedIn = result.Data.LoggedIn;
                        complete.Set();
                    });
                complete.WaitOne();
            }



            //string email = "email";
            //string password = "password";

            //var completion = new ManualResetEvent(false);

            //bool loginState = false;

            //target.Login(email, password, (result) =>
            //{
            //    loginState = result.Data.LoggedIn;
            //    completion.Set();
            //});

            //completion.WaitOne();
            //Assert.IsTrue(loginState);
        }

        /// <summary>
        ///A test for AddToPlaylist
        ///</summary>
        [TestMethod()]
        public void AddToPlaylistTest()
        {

            API target = new API(); // TODO: Initialize to an appropriate value
            string playlistId = string.Empty; // TODO: Initialize to an appropriate value
            List<string> songIds = null; // TODO: Initialize to an appropriate value
            //target.AddToPlaylist(playlistId, songIds);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for AuthenticateUploader
        ///</summary>
        [TestMethod()]
        public void AuthenticateUploaderTest()
        {
            API target = new API(); // TODO: Initialize to an appropriate value
            string uploaderId = string.Empty; // TODO: Initialize to an appropriate value
            string uploaderFriendlyName = string.Empty; // TODO: Initialize to an appropriate value
            Action<APIResponse<bool>> completeCallback = null; // TODO: Initialize to an appropriate value
            target.AuthenticateUploader(uploaderId, uploaderFriendlyName, completeCallback);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for CancelUploadJobs
        ///</summary>
        [TestMethod()]
        public void CancelUploadJobsTest()
        {
            API target = new API(); // TODO: Initialize to an appropriate value
            string uploaderId = string.Empty; // TODO: Initialize to an appropriate value
            Action<APIResponse<bool>> completeCallback = null; // TODO: Initialize to an appropriate value
            target.CancelUploadJobs(uploaderId, completeCallback);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ChangePlaylistName
        ///</summary>
        [TestMethod()]
        public void ChangePlaylistNameTest()
        {
            API target = new API(); // TODO: Initialize to an appropriate value
            string playlistId = string.Empty; // TODO: Initialize to an appropriate value
            string newTitle = string.Empty; // TODO: Initialize to an appropriate value
            Action<APIResponse<bool>> completeCallback = null; // TODO: Initialize to an appropriate value
            target.ChangePlaylistName(playlistId, newTitle, completeCallback);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ChangePlaylistOrder
        ///</summary>
        [TestMethod()]
        public void ChangePlaylistOrderTest()
        {
            API target = new API(); // TODO: Initialize to an appropriate value
            string playlistId = string.Empty; // TODO: Initialize to an appropriate value
            List<string> songIds = null; // TODO: Initialize to an appropriate value
            List<string> entryIds = null; // TODO: Initialize to an appropriate value
            Action<APIResponse<bool>> completeCallback = null; // TODO: Initialize to an appropriate value
            string afterId = string.Empty; // TODO: Initialize to an appropriate value
            string beforeId = string.Empty; // TODO: Initialize to an appropriate value
            target.ChangePlaylistOrder(playlistId, songIds, entryIds, completeCallback, afterId, beforeId);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ChangeSongMetadata
        ///</summary>
        [TestMethod()]
        public void ChangeSongMetadataTest()
        {
            var completion = new ManualResetEvent(false);
            GoogleMusicChangeMetadata md = new GoogleMusicChangeMetadata();
            md.Songs = new List<GoogleMusicSong> { new GoogleMusicSong(), new GoogleMusicSong() };

            target.ChangeSongMetadata(md, result =>
            {
                var t = result.Data;
                completion.Set();
            });

            completion.WaitOne();
        }

        /// <summary>
        ///A test for DeleteSongs
        ///</summary>
        [TestMethod()]
        public void DeleteSongsTest()
        {
            API target = new API(); // TODO: Initialize to an appropriate value
            List<string> songIds = null; // TODO: Initialize to an appropriate value
            Action<APIResponse<bool>> completeCallback = null; // TODO: Initialize to an appropriate value
            string playlistId = string.Empty; // TODO: Initialize to an appropriate value
            List<string> entryIds = null; // TODO: Initialize to an appropriate value
            target.DeleteSongs(songIds, completeCallback, playlistId, entryIds);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for GetAllPlaylistSongs
        ///</summary>
        [TestMethod()]
        public void GetAllPlaylistSongsTest()
        {
            Assert.IsTrue(loggedIn);
            GoogleMusicPlaylists playlists = null;
            ManualResetEvent completion = new ManualResetEvent(false);

            target.GetAllPlaylistSongs(result =>
            {
                playlists = result.Data;
                completion.Set();
            });

            completion.WaitOne();

            Assert.IsNotNull(playlists);
        }

        /// <summary>
        ///A test for GetDownloadInfo
        ///</summary>
        [TestMethod()]
        public void GetDownloadInfoTest()
        {
            API target = new API(); // TODO: Initialize to an appropriate value
            List<string> songIds = null; // TODO: Initialize to an appropriate value
            Action<APIResponse<bool>> completeCallback = null; // TODO: Initialize to an appropriate value
            target.GetDownloadInfo(songIds, completeCallback);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for GetLibrarySongs
        ///</summary>
        [TestMethod()]
        public void GetLibrarySongsTest()
        {
            ManualResetEvent completion = new ManualResetEvent(false);
            List<GoogleMusicSong> songs = null;

            target.GetLibrarySongs(result =>
            {
                songs = result.Data;
                completion.Set();
            });

            completion.WaitOne();

            Assert.IsNotNull(songs);
        }

        /// <summary>
        ///A test for GetStreamURL
        ///</summary>
        [TestMethod()]
        public void GetStreamURLTest()
        {
            API target = new API(); // TODO: Initialize to an appropriate value
            string id = string.Empty; // TODO: Initialize to an appropriate value
            Action<APIResponse<GoogleMusicSongUrl>> completedCallback = null; // TODO: Initialize to an appropriate value
            target.GetStreamURL(id, completedCallback);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for GetUploadJobs
        ///</summary>
        [TestMethod()]
        public void GetUploadJobsTest()
        {
            API target = new API(); // TODO: Initialize to an appropriate value
            string uploaderId = string.Empty; // TODO: Initialize to an appropriate value
            Action<APIResponse<bool>> completeCallback = null; // TODO: Initialize to an appropriate value
            target.GetUploadJobs(uploaderId, completeCallback);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for GetUploadSession
        ///</summary>
        [TestMethod()]
        public void GetUploadSessionTest()
        {
            API target = new API(); // TODO: Initialize to an appropriate value
            string uploaderId = string.Empty; // TODO: Initialize to an appropriate value
            int numberAlreadyUploaded = 0; // TODO: Initialize to an appropriate value
            string track = string.Empty; // TODO: Initialize to an appropriate value
            string filePath = string.Empty; // TODO: Initialize to an appropriate value
            string serverId = string.Empty; // TODO: Initialize to an appropriate value
            Action<APIResponse<bool>> completeCallback = null; // TODO: Initialize to an appropriate value
            bool doNotRematch = false; // TODO: Initialize to an appropriate value
            target.GetUploadSession(uploaderId, numberAlreadyUploaded, track, filePath, serverId, completeCallback, doNotRematch);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ReportBadSongMatch
        ///</summary>
        [TestMethod()]
        public void ReportBadSongMatchTest()
        {
            API target = new API(); // TODO: Initialize to an appropriate value
            List<string> songIds = null; // TODO: Initialize to an appropriate value
            Action<APIResponse<bool>> completeCallback = null; // TODO: Initialize to an appropriate value
            target.ReportBadSongMatch(songIds, completeCallback);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Search
        ///</summary>
        [TestMethod()]
        public void SearchTest()
        {
            var completion = new ManualResetEvent(false);
            GoogleMusicSearchResult results = null;

            string query = "searchtext";
            target.Search(query, result =>
                {
                    results = result.Data;
                    completion.Set();
                });

            completion.WaitOne();

            Assert.IsNotNull(results);
        }

        /// <summary>
        ///A test for UpdateUploadState
        ///</summary>
        [TestMethod()]
        public void UpdateUploadStateTest()
        {
            //string toState = string.Empty; // TODO: Initialize to an appropriate value
            //string uploaderId = string.Empty; // TODO: Initialize to an appropriate value
            //target.UpdateUploadState(toState, uploaderId, completeCallback);
            //Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for UploadFile
        ///</summary>
        [TestMethod()]
        public void UploadFileTest()
        {
            API target = new API(); // TODO: Initialize to an appropriate value
            string sessionUrl = string.Empty; // TODO: Initialize to an appropriate value
            Action<APIResponse<bool>> completeCallback = null; // TODO: Initialize to an appropriate value
            target.UploadFile(sessionUrl, completeCallback);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for UploadImage
        ///</summary>
        [TestMethod()]
        public void UploadImageTest()
        {
            API target = new API(); // TODO: Initialize to an appropriate value
            string imageFilepath = string.Empty; // TODO: Initialize to an appropriate value
            Action<APIResponse<bool>> completeCallback = null; // TODO: Initialize to an appropriate value
            target.UploadImage(imageFilepath, completeCallback);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for UploadMetadata
        ///</summary>
        [TestMethod()]
        public void UploadMetadataTest()
        {
            API target = new API(); // TODO: Initialize to an appropriate value
            target.UploadMetadata();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
