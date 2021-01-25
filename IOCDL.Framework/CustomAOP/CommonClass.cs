using System;

namespace IOCDL.Framework.CustomAOP
{
    /// <summary>
    /// 实现类型注入,但是类型注入有限制,只能扩展virtual方法.
    /// </summary>
    public class CommonClass
    {
        public virtual void MethodInterceptor()
        {
            Console.WriteLine("This is Interceptor");
        }

        public virtual void TestMethod1()
        {
            Console.WriteLine("This is Colrn Cong!!!!");
        }

        public void MethodNoInterceptor()
        {
            Console.WriteLine("This is without Interceptor");
        }
    }
}
