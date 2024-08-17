﻿using System;

namespace AspectCore.Abstractions
{
    public sealed class DelegateServiceDefinition : ServiceDefinition
    {
        public DelegateServiceDefinition(Type serviceType, Func<IServiceResolver, object> implementationDelegate, Lifetime lifetime, object key) : base(serviceType, lifetime, key)
        {
            ImplementationDelegate = implementationDelegate ?? throw new ArgumentNullException(nameof(implementationDelegate));
        }

        public Func<IServiceResolver, object> ImplementationDelegate { get; }
    }
}