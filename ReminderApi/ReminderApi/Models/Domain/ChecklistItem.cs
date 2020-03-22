using System;

namespace ReminderApi.Models.Domain
{
    public class ChecklistItem
    {
        public int ChecklistItemId { get; set; }
        public bool Checked { get; set; }
        public string Title { get; set; }
        public int Volgorde { get; set; }
        public DateTime? Finished { get; set; }
        public ChecklistHeader Header { get; set; }
        public ChecklistItem()
        {

        }
        public ChecklistItem(string title, ChecklistHeader header, int volgorde,DateTime? finished = null, bool ischecked = false)
        {
            this.Checked = ischecked;
            this.Title = title;
            this.Finished = null;
            this.Header = header;
            this.Header.AddItem(this);
            this.Finished = finished;
            this.Volgorde = volgorde;

        }
        public void ZetChecked(bool status)
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
        }
    }
}
