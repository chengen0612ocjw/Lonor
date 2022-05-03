﻿using AspectCore.Lite.Abstractions;
using AspectCore.Lite.Common;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspectCore.Lite.Internal
{
    internal sealed class AspectContext : IAspectContext
    {
        private readonly IServiceScope serviceScope;
        public IServiceProvider ApplicationServices { get; }
        public IServiceProvider AspectServices { get; }
        public ParameterCollection Parameters { get; set; }
        public Proxy Proxy { get; set; }
        public ParameterDescriptor ReturnParameter { get; set; }
        public Target Target { get; set; }

        private bool dispose = false;

        public AspectContext(IServiceProvider serviceProvider)
        {
            ExceptionHelper.ThrowArgumentNull(serviceProvider , nameof(serviceProvider));
            ApplicationServices = serviceProvider;
            serviceScope = ApplicationServices.GetRequiredService<IServiceScope>();
            AspectServices = serviceScope.ServiceProvider;
        }

        public void Dispose()
        {
            if (!dispose)
            {
                serviceScope.Dispose();
                dispose = true;
            }
        }
    }
}