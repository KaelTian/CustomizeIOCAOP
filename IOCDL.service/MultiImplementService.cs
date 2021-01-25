using IOCDL.Interface;

namespace IOCDL.Service
{
    public class DefaultMultiImplementService : IMultiImplementServiceA
    {
        public string ShowServiceMessage(string message)
        {
            return $"Default service:{message}";
        }
    }

    public class ExchangeMultiImplementService : IMultiImplementServiceA
    {
        public string ShowServiceMessage(string message)
        {
            return $"Exchange service:{message}";
        }
    }

    public class AzureMultiImplementService : IMultiImplementServiceA
    {
        public string ShowServiceMessage(string message)
        {
            return $"Azure service:{message}";
        }
    }

    public class SharePointMultiImplementService : IMultiImplementServiceA
    {
        public string ShowServiceMessage(string message)
        {
            return $"SharePoint service:{message}";
        }
    }
}
