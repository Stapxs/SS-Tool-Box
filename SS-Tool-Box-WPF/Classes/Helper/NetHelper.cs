using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SS_Tool_Box.Classes.Helper
{
    class NetHelper
    {
        /// <summary>
        /// Ping 测试
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public PingReply PingTest(string ip)
        {
            PingReply reply = null;
            Ping pingSender = null;
            try
            {
                pingSender = new Ping();

                PingOptions options = new PingOptions();
                options.DontFragment = true;

                string data = "hello world";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 1000;

                IPAddress ipa = IPAddress.Parse(ip);
                PingReply replyPing = pingSender.Send(ip, timeout, buffer, options);
                reply = replyPing;
            }
            catch (Exception ex)
            {
                Log.AddLog("ping", "ping IP 失败：" + ex);
                reply = null;
            }
            finally
            {
                pingSender.Dispose();
            }
            return reply;
        }

        /// <summary>
        /// HTTP 请求
        /// </summary>
        public class HttpUitls
        {
            public string Get(string Url, string ContentType)
            {
                GC.Collect();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Proxy = null;
                request.KeepAlive = false;
                request.Method = "GET";
                if (ContentType.Equals("DEFALT"))      //垃圾html post轮子不支持自定义Content-Type
                {
                    request.ContentType = "application/json; charset=UTF-8";
                }
                else
                {
                    request.ContentType = ContentType;
                }
                request.AutomaticDecompression = DecompressionMethods.GZip;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                string retString = myStreamReader.ReadToEnd();

                myStreamReader.Close();
                myResponseStream.Close();

                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }

                return retString;
            }

            public string Get(string Url)
            {
                try
                {
                    GC.Collect();
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                    request.Proxy = null;
                    request.KeepAlive = false;
                    request.Method = "GET";
                    request.ContentType = "application/json; charset=UTF-8";
                    request.AutomaticDecompression = DecompressionMethods.GZip;

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream myResponseStream = response.GetResponseStream();
                    StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                    string retString = myStreamReader.ReadToEnd();

                    myStreamReader.Close();
                    myResponseStream.Close();

                    if (response != null)
                    {
                        response.Close();
                    }
                    if (request != null)
                    {
                        request.Abort();
                    }

                    return retString;
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }

            public string Get(string Url, string ContentType, string HeanderName, string HeanderString)
            {
                try
                {
                    GC.Collect();
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                    request.Proxy = null;
                    request.KeepAlive = false;
                    request.Method = "GET";
                    SetHeaderValue(request.Headers, HeanderName, HeanderString);
                    if (ContentType.Equals("DEFALT"))      //垃圾html post轮子不支持自定义Content-Type
                    {
                        request.ContentType = "application/json; charset=UTF-8";
                    }
                    else
                    {
                        request.ContentType = ContentType;
                    }
                    request.AutomaticDecompression = DecompressionMethods.GZip;

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream myResponseStream = response.GetResponseStream();
                    StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                    string retString = myStreamReader.ReadToEnd();

                    myStreamReader.Close();
                    myResponseStream.Close();

                    if (response != null)
                    {
                        response.Close();
                    }
                    if (request != null)
                    {
                        request.Abort();
                    }

                    return retString;
                }
                catch (Exception e)
                {
                    return "Err - " + e.Message;
                }
            }

            public string Post(string Url, string Data, string Referer, string ContentType)
            {
                GC.Collect();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "POST";
                request.Referer = Referer;
                byte[] bytes = Encoding.UTF8.GetBytes(Data);
                if (ContentType.Equals("DEFALT"))      //垃圾html post轮子不支持自定义Content-Type
                {
                    request.ContentType = "application/x-www-form-urlencoded";
                }
                else
                {
                    request.ContentType = ContentType;
                }
                request.ContentLength = bytes.Length;
                Stream myResponseStream = request.GetRequestStream();
                myResponseStream.Write(bytes, 0, bytes.Length);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader myStreamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string retString = myStreamReader.ReadToEnd();

                myStreamReader.Close();
                myResponseStream.Close();

                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
                return retString;
            }

            public string Post(string Url, string Data, string ContentType, string HeanderName, string HeanderString)
            {
                GC.Collect();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "POST";
                SetHeaderValue(request.Headers, HeanderName, HeanderString);
                byte[] bytes = Encoding.UTF8.GetBytes(Data);
                if (ContentType.Equals("DEFALT"))      //垃圾html post轮子不支持自定义Content-Type
                {
                    request.ContentType = "application/x-www-form-urlencoded";
                }
                else
                {
                    request.ContentType = ContentType;
                }
                request.ContentLength = bytes.Length;
                Stream myResponseStream = request.GetRequestStream();
                myResponseStream.Write(bytes, 0, bytes.Length);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader myStreamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string retString = myStreamReader.ReadToEnd();

                myStreamReader.Close();
                myResponseStream.Close();

                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
                return retString;
            }

            private static void SetHeaderValue(WebHeaderCollection header, string name, string value)
            {
                PropertyInfo property = typeof(WebHeaderCollection).GetProperty("InnerCollection", BindingFlags.Instance | BindingFlags.NonPublic);
                if (property != null)
                {
                    NameValueCollection collection = property.GetValue(header, null) as NameValueCollection;
                    collection[name] = value;
                }
            }
        }
    }
}
