using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReminderApi.Models.DTOs
{
    public class ChecklistHeaderDTO
    {
        public bool Checked { get; set; }
        [Required]
        public string Title { get; set; }
        public int Volgorde { get; set; }
        public DateTime? Finished { get; set; }
        public ICollection<ChecklistItemDTO> Items { get; set; }
    }
}
