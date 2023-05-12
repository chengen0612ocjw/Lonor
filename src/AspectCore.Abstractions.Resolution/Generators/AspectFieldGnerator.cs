﻿using AspectCore.Abstractions.Generator;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace AspectCore.Abstractions.Resolution.Generators
{
    internal class AspectFieldGenerator : FieldGenerator
    {
        public AspectFieldGenerator(string name, Type fieldType, TypeBuilder declaringMember) : base(declaringMember)
        {
            FieldName = name;
            FieldType = fieldType;
        }

        public override FieldAttributes FieldAttributes
        {
            get
            {
                return FieldAttributes.Private;
            }
        }

        public override string FieldName { get; }

        public override Type FieldType { get; }
    }
}
