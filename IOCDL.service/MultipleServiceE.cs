using IOCDL.Framework;
using IOCDL.Interface;

namespace IOCDL.Service
{
    public class MultipleServiceE : IMultipleServiceE
    {
        private int _num;
        private readonly ITestServiceA _testServiceA;
        private string _str;
        private string _str1;
        [CustomConstructor]
        public MultipleServiceE([CustomParameterInjection] int num, ITestServiceA testServiceA, [CustomParameterInjection] string str, [CustomParameterInjection] string str1)
        {
            _num = num;
            _testServiceA = testServiceA;
            _str = str;
            _str1 = str1;
        }
        public string Show(string message)
        {
            return $"Multiple Service E message:number:{_num}." +
                $"stringinfo:{_str}." +
                $"serviceA:{_testServiceA.ShowMessage(message)}." +
                $"string1Info:{_str1}.";
        }
    }

    public class MultipleServiceExtend1E : IMultipleServiceE
    {
        public string Show(string message)
        {
            return $"Extend Multiple Service 1E message:{message}";
        }
    }
}
