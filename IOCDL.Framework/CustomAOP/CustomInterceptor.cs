using Castle.DynamicProxy;
using System;
using System.Reflection;

namespace IOCDL.Framework.CustomAOP
{
    public class CustomInterceptor : StandardInterceptor
    {
        protected override void PreProceed(IInvocation invocation)
        {
            Console.WriteLine("CustomInterceptor PreProceed,method name:" + invocation.Method.Name);
        }

        protected override void PerformProceed(IInvocation invocation)
        {
            Console.WriteLine("CustomInterceptor PerformProceed,method name:" + invocation.Method.Name);
            base.PerformProceed(invocation);
        }

        protected override void PostProceed(IInvocation invocation)
        {
            Console.WriteLine("CustomInterceptor PostProceed,method name:" + invocation.Method.Name);
        }
    }


}
