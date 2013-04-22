using System;
using System.IO;
using System.Net;

namespace GoogleMusicAPI
{
    public class GoogleHTTPResponse
    {
        public HttpWebRequest Request;
        public HttpWebResponse Response;
        public string JsonData;
        public Exception Exception;
    }

    public class GoogleHTTP
    {
        public static String AuthroizationToken = null;
        public static CookieContainer AuthorizationCookieCont = new CookieContainer();
        public static CookieCollection AuthorizationCookies = new CookieCollection();

        private class RequestState
        {
            public HttpWebRequest Request { get; set; }
            public byte[] UploadData { get; set; }
            public Action<GoogleHTTPResponse> CompletedCallback { get; set; }
        }

        /// <summary>
        /// Uploads the data async.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="form">The form.</param>
        /// <param name="completedCallback">The completed callback.</param>
        /// <returns></returns>
        public HttpWebRequest UploadDataAsync(Uri address, FormBuilder form, Action<GoogleHTTPResponse> completedCallback)
        {
            return UploadDataAsync(address, form.ContentType, form.GetBytes(), completedCallback);
        }

        /// <summary>
        /// Uploads the data async.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="data">The data.</param>
        /// <param name="completedCallback">The completed callback.</param>
        /// <returns></returns>
        public HttpWebRequest UploadDataAsync(Uri address, string contentType, byte[] data, Action<GoogleHTTPResponse> completedCallback)
        {
            HttpWebRequest request = SetupRequest(address);

            if (!String.IsNullOrEmpty(contentType))
                request.ContentType = contentType;

            request.Method = "POST";
            RequestState state = new RequestState { Request = request, UploadData = data, CompletedCallback = completedCallback };
            IAsyncResult result = request.BeginGetRequestStream(OpenWrite, state);

            return request;
        }


        /// <summary>
        /// Downloads the string async.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="completedCallback">The completed callback.</param>
        /// <param name="millisecondsTimeout">The milliseconds timeout.</param>
        /// <returns></returns>
        public HttpWebRequest DownloadStringAsync(Uri address, Action<GoogleHTTPResponse> completedCallback, int millisecondsTimeout = 10000)
        {
            HttpWebRequest request = SetupRequest(address);
            request.Method = "GET";
            DownloadDataAsync(request, null, millisecondsTimeout, completedCallback);
            return request;
        }

        /// <summary>
        /// Downloads the data async.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="data">The data.</param>
        /// <param name="millisecondsTimeout">The milliseconds timeout.</param>
        /// <param name="completedCallback">The completed callback.</param>
        public void DownloadDataAsync(HttpWebRequest request, byte[] data, int millisecondsTimeout,
            Action<GoogleHTTPResponse> completedCallback)
        {
            RequestState state = new RequestState { Request = request, UploadData = data, CompletedCallback = completedCallback };
            IAsyncResult result = request.BeginGetResponse(GetResponse, state);
        }


        /// <summary>
        /// Setups the request.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">address</exception>
        public virtual HttpWebRequest SetupRequest(Uri address)
        {
            if (address == null)
                throw new ArgumentNullException("address");

            if (address.ToString().StartsWith("https://play.google.com/music/services/"))
            {
                address = new Uri(address.OriginalString + String.Format("?u=0&xt={0}", GoogleHTTP.GetCookieValue("xt")));
            }

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(address);

            request.CookieContainer = AuthorizationCookieCont;

            if (AuthroizationToken != null)
                request.Headers[HttpRequestHeader.Authorization] = String.Format("GoogleLogin auth={0}", AuthroizationToken);


            return request;
        }

        /// <summary>
        /// Opens the write.
        /// </summary>
        /// <param name="ar">The asynchronous result.</param>
        private void OpenWrite(IAsyncResult ar)
        {
            RequestState state = (RequestState)ar.AsyncState;

            try
            {
                // Get the stream to write our upload to
                using (Stream uploadStream = state.Request.EndGetRequestStream(ar))
                {
                    byte[] buffer = new Byte[checked((uint)Math.Min(1024, (int)state.UploadData.Length))];

                    MemoryStream ms = new MemoryStream(state.UploadData);

                    int bytesRead;
                    int i = 0;
                    while ((bytesRead = ms.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        int prog = (int)Math.Floor(Math.Min(100.0,
                                (((double)(bytesRead * i) / (double)ms.Length) * 100.0)));


                        uploadStream.Write(buffer, 0, bytesRead);

                        i++;
                    }
                }

                IAsyncResult result = state.Request.BeginGetResponse(GetResponse, state);


            }
            catch (Exception ex)
            {
                if (state.CompletedCallback != null)
                    state.CompletedCallback(new GoogleHTTPResponse
                    {
                        Request = state.Request,
                        Exception = ex
                    });//(state.Request, null, null, ex);
            }
        }

        /// <summary>
        /// Gets the response.
        /// </summary>
        /// <param name="ar">The asynchronous result.</param>
        private void GetResponse(IAsyncResult ar)
        {
            RequestState state = (RequestState)ar.AsyncState;
            HttpWebResponse response = null;
            Exception error = null;
            String result = string.Empty;

            try
            {
                response = (HttpWebResponse)state.Request.EndGetResponse(ar);
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream);

                    result = reader.ReadToEnd();

                }
            }
            catch (Exception ex)
            {
                error = ex;
            }

            if (state.CompletedCallback != null)
                state.CompletedCallback(new GoogleHTTPResponse
                {
                    Request = state.Request,
                    Response = response,
                    JsonData = result,
                    Exception = error
                });
        }

        /// <summary>
        /// Gets the cookie value.
        /// </summary>
        /// <param name="cookieName">Name of the cookie.</param>
        /// <returns></returns>
        public static String GetCookieValue(String cookieName)
        {
            foreach (Cookie cookie in AuthorizationCookies)
            {
                if (cookie.Name.Equals(cookieName))
                    return cookie.Value;
            }

            return null;
        }

        /// <summary>
        /// Sets the cookie data.
        /// </summary>
        /// <param name="cookieContainer">The cookie container.</param>
        /// <param name="cookieCollection">The cookie collection.</param>
        public static void SetCookieData(CookieContainer cookieContainer, CookieCollection cookieCollection)
        {
            AuthorizationCookieCont = cookieContainer;
            AuthorizationCookies = cookieCollection;
        }
    }
}