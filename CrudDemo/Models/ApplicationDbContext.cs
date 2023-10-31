using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CrudDemo.Models
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<CountryModel> Country { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Employee> Employees { get; set; }

    }
}
