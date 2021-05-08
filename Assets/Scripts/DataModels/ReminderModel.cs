using System;
using System.Collections;
using System.Collections.Generic;
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

    public static void SaveReminder(ReminderModel reminder)
    {

    }

    public static ReminderModel[] GetReminders()
    {
        //Todo: implement this method
        return new ReminderModel[] { new ReminderModel(_reminderTitle: "reminder 1"), new ReminderModel(_reminderTitle: "reminder 2"), new ReminderModel(_reminderTitle: "reminder 3"), };
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
