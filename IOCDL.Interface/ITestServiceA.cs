using IOCDL.Framework;

namespace IOCDL.Interface
{
    public interface ITestServiceA
    {
       
        string ShowMessage(string message);

        [LogBefore]
        [LogAfter]
        [Monitor]
        string ShowMessage111(string message);
    }
}
