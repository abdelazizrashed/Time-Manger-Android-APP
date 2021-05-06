
using System.Collections.Generic;

public class EventModel
{
    public int eventID {get; set;}
    public string eventTitle { get; set; }
    public string eventDescription { get; set; }
    public int userID { get; set; }
    public EventTimeSlotModel timeSlot { get; set; }
    public ColorModel color { get; set; }
    public EventModel parentEvent { get; set; }

    public EventModel(
        int id = 0,
        string title = "",
        string description = "",
        int uID = 0,
        EventTimeSlotModel tSlot = null,
        ColorModel c = null,
        EventModel parent = null
        )
    {
        eventID = id;
        eventTitle = title;
        eventDescription = description;
        userID = uID;
        timeSlot = tSlot;
        color = c;
        parentEvent = parent;
    }

    public static EventModel[] GetEvents()
    {
        //Todo: implement this method
        EventModel[] events = new EventModel[] { new EventModel(title: "e 1"), new EventModel(title: "e 2") }; //Just a place holder
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
}
