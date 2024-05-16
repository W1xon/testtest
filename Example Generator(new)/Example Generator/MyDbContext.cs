using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Example_Generator
{
    public class MyDbContext : DbContext
    {
        public MyDbContext() : base("DbConnectionString") { }
        public DbSet<ExampleData> ExampleData { get; set; }
        public DbSet<Name> Name { get; set; }
    }
}
