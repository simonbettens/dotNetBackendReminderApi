using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReminderApi.Models.Domain
{
    public class Tag
    {
        public int TagId { get; set; }
        public string Name { get; set; }
        public ICollection<ReminderTag> Reminders { get; set; }

        public Tag()
        {
            this.Reminders = new List<ReminderTag>();
        }

        public Tag(string name)
        {
            this.Name = name;
            this.Reminders = new List<ReminderTag>();
        }
    }
}
