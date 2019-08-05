using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fooxboy.MusicX.Server.Databases.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Fooxboy.MusicX.Server.Databases
{
    public class DataContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=data.db"); 
        }
    }
}
