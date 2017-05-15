using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.r_eg.MoConTool.Extensions;

namespace net.r_eg.MoConToolTest
{
    [TestClass]
    public class ExtensionsTest
    {
        [TestMethod]
        public void GuidTest1()
        {
            string data = " MoConTool_v1 ";

            Assert.AreEqual(new Guid("{3b74ac33-8502-e58c-cfd6-47342dbfd734}"), data.Guid());
            Assert.AreEqual(new Guid("{d98c1dd4-008f-04b2-e980-0998ecf8427e}"), "".Guid());
            Assert.AreEqual(new Guid("{ef8db523-b411-2757-d335-1702515f86af}"), "  ".Guid());
            Assert.AreEqual(new Guid("{d98c1dd4-008f-04b2-e980-0998ecf8427e}"), ((string)null).Guid());
        }

        [TestMethod]
        public void EqTest1()
        {
            string data = " MoConTool_v1 ";

            Assert.AreEqual(true, data.Eq(" MoConTool_v1 "));
            Assert.AreEqual(true, data.Eq(" mocontool_V1 "));
            Assert.AreEqual(false, data.Eq("MoConTool_v1"));
            Assert.AreEqual(false, data.Eq("MoConTool"));
            Assert.AreEqual(false, data.Eq(""));
            Assert.AreEqual(false, data.Eq(null));
        }
    }
}
