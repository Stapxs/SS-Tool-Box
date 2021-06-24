using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS_Tool_Box.Pages.Tools;
using System;

namespace SS_Tool_Box.Tests
{
    [TestClass]
    public class ToolHelperTests
    {
        /// <summary>
        /// 使用 Type 对象创建页面
        /// </summary>
        [TestMethod]
        public void CreatePage()
        {
            Assert.AreNotEqual(Activator.CreateInstance(typeof(ColorCard)), null);
        }
    }
}