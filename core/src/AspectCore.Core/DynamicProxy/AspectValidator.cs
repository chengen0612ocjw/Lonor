﻿using System.Linq;
using System.Reflection;
using AspectCore.Extensions.Reflection;

namespace AspectCore.DynamicProxy
{
    [NonAspect]
    public sealed class AspectValidator : IAspectValidator
    {
        private readonly AspectValidationDelegate _aspectValidationDelegate;

        public AspectValidator(AspectValidationDelegate aspectValidationDelegate)
        {
            _aspectValidationDelegate = aspectValidationDelegate;
        }

        public bool Validate(MethodInfo method)
        {
            if (method == null)
            {
                return false;
            }

            if (_aspectValidationDelegate(method))
            {
                return true;
            }

            var declaringTypeInfo = method.DeclaringType.GetTypeInfo();
            if (!declaringTypeInfo.IsClass)
            {
                return false;
            }

            foreach (var interfaceTypeInfo in declaringTypeInfo.GetInterfaces().Select(x => x.GetTypeInfo()))
            {
                var interfaceMethod = interfaceTypeInfo.GetMethod(new MethodSignature(method));
                if (interfaceMethod != null)
                {
                    if (Validate(interfaceMethod))
                    {
                        return true;
                    }
                }
            }

            var baseType = declaringTypeInfo.BaseType;
            if (baseType == typeof(object) || baseType == null)
            {
                return false;
            }

            var baseMethod = baseType.GetTypeInfo().GetMethod(new MethodSignature(method));
            return baseMethod != null && Validate(baseMethod);
        }
    }
}