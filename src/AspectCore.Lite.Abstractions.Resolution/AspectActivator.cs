﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
#if NET451
using Nito.AsyncEx;
#endif

namespace AspectCore.Lite.Abstractions.Resolution
{
    public class AspectActivator : IAspectActivator
    {

#region Aspect metaData

        private Type serviceType;
        private MethodInfo serviceMethod;
        private MethodInfo targetMethod;
        private MethodInfo proxyMethod;

#endregion

#region Dependency injection

        private readonly IServiceProvider serviceProvider;
        private readonly IAspectBuilder aspectBuilder;
        private readonly IInterceptorMatcher interceptorMatcher;
        private readonly IInterceptorInjector interceptorInjector;

#endregion

        public AspectActivator(IServiceProvider serviceProvider,
            IAspectBuilder aspectBuilder, IInterceptorMatcher interceptorMatcher, IInterceptorInjector interceptorInjector)
        {
            this.serviceProvider = serviceProvider;
            this.aspectBuilder = aspectBuilder;
            this.interceptorMatcher = interceptorMatcher;
            this.interceptorInjector = interceptorInjector;
        }

        public void InitializeMetaData(Type serviceType, MethodInfo serviceMethod, MethodInfo targetMethod, MethodInfo proxyMethod)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }
            if (serviceMethod == null)
            {
                throw new ArgumentNullException(nameof(serviceMethod));
            }
            if (targetMethod == null)
            {
                throw new ArgumentNullException(nameof(targetMethod));
            }
            if (proxyMethod == null)
            {
                throw new ArgumentNullException(nameof(proxyMethod));
            }

            this.serviceType = serviceType;
            this.serviceMethod = serviceMethod;
            this.targetMethod = targetMethod;
            this.proxyMethod = proxyMethod;
        }

        public T Invoke<T>(object targetInstance, object proxyInstance, params object[] paramters)
        {
#if NET451
            return AsyncContext.Run(() => InvokeAsync<T>(targetInstance, proxyInstance, paramters));
#else
            return InvokeAsync<T>(targetInstance, proxyInstance, paramters).GetAwaiter().GetResult();
#endif
        }

        public Task<T> InvokeAsync<T>(object targetInstance, object proxyInstance, params object[] paramters)
        {
            var parameters = new ParameterCollection(paramters, serviceMethod.GetParameters());
            var returnParameter = new ReturnParameterDescriptor(default(T), serviceMethod.ReturnParameter);
            var targetDescriptor = new TargetDescriptor(targetInstance, serviceMethod, serviceType, targetMethod, targetInstance.GetType());
            var proxyDescriptor = new ProxyDescriptor(proxyInstance, proxyMethod, proxyInstance.GetType());

            var context = CreateAspectContext(serviceProvider, targetDescriptor, proxyDescriptor, parameters, returnParameter);

            var interceptors = interceptorMatcher.Match(serviceMethod, serviceType.GetTypeInfo());

            foreach (var interceptor in interceptors)
            {
                interceptorInjector.Inject(interceptor);
                aspectBuilder.AddAspectDelegate(interceptor.Invoke);
            }

            return TryInvoke<T>(aspectBuilder, context, interceptors);
        }

        protected virtual AspectContext CreateAspectContext(IServiceProvider serviceProvider, TargetDescriptor target, ProxyDescriptor proxy, ParameterCollection parameters, ReturnParameterDescriptor returnParameter)
        {
            return new DefaultAspectContext(serviceProvider, target, proxy, parameters, returnParameter);
        }

        private async Task<T> TryInvoke<T>(IAspectBuilder aspectBuilder, AspectContext context, IEnumerable<IInterceptor> interceptors)
        {
            try
            {
                await aspectBuilder.Build(() => context.Target.Invoke(context.Parameters))(context);
                return await ConvertReturnVaule<T>(context.ReturnParameter.Value);
            }
            finally
            {
                foreach (var disposable in interceptors.OfType<IDisposable>())
                {
                    disposable.Dispose();
                }
            }
        }

        private async Task<T> ConvertReturnVaule<T>(object value)
        {
            if (value is Task<T>)
            {
                return await (Task<T>)value;
            }
            else if (value is Task)
            {
                await (Task)value;
                return default(T);
            }
            else
            {
                return (T)value;
            }
        }

    }
}
