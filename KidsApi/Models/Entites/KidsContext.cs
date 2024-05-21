using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
//using System.Data.Entity;

namespace KidsApi.Models.Entites
{
    public class KidsContext : DbContext
    {

        public DbSet<Child> Children { get; set; }
        public KidsContext(DbContextOptions<KidsContext> options) : base(options)
        {
          
        }

       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}