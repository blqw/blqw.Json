using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace blqw.IOC
{
    class Components
    {

        public Components()
        {
            MEFLite.Import(typeof(Components));
        }

        /// <summary> 包装反射对象
        /// </summary>
        [Import("MemberInfoWrapper")]
        public static readonly Func<MemberInfo, MemberInfo> Wrapper = m => m;
        
        [Import("CreateGetter")]
        public static Func<MemberInfo, Func<object, object>> GetGeter;

        [Import("CreateSetter")]
        public static Func<MemberInfo, Action<object, object>> GetSeter;

    }
}
