using IOCDL.Framework;
using IOCDL.Interface;

namespace IOCDL.Service
{
    public class TestServiceD : ITestServiceD
    {
        public string ShowCustom(string custom)
        {
            return $"Test service D show custom:{custom}";
        }
    }

    public class MultipleServiceD : ITestServiceD
    {
        [CustomPropertyInjection]
        private IMultiImplementServiceA _defaultServiceA { get; set; }
        //private IMultiImplementServiceA _exchangeServiceA;
        [CustomPropertyInjection]
        [CustomImplementType("Azure")]
        private IMultiImplementServiceA _azureServiceA { get; set; }

        private IMultiImplementServiceA _sharepointServiceA;

        public MultipleServiceD([CustomImplementType("SharePoint")] IMultiImplementServiceA sharepointServiceA)
        {
            _sharepointServiceA = sharepointServiceA;
        }

        public string ShowCustom(string custom)
        {
            if (_defaultServiceA != null)
            {
                custom += _defaultServiceA.ShowServiceMessage("DefaultA");
            }
            if (_azureServiceA != null)
            {
                custom += _azureServiceA.ShowServiceMessage("AzureA");
            }
            if (_sharepointServiceA != null)
            {
                custom += _sharepointServiceA.ShowServiceMessage("SharePointA");
            }
            return custom;
        }
    }
}
