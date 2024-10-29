using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Demo_WCF2.Models
{
    public class MyDbContext : DbContext
    {
        public MyDbContext() : base("MyDbContext")
        {

        }

        public DbSet<User> Users { get; set; }


    }
}