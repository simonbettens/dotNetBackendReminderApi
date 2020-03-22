using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReminderApi.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReminderApi.Data.Mapping
{
    public class ChecklistHeaderConfig : IEntityTypeConfiguration<ChecklistHeader>
    {
        public void Configure(EntityTypeBuilder<ChecklistHeader> builder)
        {
            builder.ToTable("Checklist");
            builder.HasKey(ch=>ch.ChecklistHeaderId);
            builder.HasMany(ch => ch.Items).WithOne(ci => ci.Header).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
