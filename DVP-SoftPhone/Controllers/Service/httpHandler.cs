using System;
using System.IO;
using System.Net;

using System.Text;

using DuoSoftware.DuoTools.DuoLogger;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using Alchemy.Classes;

namespace DuoSoftware.DuoSoftPhone.Controllers.Service
{
    public static class HttpHandler
    {
        public static void CallService(Uri uri, string data, string method, string authHeader, string callSessionId,string state)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault,string.Format("FreezeAcw - Method Call : {0},state : {1}" ,callSessionId,state), Logger.LogLevel.Info);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);

            request.Method = method.ToUpper();
            request.ContentType = "application/json";
            request.Headers["Authorization"] = "Bearer " + authHeader;
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] bytes = encoding.GetBytes(data);

            request.ContentLength = bytes.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                // Send the data.
                requestStream.Write(bytes, 0, bytes.Length);
            }

            request.BeginGetResponse((x) =>
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {

                    if (response.StatusCode != HttpStatusCode.OK) throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                    response.StatusDescription));

                    Stream stream1 = response.GetResponseStream();
                    StreamReader sr = new StreamReader(stream1);
                    string strsb = sr.ReadToEnd();
                    //object objResponse = JsonConvert.DeserializeObject(strsb, JSONResponseType);

                }

                //using (HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(x))
                //{

                //    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Response));
                //    var reply = ser.ReadObject(response.GetResponseStream()) as Response;
                //    Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("FreezeAcw - Method Call : {0},state : {1} , {2}", callSessionId, state, reply), Logger.LogLevel.Info);

                //}
            }, null);
        }

        public static void MakeRequestAsyn(string requestUrl, string authHeader, object JSONRequest, string JSONmethod)//, Type JSONResponseType)
        {

            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;

                request.Headers["Authorization"] = authHeader;
                //WebRequest WR = WebRequest.Create(requestUrl);   

                request.Method = JSONmethod.ToUpper();
                request.ContentType = "application/json";

                if (JSONRequest != null)
                {
                    var sb = JsonConvert.SerializeObject(JSONRequest);
                    var bt = Encoding.UTF8.GetBytes(sb);
                    Stream st = request.GetRequestStream();
                    st.Write(bt, 0, bt.Length);
                    st.Close();
                }

                request.BeginGetResponse((x) =>
                {
                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {

                        if (response.StatusCode != HttpStatusCode.OK) throw new Exception(String.Format(
                            "Server error (HTTP {0}: {1}).", response.StatusCode,
                        response.StatusDescription));

                        Stream stream1 = response.GetResponseStream();
                        StreamReader sr = new StreamReader(stream1);
                        string strsb = sr.ReadToEnd();
                        
                    }
                },null);


                
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "Service Call", exception, Logger.LogLevel.Error);
                
            }
        } 

        public static object MakeRequest(string requestUrl, string authHeader, object JSONRequest, string JSONmethod)//, Type JSONResponseType)
        {

            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;

                request.Headers["Authorization"] = authHeader;
                //WebRequest WR = WebRequest.Create(requestUrl);   

                request.Method = JSONmethod.ToUpper();
                request.ContentType = "application/json";

                if (JSONRequest != null)
                {
                    var sb = JsonConvert.SerializeObject(JSONRequest);
                    var bt = Encoding.UTF8.GetBytes(sb);
                    Stream st = request.GetRequestStream();
                    st.Write(bt, 0, bt.Length);
                    st.Close();
                }

                
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {

                    if (response.StatusCode != HttpStatusCode.OK) throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                    response.StatusDescription));

                    Stream stream1 = response.GetResponseStream();
                    StreamReader sr = new StreamReader(stream1);
                    string strsb = sr.ReadToEnd();
                    //object objResponse = JsonConvert.DeserializeObject(strsb, JSONResponseType);

                    return strsb;
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "Service Call", exception, Logger.LogLevel.Error);
                return null;
            }
        } 


        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static void GetResponse(Uri uri, string authHeader, Action<Response> callback)
        {
            var wc = new WebClient();
            wc.Headers["Authorization"] = authHeader;
            wc.OpenReadCompleted += (o, a) =>
            {
                if (callback == null) return;
                var ser = new DataContractJsonSerializer(typeof(Response));
                callback(ser.ReadObject(a.Result) as Response);
            };
            wc.OpenReadAsync(uri);
        }

        public static void GetPostResponse(Uri uri,string authHeader,string data, Action<Response> callback)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers["Authorization"] = authHeader;

            var encoding = new UTF8Encoding();
            var bytes = encoding.GetBytes(data);

            request.ContentLength = bytes.Length;

            using (var requestStream = request.GetRequestStream())
            {
                // Send the data.
                requestStream.Write(bytes, 0, bytes.Length);
            }

            request.BeginGetResponse((x) =>
            {
                using (var response = (HttpWebResponse)request.EndGetResponse(x))
                {
                    if (callback == null) return;
                    var ser = new DataContractJsonSerializer(typeof(Response));
                    callback(ser.ReadObject(response.GetResponseStream()) as Response);
                }
            }, null);
        }
    }
}
