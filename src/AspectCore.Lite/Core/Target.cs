﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.Lite.Core.Descriptors;

namespace AspectCore.Lite.Core
{
    public sealed class Target : IMethodInvoker
    {
        public MethodInfo Method { get; }
        public Type ServiceType { get; }
        public Type ImplementationType { get; }
        public object Instance { get; }

        internal Target(MethodInfo method , Type serviceType , Type implementationType , object implementationInstance)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));

            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));

            if (implementationType == null)
                throw new ArgumentNullException(nameof(implementationType));

            if (implementationInstance == null)
                throw new ArgumentNullException(nameof(implementationInstance));

            Method = method;
            ServiceType = serviceType;
            ImplementationType = implementationType;
            Instance = implementationInstance;
        }

        public object Invoke(ParameterCollection parameterCollection)
        {
            object[] args = parameterCollection.Select(p => p.Value).ToArray();
            return Method.Invoke(Instance , args); throw new NotImplementedException();
        }
    }
}