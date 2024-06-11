﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;

namespace AspectCore.Extensions.Reflection.Benchmark.Benchmarks
{
    [AllStatisticsColumn]
    [MemoryDiagnoser]
    public class CustomAttributeReflectorBenchmarks
    {
        private readonly MethodInfo _method;
        private readonly MethodReflector _reflector;

        public CustomAttributeReflectorBenchmarks()
        {
            _method = typeof(CustomAttributeReflectorBenchmarks).GetMethod(nameof(Reflection_GetCustomAttribute));
            _reflector = _method.GetReflector();
        }

        [Attribute1]
        [Attribute2("benchmark", Id = 10000)]
        [Benchmark]
        public Attribute Reflection_GetCustomAttribute()
        {
            return _method.GetCustomAttribute(typeof(Attribute1));
        }

        [Benchmark]
        public Attribute AspectCore_Reflector_GetCustomAttribute()
        {
            return _reflector.GetCustomAttribute(typeof(Attribute1));
        }

        [Benchmark]
        public Attribute[] Reflection_GetCustomAttributes()
        {
            return _method.GetCustomAttributes().ToArray();
        }

        [Benchmark]
        public Attribute[] AspectCore_Reflector_GetCustomAttributes()
        {
            return _reflector.GetCustomAttributes();
        }
    }

    public class Attribute1 : Attribute
    { }

    public class Attribute2 : Attribute
    {
        public int Id { get; set; }

        public string Title { get; }

        public Attribute2(string title)
        {
            Title = title;
        }
    }
}