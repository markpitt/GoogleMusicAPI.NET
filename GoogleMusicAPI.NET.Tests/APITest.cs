using GoogleMusicAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;

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
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
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
        ///A test for GetAllPlaylists
        ///</summary>
        [TestMethod()]
        public void GetAllPlaylistsTest()
        {
            API target = new API(); // TODO: Initialize to an appropriate value
            Action<APIResponse<GoogleMusicPlaylists>> completeCallback = null; // TODO: Initialize to an appropriate value
            target.GetAllPlaylists(completeCallback);
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
            target.GetAllSongs(completeCallback, reportProgressCallback, continuationToken);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for GetPlaylist
        ///</summary>
        [TestMethod()]
        public void GetPlaylistTest()
        {
            API target = new API(); // TODO: Initialize to an appropriate value
            string playlistID = string.Empty; // TODO: Initialize to an appropriate value
            Action<APIResponse<GoogleMusicPlaylist>> completeCallback = null; // TODO: Initialize to an appropriate value
            target.GetPlaylist(playlistID, completeCallback);
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
            target.GetSongURL(id, completedCallback);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Login
        ///</summary>
        [TestMethod()]
        public void LoginTest()
        {
            API target = new API();
            string email = "email";
            string password = "password";

            var completion = new ManualResetEvent(false);

            bool loginState = false;

            target.Login(email, password, (result) =>
            {
                loginState = result.Data;
                completion.Set();
            });

            completion.WaitOne();
            Assert.IsTrue(loginState);
        }
    }
}
