using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PrizeBondChecker.Core.CoreUtilities
{
    public static class HttpWebRequestHelper
    {
        private static string FormContentType = "application/x-www-form-urlencoded";
        private static string XMLContentType = "text/xml";
        private static string POST = "POST";
        private static string GET = "GET";
        private static string PUT = "PUT";

        private static string FormatPostParameters(List<KeyValuePair<string, string>> ArrayParameters)
        {
            var sb = new StringBuilder();
            if (ArrayParameters != null)
            {
                foreach (KeyValuePair<string, string> key in ArrayParameters)
                {
                    sb.Append(HttpUtility.UrlEncode(key.Key) + "[]=" + HttpUtility.UrlEncode(key.Value) + "&");
                }
            }

            if (sb.Length > 0)
                sb.Length = sb.Length - 1;

            return sb.ToString();
        }

        private static string FormatPostParameters(Dictionary<string, string> parameters)
        {
            var sb = new StringBuilder();
            if (parameters != null)
            {
                foreach (string key in parameters.Keys)
                {
                    sb.Append(HttpUtility.UrlEncode(key) + "=" + HttpUtility.UrlEncode(parameters[key]) + "&");
                }
            }
            if (sb.Length > 0)
                sb.Length = sb.Length - 1;

            return sb.ToString();
        }

        private static string FormatJsonPostParameters(Dictionary<string, string> parameters)
        {
            var sb = new StringBuilder();
            if (parameters != null)
            {
                sb.Append("{");
                foreach (string key in parameters.Keys)
                {
                    sb.Append("'" + key + "':'" + parameters[key] + "',");
                }
                if (sb.Length > 0)
                    sb.Length = sb.Length - 1;
                sb.Append("}");
            }
            return sb.ToString();
        }


        public static HttpWebResponse SendGetRequest(string url, Dictionary<string, string> parameters, List<KeyValuePair<string, string>> ArrayParameter = null)
        {
            if (parameters != null)
            {
                url += "?" + FormatPostParameters(parameters);
            }

            if (ArrayParameter != null)
            {
                if (parameters == null) { url += "?"; } else { url += "&"; }
                url += FormatPostParameters(ArrayParameter);
            }
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = FormContentType;
            httpWebRequest.Timeout = (60 * 1000) * 10;
            httpWebRequest.Method = GET;
            Task<WebResponse> responseTask = Task.Factory.FromAsync<WebResponse>(httpWebRequest.BeginGetResponse,
                httpWebRequest.EndGetResponse, null);
            return (HttpWebResponse)responseTask.Result;
        }



        public static HttpWebResponse SendFormPostRequest(string url, Dictionary<string, string> parameters)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = FormContentType;
            httpWebRequest.Timeout = (60 * 1000) * 10;
            httpWebRequest.Method = POST;

            string postData = FormatPostParameters(parameters);

            byte[] requestBytes = Encoding.UTF8.GetBytes(postData);

            httpWebRequest.ContentLength = requestBytes.Length;

            using (Stream requestStream = httpWebRequest.GetRequestStream())
            {
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();
            }

            Task<WebResponse> responseTask = Task.Factory.FromAsync<WebResponse>(httpWebRequest.BeginGetResponse,
                httpWebRequest.EndGetResponse, null);

            return (HttpWebResponse)responseTask.Result;
        }

        public static HttpWebResponse SendJsonPostRequest(string url, string postData, Dictionary<string, string> headers = null)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Timeout = (60 * 1000) * 10;
            httpWebRequest.Method = POST;

            if (headers != null)
            {
                foreach (string key in headers.Keys)
                {
                    httpWebRequest.Headers.Add(key, headers[key]);
                }
            }

            byte[] requestBytes = Encoding.UTF8.GetBytes(postData);

            httpWebRequest.ContentLength = requestBytes.Length;

            using (Stream requestStream = httpWebRequest.GetRequestStream())
            {
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();
            }

            Task<WebResponse> responseTask = Task.Factory.FromAsync<WebResponse>(httpWebRequest.BeginGetResponse,
                httpWebRequest.EndGetResponse, null);

            return (HttpWebResponse)responseTask.Result;
        }

        public static HttpWebResponse SendJsonPostRequest(string url, Dictionary<string, string> parameters, Dictionary<string, string> headers = null)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Timeout = (60 * 1000) * 10;
            httpWebRequest.Method = POST;

            if (headers != null)
            {
                foreach (string key in headers.Keys)
                {
                    httpWebRequest.Headers.Add(key, headers[key]);
                }
            }

            var postData = FormatJsonPostParameters(parameters);

            byte[] requestBytes = Encoding.UTF8.GetBytes(postData);

            httpWebRequest.ContentLength = requestBytes.Length;

            using (Stream requestStream = httpWebRequest.GetRequestStream())
            {
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();
            }

            Task<WebResponse> responseTask = Task.Factory.FromAsync<WebResponse>(httpWebRequest.BeginGetResponse,
                httpWebRequest.EndGetResponse, null);

            return (HttpWebResponse)responseTask.Result;
        }


        public static HttpWebResponse SendJsonPutRequest(string url, string postData, Dictionary<string, string> headers = null)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Timeout = (60 * 1000) * 10;
            httpWebRequest.Method = PUT;

            if (headers != null)
            {
                foreach (string key in headers.Keys)
                {
                    httpWebRequest.Headers.Add(key, headers[key]);
                }
            }

            //var postData = FormatJsonPostParameters(parameters);

            byte[] requestBytes = Encoding.UTF8.GetBytes(postData);

            httpWebRequest.ContentLength = requestBytes.Length;

            using (Stream requestStream = httpWebRequest.GetRequestStream())
            {
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();
            }

            Task<WebResponse> responseTask = Task.Factory.FromAsync<WebResponse>(httpWebRequest.BeginGetResponse,
                httpWebRequest.EndGetResponse, null);

            return (HttpWebResponse)responseTask.Result;
        }

        public static HttpWebResponse SendJsonPutRequest(string url, Dictionary<string, string> parameters, Dictionary<string, string> headers = null)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Timeout = (60 * 1000) * 10;
            httpWebRequest.Method = PUT;

            if (headers != null)
            {
                foreach (string key in headers.Keys)
                {
                    httpWebRequest.Headers.Add(key, headers[key]);
                }
            }

            var postData = FormatJsonPostParameters(parameters);

            byte[] requestBytes = Encoding.UTF8.GetBytes(postData);

            httpWebRequest.ContentLength = requestBytes.Length;

            using (Stream requestStream = httpWebRequest.GetRequestStream())
            {
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();
            }

            Task<WebResponse> responseTask = Task.Factory.FromAsync<WebResponse>(httpWebRequest.BeginGetResponse,
                httpWebRequest.EndGetResponse, null);

            return (HttpWebResponse)responseTask.Result;
        }


        public static HttpWebResponse SendJsonGetRequest(string url, Dictionary<string, string> parameters, Dictionary<string, string> headers = null)
        {
            if (parameters != null)
            {
                url += "?" + FormatPostParameters(parameters);
            }

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Timeout = (60 * 1000) * 10;
            httpWebRequest.Method = GET;
            if (headers != null)
            {
                foreach (string key in headers.Keys)
                {
                    httpWebRequest.Headers.Add(key, headers[key]);
                }
            }

            Task<WebResponse> responseTask = Task.Factory.FromAsync<WebResponse>(httpWebRequest.BeginGetResponse,
                httpWebRequest.EndGetResponse, null);

            return (HttpWebResponse)responseTask.Result;
        }

        public static HttpWebResponse SendNonFormPostRequest(string data, string url)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            httpWebRequest.ContentType = XMLContentType;
            httpWebRequest.Method = POST;
            httpWebRequest.KeepAlive = false;
            httpWebRequest.Timeout = (60 * 1000) * 10;

            using (var sw = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                sw.WriteLine(data);
                sw.Close();
            }


            Task<WebResponse> responseTask = Task.Factory.FromAsync<WebResponse>(httpWebRequest.BeginGetResponse,
                httpWebRequest.EndGetResponse, null);

            return (HttpWebResponse)responseTask.Result;

        }

        public static string GetHttpWebResponseData(HttpWebResponse response)
        {
            string data = string.Empty;
            if (response != null)
            {
                using (var incomingStreamReader = new StreamReader(response.GetResponseStream()))
                {
                    data = incomingStreamReader.ReadToEnd();
                    incomingStreamReader.Close();
                    response.GetResponseStream().Close();
                }
            }
            return data;
        }

    }
}
