using Microsoft.EntityFrameworkCore;
using UserService.Model;

namespace UserService.Context
{
    public class UserContext :DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }

        public UserContext()
        {
        }

        public UserContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("users_pkey");
                entity.HasIndex(e => e.Email).IsUnique();

                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.Password).HasColumnName("password");
                entity.Property(e => e.RoleId).HasConversion<int>();
                entity.HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId);
            });

            modelBuilder.Entity<Role>().Property(e => e.RoleId).HasConversion<int>();

            modelBuilder
                .Entity<Role>()
                .HasData(
                    Enum.GetValues(typeof(RoleId))
                        .Cast<RoleId>()
                        .Select(e => new Role
                        {
                            RoleId = e,
                            Name = e.ToString()
                        }));
        }


    }
}
