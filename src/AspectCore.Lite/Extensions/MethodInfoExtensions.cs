﻿using AspectCore.Lite.Internal;
using System.Reflection;
using System.Threading.Tasks;

namespace AspectCore.Lite.Extensions
{
    internal static class MethodInfoExtensions
    {
        internal static bool IsReturnTask(this MethodInfo methodInfo)
        {
            ExceptionUtilities.ThrowArgumentNull(methodInfo , nameof(methodInfo));

            return typeof(Task).GetTypeInfo().IsAssignableFrom(methodInfo.ReturnType);
        }
    }
}