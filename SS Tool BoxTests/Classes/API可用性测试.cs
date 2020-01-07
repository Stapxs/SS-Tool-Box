using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS_Tool_Box.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 基础模块测试
{
    [TestClass()]
    public class API可用性测试
    {
        [TestMethod()]
        public void 一言API()
        {
            try
            {
                String saysuri = "https://v1.hitokoto.cn/";
                string GetJson = HttpUitls.Get(saysuri);
                if (GetJson.IndexOf("hitokoto") == -1)
                {
                    Assert.Fail("发现错误（MAN - 001）：获取一言内容为空。");
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("" + ex);
            }
        }
    }
}
