
using System;
using System.Collections.Generic;

public class EventModel
{
    public int eventID {get; set;}
    public string eventTitle { get; set; }
    public string eventDescription { get; set; }
    public int userID { get; set; }
    public EventTimeSlotModel[] timeSlots { get; set; }
    public ColorModel color { get; set; }
    public EventModel parentEvent { get; set; }
    public TaskModel[] childrenTasks { get; set; }
    public EventModel[] childrenEvents { get; set; }
    public ReminderModel[] childrenReminders { get; set; }

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

    public static void SaveEvent(EventModel newEvent){
        //Todo: implement this method
    }

    public static TaskModel[] GetChildrenTasksOrdered(EventModel parentEvent)
    {

        TaskModel[] tasks = TaskModel.GetTasks();
        List<TaskModel> children = new List<TaskModel>();
        foreach (TaskModel child in tasks)
        {
            if (child.parentEvent.eventID == parentEvent.eventID)
            {
                children.Add(child);
            }
        }
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
        }

        return orderedChildren.ToArray();
    }

    public static EventTimeSlotModel[] GetChildrenEventsOrdered(EventModel parentEvent)
    {
        EventTimeSlotModel[] timeSlots = EventModel.SetTimeSlotParentEvent(parentEvent);
        List<EventTimeSlotModel> children = new List<EventTimeSlotModel>();
        foreach (EventTimeSlotModel child in timeSlots)
        {
            if (child.parentEvent.eventID == parentEvent.eventID)
            {
                children.Add(child);
            }
        }
        List<EventModel> orderedChildren = new List<EventModel>();
        foreach (EventModel child in children)
        {
            EventModel earlestChild = child;
            foreach (EventModel child2 in children)
            {
                if (DateTime.Compare(earlestChild., child2.timeFrom) > 0)
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
        }

        return orderedChildren.ToArray();
    }

    public static TaskModel[] GetChildrenTasksOrdered(EventModel parentEvent)
    {

        TaskModel[] tasks = TaskModel.GetTasks();
        List<TaskModel> children = new List<TaskModel>();
        foreach (TaskModel child in tasks)
        {
            if (child.parentEvent.eventID == parentEvent.eventID)
            {
                children.Add(child);
            }
        }
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
        }

        return orderedChildren.ToArray();
    }

    public static EventTimeSlotModel[] SetTimeSlotParentEvent(EventModel parentEvent)
    {
        EventTimeSlotModel[] timeSlots = parentEvent.timeSlots;
        for (int i = 0; i < timeSlots.Length; i++)
        {
            timeSlots[i].parentEvent = parentEvent;
        }
        return timeSlots;

}
