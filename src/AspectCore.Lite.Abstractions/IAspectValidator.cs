﻿using System.Reflection;

namespace AspectCore.Lite.Abstractions
{
    [NonAspect]
    public interface IAspectValidator
    {
        bool Validate(MethodInfo method);
    }
}
