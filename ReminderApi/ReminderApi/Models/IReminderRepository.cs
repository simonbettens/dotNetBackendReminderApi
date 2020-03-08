using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReminderApi.Models
{
    public interface IReminderRepository
    {
        IEnumerable<Reminder> GetAll();
        IEnumerable<Reminder> GetAllIncludeWatched();
        IEnumerable<Reminder> GetBy(string name = null, string tagname = null);
        Reminder GetById(int id);
        void Delete(Reminder reminder);
        void Add(Reminder reminder);
        void Update(Reminder reminder);
        void SaveChanges();
    }
}
