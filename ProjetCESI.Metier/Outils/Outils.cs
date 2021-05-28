using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Metier.Outils
{
    public static class Outils
    {
        private static IConfiguration currentConfig;

        public static void SetConfig(IConfiguration configuration)
        {
            currentConfig = configuration;
        }

        public static string GetConfiguration(string configKey)
        {
            try
            {
                string connectionString = currentConfig.GetConnectionString(configKey);
                return connectionString;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
