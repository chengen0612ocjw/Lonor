﻿using System;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using AspectCore.Lite.Internal;

namespace AspectCore.Lite.Generators
{
    public sealed class InterfaceProxyGenerator: ProxyGenerator
    {
        private readonly Type[] interfaceTypes;
        public InterfaceProxyGenerator(IServiceProvider serviceProvider, Type serviceType, params Type[] impInterfaceTypes) :
            base(serviceProvider, serviceType, impInterfaceTypes)
        {
            ExceptionUtilities.ThrowArgument(() => !serviceType.GetTypeInfo().IsInterface , $"Type {serviceType} should be interface." , nameof(serviceType));
            interfaceTypes = new Type[] { serviceType }.Concat(impInterfaceTypes).ToArray();
        }

        public override Type GenerateProxyType()
        {
            return moduleGenerator.DefinedType(serviceType, key =>
            {
                return GenerateProxyTypeInfo(key).AsType();
            });
        }

        protected override TypeBuilder GenerateTypeBuilder()
        {
            return moduleGenerator.CurrentModuleBuilder.DefineType($"{serviceType.Namespace}.{GeneratorConstants.Interface}{serviceType.Name}", TypeAttributes.Class | TypeAttributes.Public, typeof(object), interfaceTypes);
        }

        private TypeInfo GenerateProxyTypeInfo(Type key)
        {
            
            var serviceProviderGenerator = new FieldGenerator(TypeBuilder, typeof(IServiceProvider), GeneratorConstants.ServiceProvider);
            var serviceInstanceGenerator = new FieldGenerator(TypeBuilder, key, GeneratorConstants.ServiceInstance);

            var constructorGenerator = new InterfaceConstructorGenerator(TypeBuilder, key, serviceProviderGenerator, serviceInstanceGenerator);

            constructorGenerator.GenerateConstructor();

            foreach(var impType in interfaceTypes)
            {
                GenerateInterfaceProxy(impType , serviceProviderGenerator , serviceInstanceGenerator);
            }

            return TypeBuilder.CreateTypeInfo();
        }
    }
}
