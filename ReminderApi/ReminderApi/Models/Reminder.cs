using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReminderApi.Models
{
    public class Reminder
    {
        #region Properties
        public int ReminderId { get; set; }
        public string Title { get; set; }
        public DateTime DatumReleased { get; set; }
        public bool Watched { get; set; }
        public string Link { get; set; }
        public string? Description { get; set; }
        public ICollection<ReminderTag> Tags { get; private set; }
        #endregion

        #region Constructors
        public Reminder()
        {
            this.Tags = new List<ReminderTag>();
        }
        public Reminder(string title, DateTime date ,string? link = null, string? decr = null,bool watched = false)
        {
            this.Title = title;
            this.DatumReleased = date;
            this.Description = decr;
            this.Link = link;
            this.Watched = watched;
            this.Tags = new List<ReminderTag>();
        }
        #endregion

        #region Methodes
        public void AddTag(ReminderTag reminderTag,Tag tag) {
            if (!Tags.Any(t => t.Tag.Name.Equals(reminderTag.TagName)))
            {
                Tags.Add(reminderTag);
                tag.Reminders.Add(reminderTag);
            }
        }
        public void RemoveTagFromReminder(ReminderTag reminderTag,Tag tag) {
            if (Tags.Any(t => t.Tag.Name.Equals(reminderTag.TagName)))
            {
                Tags.Remove(reminderTag);
                tag.Reminders.Remove(reminderTag);
            }
        }
        public void HasBeenWatched() {
            this.Watched = true;
        }
        #endregion



    }
}
