﻿using System;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.Abstractions;

namespace AspectCore.Core.Configuration
{
    public sealed class ServiceInterceptorAttribute : InterceptorAttribute
    {
        private readonly Type _interceptorType;

        public override bool AllowMultiple { get; } = true;

        public ServiceInterceptorAttribute(Type interceptorType)
        {
            if (interceptorType == null)
            {
                throw new ArgumentNullException(nameof(interceptorType));
            }
            if (!typeof(IInterceptor).GetTypeInfo().IsAssignableFrom(interceptorType.GetTypeInfo()))
            {
                throw new ArgumentException($"{interceptorType} is not an interceptor.", nameof(interceptorType));
            }

            _interceptorType = interceptorType;
        }

        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var instance = (IInterceptor)context.ServiceProvider.GetService(_interceptorType);
            return instance.Invoke(context, next);
        }
    }
}