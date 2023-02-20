﻿using AspectCore.Lite.Abstractions.Attributes;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace AspectCore.Lite.Abstractions.Resolution
{
    public sealed class InterceptorInjector : IInterceptorInjector
    {
        private static readonly ConcurrentDictionary<PropertyInfo, Action<IInterceptor, object>> PropertySetterCache =
            new ConcurrentDictionary<PropertyInfo, Action<IInterceptor, object>>();

        private readonly IServiceProvider serviceProvider;

        public InterceptorInjector(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void Inject(IInterceptor interceptor)
        {
            if (interceptor == null)
            {
                throw new ArgumentNullException(nameof(interceptor));
            }

            var properties = interceptor.GetType().GetTypeInfo().DeclaredProperties.Where(x => x.CanWrite && x.IsDefined(typeof(InjectedAttribute)));

            if (!properties.Any())
            {
                return;
            }

            foreach (var property in properties)
            {
                property.SetValue(interceptor, serviceProvider.GetService(property.PropertyType));
            }
        }
    }
}
