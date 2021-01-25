using IOCDL.Framework;
using IOCDL.IDAL;
using IOCDL.Interface;
using System;

namespace IOCDL.DAL
{
    public class UserDAL : IUserDAL
    {
        private ITestServiceA _testServiceA;
        private ITestServiceB _testServiceB;
        private ITestServiceC _testServiceC;
        private IMultipleServiceE _multipleServiceE;
        [CustomConstructor]
        public UserDAL(ITestServiceA testServiceA, ITestServiceB testServiceB, IMultipleServiceE multipleServiceE)
        {
            _testServiceA = testServiceA;
            _testServiceB = testServiceB;
            _multipleServiceE = multipleServiceE;
        }

        public UserDAL(ITestServiceA testServiceA, ITestServiceB testServiceB, ITestServiceC testServiceC)
        {
            _testServiceA = testServiceA;
            _testServiceB = testServiceB;
            _testServiceC = testServiceC;
        }

        public UserDAL(ITestServiceA testServiceA)
        {
            _testServiceA = testServiceA;
        }

        public UserDAL() { }

        public string Login(string message)
        {
            var loginMessage = $"User DAL message is:{message}";
            //if (_testServiceA != null)
            //{
            //    loginMessage += $"{_testServiceA.ShowMessage(message)}";
            //}
            //if (_testServiceB != null)
            //{
            //    loginMessage += $"{_testServiceB.ShowTime(DateTime.Now)}";
            //}
            //if (_testServiceC != null)
            //{
            //    loginMessage += $"{_testServiceC.ShowAge(18)}";
            //}
            if (_multipleServiceE != null)
            {
                loginMessage+= $"{_multipleServiceE.Show("Make show!")}";
            }
            return loginMessage;
        }
    }
}
