﻿using AspectCore.Lite.Test.Fakes;
using AspectCore.Lite.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

#if NETCOREAPP1_0
using Microsoft.AspNetCore.Testing;
#endif

namespace AspectCore.Lite.Test.Abstractions.Descriptors
{
    public class ParameterCollectionTest
    {
#if NETCOREAPP1_0
        [Fact]
        public void Ctor_ThrowsArgumentNullException()
        {
            ExceptionAssert.ThrowsArgumentNull(() => { ParameterCollection collection = new ParameterCollection(null, Array.Empty<ParameterInfo>()); }, "parameters");
            ExceptionAssert.ThrowsArgumentNull(() => { ParameterCollection collection = new ParameterCollection(Array.Empty<object>(), null); }, "parameterInfos");
        }

                [Fact]
        public void Ctor_ThrowsArgumentException()
        {
            ExceptionAssert.ThrowsArgument(() =>
            {
                ParameterCollection collection = new ParameterCollection(new object[] { null }, Array.Empty<ParameterInfo>());
            },
            null , "The number of parameters must equal the number of parameterInfos.");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public void IndexedProperty_Index_With_0_ThrowsArgumentOutOfRangeException(int index)
        {
            ParameterCollection collection = new ParameterCollection(Array.Empty<object>(), Array.Empty<ParameterInfo>());
            ExceptionAssert.ThrowsArgumentOutOfRange(() => { object entry = collection[index]; }, nameof(index), "index value out of range.");
        }

        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void IndexedProperty_Name_Get_ThrowsArgumentNullException(string name)
        {
            ParameterCollection collection = new ParameterCollection(new object[] { "L", 0, null, null, }, MeaninglessService.Parameters);
            ExceptionAssert.ThrowsArgumentNull(() => { var value = collection[name]; }, nameof(name));
        }


        [Theory]
        [InlineData("age")]
        [InlineData("Name")]
        public void IndexedProperty_Name_Get_ThrowsKeyNotFoundException(string name)
        {
            ParameterCollection collection = new ParameterCollection(new object[] { "L", 0, null, null, }, MeaninglessService.Parameters);
            ExceptionAssert.Throws<KeyNotFoundException>(() => { var value = collection[name]; }, $"Does not exist the parameter nameof \"{name}\".");
        }
#endif


        [Fact]
        public void IndexedProperty_Index_Get()
        {
            ParameterCollection collection = new ParameterCollection(new object[] { "L" , 0 , null , null , } , MeaninglessService.Parameters);
            Assert.Equal(collection[0].Name , "name");
            Assert.Equal(collection[0].Value , "L");
            Assert.Equal(collection[0].ParameterType , typeof(string));
        }



        [Theory]
        [InlineData("name")]
        public void IndexedProperty_Name(string name)
        {
            ParameterCollection collection = new ParameterCollection(new object[] { "L" , 0 , null , null , } , MeaninglessService.Parameters);
            Assert.Equal(collection[name].Name , "name");
            Assert.Equal(collection[name].Value , "L");
            Assert.Equal(collection[name].ParameterType , typeof(string));
        }


        [Fact]
        public void GetEnumerator()
        {
            ParameterCollection collection = new ParameterCollection(new object[] { "L" , 0 , null , null , } , MeaninglessService.Parameters);

            foreach (var entry in ((IEnumerable<ParameterDescriptor>)collection))
            {
                Assert.IsType(typeof(ParameterDescriptor) , entry);
            }

            foreach (var entry in ((IEnumerable)collection))
            {
                Assert.IsType(typeof(ParameterDescriptor) , entry);
            }
        }

        [Theory]
        [InlineData("name" , "L")]
        public void Count(string name , object value)
        {
            ParameterCollection collection = new ParameterCollection(new object[] { "L" , 0 , null , null , } , MeaninglessService.Parameters);
            Assert.Equal(4 , collection.Count);
        }
    }
}