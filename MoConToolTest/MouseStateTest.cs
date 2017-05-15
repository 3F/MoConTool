using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.r_eg.MoConTool.Filters;
using net.r_eg.MoConTool.WinAPI;

namespace net.r_eg.MoConToolTest
{
    [TestClass]
    public class MouseStateTest
    {
        [TestMethod]
        public void ExtractTest1()
        {
            Assert.AreEqual(MouseState.Flags.None, MouseState.Extract(""));
            Assert.AreEqual(MouseState.Flags.Left, MouseState.Extract("L"));
            Assert.AreEqual(MouseState.Flags.Middle, MouseState.Extract("M"));
            Assert.AreEqual(MouseState.Flags.Right, MouseState.Extract("R"));

            Assert.AreEqual(MouseState.Flags.Left | MouseState.Flags.Middle | MouseState.Flags.Right, MouseState.Extract("RLM"));
            Assert.AreEqual(MouseState.Flags.LMR, MouseState.Extract("lmr"));
            Assert.AreEqual(MouseState.Flags.Left | MouseState.Flags.Middle, MouseState.Extract("LM"));
            Assert.AreEqual(MouseState.Flags.Left | MouseState.Flags.Right, MouseState.Extract("LR"));
            Assert.AreEqual(MouseState.Flags.Middle | MouseState.Flags.Right, MouseState.Extract("MR"));
        }

        [TestMethod]
        public void ExtractTest2()
        {
            Assert.AreEqual(MouseState.Flags.Left | MouseState.Flags.Down, MouseState.Extract(SysMessages.WM_LBUTTONDOWN));
            Assert.AreEqual(MouseState.Flags.Left | MouseState.Flags.Up, MouseState.Extract(SysMessages.WM_LBUTTONUP));

            Assert.AreEqual(MouseState.Flags.Middle | MouseState.Flags.Down, MouseState.Extract(SysMessages.WM_MBUTTONDOWN));
            Assert.AreEqual(MouseState.Flags.Middle | MouseState.Flags.Up, MouseState.Extract(SysMessages.WM_MBUTTONUP));

            Assert.AreEqual(MouseState.Flags.Right | MouseState.Flags.Down, MouseState.Extract(SysMessages.WM_RBUTTONDOWN));
            Assert.AreEqual(MouseState.Flags.Right | MouseState.Flags.Up, MouseState.Extract(SysMessages.WM_RBUTTONUP));
        }
    }
}
