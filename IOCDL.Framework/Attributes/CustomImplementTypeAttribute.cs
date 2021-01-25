using System;

namespace IOCDL.Framework
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
    public class CustomImplementTypeAttribute : Attribute
    {
        public string ImplementType
        {
            get; private set;
        }
        public CustomImplementTypeAttribute(string implementType)
        {
            this.ImplementType = implementType;
        }
    }
}
