using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.r_eg.MoConTool;
using net.r_eg.MoConTool.Filters;

namespace net.r_eg.MoConToolTest
{
    using LPARAM = IntPtr;
    using WPARAM = UIntPtr;

    [TestClass]
    public class BootloaderTest
    {
        [TestMethod]
        public void containerTest1()
        {
            IBootloader loader = new Bootloader();

            var ml1 = new MListener1();
            var ml2 = new MListener2() { Activated = true };
            loader.Filters.register(ml1);
            loader.Filters.register(ml2);

            Assert.AreEqual(2, loader.Filters.Count);
            Assert.AreEqual(1, loader.ActivatedFilters.Count());

            var enumerator = loader.ActivatedFilters.GetEnumerator();

            Assert.AreEqual(true, enumerator.MoveNext());
            Assert.AreEqual(ml2.Id, enumerator.Current.Id);

            loader.unregister();
            Assert.AreEqual(0, loader.Filters.Count);
            Assert.AreEqual(0, loader.ActivatedFilters.Count());
        }

        [TestMethod]
        public void configureTest1()
        {
            IBootloader loader = new Bootloader();
            loader.register();

            IMouseListener fic = loader.Filters[typeof(InterruptedClickFilter).GUID];
            IMouseListener fmc = loader.Filters[typeof(MixedClicksFilter).GUID];
            IMouseListener fdc = loader.Filters[typeof(DoubleClicksFilter).GUID];
            IMouseListener fhs = loader.Filters[typeof(HyperactiveScrollFilter).GUID];

            Assert.AreEqual(false, fic.Activated);
            Assert.AreEqual(false, fmc.Activated);
            Assert.AreEqual(false, fdc.Activated);
            Assert.AreEqual(false, fhs.Activated);

            string[] args = "-InterruptedClick LMR 4.1;5;6 -MixedClicks R 1 -DoubleClicks LR 8 -HyperactiveScroll 9;2".Split(' ');
            loader.configure(args);

            Assert.AreEqual(true, fic.Activated);
            Assert.AreEqual(true, fmc.Activated);
            Assert.AreEqual(true, fdc.Activated);
            Assert.AreEqual(true, fhs.Activated);


            Assert.AreEqual(MouseState.Flags.LMR, fic.Handler);
            Assert.AreEqual(4.1d, fic.Value);
            Assert.AreEqual((uint)5, ((InterruptedClickFilter.TData)fic.Data).deltaMin);
            Assert.AreEqual((uint)6, ((InterruptedClickFilter.TData)fic.Data).deltaMax);

            Assert.AreEqual(MouseState.Flags.Right, fmc.Handler);
            Assert.AreEqual(true, ((MixedClicksFilter.TData)fmc.Data).onlyDownCodes);

            Assert.AreEqual(MouseState.Flags.Left | MouseState.Flags.Right, fdc.Handler);
            Assert.AreEqual(8, fdc.Value);

            Assert.AreEqual(9, ((HyperactiveScrollFilter.TData)fhs.Data).capacity);
            Assert.AreEqual((uint)2, ((HyperactiveScrollFilter.TData)fhs.Data).limit);
        }

        private class MListener1: FilterAbstract, IMouseListener
        {
            public override FilterResult msg(int nCode, WPARAM wParam, LPARAM lParam)
            {
                return FilterResult.Continue;
            }

            public MListener1()
                : base("MListener1")
            {

            }
        }

        private class MListener2: FilterAbstract, IMouseListener
        {
            public override FilterResult msg(int nCode, WPARAM wParam, LPARAM lParam)
            {
                return FilterResult.Continue;
            }

            public MListener2()
                : base("MListener2")
            {

            }
        }
    }
}
