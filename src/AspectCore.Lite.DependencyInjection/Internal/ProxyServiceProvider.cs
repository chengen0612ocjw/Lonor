﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspectCore.Lite.DependencyInjection.Internal
{
    internal class ProxyServiceProvider : IServiceProvider
    {
        internal readonly IServiceProvider originalServiceProvider;

        internal ProxyServiceProvider(IServiceProvider serviceProvider)
        {
            originalServiceProvider = serviceProvider;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (serviceType == typeof(IServiceProvider))
            {
                return this;
            }

            var resolvedService = originalServiceProvider.GetService(serviceType);

            if (resolvedService == null)
            {
                return null;
            }

            return null;
        }
    }
}
