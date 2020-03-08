using Microsoft.AspNetCore.Mvc;
using ReminderApi.Models;
using System.Collections.Generic;

namespace ReminderApi.Controllers
{
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReminderController : ControllerBase
    {
        private readonly IReminderRepository _reminderRepository;
        public ReminderController(IReminderRepository reminderRepository)
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
                reminders=_reminderRepository.GetAll();
            }
            else {
                reminders= _reminderRepository.GetBy(title, tagname);
            }
            /*
            if (reminders == null) {
                return NotFound();
            }
            */
            return reminders;
        }

    }
}