using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserDic
{
    public class DataContext:DbContext
    {
        public DbSet<passage> passages { get; set; }
        public DbSet<customDic> customDics { get; set; }

        protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
        {
            ///数据库可以更改
            optionsBuilder.UseSqlite("Data Source=userinfo.db");
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {

        }
    }

    
}
