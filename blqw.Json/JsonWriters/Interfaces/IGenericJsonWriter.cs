﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable
{
    public interface IGenericJsonWriter : IJsonWriter
    {
        IJsonWriter MakeType(Type type);
    }
}
