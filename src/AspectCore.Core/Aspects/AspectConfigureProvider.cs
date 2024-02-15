﻿using AspectCore.Abstractions;
using AspectCore.Core.Internal;

namespace AspectCore.Core
{
    [NonAspect]
    public sealed class AspectConfigureProvider : IAspectConfigureProvider
    {
        public IAspectConfigure AspectConfigure { get; }

        public AspectConfigureProvider(AspectCoreOptions aspectCoreOptions)
        {
            aspectCoreOptions.NonAspectOptions
              .AddObjectVMethod().AddSystem().AddAspNetCore().AddEntityFramework().AddOwin().AddPageGenerator();
            AspectConfigure = new AspectConfigure(aspectCoreOptions.InterceptorFactories, aspectCoreOptions.NonAspectOptions);
        }
    }
}
