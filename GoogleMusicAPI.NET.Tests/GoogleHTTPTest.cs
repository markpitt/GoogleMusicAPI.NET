using GoogleMusicAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;

namespace GoogleMusicAPI.Tests
{
    
    
    /// <summary>
    ///This is a test class for GoogleHTTPTest and is intended
    ///to contain all GoogleHTTPTest Unit Tests
    ///</summary>
    [TestClass()]
    public class GoogleHTTPTest
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
        ///A test for DownloadDataAsync
        ///</summary>
        [TestMethod()]
        public void DownloadDataAsyncTest()
        {
            GoogleHTTP target = new GoogleHTTP(); // TODO: Initialize to an appropriate value
            HttpWebRequest request = null; // TODO: Initialize to an appropriate value
            byte[] d = null; // TODO: Initialize to an appropriate value
            int millisecondsTimeout = 0; // TODO: Initialize to an appropriate value
            Action<GoogleHTTPResponse> completedCallback = null; // TODO: Initialize to an appropriate value
            target.DownloadDataAsync(request, d, millisecondsTimeout, completedCallback);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for DownloadStringAsync
        ///</summary>
        [TestMethod()]
        public void DownloadStringAsyncTest()
        {
            GoogleHTTP target = new GoogleHTTP(); // TODO: Initialize to an appropriate value
            Uri address = null; // TODO: Initialize to an appropriate value
            Action<GoogleHTTPResponse> completedCallback = null; // TODO: Initialize to an appropriate value
            int millisecondsTimeout = 0; // TODO: Initialize to an appropriate value
            HttpWebRequest expected = null; // TODO: Initialize to an appropriate value
            HttpWebRequest actual;
            actual = target.DownloadStringAsync(address, completedCallback, millisecondsTimeout);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetCookieValue
        ///</summary>
        [TestMethod()]
        public void GetCookieValueTest()
        {
            string cookieName = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = GoogleHTTP.GetCookieValue(cookieName);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SetCookieData
        ///</summary>
        [TestMethod()]
        public void SetCookieDataTest()
        {
            CookieContainer cont = null; // TODO: Initialize to an appropriate value
            CookieCollection coll = null; // TODO: Initialize to an appropriate value
            GoogleHTTP.SetCookieData(cont, coll);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SetupRequest
        ///</summary>
        [TestMethod()]
        public void SetupRequestTest()
        {
            GoogleHTTP target = new GoogleHTTP(); // TODO: Initialize to an appropriate value
            Uri address = null; // TODO: Initialize to an appropriate value
            HttpWebRequest expected = null; // TODO: Initialize to an appropriate value
            HttpWebRequest actual;
            actual = target.SetupRequest(address);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for UploadDataAsync
        ///</summary>
        [TestMethod()]
        public void UploadDataAsyncTest()
        {
            GoogleHTTP target = new GoogleHTTP(); // TODO: Initialize to an appropriate value
            Uri address = null; // TODO: Initialize to an appropriate value
            FormBuilder form = null; // TODO: Initialize to an appropriate value
            Action<GoogleHTTPResponse> complete = null; // TODO: Initialize to an appropriate value
            HttpWebRequest expected = null; // TODO: Initialize to an appropriate value
            HttpWebRequest actual;
            actual = target.UploadDataAsync(address, form, complete);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for UploadDataAsync
        ///</summary>
        [TestMethod()]
        public void UploadDataAsyncTest1()
        {
            GoogleHTTP target = new GoogleHTTP(); // TODO: Initialize to an appropriate value
            Uri address = null; // TODO: Initialize to an appropriate value
            string contentType = string.Empty; // TODO: Initialize to an appropriate value
            byte[] data = null; // TODO: Initialize to an appropriate value
            Action<GoogleHTTPResponse> completedCallback = null; // TODO: Initialize to an appropriate value
            HttpWebRequest expected = null; // TODO: Initialize to an appropriate value
            HttpWebRequest actual;
            actual = target.UploadDataAsync(address, contentType, data, completedCallback);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
