using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CRUD
{
    internal class Context : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<User_Contact> Contacts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=127.0.0.1; Port=5432; Database=myusers; User Id=postgres; Password=Javlon2005");
        }
    }
}
