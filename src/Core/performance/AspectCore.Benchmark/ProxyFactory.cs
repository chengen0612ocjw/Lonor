﻿using System;
using System.Collections.Generic;
using System.Text;
using AspectCore.Abstractions;
using AspectCore.Core;
using AspectCore.Core.DynamicProxy;

namespace AspectCore.Benchmark
{
    class ProxyFactory
    {
        static ProxyFactory()
        {
            var aspectConfigureProvider = AspectConfigureProvider.Instance;
            var handlers = new List<IAspectValidationHandler>
            {
                new AccessibleAspectValidationHandler(),
                new AttributeAspectValidationHandler(),
                new CacheAspectValidationHandler(),
                new ConfigureAspectValidationHandler(aspectConfigureProvider),
                new DynamicallyAspectValidationHandler(),
                new NonAspectValidationHandler()
            };
            AspectConfigureProvider.AddValidationHandlers(handlers);
        }

        private static IAspectValidatorBuilder CreateValidatorBuilder()
        {
            var builder = new AspectValidatorBuilder(AspectConfigureProvider.Instance);
            return builder;
        }

        public static IAspectActivatorFactory CreateActivatorFactory()
        {
            var serviceProvider = new ServiceResolver();
            var interceptorSelectors = new List<IInterceptorSelector>();
            interceptorSelectors.Add(new ConfigureInterceptorSelector(AspectConfigureProvider.Instance, serviceProvider));
            interceptorSelectors.Add(new MethodInterceptorSelector());
            interceptorSelectors.Add(new TypeInterceptorSelector());
            return new AspectActivatorFactory(new AspectContextFactory(serviceProvider), new AspectBuilderFactory(new InterceptorCollector(interceptorSelectors, new PropertyInjectorFactory(serviceProvider))));
        }

        public static IAspectBuilderFactory CreateAspectBuilderFactory()
        {
            var serviceProvider = new ServiceResolver();
            var interceptorSelectors = new List<IInterceptorSelector>();
            interceptorSelectors.Add(new ConfigureInterceptorSelector(AspectConfigureProvider.Instance, serviceProvider));
            interceptorSelectors.Add(new MethodInterceptorSelector());
            interceptorSelectors.Add(new TypeInterceptorSelector());
            return new AspectBuilderFactory(new InterceptorCollector(interceptorSelectors, new PropertyInjectorFactory(serviceProvider)));
        }

        public static T CreateProxy<T>(T target)
        {
            var generator = new ProxyGenerator(CreateValidatorBuilder());
            var proxyType = generator.CreateInterfaceProxyType(typeof(T), target.GetType());
            return (T)Activator.CreateInstance(proxyType, CreateActivatorFactory(), target);
        }

    }

    class ServiceResolver : IServiceProvider, IServiceResolver
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public object GetService(Type serviceType)
        {
            return null;
        }

        public object Resolve(Type serviceType)
        {
            throw new NotImplementedException();
        }
    }
}
