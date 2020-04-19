using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReminderApi.Data.Mapping;
using ReminderApi.Models;
using ReminderApi.Models.Domain;

namespace ReminderApi.Data
{
    public class ReminderDbContext : IdentityDbContext
    {
        public DbSet<Reminder> Reminder { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<ReminderTag> ReminderTag { get; set; }
        public DbSet<ChecklistHeader> CheckListHeader { get; set; }
        public DbSet<ChecklistItem> ChecklistItem { get; set; }

        public ReminderDbContext(DbContextOptions<ReminderDbContext> options) : base(options) {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = @"Server=.;Database=WebIVDataBase;Integrated Security=True;";
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ReminderConfig());
            builder.ApplyConfiguration(new TagConfig());
            builder.ApplyConfiguration(new ReminderTagConfig());
            builder.ApplyConfiguration(new ChecklistHeaderConfig());
            builder.ApplyConfiguration(new ChecklistItemConfig());
            base.OnModelCreating(builder);
        }
    }
}
