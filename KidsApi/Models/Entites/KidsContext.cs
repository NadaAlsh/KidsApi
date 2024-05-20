using Microsoft.EntityFrameworkCore;

namespace KidsApi.Models.Entites
{
    public class KidsContext : DbContext
    {
        public KidsContext(DbContextOptions<KidsContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}