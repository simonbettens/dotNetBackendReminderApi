using Microsoft.AspNetCore.Mvc;
using ReminderApi.Models;
using ReminderApi.Models.Domain;
using System.Collections.Generic;

namespace ReminderApi.Controllers
{
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class RemindersController : ControllerBase
    {
        private readonly IReminderRepository _reminderRepository;
        public RemindersController(IReminderRepository reminderRepository)
        {
            this._reminderRepository = reminderRepository;
        }
        /// <summary>
        /// Gets every reminders when the name is correct or it has a tag with the given tagname
        /// else it gets every reminder
        /// </summary>
        /// <param name="title"> title of the reminder </param>
        /// <param name="tagname"> the name of a tag </param>
        /// <returns> a list of reminders </returns>
        [HttpGet]
        public IEnumerable<Reminder> GetReminders(string title = null, string tagname = null)
        {
            IEnumerable<Reminder> reminders;
            if (string.IsNullOrEmpty(title) && string.IsNullOrEmpty(tagname))
            {
                reminders = _reminderRepository.GetAllExcludeWatched();
            }
            else {
                reminders = _reminderRepository.GetBy(title, tagname);
            }
            return reminders;
        }
        /// <summary>
        /// Gets a reminder by the id given
        /// </summary>
        /// <param name="id">the reminder id you wish to get</param>
        /// <returns> a reminder if found else throw not found</returns>
        [HttpGet("{id}")]
        public ActionResult<Reminder> GetReminder(int id) {
            Reminder recipe = _reminderRepository.GetById(id);
            if (recipe == null) return NotFound();
            return recipe;
        }

    }
}