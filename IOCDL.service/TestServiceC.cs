using IOCDL.Interface;

namespace IOCDL.Service
{
    public class TestServiceC : ITestServiceC
    {
        public string ShowAge(int age)
        {
            return $"Test Service C show age is:{age}";
        }
    }
}
