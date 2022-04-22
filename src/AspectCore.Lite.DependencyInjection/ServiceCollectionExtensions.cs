﻿using AspectCore.Lite.Abstractions;
using AspectCore.Lite.Common;
using AspectCore.Lite.DependencyInjection.Internal;
using AspectCore.Lite.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AspectCore.Lite.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAspectLite(this IServiceCollection services)
        {
            ExceptionHelper.ThrowArgumentNull(services, nameof(services));

            var aspectService = ServiceCollectionHelper.CreateAspectLiteServices();
            aspectService.ForEach(services.TryAdd);
            services.AddTransient<ISupportOriginalService>(provider =>
            {
                var proxyServiceProvider = provider as ProxyServiceProvider;
                return new SupportOriginalService(proxyServiceProvider?.originalServiceProvider ?? provider);
            });
            services.Replace(ServiceDescriptor.Transient<IProxyActivator, ServiceProxyActivator>());
            services.AddTransient<IServiceScope, ServiceScope>();
            services.AddTransient<IServiceScopeFactory, ServiceScopeFactory>();
            services.AddTransient<ISupportProxyService, SupportProxyService>();
            services.AddTransient<IProxyServiceProvider, ProxyServiceProvider>();
            services.AddSingleton<IProxyMemorizer, ProxyMemorizer>();
            services.TryAddSingleton(services);

            return services;
        }
    }
}