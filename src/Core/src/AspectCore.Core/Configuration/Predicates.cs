﻿using System;

namespace AspectCore.Configuration
{
    public static class Predicates
    {
        public static AspectPredicate ForNameSpace(string nameSpace)
        {
            if (nameSpace == null)
            {
                throw new ArgumentNullException(nameof(nameSpace));
            };

            return method => method.DeclaringType.Namespace.Matches(nameSpace);
        }

        public static AspectPredicate ForService(string service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            return method =>
            {
                if (method.DeclaringType.Name.Matches(service))
                {
                    return true;
                }
                var declaringType = method.DeclaringType;
                var fullName = declaringType.FullName ?? $"{declaringType.Name}.{declaringType.Name}";
                return fullName.Matches(service);
            };
        }

        public static AspectPredicate ForMethod(string method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            return methodInfo => methodInfo.Name.Matches(method);
        }

        public static AspectPredicate ForMethod(string service, string method)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            return methodInfo => ForService(service)(methodInfo) && methodInfo.Name.Matches(method);
        }
    }
}
