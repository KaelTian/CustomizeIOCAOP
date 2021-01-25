using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IOCDL.Framework
{
    /// <summary>
    /// 用来生成对象
    /// 第三方 业务无关
    /// </summary>
    public class CustomContainer : ICustomContainer
    {
        class CacheRegister
        {
            public object[] CtorConstantParameters { get; set; }

            public Type ImplementType { get; set; }

            public List<string> CtorAbstractImplementTypes { get; set; }
        }
        private Dictionary<string, CacheRegister> customContainerDictionary = new Dictionary<string, CacheRegister>();
        public void Register<TFrom, TTo>(string specialKey = null, object[] args = null, List<string> ctorAbstractImplementTypes = null)

            where TTo : TFrom
        {
            var cacheRegister = new CacheRegister
            {
                ImplementType = typeof(TTo),
                CtorConstantParameters = args?.Length > 0 ? args : null,
                CtorAbstractImplementTypes = ctorAbstractImplementTypes?.Count > 0 ? ctorAbstractImplementTypes : null
            };
            var key = (!string.IsNullOrWhiteSpace(specialKey)) ? $"{typeof(TFrom).FullName}_{specialKey}" : typeof(TFrom).FullName;
            this.customContainerDictionary.Add(key, cacheRegister);
        }

        private string GetKey(Type type, List<string> extensionCollection = null)
        {
            var typeFullName = type.FullName;
            if (extensionCollection?.Count > 0)
            {
                var keysInContainer = customContainerDictionary.Keys.Where(a => a.Contains($"{typeFullName}_"))
                    .ToList();
                if (keysInContainer?.Count > 0)
                {
                    var specialKeys = keysInContainer.Select(a => a.Substring($"{typeFullName}_".Length))
                        .ToList();
                    var intersectCollection = specialKeys.Intersect(extensionCollection)
                        .ToList();
                    if (intersectCollection?.Count > 0)
                    {
                        return $"{typeFullName}_{intersectCollection.FirstOrDefault()}";
                    }
                }
            }
            return typeFullName;
        }

        private Object CreateInstance(Type type, object[] ctorConstantParameters = null, List<string> ctorAbstractImplementTypes = null)
        {
            #region 选择合适的构造函数
            ConstructorInfo ctor = null;
            //2 标记特性优先
            ctor = type.GetConstructors().FirstOrDefault(a => a.IsDefined(typeof(CustomConstructorAttribute), true));
            if (ctor == null)
            {
                //1 参数个数最多的构造函数
                ctor = type.GetConstructors()
                   .OrderByDescending(a => a.GetParameters().Length)
                   .First();
            }
            #endregion
            var parameters = GetCtorParameters(ctor, ctorConstantParameters, ctorAbstractImplementTypes);
            object instance;
            if (parameters?.Count > 0)
            {
                instance = Activator.CreateInstance(type, parameters.ToArray());
            }
            else
            {
                instance = Activator.CreateInstance(type);
            }
            #region 属性注入
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
                            .Where(a => a.IsDefined(typeof(CustomPropertyInjectionAttribute), true))
                            .ToList();
            if (properties?.Count > 0)
            {
                foreach (var property in properties)
                {
                    string key = GetKey(property.PropertyType, ctorAbstractImplementTypes);
                    var propertyValue = this.customContainerDictionary.ContainsKey(key) ?
                        CreateInstance(
                            this.customContainerDictionary[key].ImplementType,
                            this.customContainerDictionary[key].CtorConstantParameters,
                            this.customContainerDictionary[key].CtorAbstractImplementTypes) :
                        CreateInstance(property.PropertyType);
                    property.SetValue(instance, propertyValue);
                }
            }
            #endregion
            #region 方法注入
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
                            .Where(a => a.IsDefined(typeof(CustomMethodInjectionAttribute), true))
                            .ToList();
            if (methods?.Count > 0)
            {
                foreach (var method in methods)
                {
                    var methodParameters = new List<object>();
                    foreach (var para in method.GetParameters())
                    {
                        string key = GetKey(para.ParameterType, ctorAbstractImplementTypes);
                        methodParameters.Add(this.customContainerDictionary.ContainsKey(key) ?
                        CreateInstance(
                            this.customContainerDictionary[key].ImplementType,
                            this.customContainerDictionary[key].CtorConstantParameters,
                            this.customContainerDictionary[key].CtorAbstractImplementTypes) :
                        CreateInstance(para.ParameterType));
                    }
                    method.Invoke(instance, methodParameters.ToArray());
                }
            }
            #endregion
            return instance;
        }

        private List<object> GetCtorParameters(ConstructorInfo ctor, object[] ctorConstantParameters = null, List<string> ctorAbstractImplementTypes = null)
        {
            var parameters = new List<object>();
            if (ctor != null)
            {
                var ctorParametersIndex = 0;
                foreach (var para in ctor.GetParameters())
                {
                    if (para.IsDefined(typeof(CustomParameterInjectionAttribute), true) &&
                        ctorConstantParameters?.Length > 0)
                    {
                        parameters.Add(ctorConstantParameters[ctorParametersIndex++]);
                    }
                    else
                    {
                        string key = GetKey(para.ParameterType, ctorAbstractImplementTypes);
                        if (this.customContainerDictionary.ContainsKey(key))
                        {
                            parameters.Add(CreateInstance(
                                this.customContainerDictionary[key].ImplementType,
                                this.customContainerDictionary[key].CtorConstantParameters,
                                this.customContainerDictionary[key].CtorAbstractImplementTypes));
                        }
                        else
                        {
                            parameters.Add(CreateInstance(para.ParameterType));
                        }
                    }
                }
            }
            return parameters;
        }

        public TFrom Resolve<TFrom>(string specialKey = null)
        {
            Type type = typeof(TFrom);
            string key = GetKey(type, (!string.IsNullOrWhiteSpace(specialKey)) ? new List<string> { specialKey } : null);
            if (this.customContainerDictionary.ContainsKey(key))
            {
                return (TFrom)CreateInstance(
                    this.customContainerDictionary[key].ImplementType,
                    this.customContainerDictionary[key].CtorConstantParameters,
                    this.customContainerDictionary[key].CtorAbstractImplementTypes);
            }
            return (TFrom)CreateInstance(type);
        }





    }
}
