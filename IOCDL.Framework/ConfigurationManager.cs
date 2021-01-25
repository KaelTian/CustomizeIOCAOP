using Microsoft.Extensions.Configuration;
using System.IO;

namespace IOCDL.Framework
{
    public class ConfigurationManager
    {
        private static IConfigurationRoot _configuration;

        static ConfigurationManager()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            _configuration = builder.Build();
        }

        public static string GetNode(string nodeName)
        {
            return _configuration[nodeName];
        }
    }
}
