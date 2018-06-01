using System;
using Castle.DynamicProxy;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Diagnostics.Helpers;
using ProxyWithoutTarget;
using Xunit;

namespace Tests
{
    public class ProxyWithoutTargetTest
    {
        [Fact]
        public void CallMethodOnInterface()
        {
            Func<int, int, int> calcFunc = (a, b) => a + b;
            var calcService = DelegateWrapper.GetCalcInterfaceFromDelegate(calcFunc);
            var result = calcService.CalcExpression(5, 2);
            Assert.Equal(7, result);

            Func<int, int, int> calcFunc2 = (a, b) => a * b;
            var calcService2 = DelegateWrapper.GetCalcInterfaceFromDelegate(calcFunc2);
            var result2 = calcService2.CalcExpression(5, 2);
            Assert.Equal(10, result2);

            Func<int, int, int> calcFunc3 = (a, b) => a % b;
            var calcService3 = DelegateWrapper.GetCalcInterfaceFromDelegate(calcFunc3);
            var result3 = calcService3.CalcExpression(5, 2);
            Assert.Equal(1, result3);
        }

        [Fact]
        public void NotSameInterceptor()
        {
            Func<int, int, int> calcFunc = (a, b) => a + b;
            var calcService = DelegateWrapper.GetCalcInterfaceFromDelegate(calcFunc);
            var intrc1 = calcService as IProxyTargetAccessor;

            Func<int, int, int> calcFunc2 = (a, b) => a * b;
            var calcService2 = DelegateWrapper.GetCalcInterfaceFromDelegate(calcFunc2);
            var intrc2 = calcService2 as IProxyTargetAccessor;

            Assert.NotSame(intrc1, intrc2);
        }

        [Fact]
        public void SameInterceptor()
        {
            Func<int, int, int> calcFunc = (a, b) => a + b;
            var container = new WindsorContainer();
            container.Register(Component.For<ICalcInterface>().UsingFactoryMethod((kernel, _, context) =>
                DelegateWrapper.GetCalcInterfaceFromDelegate(calcFunc), true));

            var calcService = container.Resolve<ICalcInterface>();
            var intrc1 = calcService as IProxyTargetAccessor;

            var calcService2 = container.Resolve<ICalcInterface>();
            var intrc2 = calcService2 as IProxyTargetAccessor;

            Assert.Equal(
                container.Kernel.GetHandler(typeof(ICalcInterface)).ComponentModel.GetLifestyleDescriptionLong(),
                "Undefined (default lifestyle Singleton will be used)");
            Assert.Same(intrc1, intrc2);
        }
    }
}