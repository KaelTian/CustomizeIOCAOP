using IOCDL.BLL;
using IOCDL.DAL;
using IOCDL.Framework;
using IOCDL.IBLL;
using IOCDL.IDAL;
using IOCDL.Interface;
using IOCDL.Service;
using System;
using System.Collections.Generic;
using System.Threading;

namespace IOCDL.ConsoleProject
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                {
                    //1. 工厂模式,实现依赖倒置
                    //var userDAL = ObjectFactory.CreateUserDAL();
                    //Console.WriteLine(userDAL.Login("Colrn Cong"));
                    //var userBLL = ObjectFactory.CreateUserBLL(userDAL);
                    //Console.WriteLine(userBLL.Login("Kael Tian"));
                    //Console.Read();
                }

                {
                    //手动实现Asp.Net Core 中的依赖注入
                    //构造函数带参数  解决
                    //构造函数嵌套  解决
                    //多构造函数(选择超集) 解决
                    //其他注入方式   属性注入  方法注入
                    //相同接口的不同实例(单接口多实现) 需要进行区分去识别
                    //为何不使用原生的ioc
                    //构造函数的参数是基本类型

                    //单接口多实现的问题需要自己解决.
                    //DynamicIOC();
                    //NewDynamicTOCTest();
                    TestForNew();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.Read();
        }

        static void DynamicIOC()
        {
            //常规IOC容器:(第三方--业务无关) 容器对象---注册---生成
            // Dependency Injection(依赖注入)
            //为什么要依赖注入??
            //要控制反转,不能依赖细节,要依赖抽象,但是实际上细节部分的类实现还会依赖细节,
            //为了屏蔽细节,所以需要通过依赖注入的方式(递归反射)去解决无线层级的对象依赖,但是需要将抽象和细节的mapping关系处理好,因为new 对象时不能new 接口,
            //需要找到相应的实现细节类
            ICustomContainer container = new CustomContainer();
            container.Register<IUserDAL, UserDAL>(ctorAbstractImplementTypes: new List<string> { "lol" });
            container.Register<IUserBLL, UserBLL>();
            container.Register<ITestServiceA, TestServiceA>();
            container.Register<ITestServiceB, TestServiceB>();
            container.Register<ITestServiceC, TestServiceC>();
            container.Register<ITestServiceD, TestServiceD>();
            container.Register<IMultipleServiceE, MultipleServiceE>(specialKey: "lol", args: new object[] { 10086, "Colrn_Cong", "Dark Season" });
            container.Register<IMultipleServiceE, MultipleServiceExtend1E>(specialKey: nameof(MultipleServiceExtend1E));

            var userDAL = container.Resolve<IUserDAL>();
            var userBLL = container.Resolve<IUserBLL>();
            var serviceE = container.Resolve<IMultipleServiceE>(specialKey: "lol");
            var serviceExtend1E = container.Resolve<IMultipleServiceE>(nameof(MultipleServiceExtend1E));
            Console.WriteLine(serviceE.Show("Kael_Tian"));
            Console.WriteLine(serviceExtend1E.Show("Colrn_Cong"));
            Console.WriteLine(userBLL.Login("Kael tian"));
        }

        static void NewDynamicTOCTest()
        {
            try
            {
                #region 0114
                {
                    //INewIOCContainer newIOCContainer = new NewIOCContainer();
                    //newIOCContainer.Register<IMultiImplementServiceA, DefaultMultiImplementService>();
                    ////newIOCContainer.Register<IMultiImplementServiceA, ExchangeMultiImplementService>();
                    //newIOCContainer.Register<IMultiImplementServiceA, AzureMultiImplementService>("Azure");
                    //newIOCContainer.Register<IMultiImplementServiceA, SharePointMultiImplementService>("SharePoint");
                    //newIOCContainer.Register<ITestServiceD, MultipleServiceD>();

                    ////newIOCContainer.Resolve<IMultiImplementServiceA>("Azure");
                    ////newIOCContainer.Resolve<IMultiImplementServiceA>("SharePoint");
                    //var serviceD = newIOCContainer.Resolve<ITestServiceD>();
                    //Console.WriteLine(serviceD.ShowCustom("Size"));
                }
                #endregion
                #region 0115
                {
                    //INewIOCContainer container = new NewIOCContainer();
                    //container.Register<ITestServiceA, TestServiceA>();
                    //container.Register<ITestServiceD, TestServiceD>();

                    //var sA1 = container.Resolve<ITestServiceA>();
                    //var sA2 = container.Resolve<ITestServiceA>();
                    //Console.WriteLine(object.ReferenceEquals(sA1, sA2));

                    //container.Register<ITestServiceB, TestServiceB>(lifetimeType: LifetimeType.Singleton);
                    //var sB1 = container.ResolveLifetimeType<ITestServiceB>();
                    //var sB2 = container.ResolveLifetimeType<ITestServiceB>();
                    //Console.WriteLine(object.ReferenceEquals(sB1, sB2));
                }
                #endregion
                #region 0119;0120
                {
                    #region 瞬时,单例,容器单例
                    ////不同的生命周期
                    ////AddTransient(瞬时生命周期)
                    ////AddSingleton(单例)
                    ////AddScoped(作用域单例)
                    ////作用域单例:Http请求时,一个请求处理过程中,创建的都是同一个实例,不同的请求处理过程中,就是不同的实例
                    ////区分请求,Http请求---Asp.NetCore内置Kestrel,初始化一个容器实例,然后每次来一个Http请求,就clone一个,
                    ////或者叫创建子容器,一个请求一个子容器,实现请求单例(子容器单例????)
                    //var container1 = new NewIOCContainer();
                    //container1.Register<ITestServiceA, TestServiceA>(lifetimeType: LifetimeType.Scoped);
                    //container1.Register<ITestServiceD, TestServiceD>(lifetimeType: LifetimeType.Singleton);
                    //container1.Register<ITestServiceC, TestServiceC>();
                    //var sA1 = container1.ResolveLifetimeType<ITestServiceA>();
                    //var sA2 = container1.ResolveLifetimeType<ITestServiceA>();
                    //var sD1 = container1.ResolveLifetimeType<ITestServiceD>();

                    //var cC1 = container1.ResolveLifetimeType<ITestServiceC>();
                    //var cC2 = container1.ResolveLifetimeType<ITestServiceC>();
                    ////true
                    //Console.WriteLine(object.ReferenceEquals(sA1, sA2));
                    ////false
                    //Console.WriteLine(object.ReferenceEquals(cC1, cC2));
                    //var container2 = container1.CloneContainer();
                    //var cA1 = container2.ResolveLifetimeType<ITestServiceA>();
                    //var cA2 = container2.ResolveLifetimeType<ITestServiceA>();
                    ////true
                    //Console.WriteLine(object.ReferenceEquals(cA1, cA2));

                    //var otherContainer = container2.CloneContainer();
                    //var otherSA1 = otherContainer.ResolveLifetimeType<ITestServiceA>();
                    //var otherSA2 = otherContainer.ResolveLifetimeType<ITestServiceA>();
                    //var otherSD1 = otherContainer.ResolveLifetimeType<ITestServiceD>();
                    ////true
                    //Console.WriteLine(object.ReferenceEquals(otherSA1, otherSA2));
                    ////true
                    //Console.WriteLine(object.ReferenceEquals(sD1, otherSD1));
                    ////false
                    //Console.WriteLine(object.ReferenceEquals(cA1, otherSA1));
                    ////true
                    //Console.WriteLine(object.ReferenceEquals(cA1, sA1));
                    ////true
                    //Console.WriteLine(object.ReferenceEquals(cA1, otherSA1));
                    #endregion

                    #region 线程单例,扩展
                    //var threadContainer = new NewIOCContainer();
                    //threadContainer.Register<ITestServiceA, TestServiceA>(lifetimeType: LifetimeType.PerThread);

                    //var ta1 = threadContainer.ResolveLifetimeType<ITestServiceA>();
                    //var ta2 = threadContainer.ResolveLifetimeType<ITestServiceA>();
                    //ITestServiceA ta3 = null;
                    //ITestServiceA ta4 = null;
                    //ITestServiceA ta5 = null;
                    //ITestServiceA ta6 = null;

                    //Task.Run(() =>
                    //{
                    //    Console.WriteLine(ThreadInfo("ta3"));
                    //    ta3 = threadContainer.ResolveLifetimeType<ITestServiceA>();
                    //    Console.WriteLine($"2.ta1 and ta3 equals:{object.ReferenceEquals(ta1, ta3)}");
                    //    Console.WriteLine(ThreadInfo("ta4"));
                    //    ta4 = threadContainer.ResolveLifetimeType<ITestServiceA>();
                    //    Console.WriteLine($"3.ta3 and ta4 equals:{object.ReferenceEquals(ta3, ta4)}");
                    //});

                    //Task.Run(() =>
                    //{
                    //    Console.WriteLine(ThreadInfo("ta5"));
                    //    ta5 = threadContainer.ResolveLifetimeType<ITestServiceA>();
                    //    Console.WriteLine($"4.ta1 and ta5 equals:{object.ReferenceEquals(ta1, ta5)}");
                    //    return "aaaaa";
                    //}).ContinueWithContext(t =>
                    //{

                    //    Console.WriteLine("t id:" + t.Id);
                    //    Console.WriteLine(ThreadInfo("ta6"));
                    //    ta6 = threadContainer.ResolveLifetimeType<ITestServiceA>();
                    //    Console.WriteLine($"5.ta5 and ta6 equals:{object.ReferenceEquals(ta6, ta5)}");
                    //});
                    //Thread.Sleep(3 * 1000);
                    //Console.WriteLine(ThreadInfo("ta1"));
                    //Console.WriteLine(ThreadInfo("ta2"));
                    //Console.WriteLine($"1.ta1 and ta2 equals:{object.ReferenceEquals(ta1, ta2)}");
                    #endregion
                }
                #endregion
                #region 0121
                {
                    //AOP 类型实现
                    //CustomAOPTest.Show();

                    //interface with target
                    //IOC容器 基于抽象完成对象的实例化
                    var aopContainer = new NewIOCContainer();
                    aopContainer.Register<ITestServiceA, TestServiceA>(lifetimeType: LifetimeType.Singleton);
                    var a1 = aopContainer.ResolveLifetimeType<ITestServiceA>();
                    Console.WriteLine(a1.ShowMessage("aaa"));
                    a1 = (ITestServiceA)a1.AOP(typeof(ITestServiceA));
                    a1.ShowMessage111("1111");
                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.Read();
            }
        }

        static void TestForNew()
        {
            var yamlExchangeNames = FileNameConfiguration.GetNamesByModule(ServiceJobModule.Exchange);
            var jsonExchangeNames = FileNameConfiguration.GetNamesByModule(ServiceJobModule.Exchange,false);
            var serviceJobModule = FileNameConfiguration.GetModuleByName(FileNameConfiguration.MailBoxYaml);
            var serviceJobModule1 = FileNameConfiguration.GetModuleByName(FileNameConfiguration.DataLossPreventionPolicyJson);
            try
            {
                var serviceJobModule2 = FileNameConfiguration.GetModuleByName("aaaa");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        static string ThreadInfo(string threadName)
        {
            return $"Current thread,id:{Thread.CurrentThread.ManagedThreadId},name:{threadName}";
        }
    }
}
