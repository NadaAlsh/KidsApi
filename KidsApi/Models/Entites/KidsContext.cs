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
        public DbSet<MyTask> Task { get; set; } 
        public ICollection<Child> Children { get; set; }
        public DbSet<Reward> Rewards { get; set; }
        public DbSet<ParentChildRelationship> ParentChildRelationships { get; set; }
        public DbSet<ClaimedRewards> ClaimedRewards { get; set; }
        public KidsContext(DbContextOptions<KidsContext> options) : base(options)
        {
          
        }

       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ParentChildRelationship>()
               .HasKey(pc => new { pc.ParentId, pc.ChildId });

            //modelBuilder.Entity<ParentChildRelationship>()
            //    .HasOne(pc => pc.Parent)
            //    .WithMany(p => p.chilren)
            //    .HasForeignKey(pc => pc.ParentId);

            //modelBuilder.Entity<ParentChildRelationship>()
            //    .HasOne(pc => pc.Child)
            //    .WithOne(c => c.Parent)
            //    .HasForeignKey(pc => pc.ChildId);

         
            //modelBuilder.Entity<Task>()
            //    .HasOne(t => t.Child)
            //    .WithMany(c => c.Tasks)
            //    .HasForeignKey(t => t.ChildId);

           // modelBuilder.Entity<Task>()
           //.HasOne(t => t.Category)
           //.WithMany(c => c.Tasks)
           //.HasForeignKey(t => t.CategoryId);

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