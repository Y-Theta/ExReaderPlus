using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserDictionary
{
    public class DataContext:DbContext
    {
        public DbSet<Passage> Passages { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<Dictionary> Dictionaries { get; set; }
        public DbSet<DictionaryWord> DictionaryWords { get; set; }

        protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
        {
            ///数据库可以更改
            optionsBuilder.UseSqlite("Data Source=customDictionay.db");
        }

    }

    
}
