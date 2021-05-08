using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTimeSlotModel
{
    public DateTime dateFrom { get; set; }
    public DateTime timeFrom { get; set; }
    public DateTime dateTo { get; set; }
    public DateTime timeTo { get; set; }
    public RepeatModel repeat { get; set; }
    public string location { get; set; }
    public NotifiAlarmReminderModel[] reminders { get; set; }
    public EventModel parentEvent { get; set; }

    public EventTimeSlotModel(
        DateTime? _timeFrom = null,
        DateTime? _dateFrom = null,
        DateTime? _timeTo = null,
        DateTime? _dateTo = null,
        RepeatModel _repeat = null,
        string _location = "",
        NotifiAlarmReminderModel[] _reminders = null,
        EventModel _parentEvent = null
        )
    {
        dateFrom = _dateFrom ?? dateFrom;
        dateTo = _dateTo ?? dateTo;
        timeFrom = _timeFrom ?? timeFrom;
        timeTo = _timeTo ?? timeTo;
        repeat = _repeat;
        location = _location;
        reminders = _reminders;
        parentEvent = _parentEvent;
    }

    public static EventTimeSlotModel[] OrderTimeSlots(List<EventTimeSlotModel> timeSlots)
    {
        List<EventTimeSlotModel> orderedSlots = new List<EventTimeSlotModel>();
        foreach (EventTimeSlotModel timeSlot in timeSlots)
        {
            EventTimeSlotModel earlestSlot = timeSlot;
            foreach (EventTimeSlotModel slot2 in timeSlots)
            {
                if (DateTime.Compare(earlestSlot.timeFrom, slot2.timeFrom) > 0)
                {
                    earlestSlot = slot2;
                }
                else if (DateTime.Compare(earlestSlot.timeFrom, slot2.timeFrom) == 0)
                {
                    if (DateTime.Compare(earlestSlot.timeTo, slot2.timeTo) > 0)
                    {
                        earlestSlot = slot2;
                    }
                }
            }
            orderedSlots.Add(earlestSlot);
            timeSlots.Remove(earlestSlot);
        }

        return orderedSlots.ToArray();
    }
}
