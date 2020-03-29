using Microsoft.AspNetCore.Mvc;
using ReminderApi.Models.Domain;
using ReminderApi.Models.DTOs;
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
        private readonly ITagRepository _tagRepository;
        public RemindersController(IReminderRepository reminderRepository, ITagRepository tagRepository)
        {
            this._reminderRepository = reminderRepository;
            this._tagRepository = tagRepository;
        }

        #region Get Reminders
        // [Get] api/Reminders
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
            else
            {
                reminders = _reminderRepository.GetBy(title, tagname);
            }
            foreach (var item in reminders)
            {
                item.RecalculateProcessBar();
            }
            _reminderRepository.SaveChanges();
            return reminders;
        }
        // [Get] api/Reminders/{id}
        /// <summary>
        /// Gets a reminder by the id given
        /// </summary>
        /// <param name="id">the reminder id you wish to get</param>
        /// <returns> a reminder if found else throw not found</returns>
        [HttpGet("{id}")]
        public ActionResult<Reminder> GetReminder(int id)
        {
            Reminder reminder = _reminderRepository.GetById(id);
            if (reminder == null)
            {
                return NotFound();
            }
            else {
                reminder.RecalculateProcessBar();
                _reminderRepository.SaveChanges();
            }
            return reminder;
        }
        #endregion

        #region Post & Put & Delete Reminders
        // [Post] api/reminders
        /// <summary>
        /// A new reminder is added to the database
        /// </summary>
        /// <param name="reminderDTO"> a new reminder object</param>
        /// <returns>a json object about the newly added reminder and extra info</returns>
        [HttpPost]
        public ActionResult<Reminder> PostReminder(ReminderDTO reminderDTO)
        {
            Reminder createReminder = new Reminder(reminderDTO.Title, reminderDTO.DatumReleased, reminderDTO.Link, reminderDTO.Description, reminderDTO.Watched);
            foreach (var tag in reminderDTO.Tags)
            {
                Tag createTag = _tagRepository.GetByName(tag.Name);
                if (createTag == null)
                {
                    createTag = new Tag(tag.Name,tag.Color);
                    ReminderTag reminderTag = new ReminderTag(createReminder, createTag);
                    createReminder.AddTag(reminderTag, createTag);
                    _tagRepository.Add(createTag);

                }
                else
                {
                    ReminderTag reminderTag = new ReminderTag(createReminder, createTag);
                    createReminder.AddTag(reminderTag, createTag);
                }

            }
            foreach (var checklist in reminderDTO.CheckList)
            {
                ChecklistHeader createChecklistHeader = new ChecklistHeader(checklist.Title, checklist.Volgorde, createReminder, checklist.Finished, checklist.Checked);
                foreach (var item in checklist.Items)
                {
                    ChecklistItem createChecklistItem = new ChecklistItem(item.Title, createChecklistHeader, item.Volgorde, item.Finished, item.Checked);

                }
            }
            createReminder.RecalculateProcessBar();
            _tagRepository.SaveChanges();
            _reminderRepository.SaveChanges();
            return CreatedAtAction(nameof(GetReminder), new { id = createReminder.ReminderId }, createReminder);
        }

        // [Put] api/Reminders{id}
        /// <summary>
        /// Updates the content of a reminder 
        /// </summary>
        /// <param name="id">the id of the reminder that needs changing</param>
        /// <param name="reminder">the reminder that has changed</param>
        /// <returns>if id doesn't equal the reminderid a badrequest wil be returned else a nocontent</returns>
        [HttpPut("{id}")]
        public IActionResult PutReminder(int id, Reminder reminder)
        {
            if (id != reminder.ReminderId)
            {
                return BadRequest();
            }
            _reminderRepository.Update(reminder);
            _reminderRepository.SaveChanges();
            return NoContent();
        }
        // [Delete] api/Reminders{id}
        /// <summary>
        /// Deletes a reminder and saves the changes
        /// </summary>
        /// <param name="id">the id of the soon to be deleted reminder</param>
        /// <returns>if id doesn't exista badrequest wil be returned else a nocontent</returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteReminder(int id)
        {
            Reminder reminder = _reminderRepository.GetById(id);
            if (reminder == null)
            {
                return NotFound();
            }
            _reminderRepository.Delete(reminder);
            _reminderRepository.SaveChanges();
            return NoContent();
        }
        #endregion

    }
}