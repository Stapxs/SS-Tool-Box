using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS_Tool_Box.Classes.Helper;
using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace SS_Tool_Box.Tests
{
    [TestClass()]
    public class NetTests
    {
        [TestMethod()]
        public void PingTestTest()
        {
            try
            {
                string ip = null;
                IPAddress[] ipadd = Dns.GetHostAddresses("stapxs.cn");
                for (int i = 0; i <= ipadd.Count() - 1; i++)
                {
                    ip = ipadd[i].ToString();
                }
                NetHelper netHelper = new NetHelper();
                PingReply rep = netHelper.PingTest(ip);
                Assert.AreEqual(rep.Status, IPStatus.Success);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Assert.Fail();
            }
        }
    }
}
