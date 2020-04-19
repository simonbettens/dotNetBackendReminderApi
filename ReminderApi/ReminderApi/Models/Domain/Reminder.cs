using ReminderApi.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReminderApi.Models.Domain
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
        public int? ProcessBar { get; set; }
        public User User { get; set; }
        public ICollection<ReminderTag> Tags { get; private set; }
        public ICollection<ChecklistHeader> Checklist { get; set; }
        #endregion

        #region Constructors
        public Reminder()
        {
            this.Tags = new List<ReminderTag>();
            this.Checklist = new List<ChecklistHeader>();
            this.ProcessBar = CalculateProcessBar();
        }
        public Reminder(string title, DateTime date,User user ,string? link = null, string? decr = null,bool watched = false)
        {
            this.Title = title;
            this.DatumReleased = date;
            this.Description = decr;
            this.Link = link;
            this.Watched = watched;
            this.User = user;
            this.Tags = new List<ReminderTag>();
            this.Checklist = new List<ChecklistHeader>();
            this.ProcessBar = CalculateProcessBar();
            this.User.AddReminder(this);
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
        public void AddToCheckList(ChecklistHeader header) {
            Checklist.Add(header);
        }
        public void HasBeenWatched() {
            this.Watched = true;
        }
        private int CalculateProcessBar() {
            if (Checklist.Any())
            {
                double total = 0, amountCompleted=0;
                int procent = 0;
                foreach (var item in Checklist)
                {
                    total += item.CalcTotal();
                    amountCompleted += item.CalcTotalComplete();
                }
                procent = (int) Math.Round((double)(amountCompleted / total)*100);
                return procent;
            }
            else {
                return 0;
            }
        }
        public void RecalculateProcessBar() {
            int procent = CalculateProcessBar();
            this.ProcessBar = procent;
        }
        #endregion



    }
}
