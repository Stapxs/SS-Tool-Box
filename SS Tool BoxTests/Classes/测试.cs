using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using SS_Tool_Box.Classes;
using System;
using System.IO;

namespace 基础模块测试
{
    [TestClass()]
    public class 报错模块测试
    {
        [TestMethod()]
        public void 报错剪切()
        {
            Error error = new Error();
            String back = error.StringCut("这是一个测试用的报错，它分为主报错内容和WPF报错内容，WPF报错内容将以在SStool_box等结构开头。");
            if(!back.Equals("这是一个测试用的报错，它分为主报错内容和WPF报错内容，WPF报错内容将以"))
            {
                Assert.Fail("剪切内容不符。");
            }
        }

        [TestMethod()]
        public void LOG写入()
        {
            Error error = new Error();
            if (!error.logWriter("测试LOG。", false))
            {
                String line;
                try
                {
                    using (StreamReader sr = new StreamReader("Log/log.log"))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            if(!(line.Equals('[') && line.Equals(']') && line.Equals("测试LOG。")))
                            {
                                Assert.Fail("写入的LOG不正确。");
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    Assert.Fail("" + ex);
                }
            }
        }
    }

    [TestClass()]
    public class 功能模块测试
    {
        [TestMethod()]
        public void 今日诗词Token测试()
        {
            bool errin = false;
            JObject obj = new JObject();
            String Token = "RgU1rBKtLym/MhhYIXs42WNoqLyZeXY3EkAcDNrcfKkzj8ILIsAP1Hx0NGhdOO1I";
            String saysuri = "https://v2.jinrishici.com/token";
            string GetJson = HttpUitls.Get(saysuri, "DEFALT", "X-User-Token", Token);
            try
            {
                obj = JObject.Parse(GetJson);
                if (obj["status"].ToString() != "success")
                {
                    errin = true;
                }
            }
            catch
            {
                Assert.Fail("获取无效。" + "内容：" + GetJson);
            }
            if(errin)
            {
                Assert.Fail("获取失败。" + "内容：" + obj["status"]);
            }
        }
    }
}