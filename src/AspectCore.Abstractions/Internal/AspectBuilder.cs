﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspectCore.Abstractions.Extensions;

namespace AspectCore.Abstractions.Internal
{
    public sealed class AspectBuilder : IAspectBuilder
    {
        private readonly IList<Func<AspectDelegate, AspectDelegate>> delegates;

        public AspectBuilder()
        {
            delegates = new List<Func<AspectDelegate, AspectDelegate>>();
        }

        public void AddAspectDelegate(Func<AspectContext, AspectDelegate, Task> interceptorInvoke)
        {
            if (interceptorInvoke == null)
            {
                throw new ArgumentNullException(nameof(interceptorInvoke));
            }
            delegates.Add(next => context => interceptorInvoke(context, next));
        }

        public Func<Func<object>, AspectDelegate> Build()
        {
            return targetInvoke => Build(targetInvoke);
        }

        public AspectDelegate Build(Func<object> targetInvoke)
        {
            if (targetInvoke == null)
            {
                throw new ArgumentNullException(nameof(targetInvoke));
            }
            AspectDelegate invoke = context =>
            {
                context.ReturnParameter.Value = targetInvoke();
                return TaskCache.CompletedTask;
            };
            var count = delegates.Count;
            for (var i = count - 1; i > -1; i--)
            {
                invoke = delegates[i](invoke);
            }
            return invoke;
        }
    }
}
