using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    public static EventSystem instance;

    private void Awake()
    {
        instance = this;
    }

    public event Action<Pages> onChangePage;
    public void ChangePage(Pages page)
    {
        if (onChangePage != null)
        {
            onChangePage(page);
        }
    }

    ///// <summary>
    ///// This event gets triggered when a list of Tasks list is needed.
    ///// A method attached to this event will get the list and then trigger
    ///// an event which will carry the list.
    ///// </summary>
    //public event Action needTasksLists;

    //public void NeedTasksList()
    //{
    //    if (needTasksLists != null)
    //    {
    //        needTasksLists();
    //    }
    //}

    ///// <summary>
    ///// This event get triggered when a list of tasks list is needed.
    ///// This event will carry the list needed.
    ///// </summary>
    //public event Action<TasksListModel[]> getTasksLists;
    //public void GetTasksLists(TasksListModel[] lists)
    //{
    //    if (getTasksLists != null)
    //    {
    //        getTasksLists(lists);
    //    }
    //}

    //public event Action<int> onCustomRecurrenceRepeatPeriodDropdownOptionSelected;
    //public void OnCustomRecurrenceRepeatPeriodDropdownOptionSelected(int index)
    //{
    //    if (onCustomRecurrenceRepeatPeriodDropdownOptionSelected != null)
    //    {
    //        onCustomRecurrenceRepeatPeriodDropdownOptionSelected(index);
    //    }
    //}

    //public event Action onAddTaskCustomRepeatCancel;
    //public void OnAddTaskCustomRepeatCancel()
    //{
    //    if(onAddTaskCustomRepeatCancel != null)
    //    {
    //        onAddTaskCustomRepeatCancel();
    //    }
    //}

    public event Action<RepeatModel, int> onAddTaskCustomRepeatSave;
    public void OnAddTaskCustomRepeatSave(RepeatModel repeat, int pNum)
    {
        if(onAddTaskCustomRepeatSave != null)
        {
            onAddTaskCustomRepeatSave(repeat, pNum);
        }
    }

    public event Action<NotifiAlarmReminderModel, int> onAddTaskCustomReminderSave;
    public void OnAddTaskCustomReminderSave(NotifiAlarmReminderModel reminder, int pNum)
    {
        if(onAddTaskCustomReminderSave != null)
        {
            onAddTaskCustomReminderSave(reminder, pNum);
        }
    }

    public event Action onNewTaskAdded;
    public void NewTaskAdded()
    {
        if(onNewTaskAdded != null)
        {
            onNewTaskAdded();
        }
    }

    public event Action onAddNewEvent;
    public void AddNewEvent()
    {
        if (onAddNewEvent != null)
        {
            onNewTaskAdded();
        }
    }

    public event Action onAddNewReminder;
    public void AddNewReminder()
    {
        if(onAddNewReminder != null)
        {
            onAddNewReminder();
        }
    }

    public event Action onAddNewTasksList;
    public void AddNewTasksList()
    {
        if(onAddNewTasksList != null)
        {
            onAddNewTasksList();
        }
    }

    public event Action closeSideMenuPanel;
    public void CloseSideMenuPanel()
    {
        if(closeSideMenuPanel != null)
        {
            closeSideMenuPanel();
        }
    }

    public event Action hideAddOptionsBtns;
    public void HideAddOptionsBtns()
    {
        if(hideAddOptionsBtns!= null)
        {
            hideAddOptionsBtns();
        }
    }
}
