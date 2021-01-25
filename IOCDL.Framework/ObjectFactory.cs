//using IOCDL.IBLL;
//using IOCDL.IDAL;
using System;
using System.Reflection;

namespace IOCDL.Framework
{
    public class ObjectFactory
    {
        //如果没有事先在项目中引用USerDAL和UserBLL,只是将相应的dll拷贝到程序运行的目录下,则assemmbly.load 失败
        //public static IUserDAL CreateUserDAL()
        //{
        //    var config = ConfigurationManager.GetNode("IUserDAL");
        //    var configArray = config.Split((','));
        //    Assembly assembly = Assembly.Load(configArray[1]);
        //    Type type = assembly.GetType(configArray[0]);
        //    var userDAL = (IUserDAL)Activator.CreateInstance(type);
        //    return userDAL;
        //}

        //public static IUserBLL CreateUserBLL(IUserDAL userDAL)
        //{
        //    var config = ConfigurationManager.GetNode("IUserBLL");
        //    var configArray = config.Split((','));
        //    Assembly assembly = Assembly.Load(configArray[1]);
        //    Type type = assembly.GetType(configArray[0]);
        //    var userBLL = (IUserBLL)Activator.CreateInstance(type, userDAL);
        //    return userBLL;
        //}
    }
}
