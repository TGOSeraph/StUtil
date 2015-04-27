using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace StUtil.Core.Tests
{
    [TestClass]
    public class ReflectionTests
    {
        private class TestObject
        {
            public event EventHandler TestEvent;

            private EventHandler _explicitEvent;
            public event EventHandler ExplicitEvent
            {
                add { _explicitEvent += value; }
                remove { _explicitEvent -= value; }
            }

            private EventHandler _MyexplicitEvent;
            public event EventHandler ExplicitEvent2
            {
                add { _MyexplicitEvent += value; }
                remove { _MyexplicitEvent -= value; }
            }

            public string Name { get; set; }
            public int Value { get; set; }
            private bool triggered = false;

            private InternalObject obj = new InternalObject { Test = "aa" };

            public TestObject()
            {
                Name = "Name";
                Value = 100;
            }

            private void TriggeredHandler(object sender, EventArgs e)
            {
                Assert.IsFalse(triggered);
                triggered = true;
            }

            private void OnExplicitEvent2(EventArgs e)
            {
                if (_MyexplicitEvent != null)
                {
                    _MyexplicitEvent(this, e);
                }
            }

            public bool Test()
            {
                return true;
            }

            private string Reverse(string arg)
            {
                return string.Join("", arg.ToCharArray().Reverse());
            }

            private class InternalObject
            {
                private event EventHandler InternalEvent;

                public string Test { get; set; }
                private int Value { get; set; }

                public InternalObject()
                {
                    Value = 5;
                    Test = "Test";
                }
            }

        }

        [TestMethod]
        public void MethodInvoking()
        {
            var obj = new TestObject();

            dynamic d = new StUtil.Reflection.DynamicReflector(obj);
            //Public method
            Assert.IsTrue((bool)d.Test());
            //Private method
            Assert.AreEqual("olleh", (string)d.Reverse("hello"));
        }

        [TestMethod]
        public void ObjectCreation()
        {
            var obj = new TestObject();

            dynamic d = new StUtil.Reflection.DynamicReflector(obj) { WrapReturn = false };
            var q = d.InternalObject();
            Assert.AreEqual("InternalObject", (string)q.GetType().Name);
        }

        [TestMethod]
        public void EventHandling()
        {
            var obj = new TestObject();

            dynamic d = new StUtil.Reflection.DynamicReflector(obj);
            bool set = false;
            //Attach to the event and raise it
            var evtHandler = new EventHandler((s, e) =>
            {
                Assert.IsFalse(set);
                set = true;
            });
            d.TestEvent += evtHandler;

            d.TestEvent(obj, EventArgs.Empty);
            Assert.IsTrue(set);

            //Detatch from the event and raise it
            set = false;
            d.TestEvent -= evtHandler;
            try
            {
                d.TestEvent(obj, EventArgs.Empty);
                Assert.Fail("Invoke should fail as no handlers are attached");
            }
            catch (Exception)
            {
            }
            Assert.IsFalse(set);

            //Attach to a nested private event
            d.obj.InternalEvent += evtHandler;
            d.obj.InternalEvent(obj, EventArgs.Empty);
            Assert.IsTrue(set);

            //And detatch
            set = false;
            d.obj.InternalEvent -= evtHandler;
            try
            {
                d.obj.InternalEvent(obj, EventArgs.Empty);
                Assert.Fail("Invoke should fail as no handlers are attached");
            }
            catch (Exception)
            {
            }
            Assert.IsFalse(set);

            //Bind to an explicitly defined event
            d.ExplicitEvent += evtHandler;
            d.ExplicitEvent(obj, EventArgs.Empty);
            Assert.IsTrue(set);

            //Detatch
            set = false;
            d.ExplicitEvent -= evtHandler;
            try
            {
                d.ExplicitEvent(obj, EventArgs.Empty);
                Assert.Fail("Invoke should fail as no handlers are attached");
            }
            catch (Exception)
            {
            }
            Assert.IsFalse(set);

            //Attach to an explicitly defined event with an On method
            d.ExplicitEvent2 += evtHandler;
            d.ExplicitEvent2(obj, EventArgs.Empty);
            Assert.IsTrue(set);

            //Detatch
            set = false;
            d.ExplicitEvent2 -= evtHandler;
            d.ExplicitEvent2(obj, EventArgs.Empty);
            Assert.IsFalse(set);


            //Attach to a reflected method
            d.TestEvent += d.TriggeredHandler;
            d.TestEvent(obj, EventArgs.Empty);
            Assert.IsTrue((bool)d.triggered);

            //Detatch from the event and raise it
            d.triggered = false;
            d.TestEvent -= d.TriggeredHandler;
            try
            {
                d.TestEvent(obj, EventArgs.Empty);
                Assert.Fail("Invoke should fail as no handlers are attached");
            }
            catch (Exception)
            {
            }
            Assert.IsFalse((bool)d.triggered);
        }

        [TestMethod]
        public void MemberAccess()
        {
            var obj = new TestObject();

            dynamic d = new StUtil.Reflection.DynamicReflector(obj) { WrapReturn = false };
            Assert.AreEqual("Name", d.Name);
            Assert.AreEqual(100, d.Value);
            d.Value = 500;
            Assert.AreEqual(500, d.Value);
            d.Name = "Testing";
            Assert.AreEqual("Testing", d.Name);

            d.WrapReturn = true;
            Assert.AreEqual(500, (int)d.Value);

            try
            {
                d.Value = "Hello";
                Assert.Fail("Type mismatch");
            }
            catch (Exception)
            {
            }

            d.WrapReturn = false;
            Assert.AreEqual("InternalObject", (string)d.obj.GetType().Name);
        }

        [TestMethod]
        public void NestedPropertyAccess()
        {
            var obj = new TestObject();

            dynamic d = new StUtil.Reflection.DynamicReflector(obj) { WrapReturn = true };

            Assert.AreEqual("aa", (string)d.obj.Test);
            Assert.AreEqual(5, (int)d.obj.Value);

            d.obj.Value = 999;
            Assert.AreEqual(999, (int)d.obj.Value);
        }
    }
}
