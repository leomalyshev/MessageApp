using MessageService.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MessageService.Context
{
    public class MessageContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }

        public MessageContext(DbContextOptions<MessageContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>(ent =>
            {
                ent.HasKey(x => x.Id).HasName("messages_key");

                ent.ToTable("messages");

                ent.Property(e => e.Id).HasColumnName("id");
                ent.Property(e => e.SenderId).HasColumnName("sender_id");
                ent.Property(e => e.RecipientId).HasColumnName("recipient_id");
                ent.Property(e => e.Content).HasColumnName("content");
                ent.Property(e => e.SentAt).HasColumnName("sent_at");
                ent.Property(e => e.IsRead).HasColumnName("is_read");
            });

        }
    }
}