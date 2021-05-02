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

    public event Action<string> onChangePage;
    public void ChangePage(string pageTitle)
    {
        if (onChangePage != null)
        {
            onChangePage(pageTitle);
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

    public event Action<RepeatModel> onAddTaskCustomRepeatSave;
    public void OnAddTaskCustomRepeatSave(RepeatModel repeat)
    {
        if(onAddTaskCustomRepeatSave != null)
        {
            onAddTaskCustomRepeatSave(repeat);
        }
    }

}
