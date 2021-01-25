using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace IOCDL.ConsoleProject
{
    public enum ServiceJobModule
    {
        AzureAD = 1,
        Intune = 2,
        Exchange = 3,
        SPOneDrive = 4,
        Teams = 5
    }
    [AttributeUsage(AttributeTargets.Field)]
    public class FileNameInjectionAttribute : Attribute
    {
        private ServiceJobModule _serviceJobModule;
        public ServiceJobModule ServiceJobModule
        {
            get { return _serviceJobModule; }
        }
        public FileNameInjectionAttribute(ServiceJobModule serviceJobModule)
        {
            _serviceJobModule = serviceJobModule;
        }
    }
    [AttributeUsage(AttributeTargets.Field)]
    public class YamlFileInjectionAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Field)]
    public class JsonFileInjectionAttribute : Attribute
    {

    }
    public class FileNameConfiguration
    {
        [FileNameInjection(ServiceJobModule.AzureAD)]
        public const string User = "User";
        [FileNameInjection(ServiceJobModule.AzureAD)]
        public const string Group = "Group";
        [FileNameInjection(ServiceJobModule.Intune)]
        public const string DeviceEnrollment = "DeviceEnrollment";
        [FileNameInjection(ServiceJobModule.SPOneDrive)]
        public const string SPOnlineSharingSetting = "SPOnlineSharingSetting";
        [FileNameInjection(ServiceJobModule.Teams)]
        public const string TeamsSettings = "TeamsSettings";
        [FileNameInjection(ServiceJobModule.Teams)]
        public const string TeamsCollection = "TeamsCollection";
        [FileNameInjection(ServiceJobModule.Teams)]
        public const string TeamsArray = "TeamsArray";
        #region Exchange.
        #region yaml
        [YamlFileInjection]
        [FileNameInjection(ServiceJobModule.Exchange)]
        public const string DataLossPreventionPolicyYaml = "DataLossPreventionPolicy.yaml";
        [YamlFileInjection]
        [FileNameInjection(ServiceJobModule.Exchange)]
        public const string MailBoxYaml = "MailBox.yaml";
        [YamlFileInjection]
        [FileNameInjection(ServiceJobModule.Exchange)]
        public const string AlertPolicyYaml = "AlertPolicy.yaml";
        [YamlFileInjection]
        [FileNameInjection(ServiceJobModule.Exchange)]
        public const string MailFlowRuleYaml = "MailFlowRule.yaml";
        [YamlFileInjection]
        [FileNameInjection(ServiceJobModule.Exchange)]
        public const string MalwareFilterPolicyYaml = "MalwareFilterPolicy.yaml";
        [YamlFileInjection]
        [FileNameInjection(ServiceJobModule.Exchange)]
        public const string SensitivityLabelYaml = "SensitivityLabel.yaml";
        [YamlFileInjection]
        [FileNameInjection(ServiceJobModule.Exchange)]
        public const string SpamFilterPolicyYaml = "SpamFilterPolicy.yaml";
        #endregion
        #region json
        [JsonFileInjection]
        [FileNameInjection(ServiceJobModule.Exchange)]
        public const string DataLossPreventionPolicyJson = "DataLossPreventionPolicy.json";
        [JsonFileInjection]
        [FileNameInjection(ServiceJobModule.Exchange)]
        public const string MailBoxJson = "MailBox.json";
        [JsonFileInjection]
        [FileNameInjection(ServiceJobModule.Exchange)]
        public const string AlertPolicyJson = "AlertPolicy.json";
        [JsonFileInjection]
        [FileNameInjection(ServiceJobModule.Exchange)]
        public const string MailFlowRuleJson = "MailFlowRule.json";
        [JsonFileInjection]
        [FileNameInjection(ServiceJobModule.Exchange)]
        public const string MalwareFilterPolicyJson = "MalwareFilterPolicy.json";
        [JsonFileInjection]
        [FileNameInjection(ServiceJobModule.Exchange)]
        public const string SensitivityLabelJson = "SensitivityLabel.json";
        [JsonFileInjection]
        [FileNameInjection(ServiceJobModule.Exchange)]
        public const string SpamFilterPolicyJson = "SpamFilterPolicy.json";
        #endregion
        #endregion

        public static List<string> GetNamesByModule(ServiceJobModule serviceJobModule, bool isYamlFile = true)
        {
            var nameList = new List<string>();
            var type = typeof(FileNameConfiguration);
            var fields = new List<FieldInfo>();
            if (isYamlFile)
            {
                fields = type.GetFields(BindingFlags.Static | BindingFlags.Public)
                  .Where(a => a.IsDefined(typeof(FileNameInjectionAttribute), true) &&
                  a.IsDefined(typeof(YamlFileInjectionAttribute), true)).ToList();
            }
            else
            {
                fields = type.GetFields(BindingFlags.Static | BindingFlags.Public)
                  .Where(a => a.IsDefined(typeof(FileNameInjectionAttribute), true) &&
                  a.IsDefined(typeof(JsonFileInjectionAttribute), true)).ToList();
            }
            if (fields?.Count > 0)
            {
                var instance = Activator.CreateInstance(type);
                foreach (var fieldInfo in fields)
                {
                    var attribute = fieldInfo.GetCustomAttribute<FileNameInjectionAttribute>();
                    if (attribute.ServiceJobModule == serviceJobModule)
                    {
                        nameList.Add((string)fieldInfo.GetValue(instance));
                    }
                }
            }
            return nameList;
        }
        public static ServiceJobModule GetModuleByName(string fileName)
        {
            var type = typeof(FileNameConfiguration);
            var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public)
                  .Where(a => a.IsDefined(typeof(FileNameInjectionAttribute), true)).ToList();
            if (fields?.Count > 0)
            {
                var nameModuleDic = new Dictionary<string, ServiceJobModule>();
                var instance = Activator.CreateInstance(type);
                foreach (var fieldInfo in fields)
                {
                    var attribute = fieldInfo.GetCustomAttribute<FileNameInjectionAttribute>();
                    var nameKey = (string)fieldInfo.GetValue(instance);
                    nameModuleDic.Add(nameKey, attribute.ServiceJobModule);
                }
                if (nameModuleDic.ContainsKey(fileName))
                {
                    return nameModuleDic[fileName];
                }
            }
            throw new Exception($"Can not find file:{fileName}'s service job module.");
        }
    }
}
