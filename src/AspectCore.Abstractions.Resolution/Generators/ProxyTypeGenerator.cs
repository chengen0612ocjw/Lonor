﻿using AspectCore.Abstractions.Extensions;
using AspectCore.Abstractions.Generator;
using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace AspectCore.Abstractions.Resolution.Generators
{
    internal abstract class ProxyTypeGenerator : TypeGenerator
    {
        public virtual Type ServiceType { get; }

        public virtual IAspectValidator AspectValidator { get; }

        public override TypeAttributes TypeAttributes => TypeAttributes.Class | TypeAttributes.Public;

        public override string TypeName => $"{ServiceType.Namespace}.{ServiceType.Name}Proxy";

        protected FieldBuilder serviceInstanceFieldBuilder { get; set; }
        protected FieldBuilder serviceProviderFieldBuilder { get; set; }

        public ProxyTypeGenerator(Type serviceType, IAspectValidator aspectValidator) : base(ModuleGenerator.Default.ModuleBuilder)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }
            if (aspectValidator == null)
            {
                throw new InvalidOperationException("No service for type 'AspectCore.Abstractions.IAspectValidator' has been registered.");
            }
            if (!aspectValidator.Validate(serviceType))
            {
                throw new InvalidOperationException($"Validate '{serviceType}' failed because the type  does not satisfy the conditions of the generate proxy class.");
            }

            ServiceType = serviceType;
            AspectValidator = aspectValidator;
        }

        public override TypeInfo CreateTypeInfo()
        {
            return ModuleGenerator.Default.DefineTypeInfo(TypeName, key => Build().CreateTypeInfo());
        }

        protected override TypeBuilder ExecuteBuild()
        {
            var builder = base.ExecuteBuild();

            if (ServiceType.GetTypeInfo().IsGenericTypeDefinition)
            {
                GeneratingGenericParameter(builder);
            }

            var serviceInstanceFieldGenerator = new ProxyFieldGnerator("__serviceInstance", ServiceType, builder);
            var serviceProviderFieldGenerator = new ProxyFieldGnerator("__serviceProvider", typeof(IServiceProvider), builder);

            serviceInstanceFieldBuilder = serviceInstanceFieldGenerator.Build();
            serviceProviderFieldBuilder = serviceProviderFieldGenerator.Build();

            GeneratingConstructor(builder);
            GeneratingMethod(builder);
            GeneratingProperty(builder);

            builder.SetCustomAttribute(new CustomAttributeBuilder(typeof(NonAspectAttribute).GetTypeInfo().DeclaredConstructors.First(), EmptyArray<object>.Value));
            builder.SetCustomAttribute(new CustomAttributeBuilder(typeof(DynamicallyAttribute).GetTypeInfo().DeclaredConstructors.First(), EmptyArray<object>.Value));

            return builder;
        }

        protected abstract void GeneratingProperty(TypeBuilder declaringType);

        protected abstract void GeneratingMethod(TypeBuilder declaringType);

        protected abstract void GeneratingConstructor(TypeBuilder declaringType);

        protected virtual void GeneratingGenericParameter(TypeBuilder declaringType)
        {
            var genericArguments = ServiceType.GetTypeInfo().GetGenericArguments().Select(t => t.GetTypeInfo()).ToArray();
            var genericArgumentsBuilders = declaringType.DefineGenericParameters(genericArguments.Select(a => a.Name).ToArray());
            for (var index = 0; index < genericArguments.Length; index++)
            {
                genericArgumentsBuilders[index].SetGenericParameterAttributes(genericArguments[index].GenericParameterAttributes);
                foreach (var constraint in genericArguments[index].GetGenericParameterConstraints().Select(t => t.GetTypeInfo()))
                {
                    if (constraint.IsClass) genericArgumentsBuilders[index].SetBaseTypeConstraint(constraint.AsType());
                    if (constraint.IsInterface) genericArgumentsBuilders[index].SetInterfaceConstraints(constraint.AsType());
                }
            }
        }
    }
}
