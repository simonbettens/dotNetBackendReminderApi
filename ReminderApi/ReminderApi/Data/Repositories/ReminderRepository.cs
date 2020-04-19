using Microsoft.EntityFrameworkCore;
using ReminderApi.Models;
using ReminderApi.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReminderApi.Data.Repositories
{
    public class ReminderRepository : IReminderRepository
    {
        private readonly ReminderDbContext _reminderDb;
        private readonly DbSet<Reminder> _reminders;
        public ReminderRepository(ReminderDbContext dbContext)
        {
            this._reminderDb = dbContext;
            this._reminders = dbContext.Reminder;
        }
        public void Add(Reminder reminder)
        {
            _reminders.Add(reminder);
        }

        public void Delete(Reminder reminder)
        {
            _reminders.Remove(reminder);
        }

        public void Update(Reminder reminder)
        {
            _reminderDb.Update(reminder);
        }

        public IEnumerable<Reminder> GetAllExcludeWatched(int gebruikerId)
        {
            return _reminders.Where(t => t.User.UserId == gebruikerId).Where(r=>r.Watched==false).Include(r => r.Tags).Include(r => r.Checklist).ThenInclude(cl => cl.Items).ToList();
        }

        public IEnumerable<Reminder> GetBy(int gebruikerId,string name = null, string tagname = null)
        {
            var reminders = _reminders.Where(t => t.User.UserId == gebruikerId).Where(r => r.Watched == false).Include(r=>r.Tags).Include(r => r.Checklist).ThenInclude(cl => cl.Items).AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                reminders=reminders.Where(r => r.Title.Equals(name));
            }
            if (!string.IsNullOrEmpty(tagname))
            {
                reminders=reminders.Where(r => r.Tags.Any(t=>t.TagName == tagname));
            }
            return reminders.OrderBy(r=>r.DatumReleased).ThenBy(r=>r.Title).ToList();
        }

        public Reminder GetById(int id)
        {
            return _reminders.Where(r => r.ReminderId == id).Include(r=>r.Tags).Include(r=>r.Checklist).ThenInclude(cl=>cl.Items).FirstOrDefault();
        }

        public void SaveChanges()
        {
            _reminderDb.SaveChanges();
        }

        public IEnumerable<Reminder> GetAllIncludeWatched(int gebruikerId)
        {
            return _reminders.ToList();
        }
    }
}
