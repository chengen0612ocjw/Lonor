﻿using System;
using AspectCore.Abstractions;

namespace AspectCore.Extensions.IoC
{
    public static class ServiceResolverExtensions
    {
        public static object Resolve(this IServiceResolver serviceResolver, Type serviceType)
        {
            return serviceResolver?.Resolve(serviceType, null);
        }

        public static T Resolve<T>(this IServiceResolver serviceResolver)
        {
            return (T)serviceResolver?.Resolve(typeof(T), null);
        }

        public static T Resolve<T>(this IServiceResolver serviceResolver, object key)
        {
            return (T)serviceResolver?.Resolve(typeof(T), key);
        }
    }
}
