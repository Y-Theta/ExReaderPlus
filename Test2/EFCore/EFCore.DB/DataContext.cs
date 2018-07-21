using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace EFCore.DB {
    public class DataContext : DbContext {
        public DbSet<Student> Students { get; set; }
        public DbSet<Mission> Missions { get; set; }
        public DbSet<StudentMission> StudentMissions { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite("Data Source=db.db");
        }
    }
}