using Microsoft.VisualStudio.TestTools.UnitTesting;
using blqw.Serializable;
using System;
using System.Collections.Generic;
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
            var a = JsonWriterContainer.Get(typeof(TypeCode));
            Assert.AreEqual(typeof(TypeCode), a.Type);
            Assert.IsNotInstanceOfType(a, typeof(MyTestJsonWriter));
            JsonWriterContainer.Set(new MyTestJsonWriter(null));
            a = JsonWriterContainer.Get(typeof(TypeCode));
            Assert.AreEqual(typeof(TypeCode), a.Type);
            Assert.IsInstanceOfType(a, typeof(MyTestJsonWriter));

            a = JsonWriterContainer.Get(typeof(AttributeTargets));
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
            var a = JsonWriterContainer.Get(typeof(int?));
            Assert.AreEqual(typeof(int?), a.Type);
            JsonWriterContainer.Set(new MyTest2JsonWriter(null));
            a = JsonWriterContainer.Get(typeof(int?));
            Assert.IsInstanceOfType(a, typeof(MyTest2JsonWriter));
            a = JsonWriterContainer.Get(typeof(long?));
            Assert.AreEqual(typeof(long?), a.Type);
            Assert.IsNotInstanceOfType(a, typeof(MyTest2JsonWriter));
        }

        class MyTest3JsonWriter : IGenericJsonWriter
        {
            public MyTest3JsonWriter(Type type)
            {
                Type = type;
            }
            public Type Type { get; } 

            public IJsonWriter MakeType(Type type)
            {
                return new MyTest3JsonWriter(type);
            }

            public void Write(object obj, JsonWriterArgs args)
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void 测试泛型替换功能2()
        {
            var a = JsonWriterContainer.Get(typeof(int?));
            Assert.AreEqual(typeof(int?), a.Type);
            Assert.IsNotInstanceOfType(a, typeof(MyTest3JsonWriter));

            a = JsonWriterContainer.Get(typeof(long?));
            Assert.AreEqual(typeof(long?), a.Type);
            Assert.IsNotInstanceOfType(a, typeof(MyTest3JsonWriter));

            JsonWriterContainer.Set(new MyTest3JsonWriter(typeof(Nullable<>)));

            a = JsonWriterContainer.Get(typeof(int?));
            Assert.AreEqual(typeof(int?), a.Type);
            Assert.IsInstanceOfType(a, typeof(MyTest3JsonWriter));

            a = JsonWriterContainer.Get(typeof(long?));
            Assert.AreEqual(typeof(long?), a.Type);
            Assert.IsInstanceOfType(a, typeof(MyTest3JsonWriter));

        }

    }
}