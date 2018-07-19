using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserDic
{
    public class DataContext:DbContext
    {
        public DbSet<user> users { get; set; }
        public DbSet<passage> passages { get; set; }
        public DbSet<customDic> customDics { get; set; }

        protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=db.db");
        }
    }

    
}
