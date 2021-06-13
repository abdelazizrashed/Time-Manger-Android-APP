using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public class EventModel: System.Object
{
    public int eventID {get; set;}
    public string eventTitle { get; set; }
    public string eventDescription { get; set; }
    public int userID { get; set; }
    public EventTimeSlotModel[] timeSlots { get; set; }
    public ColorModel color { get; set; }
    public EventModel parentEvent { get; set; }
    public TaskModel[] childrenTasks { get; set; }
    public EventTimeSlotModel[] childrenEventsTimeSlots { get; set; }
    public ReminderTimeSlotModel[] childrenRemindersTimeSlots { get; set; }
    public bool isChildrenTasksOrdered { get; set; }
    public bool isChildrenEventsOrdered { get; set; }
    public bool isChildrenRemindersOrdered { get; set; }

    public EventModel(
        int _eventID = 0,
        string _eventTitle = "",
        string _eventDescription = "",
        int _userID = 0,
        EventTimeSlotModel[] _timeSlots = null,
        ColorModel _color = null,
        EventModel _parent = null
        )
    {
        eventID = _eventID;
        eventTitle = _eventTitle;
        eventDescription = _eventDescription;
        userID = _userID;
        timeSlots = _timeSlots;
        color = _color;
        parentEvent = _parent;
        childrenEventsTimeSlots = new EventTimeSlotModel[] { };
        childrenTasks = new TaskModel[] { };
        childrenRemindersTimeSlots = new ReminderTimeSlotModel[] { };
        isChildrenEventsOrdered = false;
        isChildrenRemindersOrdered = false;
        isChildrenTasksOrdered = false;

    }

    public static EventModel GetEventByEventID(int id)
    {
        string query = @" SELECT * FROM Events WHERE event_id = " + id.ToString() + " ;";
        EventModel[] events = Enumerable.ToArray < EventModel > (DBMan.Instance.ExecuteQueryAndReturnRows<EventModel>(query, dbDataReader =>
        {
            EventModel newEvent =  new EventModel(
                dbDataReader.GetInt32(0),
                dbDataReader.GetString(1),
                dbDataReader.GetString(2),
                dbDataReader.GetInt32(3),
                EventTimeSlotModel.GetTimeSlotsByParentEventID(dbDataReader.GetInt32(0)),
                ColorModel.GetColorByColorID(dbDataReader.GetInt32(4)),
                !String.IsNullOrEmpty(dbDataReader.GetValue(5).ToString()) ? EventModel.GetEventByEventID(dbDataReader.GetInt32(5)) : null
                );
            return newEvent;
        }));
        return events[0];
    }

    public static EventModel[] GetEvents()
    {
        string query = @"
                        SELECT * FROM Events;
                        ";
        EventModel[] events = Enumerable.ToArray < EventModel > (DBMan.Instance.ExecuteQueryAndReturnRows<EventModel>(query, dbDataReader =>
        {
            EventModel newEvent = new EventModel(
                dbDataReader.GetInt32(0),
                dbDataReader.GetString(1),
                dbDataReader.GetString(2),
                dbDataReader.GetInt32(3),
                EventTimeSlotModel.GetTimeSlotsByParentEventID(dbDataReader.GetInt32(0)),
                ColorModel.GetColorByColorID(dbDataReader.GetInt32(4)),
                !String.IsNullOrEmpty(dbDataReader.GetValue(5).ToString()) ? EventModel.GetEventByEventID(dbDataReader.GetInt32(5)) : null
                );
            //DBMan.Instance.PrintDataReader(dbDataReader);
            return newEvent;
        }));
        return events;
    }

    public static string[] GetEventsTitles(EventModel[] events)
    {
        List<string> eventsTitles = new List<string>();
        foreach(EventModel e in events)
        {
            eventsTitles.Add(e.eventTitle);
        }
        return eventsTitles.ToArray();
    }

    public static void SaveEvent(ref EventModel newEvent)
    {
        string query =
            "INSERT INTO Events (event_title, event_description, user_id, color_id, parent_event_id)"
            + "VALUES ( \""
            + newEvent.eventTitle + "\", \""
            + newEvent.eventDescription + "\", "
            + newEvent.userID + ", "
            + newEvent.color?.colorID;
        if(newEvent.parentEvent != null)
        {
            query += ", "
            + newEvent.parentEvent?.eventID
            + ");"
            ;
        }
        else
        {
            query += ",  null);";
        }
        newEvent.eventID = Convert.ToInt32(DBMan.Instance.ExecuteQueryAndReturnTheRowID(query));

        Debug.Log("debug  log works");
        Debug.Log(JsonConvert.SerializeObject(newEvent));
        for (int i = 0; i < newEvent.timeSlots?.Length; i++)
        {
            if (newEvent.timeSlots[i] == null)
            {
                Debug.Log("the time slot is null");
            }
            newEvent.timeSlots[i].parentEventID = newEvent.eventID;

            Debug.Log(JsonConvert.SerializeObject(newEvent));
            EventTimeSlotModel.SaveTimeSlot(ref newEvent.timeSlots[i]);
        }

    }

    public static void UpdateEvent(ref EventModel updatedEvent)
    {
        string query =
            "UPDATE Events"
            + "SET "
            + "event_title = " + updatedEvent.eventTitle + ", "
            + "event_description = " + updatedEvent.eventDescription + ", "
            + "user_id = " + updatedEvent.userID + ", "
            + "color_id = " + updatedEvent.color?.colorID
            + " WHERE event_id = " + updatedEvent.eventID + ";" ;
        DBMan.Instance.ExecuteQueryAndReturnRowsAffected(query);
        for (int i = 0; i < updatedEvent.timeSlots.Length; i++)
        {
            EventTimeSlotModel.UpdateTimeSlot(ref updatedEvent.timeSlots[i]);
        }
    }

    public static TaskModel[] GetChildrenTasksOrdered(ref EventModel parentEvent)
    {
        if (parentEvent?.isChildrenTasksOrdered ?? false)
        {
            return parentEvent.childrenTasks;
        }
        TaskModel[] tasks = TaskModel.GetTasks();
        List<TaskModel> children = new List<TaskModel>();
        for(int i = 0; i< tasks?.Length; i++)
        {
            if (tasks[i].parentEvent?.eventID == parentEvent.eventID)
            {
                tasks[i].childrenTasks = TaskModel.GetTaskChildrenOrderedByStartTime(ref tasks[i]);
                children.Add(tasks[i]);
            }
        }
        tasks = null;
        children.Sort((x, y) => DateTime.Compare(x.timeFrom, y.timeFrom));
        parentEvent.isChildrenTasksOrdered = true;
        return children.ToArray();
    }

    public static EventTimeSlotModel[] GetChildrenEventsTimeSlotsOrdered(ref EventModel parentEvent)
    {
        if (parentEvent?.isChildrenEventsOrdered ?? false)
        {
            return parentEvent.childrenEventsTimeSlots;
        }
        EventModel[] events = EventModel.GetEvents();
        List<EventModel> children = new List<EventModel>();
        for(int i = 0; i< events?.Length; i++)
        {
            if (events[i].parentEvent?.eventID == parentEvent.eventID)
            {
                events[i].childrenEventsTimeSlots = EventModel.GetChildrenEventsTimeSlotsOrdered(ref events[i]);
                children.Add(events[i]);
            }
        }
        events = null;
        List<EventTimeSlotModel> timeSlots = new List<EventTimeSlotModel>();
        parentEvent.timeSlots = EventModel.SetTimeSlotParentEvent(parentEvent);
        if(parentEvent.timeSlots != null)
        {
            timeSlots.AddRange(parentEvent.timeSlots);
        }
        foreach(EventModel child in children)
        {
            timeSlots.AddRange(EventModel.SetTimeSlotParentEvent(child));
        }
        timeSlots.Sort((x, y) => DateTime.Compare(x.timeFrom, y.timeFrom));
        
        parentEvent.isChildrenEventsOrdered = true;
        return timeSlots.ToArray();
    }

    public static ReminderTimeSlotModel[] GetChildrenReminderTimeSlotsOrdered(ref EventModel parentEvent)
    {
        if (parentEvent?.isChildrenRemindersOrdered ?? false)
        {
            return parentEvent.childrenRemindersTimeSlots;
        }
        ReminderModel[] reminders = ReminderModel.GetReminders();
        List<ReminderModel> children = new List<ReminderModel>();
        for(int i = 0; i<reminders?.Length; i++)
        {
            if (reminders[i].parentEvent?.eventID == parentEvent.eventID)
            {
                children.Add(reminders[i]);
            }
        }
        List<ReminderTimeSlotModel> timeSlots = new List<ReminderTimeSlotModel>();
        foreach (ReminderModel child in children)
        {
            timeSlots.AddRange(ReminderModel.SetTimeSlotParentReminder(child));
        }

        timeSlots.Sort((x, y) => DateTime.Compare(x.time, y.time));
        //List<ReminderTimeSlotModel> orderedSlots = new List<ReminderTimeSlotModel>();
        //foreach (ReminderTimeSlotModel timeSlot in timeSlots)
        //{
        //    ReminderTimeSlotModel earlestSlot = timeSlot;
        //    foreach (ReminderTimeSlotModel slot2 in timeSlots)
        //    {
        //        if (DateTime.Compare(earlestSlot.time, slot2.time) > 0)
        //        {
        //            earlestSlot = slot2;
        //        }
        //    }
        //    orderedSlots.Add(earlestSlot);
        //    timeSlots.Remove(earlestSlot);
        //}
        parentEvent.isChildrenRemindersOrdered = true;
        return timeSlots.ToArray();
    }

    public static EventTimeSlotModel[] SetTimeSlotParentEvent(EventModel parentEvent)
    {
        EventTimeSlotModel[] timeSlots = parentEvent.timeSlots;
        for (int i = 0; i < timeSlots?.Length; i++)
        {
            timeSlots[i].parentEventID = parentEvent.eventID;
        }
        return timeSlots;
    }

    public static EventTimeSlotModel[] OrderEventsTimeSlots(ref EventModel[] events)
    {
        for (int i = 0; i < events?.Length; i++)
        {
            events[i].childrenTasks = EventModel.GetChildrenTasksOrdered(ref events[i]);
            events[i].childrenEventsTimeSlots = EventModel.GetChildrenEventsTimeSlotsOrdered(ref events[i]);
            events[i].childrenRemindersTimeSlots = EventModel.GetChildrenReminderTimeSlotsOrdered(ref events[i]);
            events[i].timeSlots = EventModel.SetTimeSlotParentEvent(events[i]);
        }
        List<EventTimeSlotModel> timeSlots = new List<EventTimeSlotModel>();
        foreach (EventModel e in events)
        {
            e.timeSlots = EventModel.SetTimeSlotParentEvent(e);
            if (e.timeSlots != null)
            {
                timeSlots.AddRange(e.timeSlots);
            }
        }

        timeSlots.Sort((x, y) => DateTime.Compare(x.timeFrom, y.timeFrom));

        return timeSlots.ToArray();
    }
    
    public static void StartEvent(EventTimeSlotModel timeSlot, DateTime time)
    {
        string query =
            "UPDATE EventsTimeSlots SET "
            + "time_started = " + time.ToString()
            + " WHERE time_slot_id = " + timeSlot.timeSlotID + ";";
        DBMan.Instance.ExecuteQueryAndReturnRowsAffected(query);
    }

    public static void FinishEvent(EventTimeSlotModel timeSlot, DateTime time)
    {
        string query =
            "UPDATE EventsTimeSlots SET "
           + "time_finished = " + time.ToString() + ", "
           + "is_completed = " + 1 + " "
           + "WHERE time_slot_id = " + timeSlot.timeSlotID + ";";
        DBMan.Instance.ExecuteQueryAndReturnRowsAffected(query);
    }
}
