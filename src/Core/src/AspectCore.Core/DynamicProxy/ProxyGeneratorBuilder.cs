﻿using System;
using System.Collections.Generic;
using AspectCore.Configuration;
using AspectCore.Injector;

namespace AspectCore.DynamicProxy
{
    public sealed class ProxyGeneratorBuilder
    {
        private readonly IAspectConfiguration _configuration;
        private readonly HashSet<IInterceptorSelector> _selectors;
        private readonly IServiceContainer _serviceContainer;

        public ProxyGeneratorBuilder()
        {
            _configuration = new AspectConfiguration();
            _serviceContainer = new ServiceContainer();
            _selectors = new HashSet<IInterceptorSelector>(new EqualityComparer());
            _selectors.Add(new ConfigureInterceptorSelector(_configuration));
            _selectors.Add(new TypeInterceptorSelector());
            _selectors.Add(new MethodInterceptorSelector());
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

        public ProxyGeneratorBuilder UseSelector(IInterceptorSelector interceptorSelector)
        {
            if (interceptorSelector == null)
            {
                throw new ArgumentNullException(nameof(interceptorSelector));
            }
            _selectors.Add(interceptorSelector);
            return this;
        }

        public IProxyGenerator Build()
        {     
            var serviceResolver = _serviceContainer.Build();
            var validatorBuilder = new AspectValidatorBuilder(_configuration);
            var proxyTypeGenerator = new ProxyTypeGenerator(validatorBuilder);
            var injectorFactory = new PropertyInjectorFactory(serviceResolver);
            var collector = new InterceptorCollector(_selectors, injectorFactory);
            var builderFactory = new AspectBuilderFactory(collector);
            var contextFactory = new AspectContextFactory(serviceResolver);
            var activatorFactory = new AspectActivatorFactory(contextFactory, builderFactory);
            return new ProxyGenerator(proxyTypeGenerator, activatorFactory);
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