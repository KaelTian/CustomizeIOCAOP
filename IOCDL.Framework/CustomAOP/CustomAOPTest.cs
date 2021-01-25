using Castle.DynamicProxy;
using System;

namespace IOCDL.Framework.CustomAOP
{
    public class CustomAOPTest
    {
        public static void Show()
        {
            ProxyGenerator generator = new ProxyGenerator();
            CustomInterceptor interceptor = new CustomInterceptor();
            //aop class type.
            CommonClass test = generator.CreateClassProxy<CommonClass>(interceptor);
            Console.WriteLine("Current type:{0},parent type:{1}", test.GetType(), test.GetType().BaseType);
            Console.WriteLine();
            test.MethodInterceptor();
            Console.WriteLine();
            test.TestMethod1();
            Console.WriteLine();
            test.MethodNoInterceptor();
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
