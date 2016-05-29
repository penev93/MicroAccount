using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace finalTaskz.Controllers
{
    public class Connection
    {
        public static string ConnStr()
        {
            string connStr = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            return connStr;
        }
    }
}