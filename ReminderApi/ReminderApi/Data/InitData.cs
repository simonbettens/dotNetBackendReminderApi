﻿using Microsoft.AspNetCore.Identity;
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
                Tag spacex = new Tag("Spacex", "#b80404", simon);
                Tag space = new Tag("Space", "#b5ab24", simon);
                Tag nasa = new Tag("Nasa", "#43d433", simon);
                Tag flying = new Tag("Flying", "#2adbe8", simon);
                Tag airbus = new Tag("Airbus", "#339ed4", simon);
                Tag boeing = new Tag("Boeing", "#540acc", simon);
                Tag[] tags = { space, spacex, nasa, flying, airbus, boeing };
                _dbContext.Tag.AddRange(tags);
                _dbContext.SaveChanges();

                Reminder reminderRacket = new Reminder("Lancering raket", huidigeDagEnTijd, simon, watched: true);
                Reminder reminderAirbus = new Reminder("Nieuwe airbus", huidigeDagEnTijd, simon, watched: false,decr:"Test description");
                Reminder reminderBoeing = new Reminder("Nieuwe boeing", huidigeDagEnTijd, simon, watched: false);
                Reminder[] reminders = { reminderRacket, reminderAirbus, reminderBoeing };
                _dbContext.Reminder.AddRange(reminders);
                _dbContext.SaveChanges();
                #endregion

                #region ReminderTags
                ReminderTag reminderTag1 = new ReminderTag(reminderRacket, spacex);
                ReminderTag reminderTag2 = new ReminderTag(reminderRacket, space);
                ReminderTag reminderTag3 = new ReminderTag(reminderRacket, nasa);
                ReminderTag reminderTag4 = new ReminderTag(reminderRacket, flying);
                reminderRacket.AddTag(reminderTag1, spacex);
                reminderRacket.AddTag(reminderTag2, space);
                reminderRacket.AddTag(reminderTag3, nasa);
                reminderRacket.AddTag(reminderTag4, flying);

                ReminderTag reminderTag5 = new ReminderTag(reminderAirbus, flying);
                ReminderTag reminderTag6 = new ReminderTag(reminderAirbus, airbus);
                reminderAirbus.AddTag(reminderTag5, flying);
                reminderAirbus.AddTag(reminderTag6, airbus);

                ReminderTag reminderTag7 = new ReminderTag(reminderBoeing, flying);
                ReminderTag reminderTag8 = new ReminderTag(reminderBoeing, boeing);
                reminderBoeing.AddTag(reminderTag7, flying);
                reminderBoeing.AddTag(reminderTag8, boeing);

                ReminderTag[] reminderTags = { reminderTag1, reminderTag2, reminderTag3, reminderTag4, reminderTag5, reminderTag6, reminderTag7, reminderTag8 };

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
                ChecklistHeader[] headers = { checklistHeader1, checklistHeader2 };
                ChecklistItem[] items = { checklistItem1, checklistItem2, checklistItem3, checklistItem4 };
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
