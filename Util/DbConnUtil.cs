using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Util
{
    public class DbConnUtil
    {
        private static IConfiguration _iconfig;

        static DbConnUtil()
        {
            getAppSettingsFile();
        }

        private static void getAppSettingsFile()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            _iconfig = builder.Build();
        }

        public static string getConnectionString()
        {
            return _iconfig.GetConnectionString("LocalConnectionString");
        }

        public static SqlConnection returnConnection()
        {
            string connString = getConnectionString();
            SqlConnection conn = new SqlConnection(connString);

            return conn;
        }
    }
}
