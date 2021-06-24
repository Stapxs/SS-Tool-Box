using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace SS_Tool_Box.Tests
{
    [TestClass()]
    public class OtherTests
    {
        [TestMethod()]
        public void ReplacingToast()
        {
            new ToastContentBuilder()
                .AddText("只是个单元测试")
                .AddText("正在摸鱼中……")
                .Show(toast =>
                {
                    toast.Tag = "Test";
                    toast.Group = "SSTB";
                });

            Thread.Sleep(5000);

            new ToastContentBuilder()
                .AddText("只是个单元测试")
                .AddText("被发现了！")
                .Show(toast =>
                {
                    toast.Tag = "Test";
                    toast.Group = "SSTB";
                });
        }
    }
}
