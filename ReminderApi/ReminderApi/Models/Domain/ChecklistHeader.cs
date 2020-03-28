using System;
using System.Collections.Generic;
using System.Linq;

namespace ReminderApi.Models.Domain
{
    public class ChecklistHeader
    {
        public int ChecklistHeaderId { get; set; }
        public bool Checked { get; set; }
        public string Title { get; set; }
        public int Volgorde { get; set; }
        public DateTime? Finished { get; set; }
        public Reminder Reminder { get; set; }
        public ICollection<ChecklistItem> Items { get; private set; }
        public ChecklistHeader()
        {
            this.Items = new List<ChecklistItem>();
        }
        public ChecklistHeader(string title, int volgorde, Reminder reminder,DateTime? finished = null ,bool isChecked = false)
        {
            this.Checked = isChecked;
            this.Title = title;
            this.Volgorde = volgorde;
            this.Reminder = reminder;
            this.Reminder.AddToCheckList(this);
            this.Finished = finished;
            this.Items = new List<ChecklistItem>();
        }
        public void CheckedChange(bool status)
        {
            this.Checked = status;
            if (status)
            {
                this.Finished = DateTime.Now;
            }
            else
            {
                this.Finished = null;
            }
            foreach (ChecklistItem item in Items)
            {
                item.ZetChecked(status);
            }
        }
        public void CheckOutAll()
        {
            Checked = Items.All(ci => ci.Checked);
        }
        public ICollection<ChecklistItem> GetChecklistItems()
        {
            return Items.OrderBy(ci => ci.Volgorde).ToList();
        }
        public void AddItem(ChecklistItem item)
        {
            Items.Add(item);
        }
        public int CalcTotal() {
            return Items.Count();
        }
        public int CalcTotalComplete()
        {
            if (Checked) {
                return Items.Count();
            }
            return Items.Count(i => i.Checked);
            
        }
    }
}
