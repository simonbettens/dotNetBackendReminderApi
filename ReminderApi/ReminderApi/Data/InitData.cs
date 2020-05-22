using Microsoft.AspNetCore.Identity;
using ReminderApi.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReminderApi.Data
{
    public class InitData
    {
        private readonly ReminderDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public InitData(ReminderDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task InitializeData()
        {
            _dbContext.Database.EnsureDeleted();
            if (_dbContext.Database.EnsureCreated())
            {
                DateTime huidigeDagEnTijd = DateTime.Today.AddDays(2);
                #region Users
                User simon = new User { Email = "simon.bettens@hogent.be", FirstName = "Simon", LastName = "Bettens" };
                _dbContext.User.Add(simon);
                
                await CreateUser(simon.Email, "P@ssword12345");

                User student = new User { Email = "student@hogent.be", FirstName = "Student", LastName = "Hogent" };
                _dbContext.User.Add(student);
                await CreateUser(student.Email, "P@ssword54321");
                #endregion

                #region Tags & Reminders
                Tag school = new Tag("School", "#b80404", simon);
                Tag web4 = new Tag("Web4", "#d48eed", simon);
                Tag project = new Tag("Project", "#b5ab24", simon);
                Tag intresses = new Tag("Intresses", "#43d433", simon);
                Tag flying = new Tag("Vliegen", "#2adbe8", simon);
                Tag airbus = new Tag("Airbus", "#339ed4", simon);
                Tag boeing = new Tag("Boeing", "#540acc", simon);
                Tag[] tags = { school, web4, project, intresses, flying, airbus, boeing };
                _dbContext.Tag.AddRange(tags);
                _dbContext.SaveChanges();

                Reminder web4Reminder = new Reminder("Web4 project", huidigeDagEnTijd, simon, decr:"Webapps project");
                Reminder projectReminder = new Reminder("Groepsproject", huidigeDagEnTijd, simon, decr: "Java en dotnet project");
                Reminder reminderAirbus = new Reminder("Nieuwe airbus", huidigeDagEnTijd, simon, watched: false,decr:"Lanceering airus");
                Reminder reminderBoeing = new Reminder("Nieuwe boeing", huidigeDagEnTijd, simon, watched: false);
                Reminder[] reminders = { web4Reminder, projectReminder, reminderAirbus, reminderBoeing };
                _dbContext.Reminder.AddRange(reminders);
                _dbContext.SaveChanges();
                #endregion

                #region ReminderTags
                ReminderTag reminderTag1 = new ReminderTag(web4Reminder, school);
                ReminderTag reminderTag2 = new ReminderTag(web4Reminder, web4);
                web4Reminder.AddTag(reminderTag1, school);
                web4Reminder.AddTag(reminderTag2, web4);

                ReminderTag reminderTag3 = new ReminderTag(projectReminder, school);
                ReminderTag reminderTag4 = new ReminderTag(projectReminder, project);
                projectReminder.AddTag(reminderTag3, school);
                projectReminder.AddTag(reminderTag4, project);

                ReminderTag reminderTag5 = new ReminderTag(reminderAirbus, flying);
                ReminderTag reminderTag6 = new ReminderTag(reminderAirbus, airbus);
                ReminderTag reminderTag9 = new ReminderTag(reminderAirbus, intresses);
                reminderAirbus.AddTag(reminderTag5, flying);
                reminderAirbus.AddTag(reminderTag6, airbus);
                reminderAirbus.AddTag(reminderTag9, intresses);

                ReminderTag reminderTag7 = new ReminderTag(reminderBoeing, flying);
                ReminderTag reminderTag8 = new ReminderTag(reminderBoeing, boeing);
                ReminderTag reminderTag10 = new ReminderTag(reminderBoeing, intresses);
                reminderBoeing.AddTag(reminderTag7, flying);
                reminderBoeing.AddTag(reminderTag8, boeing);
                reminderBoeing.AddTag(reminderTag10, intresses);

                ReminderTag[] reminderTags = { reminderTag1, reminderTag2, reminderTag3, reminderTag4, reminderTag5, reminderTag6, reminderTag7, reminderTag8 , reminderTag9 , reminderTag10 };

                _dbContext.ReminderTag.AddRange(reminderTags);
                _dbContext.SaveChanges();
                #endregion

                #region CheckList

                ChecklistHeader checklistHeader1 = new ChecklistHeader("Zoek video", 1, reminderBoeing);
                ChecklistItem checklistItem1 = new ChecklistItem("Ga naar YT", checklistHeader1, 1);
                ChecklistItem checklistItem2 = new ChecklistItem("Ga naar het kanaal", checklistHeader1, 2);
                checklistHeader1.CheckedChange(true);

                ChecklistHeader checklistHeader2 = new ChecklistHeader("Reageer op de video", 2, reminderBoeing);
                ChecklistItem checklistItem3 = new ChecklistItem("Druk op comments", checklistHeader2, 1);
                ChecklistItem checklistItem4 = new ChecklistItem("Schrijf tekst", checklistHeader2, 2);
                checklistItem4.ZetChecked(true);
                reminderBoeing.RecalculateProcessBar();

                ChecklistHeader clh3 = new ChecklistHeader("Maak backend", 1, web4Reminder, huidigeDagEnTijd, true);
                ChecklistItem cli5 = new ChecklistItem("Maak DCD en klassen", clh3, 1,huidigeDagEnTijd,true);
                ChecklistItem cli6 = new ChecklistItem("Maak controllers", clh3, 2, huidigeDagEnTijd, true);

                ChecklistHeader clh4 = new ChecklistHeader("Maak Frontend", 2, web4Reminder);
                ChecklistItem cli7 = new ChecklistItem("Components aanmaken", clh4, 1, huidigeDagEnTijd, true);
                ChecklistItem cli8 = new ChecklistItem("Generic dataservice maken", clh4, 2, huidigeDagEnTijd, true);
                ChecklistItem cli9 = new ChecklistItem("Testen maken", clh4, 3);
                ChecklistItem cli10 = new ChecklistItem("Extra tech", clh4, 4, huidigeDagEnTijd, true);

                ChecklistHeader clh5 = new ChecklistHeader(".NET maken", 1, projectReminder, huidigeDagEnTijd, true);
                ChecklistHeader clh6 = new ChecklistHeader("Java maken", 2, projectReminder);

                ChecklistHeader[] headers = { checklistHeader1, checklistHeader2, clh3, clh4, clh5, clh6 };
                ChecklistItem[] items = { checklistItem1, checklistItem2, checklistItem3, checklistItem4, cli5, cli6, cli7, cli8, cli9, cli10 };
                _dbContext.CheckListHeader.AddRange(headers);
                _dbContext.ChecklistItem.AddRange(items);
                _dbContext.SaveChanges();
                #endregion

            }
        }
        private async Task CreateUser(string email, string password)
        {
            var user = new IdentityUser { UserName = email, Email = email };
            await _userManager.CreateAsync(user, password);
        }
    }
}
