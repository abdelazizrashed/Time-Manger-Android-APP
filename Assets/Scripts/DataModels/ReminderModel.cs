using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ReminderModel: System.Object
{
    public int reminderID { get; set; }
    public string reminderTitle { get; set; }
    public string reminderDescription { get; set; }
    public int userID { get; set; }
    public ReminderTimeSlotModel[] timeSlots { get; set; }
    public ColorModel color { get; set; }
    public EventModel parentEvent { get; set; }

    public ReminderModel(
        int _reminderID = 0,
        string _reminderTitle = "",
        string _description = "",
        int _userID = 0,
        ReminderTimeSlotModel[] _timeSlots = null,
        ColorModel _color = null,
        EventModel _parentEvent = null
        )
    {
        reminderID = _reminderID;
        reminderTitle = _reminderTitle;
        reminderDescription = _description;
        timeSlots = _timeSlots;
        color = _color;
        parentEvent = _parentEvent;
    }

    public static void SaveReminder(ref ReminderModel reminder)
    {
        for (int i = 0; i< reminder.timeSlots.Length; i++)
        {
            ReminderTimeSlotModel.SaveTimeSlot(ref reminder.timeSlots[i]);
        }
        string query =
            "INSERT INTO Reminders (reminder_title, reminder_description, user_id, color_id, parent_event_id)" + " " +
            "VALUES (" +
            reminder.reminderTitle + ", " +
            reminder.reminderDescription + ", " +
            reminder.userID + ", " +
            reminder.color.colorID + ", " +
            reminder.parentEvent.eventID + ");";
        reminder.reminderID = Convert.ToInt32(DBMan.Instance.ExecuteQueryAndReturnTheRowID(query));
    }

    public static ReminderModel[] GetReminders()
    {
        string query = "SELECT * FROM Reminders;";
        IDataReader dbDataReader = DBMan.Instance.ExecuteQueryAndReturnDataReader(query);
        List<ReminderModel> reminders = new List<ReminderModel>();
        while (dbDataReader.Read())
        {
            reminders.Add(new ReminderModel(
                dbDataReader.GetInt32(0),
                dbDataReader.GetString(1),
                dbDataReader.GetString(2),
                dbDataReader.GetInt32(3),
                ReminderTimeSlotModel.GetTimeSlotsByReminderID(dbDataReader.GetInt32(0)),
                ColorModel.GetColorByColorID(dbDataReader.GetInt32(4)),
                EventModel.GetEventByEventID(dbDataReader.GetInt32(5))
                ));
        }
        return reminders.ToArray(); 
    }

    public static void MarkReminderDone(ReminderTimeSlotModel timeSlot, DateTime time)
    {
        string query = 
            "UPDATE ReminderTimeSlots " +
            "SET time_done = " + time.ToString() + 
            ", is_completed = " + 1 + 
            "WHERE time_slot_id = " + timeSlot.timeSlotID + ";";

        DBMan.Instance.ExecuteQueryAndReturnRowsAffected(query);
    }

    public static ReminderTimeSlotModel[] SetTimeSlotParentReminder(ReminderModel parentReminder)
    {
        ReminderTimeSlotModel[] timeSlots = parentReminder.timeSlots;
        for (int i = 0; i < timeSlots.Length; i++)
        {
            timeSlots[i].parentReminder = parentReminder;
        }
        return timeSlots;
    }

    public static ReminderTimeSlotModel[] OrderRemindersTimeSlots(ref ReminderModel[] reminders)
    {
        for (int i = 0; i < reminders.Length; i++)
        {
            reminders[i].timeSlots = ReminderModel.SetTimeSlotParentReminder(reminders[i]);
        }
        List<ReminderTimeSlotModel> timeSlots = new List<ReminderTimeSlotModel>();
        foreach (ReminderModel reminder in reminders)
        {
            timeSlots.AddRange(ReminderModel.SetTimeSlotParentReminder(reminder));
        }
        List<ReminderTimeSlotModel> orderedSlots = new List<ReminderTimeSlotModel>();
        foreach (ReminderTimeSlotModel timeSlot in timeSlots)
        {
            ReminderTimeSlotModel earlestSlot = timeSlot;
            foreach (ReminderTimeSlotModel slot2 in timeSlots)
            {
                if (DateTime.Compare(earlestSlot.time, slot2.time) > 0)
                {
                    earlestSlot = slot2;
                }
            }
            orderedSlots.Add(earlestSlot);
            timeSlots.Remove(earlestSlot);
        }

        return orderedSlots.ToArray();
    }
}
