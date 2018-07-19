using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteHelper
{
    public class main_Program
    {
        private static SqLiteHelper sql;
        public static void test_Main()
        {
            string path = "DB/user.db";
            path = Path.GetFullPath(path);
            sql = new SqLiteHelper("data source="+path);
            Debug.WriteLine(path);
            //创建名为table1的数据表
            //sql.CreateTable("table1", new string[] { "ID", "Name", "Age", "Email" }, new string[] { "INTEGER", "TEXT", "INTEGER", "TEXT" });
            //插入两条数据
            // sql.InsertValues("table1", new string[] { "1", "张三", "22", "Zhang@163.com" });
            //sql.InsertValues("table1", new string[] { "2", "李四", "25", "Li4@163.com" });

            //更新数据，将Name="张三"的记录中的Name改为"Zhang3"
            // sql.UpdateValues("table1", new string[] { "Name" }, new string[] { "ZhangSan" }, "Name", "Zhang3");

            //删除Name="张三"且Age=26的记录,DeleteValuesOR方法类似
            //sql.DeleteValuesAND("table1", new string[] { "Name", "Age" }, new string[] { "张三", "22" }, new string[] { "=", "=" });


            //读取整张表
            sql.InsertValues("userinfo", new string[] { "2", "张三", "Zhang@163.com" });
            SQLiteDataReader reader = sql.ReadFullTable("userinfo");
            while (reader.Read())
            {
     
                Log("名字" + reader.GetString(reader.GetOrdinal("userName")));
             
            }

            while (true)
            {
                Console.ReadLine();
            }
        }

        static void Log(string s)
        {
            Debug.WriteLine("" + s);
        }
    }
}
