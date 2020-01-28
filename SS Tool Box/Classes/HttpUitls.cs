using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SS_Tool_Box.Classes
{
    public class HttpUitls
    {
        public static string Get(string Url, string ContentType)
        {
            //System.GC.Collect();
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

        public static string Post(string Url, string Data, string Referer, string ContentType)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.Referer = Referer;
            byte[] bytes = Encoding.UTF8.GetBytes(Data);
            if(ContentType.Equals("DEFALT"))      //垃圾html post轮子不支持自定义Content-Type
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

    }
}