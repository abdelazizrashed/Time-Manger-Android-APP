using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReminderTimeSlotModel
{
    public DateTime time { get; set; }
    public DateTime date { get; set; }
    public RepeatModel repeat { get; set; }
    public NotifiAlarmReminderModel[] reminders { get; set; }
    public ReminderModel parentReminder { get; set; }
    
    public ReminderTimeSlotModel(
        DateTime? _time = null,
        DateTime? _date = null,
        RepeatModel _repeat = null,
        NotifiAlarmReminderModel[] _reminders = null
        )
    {
        time = _time ?? time;
        date = _date ?? date;
        repeat = _repeat;
        reminders = _reminders;
    }
}
