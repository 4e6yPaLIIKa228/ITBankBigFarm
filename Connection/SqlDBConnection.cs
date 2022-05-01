using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITBankBigFarm.Connection
{
    class SqlDBConnection
    {
        //public static string connection =
        //          "Network Library=DBMSSOCN;" +
        //          "Data Source=192.168.50.136;" +
        //          "Initial Catalog=PushkarevDB;" +
        //          "User Id=Pushkarev;" +
        //          "Password=QWEasd123;";

        //public static string connection =
        //         "Network Library=DBMSSOCN;" +
        //         "Data Source=DESKTOP-4625JV3;" +
        //         "Initial Catalog=BaseBank;";
        //public static string connection =
        //            "Server=DESKTOP-4625JV3" + "Database = BaseBank" + "Trusted_Connection = True";

        public static string connection = $@"Data Source=ITBank.db;Version=3;";
    }
}
