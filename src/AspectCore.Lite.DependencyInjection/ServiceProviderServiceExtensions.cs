﻿using AspectCore.Lite.Common;
using AspectCore.Lite.DependencyInjection.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspectCore.Lite.DependencyInjection
{
    public static class ServiceProviderServiceExtensions
    {
        public static object GetOriginalService(this IServiceProvider provider, Type serviceType)
        {
            ExceptionHelper.ThrowArgumentNull(provider , nameof(provider));

            var supportOriginalServiceProvider = provider as ISupportOriginalService;
            if (supportOriginalServiceProvider != null)
            {
                supportOriginalServiceProvider.GetService(serviceType);
            }

            var proxyProvider = provider as ProxyServiceProvider;
            if (proxyProvider != null)
            {
                supportOriginalServiceProvider = provider.GetRequiredService<ISupportOriginalService>();
                return supportOriginalServiceProvider.GetService(serviceType);
            }

            return provider.GetService(serviceType);
        }

        public static T GetOriginalService<T>(this IServiceProvider provider)
        {
            return (T) GetOriginalService(provider, typeof(T));
        }

        public static object GetProxyService(this IServiceProvider provider, Type serviceType)
        {
            ExceptionHelper.ThrowArgumentNull(provider , nameof(provider));
            var supportProxyService = provider.GetRequiredService<ISupportProxyService>();
            return supportProxyService.GetService(serviceType, provider.GetOriginalService(serviceType));
        }

        public static T GetProxyService<T>(this IServiceProvider provider)
        {
            return (T) GetProxyService(provider, typeof(T));
        }
    }
}
