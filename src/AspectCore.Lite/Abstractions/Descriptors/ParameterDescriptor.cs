﻿using AspectCore.Lite.Internal;
using System;
using System.Linq;
using System.Reflection;

namespace AspectCore.Lite.Abstractions
{
    public class ParameterDescriptor
    {
        private object value;
        private ParameterInfo parameterInfo;

        public ParameterDescriptor(object value, ParameterInfo parameterInfo)
        {
            ExceptionUtilities.ThrowArgumentNull(parameterInfo , nameof(parameterInfo));
            this.parameterInfo = parameterInfo;
            this.value = value;         
        }

        public string Name
        {
            get
            {
                return parameterInfo.Name;
            }
        }

        public virtual object Value
        {
            get
            {
                return value;
            }

            set
            {
                if (value == null)
                {
                    ExceptionUtilities.Throw<InvalidOperationException>(() =>
                       ParameterType.GetTypeInfo().IsValueType && !(ParameterType.GetTypeInfo().IsGenericType && ParameterType.GetTypeInfo().GetGenericTypeDefinition() == typeof(Nullable<>)) ,
                       $"object type are not equal \"{Name}\" parameter type or not a derived type of parameter type.");
                    this.value = value;
                    return;
                }

                Type valueType = value.GetType();

                ExceptionUtilities.Throw<InvalidOperationException>(() => 
                    valueType != ParameterType && !ParameterType.GetTypeInfo().IsAssignableFrom(valueType.GetTypeInfo()) ,
                    $"object type are not equal \"{Name}\" parameter type or not a derived type of parameter type.");

                this.value = value;
            }
        }

        public Type ParameterType
        {
            get
            {
                return parameterInfo.ParameterType;
            }
        }

        public ParameterInfo MetaDataInfo
        {
            get
            {
                return parameterInfo;
            }
        }

        public Attribute[] CustomAttributes
        {
            get
            {
                return parameterInfo.GetCustomAttributes().ToArray();
            }
        }
    }
}
