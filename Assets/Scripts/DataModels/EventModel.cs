using System;
using System.Collections.Generic;

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

    public static EventModel[] GetEvents()
    {
        //Todo: implement this method
        EventModel[] events = new EventModel[] { new EventModel(_eventTitle: "e 1"), new EventModel(_eventTitle: "e 2") }; //Just a place holder
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

    public static void SaveEvent(ref EventModel newEvent){
        //Todo: implement this method
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
        List<TaskModel> orderedChildren = new List<TaskModel>();
        foreach (TaskModel child in children)
        {
            TaskModel earlestChild = child;
            foreach (TaskModel child2 in children)
            {
                if (DateTime.Compare(earlestChild.timeFrom, child2.timeFrom) > 0)
                {
                    earlestChild = child2;
                }
                else if (DateTime.Compare(earlestChild.timeFrom, child2.timeFrom) == 0)
                {
                    if (DateTime.Compare(earlestChild.timeTo, child2.timeTo) > 0)
                    {
                        earlestChild = child2;
                    }
                }
            }
            orderedChildren.Add(earlestChild);
            children.Remove(earlestChild);
        }
        parentEvent.isChildrenTasksOrdered = true;
        return orderedChildren.ToArray();
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
        parentEvent.isChildrenEventsOrdered = true;
        return orderedSlots.ToArray();
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
        parentEvent.isChildrenRemindersOrdered = true;
        return orderedSlots.ToArray();
    }

    public static EventTimeSlotModel[] SetTimeSlotParentEvent(EventModel parentEvent)
    {
        EventTimeSlotModel[] timeSlots = parentEvent.timeSlots;
        for (int i = 0; i < timeSlots?.Length; i++)
        {
            timeSlots[i].parentEvent = parentEvent;
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
    
    public static void StartEvent(EventModel _event, DateTime _time)
    {
        //Todo: implement this method
    }

    public static void FinishEvent(EventModel _event, DateTime _time)
    {
        //Todo: Implement this method
    }
}
