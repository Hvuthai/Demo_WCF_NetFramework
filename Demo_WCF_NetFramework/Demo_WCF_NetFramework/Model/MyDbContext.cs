using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_WCF_NetFramework.Model
{
    public class MyDbContext: DbContext
    {
        public MyDbContext() : base("MyDbContext")
        {

        }

        public DbSet<User> Users { get; set; }


    }
}
