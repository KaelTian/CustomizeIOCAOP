using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace IOCDL.Framework
{
    public static class ActionExtensions
    {
        public static Task InvokeWith(this Action action, SynchronizationContext synchronizationContext, ExecutionContext executionContext)
        {
            Contract.Requires<ArgumentNullException>(action != null);

            return new Func<object>(() =>
            {
                action();
                return null;
            }).InvokeWith(synchronizationContext, executionContext);
        }
    }
}
