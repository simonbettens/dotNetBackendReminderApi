﻿using ReminderApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReminderApi.Data
{
    public class InitData
    {
        private readonly ReminderDbContext _dbContext;

        public InitData(ReminderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void InitializeData()
        {
            _dbContext.Database.EnsureDeleted();
            if (_dbContext.Database.EnsureCreated())
            {
                DateTime huidigeDagEnTijd = DateTime.Today.AddDays(2);
                Tag spacex = new Tag("Spacex");
                Tag space = new Tag("Space");
                Tag nasa = new Tag("Nasa");
                Tag flying = new Tag("Flying");
                Tag airbus = new Tag("Airbus");
                Tag boeing = new Tag("Boeing");
                Tag[] tags = { space, spacex, nasa, flying, airbus, boeing };
                _dbContext.Tag.AddRange(tags);

                Reminder reminderRacket = new Reminder("Lancering raket", huidigeDagEnTijd, watched : true);           
                Reminder reminderAirbus = new Reminder("Nieuwe airbus", huidigeDagEnTijd, watched : false);           
                Reminder reminderBoeing = new Reminder("Nieuwe boeing", huidigeDagEnTijd, watched : false);
                Reminder[] reminders = { reminderRacket, reminderAirbus, reminderBoeing };
                _dbContext.Reminder.AddRange(reminders);

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
            }
        }
    }
}