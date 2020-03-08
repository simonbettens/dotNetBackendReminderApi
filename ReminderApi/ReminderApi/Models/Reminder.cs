using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReminderApi.Models
{
    public class Reminder
    {
        #region Properties
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime DatumReleased { get; set; }
        public bool Watched { get; set; }
        public string Description { get; set; }
        public ICollection<Tag> Tags { get; private set; }
        #endregion

        #region Constructors
        public Reminder()
        {
            this.Tags = new List<Tag>();
        }
        public Reminder(string title, DateTime date , string decr,bool watched = false)
        {
            this.Title = title;
            this.DatumReleased = date;
            this.Description = decr;
            this.Watched = watched;
            this.Tags = new List<Tag>();
        }
        #endregion

        #region Methodes
        public void AddTag(Tag tag) {
            if (!Tags.Any(t => t.Naam.Equals(tag.Naam)))
            {
                this.Tags.Add(tag);
            }
        }
        public void RemoveTag(Tag tag) {
            if (Tags.Any(t => t.Naam.Equals(tag.Naam)))
            {
                this.Tags.Remove(tag);
            }
        }
        public void HasBeenWatched() {
            this.Watched = true;
        }
        #endregion



    }
}
