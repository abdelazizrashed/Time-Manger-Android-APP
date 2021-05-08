using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayLayoutController : MonoBehaviour
{
    #region Main functions
    // Start is called before the first frame update
    void Start()
    {
        AttachEvents();
        OnStartUp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnApplicationQuit()
    {
        DeattachEvents();
    }

    #endregion

    #region Controls

    #region Gesture Control

    #endregion

    #region Page change 

    private Pages currentPage;

    private void OnAllPageSelected()
    {
        TaskModel[] tasks = TaskModel.GetTasks();
        EventModel[] events = EventModel.GetEvents();
        ReminderModel[] reminders = ReminderModel.GetReminders();
        List<object> all = new List<object>();
    }

    private void OnRemindersPageSelected()
    {
        //Todo: implement this method
    }

    private void OnEventsPageSelected()
    {
        //Todo: implement this method
    }

    #endregion

    #region Change Day

    #endregion

    #endregion

    #region Event System

    private void OnChangePage(Pages page)
    {
        if(page.Value == Pages.All.Value)
        {
            OnAllPageSelected();
        }else if(page.Value == Pages.Events.Value)
        {
            OnEventsPageSelected();
        }else if(page.Value == Pages.Reminders.Value)
        {
            OnRemindersPageSelected();
        }
    }

    private void AttachEvents()
    {
        EventSystem.instance.onChangePage += OnChangePage;
    }

    private void DeattachEvents()
    {
        EventSystem.instance.onChangePage -= OnChangePage;
    }

    #endregion

    #region General
    
    private void OnStartUp()
    {
        currentPage = Pages.All;
    }

    #region Helpers

    private TaskModel[] GetDaysTasksOrdered(DateTime day)
    {
        TaskModel[] tasks = TaskModel.GetTasks();
        List<TaskModel> daysTasks = new List<TaskModel>();
        foreach(TaskModel task in tasks)
        {
            if(task.timeFrom.Year == day.Year && task.timeFrom.Month == day.Month && task.timeFrom.Day == day.Day)
            {
                daysTasks.Add(task);
            }
        }
        tasks = null;
        tasks = daysTasks.ToArray();
        return TaskModel.OrderTasks(ref tasks);
    }
    private EventTimeSlotModel[] GetDaysEventsOrdered(DateTime day)
    {
        EventModel[] events = EventModel.GetEvents();
        EventTimeSlotModel[] timeSlots = EventModel.OrderEventsTimeSlots(ref events);
        List<EventTimeSlotModel> orderedSlots = new List<EventTimeSlotModel>();
        for(int i = 0; i< timeSlots.Length; i++)
        {
            if(timeSlots[i].dateFrom.Year == day.Year && timeSlots[i].dateFrom.Month == day.Month && timeSlots[i].dateFrom.Day == day.Day) { }
            {
                orderedSlots.Add(timeSlots[i]);
            }
        }
        return orderedSlots.ToArray();
    }
    private ReminderTimeSlotModel[] GetDaysRemindersOrdered(DateTime day)
    {
        ReminderModel[] reminders = ReminderModel.GetReminders();
        ReminderTimeSlotModel[] timeSlots = ReminderModel.OrderRemindersTimeSlots(ref reminders);
        List<ReminderTimeSlotModel> orderedSlots = new List<ReminderTimeSlotModel>();
        for (int i = 0; i < timeSlots.Length; i++)
        {
            if (timeSlots[i].date.Year == day.Year && timeSlots[i].date.Month == day.Month && timeSlots[i].date.Day == day.Day) { }
            {
                orderedSlots.Add(timeSlots[i]);
            }
        }
        return orderedSlots.ToArray();
    }

    #endregion

    #endregion

}
