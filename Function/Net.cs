using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace SS_Tool_Box
{
    /// <summary>
    /// 网络操作
    /// </summary>
    public class Net
    {
        /// <summary>
        /// 其他网络相关操作
        /// </summary>
        public class Other
        { 

            /// <summary>
            /// 获取一言
            /// </summary>
            /// <returns>一言</returns>
            public static string getHitokoto()
            {
                try
                {
                    // 初始化开始时间
                    DateTime startRun = DateTime.Now;
                    Log.AddLog("net", "正在获取 Hitokoto……");

                    // 获取信息
                    string Url = "https://v1.hitokoto.cn/?encode=json";
                    string Get = GetWebRequest(Url, "");
                    JObject jb = JObject.Parse(Get);

                    // 返回
                    Log.AddLog("net", "获取成功：" + jb["id"].ToString() + "，耗时" + DateTime.Now.Subtract(startRun).TotalSeconds + "秒");
                    return jb["hitokoto"].ToString() + " —— " + jb["from"].ToString();
                }
                catch(Exception e)
                {
                    Log.AddLog("net", "获取失败：" + e.Message);
                    return "ERR";
                }
            }

        }



        #region HTTP | GET/POST

        /// <summary>
        /// Post提交数据
        /// </summary>
        /// <param name="postUrl">URL</param>
        /// <param name="paramData">参数</param>
        /// <returns></returns>
        public static string PostWebRequest(string postUrl, string paramData)
        {
            string ret = string.Empty;
            try
            {
                if (!postUrl.StartsWith("https://"))
                    return "ERR";

                byte[] byteArray = Encoding.Default.GetBytes(paramData); //转化 /
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";

                webReq.ContentLength = byteArray.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                return "ERR";
            }
            return ret;
        }

        /// <summary>
        /// Post提交数据
        /// </summary>
        /// <param name="postUrl">URL</param>
        /// <param name="paramData">参数</param>
        /// <returns></returns>
        public static string GetWebRequest(string getUrl, string paramData)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string serviceAddress = getUrl + "/?" + paramData;
             HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceAddress);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }


        #endregion

        #region HASH | PBKDF2 HMAC SHA-256

        class PasswordHash
        {
            private const int SaltByteSize = 32;
            private const int HashByteSize = 64;
            private const int Iterations = 30000;

            private static string GetSalt()
            {
                var cryptoProvider = new RNGCryptoServiceProvider();
                byte[] b_salt = new byte[SaltByteSize];
                cryptoProvider.GetBytes(b_salt);
                return Convert.ToBase64String(b_salt);
            }

            public static string GetPasswordHash(string password, string salt)
            {

                byte[] saltBytes = Convert.FromBase64String(salt);
                byte[] derived;

                using (var pbkdf2 = new Rfc2898DeriveBytes(
                    password,
                    saltBytes,
                    Iterations,
                    HashAlgorithmName.SHA512))
                {
                    derived = pbkdf2.GetBytes(HashByteSize);
                }

                return ":pbkdf2:sha512:" + string.Format("{0}:{1}:{2}:{3}", Iterations, HashByteSize, Convert.ToBase64String(saltBytes), Convert.ToBase64String(derived));
            }

            public static bool VerifyPasswordHash(string password, string hash)
            {
                try
                {
                    string[] parts = hash.Split(new char[] { ':' });

                    byte[] saltBytes = Convert.FromBase64String(parts[2]);
                    byte[] derived;

                    int iterations = Convert.ToInt32(parts[0]);

                    using (var pbkdf2 = new Rfc2898DeriveBytes(
                        password,
                        saltBytes,
                        iterations,
                        HashAlgorithmName.SHA512))
                    {
                        derived = pbkdf2.GetBytes(HashByteSize);
                    }

                    string new_hash = string.Format("{0}:{1}:{2}", Iterations, Convert.ToBase64String(derived), Convert.ToBase64String(saltBytes));

                    return hash == new_hash;
                }
                catch
                {
                    return false;
                }
            }
        }

        #endregion

        #region Net | Others



        #endregion

    }
}
