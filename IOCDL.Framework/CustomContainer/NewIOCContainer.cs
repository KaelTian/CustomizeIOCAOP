using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace IOCDL.Framework
{
    public class NewIOCContainer : INewIOCContainer
    {
        #region Privates.

        class ObjectRegister
        {
            public object[] CtorConstantParameters { get; set; }

            public Type ImplementType { get; set; }

            public LifetimeType LifetimeType { get; set; } = LifetimeType.Transient;

            public object ScopedInstance { get; set; }
        }

        private Dictionary<string, ObjectRegister> _customContainerDictionary = new Dictionary<string, ObjectRegister>();
        private static Dictionary<string, object> _customSingletonContainerDictionary = new Dictionary<string, object>();
        private string GetKey(Type type, string extension)
        {
            if (!string.IsNullOrWhiteSpace(extension))
            {
                return $"{type.FullName}_{extension}";
            }
            return type.FullName;
        }
        #endregion
        #region Scoped.

        private NewIOCContainer(Dictionary<string, ObjectRegister> customContainerDictionary)
        {
            _customContainerDictionary = customContainerDictionary;
        }

        public INewIOCContainer CloneContainer()
        {
            var customContainerDictionary = new Dictionary<string, ObjectRegister>();
            foreach (var dicItem in this._customContainerDictionary)
            {
                customContainerDictionary.Add(dicItem.Key, new ObjectRegister
                {
                    ImplementType = dicItem.Value.ImplementType,
                    CtorConstantParameters = dicItem.Value.CtorConstantParameters,
                    LifetimeType = dicItem.Value.LifetimeType
                });
            }
            return new NewIOCContainer(customContainerDictionary);
        }

        #endregion

        public NewIOCContainer() { }

        private Object CreateInstance(Type type, object[] ctorConstantParameters = null)
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
            var parameters = GetCtorParameters(ctor, ctorConstantParameters);
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
                    string key = GetKey(property.PropertyType, GetImplementTypeExtension(property));
                    var propertyValue = this._customContainerDictionary.ContainsKey(key) ?
                        CreateInstance(
                            this._customContainerDictionary[key].ImplementType,
                            this._customContainerDictionary[key].CtorConstantParameters) :
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
                        string key = GetKey(para.ParameterType, GetImplementTypeExtension(para));
                        methodParameters.Add(this._customContainerDictionary.ContainsKey(key) ?
                        CreateInstance(
                            this._customContainerDictionary[key].ImplementType,
                            this._customContainerDictionary[key].CtorConstantParameters) :
                        CreateInstance(para.ParameterType));
                    }
                    method.Invoke(instance, methodParameters.ToArray());
                }
            }
            #endregion
            return instance;
        }

        private string GetImplementTypeExtension(ICustomAttributeProvider provider)
        {
            if (provider != null &&
                provider.IsDefined(typeof(CustomImplementTypeAttribute), true))
            {
                var customImplementTypeAttributes = provider.GetCustomAttributes(typeof(CustomImplementTypeAttribute), true);
                if (customImplementTypeAttributes?.Length > 0 &&
                    (customImplementTypeAttributes[0] is CustomImplementTypeAttribute))
                {
                    var customImplementTypeAttribute = customImplementTypeAttributes[0] as CustomImplementTypeAttribute;
                    return customImplementTypeAttribute.ImplementType;
                }
            }
            return null;
        }

        private List<object> GetCtorParameters(ConstructorInfo ctor, object[] ctorConstantParameters = null)
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
                        string key = GetKey(para.ParameterType, GetImplementTypeExtension(para));
                        if (this._customContainerDictionary.ContainsKey(key))
                        {
                            parameters.Add(CreateInstance(
                                this._customContainerDictionary[key].ImplementType,
                                this._customContainerDictionary[key].CtorConstantParameters));
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

        public void Register<TFrom, TTo>(string extension = null, object[] args = null, LifetimeType lifetimeType = LifetimeType.Transient) where TTo : TFrom
        {
            var cacheRegister = new ObjectRegister
            {
                ImplementType = typeof(TTo),
                CtorConstantParameters = args?.Length > 0 ? args : null,
                LifetimeType = lifetimeType
            };
            var key = GetKey(typeof(TFrom), extension);
            if (_customContainerDictionary.ContainsKey(key))
            {
                this._customContainerDictionary[key] = cacheRegister;
            }
            else
            {
                this._customContainerDictionary.Add(GetKey(typeof(TFrom), extension), cacheRegister);
            }
        }

        public TFrom Resolve<TFrom>(string extension = null)
        {
            Type type = typeof(TFrom);
            string key = GetKey(type, extension);
            if (this._customContainerDictionary.ContainsKey(key))
            {
                return (TFrom)CreateInstance(
                    this._customContainerDictionary[key].ImplementType,
                    this._customContainerDictionary[key].CtorConstantParameters);
            }
            return (TFrom)CreateInstance(type);
        }

        public TFrom ResolveLifetimeType<TFrom>(string extension = null)
        {
            var key = GetKey(typeof(TFrom), extension);
            if (this._customContainerDictionary.ContainsKey(key))
            {
                var cacheInfo = this._customContainerDictionary[key];
                if (cacheInfo.LifetimeType == LifetimeType.Singleton)
                {
                    if (!_customSingletonContainerDictionary.ContainsKey(key))
                    {
                        var implementValue = Resolve<TFrom>(extension);
                        _customSingletonContainerDictionary.Add(key, implementValue);
                        return implementValue;
                    }
                    else
                    {
                        return (TFrom)_customSingletonContainerDictionary[key];
                    }
                }
                else if (cacheInfo.LifetimeType == LifetimeType.Scoped)
                {
                    if (cacheInfo.ScopedInstance != null)
                    {
                        return (TFrom)cacheInfo.ScopedInstance;
                    }
                    else
                    {
                        var implementValue = Resolve<TFrom>(extension);
                        cacheInfo.ScopedInstance = implementValue;
                        return implementValue;
                    }
                }
                else if (cacheInfo.LifetimeType == LifetimeType.PerThread)
                {
                    var threadKey = $"{key}_{Thread.CurrentThread.ManagedThreadId}";
                    if (!CallContext<TFrom>.ContainsKey(threadKey))
                    {
                        CallContext<TFrom>.SetData(threadKey, Resolve<TFrom>(extension));
                    }
                    return CallContext<TFrom>.GetData(threadKey);
                }
                else
                {
                    return Resolve<TFrom>(extension);
                }
            }
            else
            {
                throw new NotImplementedException("Item type has not been implemented.");
            }
        }


    }
}
