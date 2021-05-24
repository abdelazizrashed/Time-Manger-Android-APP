using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

public class EventTimeSlotModel
{
    public int timeSlotID { get; set; }
    public DateTime timeFrom { get; set; }
    public DateTime timeTo { get; set; }
    public DateTime timeStarted { get; set; }
    public DateTime timeFinished { get; set; }
    public bool isCompleted { get; set; }
    public RepeatModel repeat { get; set; }
    public string location { get; set; }
    public NotifiAlarmReminderModel[] reminders { get; set; }
    public EventModel parentEvent { get; set; }

    public EventTimeSlotModel(
        int _timeSlotID = 0,
        DateTime? _timeFrom = null,
        DateTime? _timeTo = null,
        DateTime? _timeStarted = null,
        DateTime? _timeFinished = null,
        int _isCompleted = 0,
        RepeatModel _repeat = null,
        string _location = "",
        NotifiAlarmReminderModel[] _reminders = null
        )
    {
        timeSlotID = _timeSlotID;
        timeFrom = _timeFrom ?? timeFrom;
        timeTo = _timeTo ?? timeTo;
        timeStarted = _timeStarted ?? timeStarted;
        timeFinished = _timeFinished ?? timeFinished;
        isCompleted = _isCompleted == 1;
        repeat = _repeat;
        location = _location;
        reminders = _reminders;
    }

    public static void SaveTimeSlot(ref EventTimeSlotModel timeSlot)
    {

        string query =
            "INSERT INTO EventsTimeSlots(" +
            "time_from, " +
            "time_to, " +
            "time_started, " +
            "time_finished, " +
            "is_completed, " +
            "location, " +
            "repeat, " +
            "reminder, " +
            "event_id" + ")" +
            "VALUES ( " +
            timeSlot.timeFrom + ", " +
            timeSlot.timeTo + ", " +
            timeSlot.timeStarted + ", " +
            timeSlot.timeFinished + ", " +
            timeSlot.isCompleted + ", " +
            timeSlot.location + ", " +
            timeSlot.repeat.JSON() + ", " +
            JsonConvert.SerializeObject(timeSlot.reminders) + ", " +
            timeSlot.parentEvent.eventID + ");";

        timeSlot.timeSlotID = Convert.ToInt32(DBMan.Instance.ExecuteQueryAndReturnTheRowID(query));
    }

    public static void UpdateTimeSlot(ref EventTimeSlotModel timeSlot)
    {
        string query =
            "UPDATE EventsTimeSlots SET " +
            "time_from = " + timeSlot.timeFrom + ", " +
            "time_to = " + timeSlot.timeTo + ", " +
            "time_started = " + timeSlot.timeStarted + ", " +
            "time_finished = " + timeSlot.timeFinished + ", " +
            "is_completed = " + timeSlot.isCompleted + ", " +
            "location = " + timeSlot.location + ", " +
            "repeat = " + timeSlot.repeat.JSON() + ", " +
            "reminder = " + JsonConvert.SerializeObject(timeSlot.reminders) + ", " +
            "event_id = " + timeSlot.parentEvent.eventID + ", " +
            "WHERE time_slot_id = " + timeSlot.timeSlotID + "; ";
        DBMan.Instance.ExecuteQueryAndReturnRowsAffected(query);
    }

    public static EventTimeSlotModel[] GetTimeSlotsByParentEventID(int parentEventID)
    {
        string query = "SELECT * FROM EventsTimeSlots WHERE event_id = " + parentEventID + ";";
        IDataReader dbDataReader = DBMan.Instance.ExecuteQueryAndReturnDataReader(query);
        List<EventTimeSlotModel> timeSlots = new List<EventTimeSlotModel>();
        while (dbDataReader.Read())
        {
            timeSlots.Add(new EventTimeSlotModel(
                dbDataReader.GetInt32(0),
                DateTime.Parse(dbDataReader.GetString(1)),
                DateTime.Parse(dbDataReader.GetString(2)),
                DateTime.Parse(dbDataReader.GetString(3)),
                DateTime.Parse(dbDataReader.GetString(4)),
                dbDataReader.GetInt32(5),
                (RepeatModel)JsonConvert.DeserializeObject(dbDataReader.GetString(7)),
                dbDataReader.GetString(6),
                (NotifiAlarmReminderModel[])JsonConvert.DeserializeObject(dbDataReader.GetString(8))
                ));
        }
        return timeSlots.ToArray();
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
