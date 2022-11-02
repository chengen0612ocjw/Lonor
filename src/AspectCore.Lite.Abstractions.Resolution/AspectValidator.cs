﻿using AspectCore.Lite.Abstractions.Resolution.Utils;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace AspectCore.Lite.Abstractions.Resolution
{
    public sealed class AspectValidator : IAspectValidator
    {
        private static readonly ConcurrentDictionary<MethodInfo, bool> DetectorCache;

        private readonly IInterceptorConfiguration interceptorCollection;

        static AspectValidator()
        {
            DetectorCache = new ConcurrentDictionary<MethodInfo, bool>();
        }

        public AspectValidator(IInterceptorConfiguration interceptorCollection)
        {
            this.interceptorCollection = interceptorCollection;
        }

        public bool Validate(MethodInfo method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            return DetectorCache.GetOrAdd(method, ValidateCache);
        }

        private bool ValidateCache(MethodInfo method)
        {
            var declaringType = method.DeclaringType.GetTypeInfo();

            if (!ValidateDeclaringType(declaringType) || !ValidateDeclaringMethod(method))
            {
                return false;
            }

            if (ValidateIgnoredList(method))
            {
                return false;
            }

            if (ValidateNonAspect(method) || ValidateNonAspect(declaringType))
            {
                return false;
            }

            return ValidateInterceptor(method) || ValidateInterceptor(declaringType) || ValidateInterceptor(interceptorCollection);
        }

        private bool ValidateDeclaringType(TypeInfo declaringType)
        {
            return !(declaringType.IsNotPublic || declaringType.IsValueType || declaringType.IsSealed);
        }

        private bool ValidateDeclaringMethod(MethodInfo method)
        {
            return !method.IsStatic && !method.IsFinal && method.IsVirtual && (method.IsPublic || method.IsFamily || method.IsFamilyOrAssembly);
        }

        private bool ValidateNonAspect(MemberInfo member)
        {
            return member.IsDefined(typeof(NonAspectAttribute), true);
        }

        private bool ValidateIgnoredList(MethodInfo method)
        {
            return method.IsIgnored();
        }

        private bool ValidateInterceptor(MemberInfo member)
        {
            return member.CustomAttributes.Any(data => typeof(IInterceptor).GetTypeInfo().IsAssignableFrom(data.AttributeType));
        }

        private bool ValidateInterceptor(IInterceptorConfiguration interceptorCollection)
        {
            return interceptorCollection.Any();
        }
    }
}
