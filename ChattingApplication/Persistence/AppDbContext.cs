using ChattingApplication.Core.Domains;
using Microsoft.EntityFrameworkCore;

namespace ChattingApplication.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<ConfirmationCode> ConfirmationCodes { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<User>()
                .Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<User>()
                .Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<User>()
                .Property(u => u.IsVerified)
                .IsRequired()
                .HasDefaultValue(false);

            modelBuilder.Entity<User>()
                .HasOne(u => u.ConfirmationCode)
                .WithOne(c => c.User)
                .HasForeignKey<ConfirmationCode>(c => c.UserId);


            modelBuilder.Entity<ConfirmationCode>()
                .Property(c => c.Code)
                .IsRequired()
                .HasMaxLength(6);


            modelBuilder.Entity<ConfirmationCode>()
                .Property(c => c.Trials)
                .IsRequired()
                .HasDefaultValue(3);

            modelBuilder.Entity<ConfirmationCode>()
                .Property(c => c.RemainingCodesForThisDay)
                .IsRequired()
                .HasDefaultValue(5);

        }

    }
}
