using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public class ReminderTimeSlotModel
{
    public int timeSlotID { get; set; }
    public DateTime time { get; set; }
    public DateTime date { get; set; }
    public DateTime timeDone { get; set; }
    public bool isCompleted { get; set; }
    public RepeatModel repeat { get; set; }
    public NotifiAlarmReminderModel[] reminders { get; set; }
    public ReminderModel parentReminder { get; set; }
    
    public ReminderTimeSlotModel(
        int id = 0,
        DateTime? _time = null,
        DateTime? _date = null,
        DateTime? _timeDone = null,
        int _isCompleted = 0,
        RepeatModel _repeat = null,
        NotifiAlarmReminderModel[] _reminders = null
        )
    {
        timeSlotID = id;
        time = _time ?? time;
        date = _date ?? date;
        timeDone = _timeDone ?? timeDone;
        isCompleted = _isCompleted == 1;
        repeat = _repeat;
        reminders = _reminders;
    }

    public static void SaveTimeSlot(ref ReminderTimeSlotModel timeSlot)
    {
        string query =
            "INSERT INTO ReminderTimeSlots(time, time_done, is_completed, repeat, reminder, reminder_id) " +
            "VALUES ( " +
            timeSlot.time + ", " +
            timeSlot.timeDone + ", " +
            timeSlot.isCompleted + ", " +
            JsonConvert.SerializeObject(timeSlot.repeat) + ", " +
            JsonConvert.SerializeObject(timeSlot.reminders) + ", " +
            timeSlot.parentReminder.reminderID + "); ";

        timeSlot.timeSlotID = Convert.ToInt32(DBMan.Instance.ExecuteQueryAndReturnTheRowID(query));

    }

    public static ReminderTimeSlotModel[] GetTimeSlotsByReminderID(int id)
    {
        string query = "SELECT * FROM RemindersTimeSlots WHERE reminder_id = " + id + ";";
        ReminderTimeSlotModel[] timeSlots = Enumerable.ToArray < ReminderTimeSlotModel > (DBMan.Instance.ExecuteQueryAndReturnRows<ReminderTimeSlotModel>(query, dbDataReader =>
        {
            return new ReminderTimeSlotModel(
                dbDataReader.GetInt32(0),
                DateTime.Parse(dbDataReader.GetString(1)),
                DateTime.Parse(dbDataReader.GetString(1)).Date,
                DateTime.Parse(dbDataReader.GetString(2)),
                dbDataReader.GetInt32(3),
                (RepeatModel)JsonConvert.DeserializeObject(dbDataReader.GetString(4)),
                (NotifiAlarmReminderModel[])JsonConvert.DeserializeObject(dbDataReader.GetString(5))
                );
        }));
        //IDataReader dbDataReader = DBMan.Instance.ExecuteQueryAndReturnDataReader(query);
        //List<ReminderTimeSlotModel> timeSlots = new List<ReminderTimeSlotModel>();
        //while (dbDataReader.Read())
        //{
        //    DBMan.Instance.PrintDataReader(dbDataReader);
        //    timeSlots.Add(new ReminderTimeSlotModel(
        //        dbDataReader.GetInt32(0), 
        //        DateTime.Parse(dbDataReader.GetString(1)), 
        //        DateTime.Parse(dbDataReader.GetString(1)).Date,
        //        DateTime.Parse(dbDataReader.GetString(2)),
        //        dbDataReader.GetInt32(3),
        //        (RepeatModel)JsonConvert.DeserializeObject(dbDataReader.GetString(4)),
        //        (NotifiAlarmReminderModel[]) JsonConvert.DeserializeObject(dbDataReader.GetString(5))
        //        ));
        //}
        //dbDataReader?.Close();
        //dbDataReader = null;
        return timeSlots;
    }
}
