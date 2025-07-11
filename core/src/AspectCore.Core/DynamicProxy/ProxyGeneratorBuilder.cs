﻿using System;
using System.Collections.Generic;
using AspectCore.Configuration;
using AspectCore.Injector;

namespace AspectCore.DynamicProxy
{
    [NonAspect]
    public sealed class ProxyGeneratorBuilder
    {
        private readonly IAspectConfiguration _configuration;
        private readonly IServiceContainer _serviceContainer;

        public ProxyGeneratorBuilder()
        {
            _configuration = new AspectConfiguration();
            _serviceContainer = new ServiceContainer(_configuration);
        }

        public ProxyGeneratorBuilder Configure(Action<IAspectConfiguration> options)
        {
            options?.Invoke(_configuration);
            return this;
        }

        public ProxyGeneratorBuilder ConfigureService(Action<IServiceContainer> options)
        {
            options?.Invoke(_serviceContainer);
            return this;
        }

        public IProxyGenerator Build()
        {
            var serviceResolver = _serviceContainer.Build();
            return new DisposedProxyGenerator(serviceResolver);
        }

        private class EqualityComparer : IEqualityComparer<IInterceptorSelector>
        {
            public bool Equals(IInterceptorSelector x, IInterceptorSelector y)
            {
                if (x == null || y == null) return false;
                return x.GetType().Equals(y.GetType());
            }

            public int GetHashCode(IInterceptorSelector obj)
            {
                return obj.GetType().GetHashCode();
            }
        }
    }
}