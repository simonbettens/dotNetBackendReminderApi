using Microsoft.EntityFrameworkCore;
using ReminderApi.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReminderApi.Data.Repositories
{
    public class TagRepository : ITagRepository
    {
        public readonly ReminderDbContext _reminderDb;
        public readonly DbSet<Tag> _tags;
        public TagRepository(ReminderDbContext reminderDb)
        {
            this._reminderDb = reminderDb;
            this._tags = reminderDb.Tag;
        }
        public void Add(Tag tag)
        {
            _tags.Add(tag);
        }

        public void Delete(Tag tag)
        {
            _tags.Remove(tag);
        }

        public IEnumerable<Tag> GetAll()
        {
            return _tags.OrderBy(t => t.Name).ToList();
        }

        public Tag GetById(int id)
        {
            return _tags.FirstOrDefault(t=>t.TagId==id);
        }

        public Tag GetByName(string name)
        {
            return _tags.FirstOrDefault(t => t.Name == name);
        }

        public void SaveChanges()
        {
            _reminderDb.SaveChanges();
        }

        public void Update(Tag tag)
        {
            _tags.Update(tag);
        }
    }
}
