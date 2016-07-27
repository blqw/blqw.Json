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
            MEF.Import(typeof(Components));
        }
        
        [Import("CreateGetter")]
        public static Func<MemberInfo, Func<object, object>> GetGeter;

        [Import("CreateSetter")]
        public static Func<MemberInfo, Action<object, object>> GetSeter;

    }
}
