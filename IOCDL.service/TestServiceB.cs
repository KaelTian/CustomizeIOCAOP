using IOCDL.Framework;
using IOCDL.Interface;
using System;

namespace IOCDL.Service
{
    public class TestServiceB : ITestServiceB
    {
        /// <summary>
        /// 希望属性也能初始化,但是不用构造函数
        /// </summary>
        [CustomPropertyInjection]
        private ITestServiceD _testServiceD { get; set; }

        private ITestServiceA _testServiceA { get; set; }


        /// <summary>
        /// 方法注入,舒适化私有属性:_testServiceA.
        /// </summary>
        /// <param name="testServiceA"></param>
        [CustomMethodInjection]
        private void InitTestServiceA(ITestServiceA testServiceA)
        {
            this._testServiceA = testServiceA;
        }

        public string ShowTime(DateTime dateTime)
        {
            var message = $"Test Service B show time:{dateTime.ToString()}" +
                $"";
            if (_testServiceD != null)
            {
                message += $"Property Injection {_testServiceD.ShowCustom(message)}" +
                    $"";
            }
            if (_testServiceA != null)
            {
                message += $"Property Injection {_testServiceA.ShowMessage(message)}" +
                    $"";
            }
            return message;
        }
    }
}
