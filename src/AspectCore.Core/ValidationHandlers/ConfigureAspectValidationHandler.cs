﻿using System.Linq;
using System.Reflection;
using AspectCore.Core.Internal;

namespace AspectCore.Abstractions.Internal
{
    public class ConfigureAspectValidationHandler : IAspectValidationHandler
    {
        private readonly IAspectConfigureProvider _aspectConfigureProvider;

        public ConfigureAspectValidationHandler(IAspectConfigureProvider aspectConfigureProvider)
        {
            _aspectConfigureProvider = aspectConfigureProvider;
        }

        public int Order { get; } = 13;

        public bool Invoke(MethodInfo method, AspectValidationDelegate next)
        {
            if (!method.IsPropertyBinding())
            {
                if (_aspectConfigureProvider.AspectConfigure.InterceptorFactories.Any(x => x.Predicate(method)))
                {
                    return true;
                }
            }

            return next(method);
        }
    }
}
