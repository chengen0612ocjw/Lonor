﻿using System;

namespace AspectCore.Lite.DynamicProxy
{
    public interface IProxyFactory
    {
        IServiceProvider ServiceProvider { get; }

        object CreateProxy(Type serviceType, Type implementationType, object implementationInstance, params object[] args);
    }
}
