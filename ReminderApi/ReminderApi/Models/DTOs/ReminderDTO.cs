using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReminderApi.Models.DTOs
{
    public class ReminderDTO
    {
        [Required]
        public string Title { get; set; }
        public DateTime DatumReleased { get; set; }
        public bool Watched { get; set; }
        public string Link { get; set; }
        public string? Description { get; set; }
        public ICollection<TagDTO> Tags { get; set; }
        public ICollection<ChecklistHeaderDTO> CheckList { get; set; }
    }
}
