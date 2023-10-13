﻿using System;
using System.Reflection;
using System.Reflection.Emit;

namespace AspectCore.Core.Generator
{
    public abstract class DefaultConstructorGenerator: ConstructorGenerator
    {
        public sealed override CallingConventions CallingConventions
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public sealed override Type[] ParameterTypes
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        protected DefaultConstructorGenerator(TypeBuilder declaringMember) : base(declaringMember)
        {
        }

        protected override ConstructorBuilder ExecuteBuild()
        {
            var constructorBuilder = DeclaringMember.DefineDefaultConstructor(MethodAttributes);

            var ilGenerator = constructorBuilder.GetILGenerator();

            GeneratingConstructorBody(ilGenerator);

            return constructorBuilder;
        }
    }
}
