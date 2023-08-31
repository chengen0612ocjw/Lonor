﻿using System;
using System.Linq;

namespace AspectCore.Abstractions.Internal
{
    public sealed class DefaultAspectContext : AspectContext
    {
        private IServiceProvider _serviceProvider;
        private DynamicDictionary _data;
        private bool _disposedValue = false;

        public override IServiceProvider ServiceProvider
        {
            get
            {
                if (_serviceProvider == null)
                {
                    throw new NotImplementedException("The current context does not support IServiceProvider.");
                }

                return _serviceProvider;
            }
        }

        public override DynamicDictionary Data { get { return _data ?? (_data = new DynamicDictionary()); } }

        public override ParameterCollection Parameters
        {
            get;
        }

        public override ParameterDescriptor ReturnParameter
        {
            get;
        }

        public override TargetDescriptor Target
        {
            get;
        }

        public override ProxyDescriptor Proxy
        {
            get;
        }

        public DefaultAspectContext(IServiceProvider provider, TargetDescriptor target, ProxyDescriptor proxy, ParameterCollection parameters, ReturnParameterDescriptor returnParameter)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }
            if (proxy == null)
            {
                throw new ArgumentNullException(nameof(proxy));
            }
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            if (returnParameter == null)
            {
                throw new ArgumentNullException(nameof(returnParameter));
            }

            var originalServiceProvider = provider as IOriginalServiceProvider;
            _serviceProvider = originalServiceProvider ?? (IServiceProvider)provider?.GetService(typeof(IOriginalServiceProvider));
            Target = target;
            Proxy = proxy;
            Parameters = parameters;
            ReturnParameter = returnParameter;
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposedValue)
            {
                return;
            }

            if (!disposing)
            {
                return;
            }

            if (_data == null)
            {
                return;
            }

            foreach (var key in _data.Keys.ToArray())
            {
                object value = null;

                _data.TryGetValue(key, out value);

                var disposable = value as IDisposable;

                disposable?.Dispose();

                _data.Remove(key);
            }

            _disposedValue = true;
        }
    }
}