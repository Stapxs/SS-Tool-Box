using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS_Tool_Box.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}