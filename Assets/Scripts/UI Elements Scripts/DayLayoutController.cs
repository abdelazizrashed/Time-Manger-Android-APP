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

    private TaskModel[] OrderTasks(TaskModel[] tasks)
    {
        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i].childrenTasks = TaskModel.GetTaskChildrenOrderedByStartTime(tasks[i]);
        }
        List<TaskModel> orderedTasks = new List<TaskModel>();
        foreach (TaskModel task in tasks)
        {
            TaskModel earlestTask = task;
            foreach (TaskModel task2 in tasks)
            {
                if (DateTime.Compare(earlestTask.timeFrom, task2.timeFrom) > 0)
                {
                    earlestTask = task2;
                }
                else if (DateTime.Compare(earlestTask.timeFrom, task2.timeFrom) == 0)
                {
                    if (DateTime.Compare(earlestTask.timeTo, task2.timeTo) > 0)
                    {
                        earlestTask = task2;
                    }
                }
            }
            orderedTasks.Add(earlestTask);
        }
        return orderedTasks.ToArray();
    }

    #endregion

    #endregion

}
