using System;
using System.Collections.Generic;

namespace IOCDL.Framework
{
    public interface ICustomContainer
    {
        //泛型约束保证类型关系.
        void Register<TFrom, TTo>(string specialKey = null, object[] args = null, List<string>ctorAbstractImplementTypes = null) where TTo : TFrom;
        TFrom Resolve<TFrom>(string specialKey = null);
    }


}
