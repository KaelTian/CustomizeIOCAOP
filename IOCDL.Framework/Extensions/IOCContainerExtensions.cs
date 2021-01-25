using Castle.DynamicProxy;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace IOCDL.Framework
{
    /// <summary>
    /// AOP Structure.
    /// </summary>
    public static class IOCContainerExtensions
    {

        public static object AOP(this object target, Type interfaceType)
        {
            ProxyGenerator generator = new ProxyGenerator();
            var interceptor = new IOCInterceptor();
            return generator.CreateInterfaceProxyWithTarget(interfaceType, target, interceptor);
        }

    }

    public class IOCInterceptor : StandardInterceptor
    {
        protected override void PreProceed(IInvocation invocation)
        {
            //Console.WriteLine("IOCInterceptor PreProceed,method name:" + invocation.Method.Name);
            base.PreProceed(invocation);
        }


        protected override void PerformProceed(IInvocation invocation)
        {
            var method = invocation.Method;
            Action action = () => base.PerformProceed(invocation);

            if (method.IsDefined(typeof(BaseInterceptorAttribute), true))
            {
                foreach (var attribute in method.GetCustomAttributes<BaseInterceptorAttribute>().ToArray().Reverse())
                {
                    action = attribute.Do(invocation, action);
                }
            }
            
            action.Invoke();
            Console.WriteLine("IOCInterceptor PerformProceed,method name:" + invocation.Method.Name);
            //base.PerformProceed(invocation);//真实的action
            //前面不能执行具体的动作--前面只能组装动作---配置管道模型---委托

        }


        protected override void PostProceed(IInvocation invocation)
        {
            //Console.WriteLine("IOCInterceptor PostProceed,method name:" + invocation.Method.Name);
            base.PostProceed(invocation);
        }
    }

    public class LogBeforeAttribute : BaseInterceptorAttribute
    {
        public override Action Do(IInvocation invocation, Action action)
        {
            return () =>
            {
                Console.WriteLine($"This is log before attribute: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}");
                action.Invoke();
            };
        }
    }

    public class LogAfterAttribute : BaseInterceptorAttribute
    {
        public override Action Do(IInvocation invocation, Action action)
        {
            return () =>
            {
                action.Invoke();
                Console.WriteLine($"This is log after attribute: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}");
            };
        }
    }

    public class MonitorAttribute : BaseInterceptorAttribute
    {
        public override Action Do(IInvocation invocation, Action action)
        {
            return () =>
            {
                Console.WriteLine("This is log after attribute.");
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                //执行真实逻辑
                action.Invoke();
                stopwatch.Stop();
                Console.WriteLine($"Current method spend:{stopwatch.ElapsedMilliseconds}ms");
            };
        }
    }

    public abstract class BaseInterceptorAttribute : Attribute
    {
        public abstract Action Do(IInvocation invocation, Action action);
    }
}
