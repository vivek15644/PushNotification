using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PushNotificationsSignalR.Utility
{
    public class DatabaseHandler
    {
        public static string ConnectionStringName = "Conn";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString);
        }
    }
}