
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

}
