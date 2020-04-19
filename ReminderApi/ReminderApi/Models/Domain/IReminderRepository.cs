using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReminderApi.Models.Domain
{
    public interface IReminderRepository
    {
        IEnumerable<Reminder> GetAllExcludeWatched(int gebruikerId);
        IEnumerable<Reminder> GetAllIncludeWatched(int gebruikerId);
        IEnumerable<Reminder> GetBy(int gebruikerId,string name = null, string tagname = null);
        Reminder GetById(int id);
        void Delete(Reminder reminder);
        void Add(Reminder reminder);
        void Update(Reminder reminder);
        void SaveChanges();
    }
}
