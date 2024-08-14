﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AspectCore.Abstractions;

namespace AspectCore.Extensions.IoC
{
    internal sealed class LifetimeServiceContainer : ILifetimeServiceContainer
    {
        private readonly ICollection<ServiceDefinition> _internalCollection;

        public Lifetime Lifetime { get; }

        public LifetimeServiceContainer(ICollection<ServiceDefinition> collection, Lifetime lifetime)
        {
            _internalCollection = collection;
            Lifetime = lifetime;
        }

        public int Count => _internalCollection.Count(x => x.Lifetime == Lifetime);

        public void Add(ServiceDefinition item)
        {
            if (item.Lifetime == Lifetime)
            {
                _internalCollection.Add(item);
            }
        }

        public bool Contains(Type serviceType) => _internalCollection.Any(x => x.ServiceType == serviceType && x.Lifetime == Lifetime);

        public IEnumerator<ServiceDefinition> GetEnumerator() => _internalCollection.Where(x => x.Lifetime == Lifetime).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}