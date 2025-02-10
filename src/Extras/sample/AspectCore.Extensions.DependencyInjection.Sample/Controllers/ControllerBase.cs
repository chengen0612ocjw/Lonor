﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AspectCore.Extensions.DependencyInjection.Sample.Controllers
{
    public abstract class ControllerBase<T> : Controller
    {
        //public virtual IActionResult ApiResult(Func<IActionResult> func)
        //{
        //    return func();
        //}

        public virtual IEnumerable<T> ApiResult(Func<IEnumerable<T>> func)
        {
            return func();
        }

        public virtual IEnumerable<K> ApiResultWith<K>(Func<IEnumerable<K>> func)
        {
            return func();
        }
    }
}