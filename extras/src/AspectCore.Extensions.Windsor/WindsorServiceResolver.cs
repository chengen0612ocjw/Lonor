﻿using System;
using AspectCore.DynamicProxy;
using AspectCore.Injector;
using Castle.MicroKernel;
using Castle.Windsor;

namespace AspectCore.Extensions.Windsor
{
    [NonAspect]
    public sealed class WindsorServiceResolver : IServiceResolver
    {
        private readonly IKernelInternal _kernel;

        public WindsorServiceResolver(IWindsorContainer windsorContainer)
        {
            _kernel = windsorContainer?.Kernel as IKernelInternal;
            if (_kernel == null)
                throw new ArgumentException(string.Format("The kernel must implement {0}", typeof(IKernelInternal)));
        }

        public void Dispose()
        {
            var d = _kernel as IDisposable;
            d?.Dispose();
        }

        public object GetService(Type serviceType)
        {
            return Resolve(serviceType);
        }

        public object Resolve(Type serviceType)
        {
            if (_kernel.LoadHandlerByType(null, serviceType, null) != null)
                return _kernel.Resolve(serviceType);
            return null;
        }
    }
}