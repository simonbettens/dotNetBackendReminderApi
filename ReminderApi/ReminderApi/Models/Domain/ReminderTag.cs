using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReminderApi.Models.Domain
{
    public class ReminderTag
    {
        public int ReminderId { get; set; }
        public int TagId { get; set; }
        public string TagName { get; set; }
        [JsonIgnore]
        public Reminder Reminder { get; set; }
        [JsonIgnore]
        public Tag Tag { get; set; }
        public ReminderTag()
        {

        }
        public ReminderTag(Reminder reminder, Tag tag)
        {
            this.Reminder = reminder;
            this.Tag = tag;
            this.ReminderId = reminder.ReminderId;
            this.TagId = tag.TagId;
            this.TagName = tag.Name;
        }
        #region Methodes
        public void AddToReminder() {
            Reminder.Tags.Add(this);
        }
        public void AddToTags() {
            Tag.Reminders.Add(this);
        }
        public void RemoveFromTags() {
            Reminder.Tags.Remove(this);
        }
        public void RemoveFromReminder() {
            Tag.Reminders.Remove(this);
        }
        #endregion
    }
}
