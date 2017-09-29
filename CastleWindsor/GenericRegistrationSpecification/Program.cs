using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace GenericRegistrationSpecification
{
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

    public interface IDeptDomainService : INsiDomainService<object>, IFbDomainService<object>
    {
    }

    public class DeptDomainService : IDeptDomainService
    {
    }


    internal class Program
    {
        public static void Main(string[] args)
        {
            Example1();
        }

        /// <summary> При резолве generic-компонента отдается предпочтение закрытому типу вне зависимости от порядка регистрации</summary>
        public static void Example1()
        {
            var container = new WindsorContainer();

            // регистарция generic-компонента
            container.Register(Component.For(typeof(IDomainService<>)).Forward(typeof(INsiDomainService<>))
                .ImplementedBy(typeof(FbNsiDomainService<>))
                // IsDefault - ни на что не влияет
                .IsDefault()
            );

            // регистрация компонента с закрытым типом
            var reg2 = Component.For(typeof(INsiDomainService<object>)).ImplementedBy(typeof(DeptDomainService));
            container.Register(reg2);

            var impl = container.Resolve<INsiDomainService<object>>();
            Console.WriteLine($"Resolved component type: {impl.GetType()}");
            // output: Resolved component type: GenericRegistrationSpecification.DeptDomainService

            container.Dispose();
        }

        /// <summary>
        ///     При обычной регистрации с закрытыми типами предпочтение отдается дефолтному компоненту
        ///     (первому зарегистрированному типу компонента или где регистрация отмечена методом IsDefault)
        /// </summary>
        public static void Example2()
        {
            var container = new WindsorContainer();

            container.Register(
                Component.For<IDomainService<object>>()
                    .Forward<INsiDomainService<object>>()
                    .ImplementedBy<FbNsiDomainService<object>>()
            );

            var reg2 = Component.For(typeof(INsiDomainService<object>)).ImplementedBy(typeof(DeptDomainService))
                .IsDefault();
            container.Register(reg2);

            var impl = container.Resolve<INsiDomainService<object>>();
            Console.WriteLine($"Resolved component type: {impl.GetType()}");
            // output: Resolved component type: GenericRegistrationSpecification.FbNsiDomainService`1[System.Object]

            container.Dispose();
        }
    }
}