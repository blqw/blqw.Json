using Microsoft.VisualStudio.TestTools.UnitTesting;
using blqw.Serializable;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable.Tests
{
    [TestClass()]
    public class JsonWriterContainerTests
    {
        class MyTestJsonWriter : IJsonWriter
        {
            public MyTestJsonWriter(object obj)
            {

            }
            public Type Type { get; } = typeof(TypeCode);

            public void Write(object obj, JsonWriterArgs args)
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod()]
        public void 测试替换功能()
        {
            var container = JsonWriterContainer.Instance;
            var a = container.GetWriter(typeof(TypeCode));
            Assert.AreEqual(typeof(TypeCode), a.Type);
            Assert.IsNotInstanceOfType(a, typeof(MyTestJsonWriter));
            container.AddService(typeof(TypeCode), new MyTestJsonWriter(null));
            a = container.GetWriter(typeof(TypeCode));
            Assert.AreEqual(typeof(TypeCode), a.Type);
            Assert.IsInstanceOfType(a, typeof(MyTestJsonWriter));

            a = container.GetWriter(typeof(AttributeTargets));
            Assert.AreEqual(typeof(AttributeTargets), a.Type);
            Assert.IsNotInstanceOfType(a, typeof(MyTestJsonWriter));
        }

        class MyTest2JsonWriter : IJsonWriter
        {
            public MyTest2JsonWriter(object obj)
            {

            }
            public Type Type { get; } = typeof(int?);

            public void Write(object obj, JsonWriterArgs args)
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod()]
        public void 测试泛型替换功能()
        {
            var container = JsonWriterContainer.Instance;
            var a = container.GetWriter(typeof(int?));
            Assert.AreEqual(typeof(int?), a.Type);
            container.AddService(typeof(int?), new MyTest2JsonWriter(null));
            a = container.GetWriter(typeof(int?));
            Assert.IsInstanceOfType(a, typeof(MyTest2JsonWriter));
            a = container.GetWriter(typeof(long?));
            Assert.AreEqual(typeof(long?), a.Type);
            Assert.IsNotInstanceOfType(a, typeof(MyTest2JsonWriter));
            container.RemoveService(typeof(int?)); a = container.GetWriter(typeof(int?));
            Assert.AreEqual(typeof(int?), a.Type);
            Assert.IsNotInstanceOfType(a, typeof(MyTest2JsonWriter));
        }

        class MyTest3JsonWriter : IGenericJsonWriter
        {
            public MyTest3JsonWriter(Type type)
            {
                Type = type;
            }
            public Type Type { get; }

            public object GetService(Type serviceType)
            {
                return new MyTest3JsonWriter(serviceType);
            }

            public void Write(object obj, JsonWriterArgs args)
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void 测试泛型替换功能2()
        {
            var container = JsonWriterContainer.Instance;
            var a = container.GetWriter(typeof(int?));
            Assert.AreEqual(typeof(int?), a.Type);
            Assert.IsNotInstanceOfType(a, typeof(MyTest3JsonWriter));

            a = container.GetWriter(typeof(long?));
            Assert.AreEqual(typeof(long?), a.Type);
            Assert.IsNotInstanceOfType(a, typeof(MyTest3JsonWriter));

            container.AddService(typeof(Nullable<>), new MyTest3JsonWriter(typeof(Nullable<>)));

            a = container.GetWriter(typeof(int?));
            Assert.AreEqual(typeof(int?), a.Type);
            Assert.IsInstanceOfType(a, typeof(MyTest3JsonWriter));

            a = container.GetWriter(typeof(long?));
            Assert.AreEqual(typeof(long?), a.Type);
            Assert.IsInstanceOfType(a, typeof(MyTest3JsonWriter));

            container.RemoveService(typeof(Nullable<>));

            a = container.GetWriter(typeof(int?));
            Assert.AreEqual(typeof(int?), a.Type);
            Assert.IsNotInstanceOfType(a, typeof(MyTest3JsonWriter));
            a = container.GetWriter(typeof(long?));
            Assert.AreEqual(typeof(long?), a.Type);
            Assert.IsNotInstanceOfType(a, typeof(MyTest3JsonWriter));
        }

    }
}