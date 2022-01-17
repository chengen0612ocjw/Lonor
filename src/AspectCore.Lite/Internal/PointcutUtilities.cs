﻿using AspectCore.Lite.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using static AspectCore.Lite.Internal.DefaultPointcut;

namespace AspectCore.Lite.Internal
{
    internal static class PointcutUtilities
    {
        private readonly static TypeInfo InterceptorTypeInfo = typeof(IInterceptor).GetTypeInfo();
        private readonly static ConcurrentDictionary<MethodInfo, bool> pointcutCache = new ConcurrentDictionary<MethodInfo, bool>();
        private readonly static IPointcut virtualMethodPointcut = new VirtualMethodPointcut();
        private readonly static IPointcut interfacePointcut = new InterfacePointcut();

        internal static bool IsMemberMatch(MethodInfo method, TypeInfo declaringTypeInfo)
        {
            if (declaringTypeInfo.CustomAttributes.Any(data => IsAssignableFrom(data.AttributeType)))
            {
                return true;
            }
            return method.CustomAttributes.Any(data => IsAssignableFrom(data.AttributeType));
        }

        private static bool IsAssignableFrom(Type attributeType) => InterceptorTypeInfo.IsAssignableFrom(attributeType);

        internal static bool IsMatchCache(MethodInfo method, Func<MethodInfo, bool> vauleFactory) => pointcutCache.GetOrAdd(method, vauleFactory);

        internal static IPointcut GetPointcut(TypeInfo typeInfo)
        {
            if (typeInfo.IsClass)
            {
                return virtualMethodPointcut;
            }
            else if (typeInfo.IsInterface)
            {
                return interfacePointcut;
            }

            throw new ArgumentException("type must be interface or class", nameof(typeInfo));
        }
    }
}
