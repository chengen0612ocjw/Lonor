﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AspectCore.Lite.Generators
{
    public class PropertyGenerator
    {
        private readonly TypeBuilder typeBuilder;
        private readonly PropertyInfo propertyInfo;
        private readonly FieldGenerator serviceInstanceGenerator;

        protected internal PropertyGenerator(TypeBuilder typeBuilder, PropertyInfo propertyInfo, FieldGenerator serviceInstanceGenerator)
        {
            this.typeBuilder = typeBuilder;
            this.propertyInfo = propertyInfo;
            this.serviceInstanceGenerator = serviceInstanceGenerator;
        }

        public void GenerateProperty()
        { 
            var property = typeBuilder.DefineProperty(propertyInfo.Name, PropertyAttributes.None, propertyInfo.PropertyType, Type.EmptyTypes);

            if (propertyInfo.CanRead)
            {
                var methodGenerator = new InterfaceMethodGenerator(typeBuilder, propertyInfo.GetMethod, serviceInstanceGenerator);
                methodGenerator.GenerateMethod();
                property.SetGetMethod(methodGenerator.MethodBuilder);
            }

            if (propertyInfo.CanWrite)
            {
                var methodGenerator = new InterfaceMethodGenerator(typeBuilder, propertyInfo.SetMethod, serviceInstanceGenerator);
                methodGenerator.GenerateMethod();
                property.SetSetMethod(methodGenerator.MethodBuilder);
            }
        }
    }
}
