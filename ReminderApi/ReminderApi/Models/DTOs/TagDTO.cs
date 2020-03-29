using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReminderApi.Models.DTOs
{
    public class TagDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Color { get; set; }
    }
}
