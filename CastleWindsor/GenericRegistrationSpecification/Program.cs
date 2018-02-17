using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace GenericCases
{
    #region classes, interfaces

    public class Department
    {
        
    }
    
    public interface IDomainService<T>
    {
    }

    public interface INsiDomainService<T> : IDomainService<T>
    {
    }


    public interface IFbDomainService<T> : IDomainService<T>
    {
    }

    public class NsiDomainService<T> : INsiDomainService<T>
    {
    }

    public class FbNsiDomainService<T> : NsiDomainService<T>, IFbDomainService<T>
    {
    }

    public interface IDeptDomainService : INsiDomainService<Department>, IFbDomainService<Department>
    {
    }

    public class DeptDomainService : IDeptDomainService
    {
    }

    #endregion


    internal class Program
    {
        public static void Main(string[] args)
        {
            Example1();
        }

        /// <summary>
        ///     При резолве generic-компонента отдается предпочтение закрытому типу вне зависимости от порядка регистрации
        /// </summary>
        public static void Example1()
        {
            var container = new WindsorContainer();

            var reg1 = Component.For(typeof(INsiDomainService<>)).ImplementedBy(typeof(FbNsiDomainService<>)).IsDefault();
            var reg2 = Component.For(typeof(INsiDomainService<Department>)).ImplementedBy(typeof(DeptDomainService));
            
            // регистрация generic-компонента (IsDefault - ни на что не влияет)
            container.Register(reg1);

            // регистрация компонента с закрытым типом
            container.Register(reg2);

            // так нельзя, нужно использовать Forward, иначе будет ошибка: Component GenericCases.DeptDomainService could not be registered. There is already a component with that name.
//            var reg3 = Component.For(typeof(IDomainService<Department>)).ImplementedBy(typeof(DeptDomainService));
//            container.Register(reg3);

            var impl = container.Resolve<INsiDomainService<Department>>();
            Console.WriteLine($"Resolved component type: {impl.GetType()}");
            // output: Resolved component type: GenericCases.DeptDomainService

            container.Dispose();
        }

        /// <summary>
        ///     При обычной регистрации с закрытыми типами предпочтение отдается дефолтному компоненту
        ///     (первому зарегистрированному типу компонента или где регистрация отмечена методом IsDefault)
        /// </summary>
        public static void Example2()
        {
            var container = new WindsorContainer();

            var reg1 = Component.For<INsiDomainService<Department>>().ImplementedBy<FbNsiDomainService<Department>>();
            var reg2 = Component.For(typeof(INsiDomainService<Department>)).ImplementedBy(typeof(DeptDomainService)).IsDefault();
            
            container.Register(reg1);
            container.Register(reg2);

            var impl = container.Resolve<INsiDomainService<Department>>();
            Console.WriteLine($"Resolved component type: {impl.GetType()}");
            // output: Resolved component type: GenericCases.FbNsiDomainService`1[System.Object]
        }
    }
}