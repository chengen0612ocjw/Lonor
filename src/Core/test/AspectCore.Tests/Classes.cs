﻿using System;
using AspectCore.Injector;

namespace AspectCore.Tests
{
    public interface IService
    {
        Guid Id { get; set; }

        ILogger Logger { get; set; }
    }

    public class ServiceBase : IService
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [FromContainer]
        public virtual ILogger Logger { get; set; }
    }

    public interface ITransient : IService {
        void Foo();
    }


    public class Transient : ServiceBase, ITransient, IDelegateTransient
    {
        public virtual void Foo() { }
    }

    public interface IDelegateTransient : ITransient { }

    public interface IScoped : IService, IDisposable { bool IsDisposed { get; } }

    public class Scoped : ServiceBase, IScoped
    {
        public bool IsDisposed { get; private set; }
        public void Dispose()
        {
            IsDisposed = true;
        }
    }

    public interface ISingleton : IScoped { }

    public class Singleton : Scoped, ISingleton { }

    public interface ILogger {
        void Info();
    }

    public class Logger : ILogger
    {
        public void Info()
        {
        }
    }

    public interface IPropertyInjectionService
    {

    }

    public class PropertyInjectionService : ServiceBase, IPropertyInjectionService
    {
        [FromContainer]
        internal ILogger InternalLogger { get; set; }
    }

    public interface ISimpleGeneric<T> { }

    public class SimpleGeneric<T> : ISimpleGeneric<T>, IInstanceSimpleGeneric<T>, IDelegateSimpleGeneric<T> { }

    public interface IInstanceSimpleGeneric<T> : ISimpleGeneric<T> { }

    public interface IDelegateSimpleGeneric<T> : ISimpleGeneric<T> { }

    public interface IUserService : IService
    {
        IRepository<User> Repository { get; }
    }

    public class UserService : ServiceBase, IUserService
    {
        public UserService(IRepository<User> repository) {
            Repository = repository;
        }

        public IRepository<User> Repository { get; }
    }

    public interface IRepository<T> { }

    public class Repository<T> : IRepository<T> { }

    public class User { }

}