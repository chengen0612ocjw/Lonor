﻿using System;
using System.Collections.Generic;

namespace AspectCore.Injector
{
    public interface ILifetimeServiceContainer : IEnumerable<ServiceDefinition>
    {
        Lifetime Lifetime { get; }

        int Count { get; }

        void Add(ServiceDefinition item);

        bool Contains(Type serviceType);
    }
}