﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AspectCore.Lite.Abstractions.Aspects
{
    public interface IProxy<TService, TImplementation>
    {
        TService GetProxyInstance();

        MemberInfo GetProxyMemberInfo();

        TypeInfo GetProxyTypeInfo();
    }
}
