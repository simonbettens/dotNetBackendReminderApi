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
    public class ReminderTagConfig : IEntityTypeConfiguration<ReminderTag>
    {
        public void Configure(EntityTypeBuilder<ReminderTag> builder)
        {
            builder.HasKey(t => new { t.TagId, t.ReminderId });
            builder.HasOne(t => t.Reminder).WithMany(t => t.Tags).HasForeignKey(t => t.ReminderId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(t => t.Tag).WithMany(t => t.Reminders).HasForeignKey(t => t.TagId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
