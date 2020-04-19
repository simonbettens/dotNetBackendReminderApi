using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReminderApi.Models.Domain
{
    public class User
    {
        #region Properties
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public ICollection<Tag> Tags { get; private set; }
        [JsonIgnore]
        public ICollection<Reminder> Reminders { get; private set; }

        #endregion
        #region Constructors
        public User()
        {
            Tags = new List<Tag>();
            Reminders = new List<Reminder>();
        }
        public User(string fname, string lname, string email)
        {
            this.FirstName = fname;
            this.LastName = lname;
            this.Email = email;
            Tags = new List<Tag>();
            Reminders = new List<Reminder>();
        }
        #endregion
        #region Methodes
        public void AddTag(Tag tag)
        {
            Tags.Add(tag);
        }
        public void RemoveTag(Tag tag)
        {
            Tags.Remove(tag);
        }
        public void AddReminder(Reminder r)
        {
            this.Reminders.Add(r);
        }
        public void RemoveReminder(Reminder r)
        {
            this.Reminders.Remove(r);
        }
        #endregion
    }
}
