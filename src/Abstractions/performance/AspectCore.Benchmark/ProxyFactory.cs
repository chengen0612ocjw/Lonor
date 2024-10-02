﻿using System;
using System.Collections.Generic;
using System.Text;
using AspectCore.Abstractions;
using AspectCore.Core;

namespace AspectCore.Benchmark
{
    class ProxyFactory
    {
        static ProxyFactory()
        {
            var aspectConfigureProvider = AspectConfigureProvider.Instance;
            var handlers = new List<IAspectValidationHandler>();
            handlers.Add(new AccessibleAspectValidationHandler());
            handlers.Add(new AttributeAspectValidationHandler());
            handlers.Add(new CacheAspectValidationHandler());
            handlers.Add(new ConfigureAspectValidationHandler(aspectConfigureProvider));
            handlers.Add(new DynamicallyAspectValidationHandler());
            handlers.Add(new NonAspectValidationHandler());
            AspectConfigureProvider.AddValidationHandlers(handlers);
        }
        private static IAspectValidatorBuilder CreateValidatorBuilder()
        {
            var builder = new AspectValidatorBuilder(AspectConfigureProvider.Instance);
            return builder;
        }

        private static IAspectActivatorFactory CreateActivatorFactory()
        {
            var serviceProvider = new ServiceProvider();
            var interceptorSelectors = new List<IInterceptorSelector>();
            interceptorSelectors.Add(new ConfigureInterceptorSelector(AspectConfigureProvider.Instance, serviceProvider));
            interceptorSelectors.Add(new MethodInterceptorSelector());
            interceptorSelectors.Add(new TypeInterceptorSelector());
            return new AspectActivatorFactory(new AspectContextFactory(serviceProvider), new AspectBuilderFactory(new InterceptorProvider(interceptorSelectors, new InterceptorInjectorProvider(serviceProvider, new PropertyInjectorSelector()))));
        }

        public static T CreateProxy<T>(T target)
        {
            var generator = new ProxyGenerator(CreateValidatorBuilder());
            var proxyType = generator.CreateInterfaceProxyType(typeof(T), target.GetType());
            return (T)Activator.CreateInstance(proxyType, CreateActivatorFactory(), target);
        }

    }

    class ServiceProvider:IServiceProvider
    {
        public object GetService(Type serviceType)
        {
            return null;
        }
    }
}
