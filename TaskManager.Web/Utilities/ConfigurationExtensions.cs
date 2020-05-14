using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Web.Utilities
{
    public static class ConfigurationExtensions
    {
        static public string ConstructConnectionStringFromEnv(this IConfiguration config)
        {
            var databaseName = GetRequired("DB_NAME", config);
            var host = GetRequired("DB_HOST", config);
            var userName = GetRequired("DB_USER_NAME", config);
            var password = GetRequired("DB_PASSWORD", config);

            return $"Data Source={host};Initial Catalog={databaseName};User ID={userName};Password={password}";
        }

        static private string GetRequired(string name, IConfiguration config)
        {
            return config.GetValue<string>(name) ?? throw new InvalidOperationException($"you must provide value for {name} through environment variables");
        }
    }
}
