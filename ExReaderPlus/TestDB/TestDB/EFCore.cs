using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestDB
{
    class EFCore
    {
        public class DataContext : DbContext
        {
            public DbSet<Student> Students { get; set; }
            public DbSet<Mission> Missions { get; set; }
            public DbSet<StudentMission> StudentMissions { get; set; }

            protected override void OnConfiguring(
                DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite("Data Sorce=data.db");
            }
        }
    }
}
