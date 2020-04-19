using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReminderApi.Models.Domain
{
    public interface ITagRepository
    {
        IEnumerable<Tag> GetAll(int gebruikerId);
        Tag GetById(int id);
        Tag GetByName(string name);
        void Delete(Tag tag);
        void Add(Tag tag);
        void Update(Tag tag);
        void SaveChanges();
    }
}
