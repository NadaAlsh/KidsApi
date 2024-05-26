using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
//using System.Data.Entity;

namespace KidsApi.Models.Entites
{
    public class KidsContext : DbContext
    {


        public DbSet<Child> Child { get; set; }

        public DbSet<Parent> Parent { get; set; }
        public DbSet<Category> Categories { get; set; }
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
        public ICollection<Child> Children { get; set; }
        public ICollection<Reward> Rewards { get; set; } = new List<Reward>();

        public KidsContext(DbContextOptions<KidsContext> options) : base(options)
        {
          
        }

       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(new Category { CategoryId = 1, Name = "Clean" },
                new Category { CategoryId = 2, Name = "Play" },
                new Category { CategoryId = 3, Name = "Outdoor Activity" },
                new Category { CategoryId = 4, Name = "Study" },
                new Category { CategoryId = 5, Name = "Other" });
        }

        //public static UserAEntity Create(string username, string password, bool isAdmin)
        //{
        //    return new UserAEntity
        //    {
        //        Username = username,
        //        Password = BCrypt.Net.BCrypt.EnhancedHashPassword(password),
        //        IsAdmin = isAdmin,
        //    };
        //}
    }

  
}