using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReminderApi.Models.Domain;
using ReminderApi.Models.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace ReminderApi.Controllers
{
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class RemindersController : ControllerBase
    {
        private readonly IReminderRepository _reminderRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IUserRepository _userRepository;
        public RemindersController(IReminderRepository reminderRepository, ITagRepository tagRepository,IUserRepository userRepository)
        {
            this._reminderRepository = reminderRepository;
            this._tagRepository = tagRepository;
            this._userRepository = userRepository;
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
            User user = _userRepository.GetBy(User.Identity.Name);
            IEnumerable<Reminder> reminders;
            if (string.IsNullOrEmpty(title) && string.IsNullOrEmpty(tagname))
            {
                reminders = _reminderRepository.GetAllExcludeWatched(user.UserId);
            }
            else
            {
                reminders = _reminderRepository.GetBy(user.UserId,title, tagname);
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
            User user = _userRepository.GetBy(User.Identity.Name);
            Reminder createReminder = new Reminder(reminderDTO.Title, reminderDTO.DatumReleased, user, reminderDTO.Link, reminderDTO.Description, reminderDTO.Watched);
            foreach (var tag in reminderDTO.Tags)
            {
                Tag createTag = _tagRepository.GetByName(tag.Name);
                if (createTag == null)
                {
                    createTag = new Tag(tag.Name,tag.Color, user);
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
        public IActionResult PutReminder(int id, ReminderDTO reminder)
        {
            User user = _userRepository.GetBy(User.Identity.Name);
            Reminder oldReminder = _reminderRepository.GetById(id);
            if (oldReminder == null) {
               return NotFound();
            }
            foreach (var tag in reminder.Tags)
            {
                bool notExist = oldReminder.Tags.FirstOrDefault(t => t.TagName == tag.Name) == null;
                if (notExist)
                {
                    Tag createTag = _tagRepository.GetByName(tag.Name);
                    if (createTag == null)
                    {
                        createTag = new Tag(tag.Name, tag.Color, user);
                        ReminderTag reminderTag = new ReminderTag(oldReminder, createTag);
                        oldReminder.AddTag(reminderTag, createTag);
                        _tagRepository.Add(createTag);

                    }
                    else
                    {
                        ReminderTag reminderTag = new ReminderTag(oldReminder, createTag);
                        oldReminder.AddTag(reminderTag, createTag);
                    }
                }
            }
            foreach (var checklist in reminder.CheckList)
            {
                ChecklistHeader oldHeader = oldReminder.Checklist.FirstOrDefault(c => c.Title == checklist.Title);
                bool notExist = oldHeader == null;
                if (notExist) {
                    ChecklistHeader createChecklistHeader = new ChecklistHeader(checklist.Title, checklist.Volgorde, oldReminder, checklist.Finished, checklist.Checked);
                    foreach (var item in checklist.Items)
                    {
                        ChecklistItem createChecklistItem = new ChecklistItem(item.Title, createChecklistHeader, item.Volgorde, item.Finished, item.Checked);

                    }
                }
                else {
                    oldHeader.Checked = checklist.Checked;
                    oldHeader.Finished = checklist.Finished;
                    oldHeader.Volgorde = checklist.Volgorde;
                    foreach (var item in checklist.Items)
                    {
                        ChecklistItem oldItem = oldHeader.Items.FirstOrDefault(c => c.Title == item.Title);
                        oldItem.Checked = item.Checked;
                        oldItem.Finished = item.Finished;
                        oldItem.Volgorde = item.Volgorde;
                    }
                }
            }
            oldReminder.RecalculateProcessBar();
            _reminderRepository.Update(oldReminder);
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