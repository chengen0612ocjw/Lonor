﻿using AspectCore.Lite.Abstractions;
using System;
using System.Reflection;

namespace AspectCore.Lite.Internal
{
    internal class VirtualMethodPointcut : IPointcut
    {
        public bool IsMatch(MethodInfo method)
        {
            return PointcutUtilities.IsMatchCache(method, IsMatchCache);
        }

        private bool IsMatchCache(MethodInfo method)
        {
            if (method == null) return false;
            TypeInfo declaringTypeInfo = method.DeclaringType.GetTypeInfo();
            if (!declaringTypeInfo.IsClass || declaringTypeInfo.IsSealed) return false;
            if (method.IsStatic || method.IsPrivate || !method.IsVirtual) return false;
            return PointcutUtilities.IsMemberMatch(method, declaringTypeInfo);
        }
    }
}
