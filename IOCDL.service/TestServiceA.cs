using IOCDL.Interface;
using System;

namespace IOCDL.Service
{
    public class TestServiceA : ITestServiceA
    {
        public TestServiceA()
        {
            //Console.WriteLine($"{nameof(TestServiceA)} is constructored.");
        }
        public string ShowMessage(string message)
        {
            return $"Test Service A show message:{message}";
        }

        public string ShowMessage111(string message)
        {
            Console.WriteLine("Message111 is:" + message);
            return $"11111111:{message}";
        }
    }
}
