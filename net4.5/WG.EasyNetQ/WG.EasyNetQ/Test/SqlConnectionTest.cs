using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WG.EasyNetQ.Test
{
    public class SqlConnectionTest
    {
        public static void Test()
        {
            SqlConnectionStringBuilder connStr = new SqlConnectionStringBuilder();
            connStr.DataSource = @"115.236.37.105,90";
            connStr.InitialCatalog = "Jiepei_Pcb";
            connStr.IntegratedSecurity = false;
            connStr.UserID = "WKSite_Main";
            connStr.Password = "WKSite_Main123456!@#";
            connStr.MaxPoolSize = 5;//设置最大连接池为5
            connStr.ConnectTimeout = 1;//设置超时时间为1秒

            SqlConnection conn = null;
            for (int i = 1; i <= 100; ++i)
            {
                conn = new SqlConnection(connStr.ConnectionString);
                try
                {
                    conn.Open();
                    Console.WriteLine("Connection{0} is linked", i);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\n异常信息:\n{0}", ex.Message);
                    break;
                }
         
            }
            //for (int i = 1; i <= 100; ++i)
            //{
            //    using (SqlConnection conn = new SqlConnection(connStr.ConnectionString))
            //    {

            //        try
            //        {
            //            conn.Open();
            //            Console.WriteLine("Connection{0} is linked", i);
            //        }
            //        catch (Exception ex)
            //        {
            //            Console.WriteLine("\n异常信息:\n{0}", ex.Message);
            //            break;
            //        }
            //    }
            //}
        }
    }
}
