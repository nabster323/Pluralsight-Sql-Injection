using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pluralsight_sql_injection
{
    public class CustomerContext : DbContext
    {
        public CustomerContext() : base("customerDbContext")
        {
        }

        public DbSet<Customer> Customers { get; set; }
    }

    public class Customer
    {
        [Key]
        public int Id { get; set;  } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
    }
}
