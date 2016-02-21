using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace blqw.JsonComponent
{
    class Component
    {
        public static bool IsInitialized { get; } = Initialize();

        private static bool Initialize()
        {
            MEFPart.Import(typeof(Component));
            return true;
        }

        /// <summary> 包装反射对象
        /// </summary>
        [Import("MemberInfoWrapper")]
        public static readonly Func<MemberInfo, MemberInfo> Wrapper = m => m;
        
        /// <summary> 获取动态类型
        /// </summary>
        [Import("GetDynamic")]
        public static readonly Func<object, dynamic> GetDynamic;


        /// <summary> 获取转换器
        /// </summary>
        [Import("GetConverter")]
        public static readonly Func<Type, bool, IFormatterConverter> GetConverter = (type, @throw) => {
            return new InnerComverter(@throw);
        };


        /// <summary> 获取转换器
        /// </summary>
        [Import()]
        public static readonly IFormatterConverter Converter = new InnerComverter(true);

        [Import("CreateGetter")]
        public static Func<MemberInfo, Func<object, object>> GetGeter;

        [Import("CreateSetter")]
        public static Func<MemberInfo, Action<object, object>> GetSeter;

    }
}
