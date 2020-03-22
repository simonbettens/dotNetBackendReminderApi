using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReminderApi.Models;
using ReminderApi.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReminderApi.Data.Mapping
{
    public class ReminderConfig : IEntityTypeConfiguration<Reminder>
    {

        public void Configure(EntityTypeBuilder<Reminder> builder)
        {
            builder.ToTable("Reminder");
            builder.HasKey(r=>r.ReminderId);
            builder.Property(r => r.Title).HasMaxLength(50).IsRequired();
            builder.Property(r => r.DatumReleased).IsRequired();
            builder.Property(r => r.Description).HasMaxLength(150).IsRequired(false);
            builder.HasMany(r => r.Checklist).WithOne(ch => ch.Reminder).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
