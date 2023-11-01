using ChattingApplication.Core.Domains;
using Microsoft.EntityFrameworkCore;

namespace ChattingApplication.Persistence
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _config;

        public DbSet<User> Users { get; set; }
        public DbSet<ConfirmationCode> ConfirmationCodes { get; set; }
        public DbSet<Message> Messages { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration config) 
            : base(options)
        {
            _config = config;
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
                .HasDefaultValue(Convert.ToByte(_config["ConfirmationCodes:Trials"]));

            modelBuilder.Entity<ConfirmationCode>()
                .Property(c => c.RemainingCodesForThisDay)
                .IsRequired()
                .HasDefaultValue(
                Convert.ToByte(_config["ConfirmationCodes:RemainingNumberOfCodesForThisDay"]) - 1);


            modelBuilder.Entity<Message>()
                .HasKey(m => new {m.Id, m.SenderId, m.ReceiverId });

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderId);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(r => r.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId);

            modelBuilder.Entity<Message>()
                .Property(m => m.Body)
                .HasMaxLength(1024);

        }

    }
}
