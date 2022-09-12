﻿using System.Reflection.Emit;

namespace AspectCore.Lite.Generators
{
    public abstract class MethodBodyGenerator : MetaDataGenerator<MethodBuilder, ILGenerator>
    {
        public MethodBodyGenerator(MethodBuilder declaringMember) : base(declaringMember)
        {
        }

        protected override ILGenerator Accept(GeneratorVisitor visitor)
        {
            var ilGenerator = DeclaringMember.GetILGenerator();
            GeneratingMethodBody(ilGenerator);
            return ilGenerator;
        }

        protected abstract void GeneratingMethodBody(ILGenerator ilGenerator);
    }
}
