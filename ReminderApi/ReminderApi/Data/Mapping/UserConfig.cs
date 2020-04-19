using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReminderApi.Models.Domain;
using System;
using System.Linq;


namespace ReminderApi.Data.Mapping
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(c => c.UserId);
            builder.Property(c => c.LastName).IsRequired().HasMaxLength(50);
            builder.Property(c => c.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(c => c.Email).IsRequired().HasMaxLength(100);
            builder.HasMany(c => c.Reminders).WithOne(r=>r.User).HasForeignKey().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.Tags).WithOne(t=>t.User).HasForeignKey().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
