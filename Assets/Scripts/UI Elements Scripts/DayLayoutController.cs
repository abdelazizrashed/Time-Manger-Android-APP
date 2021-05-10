using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    #region Variables
    [HideInInspector]
    public Pages currentPage;
    [HideInInspector]
    public DateTime currentDay;

    public GameObject dayInfo;

    #endregion

    private void DestroyAllChildren(GameObject parent)
    {
        while(parent.transform.childCount != 0)
        {
            Destroy(parent.transform.GetChild(0).gameObject);
        }
    }

    #region Page change 

    public GameObject dayLayoutContent;
    public GameObject dayLayoutElementPrefab;
    public GameObject rowPrefab;
    public GameObject columnPrefab;
    public GameObject emptyPanelPrefab;


    //private void OnAllPageSelected()
    //{
    //    object[] all = Helper.MergeSort3ArraysByTime(
    //        GetDaysTasksOrdered(currentDay),
    //        GetDaysEventsOrdered(currentDay),
    //        GetDaysRemindersOrdered(currentDay)
    //        );

    //    for (int i = 0; i < all.Length; i++)
    //    {
    //        DateTime currentChildTime = new DateTime();
    //        if (all[i] is TaskModel)
    //        {
    //            currentChildTime = ((TaskModel)all[i]).timeFrom;
    //        }
    //        if (all[i] is EventTimeSlotModel)
    //        {
    //            currentChildTime = ((EventTimeSlotModel)all[i]).timeFrom;
    //        }
    //        if (all[i] is ReminderTimeSlotModel)
    //        {
    //            currentChildTime = ((ReminderTimeSlotModel)all[i]).time;
    //        }
    //        DateTime previousChildTime = new DateTime
    //        if (i + 1 < all.Length)
    //        {
    //            DateTime nextChildTime = new DateTime();
    //            if (all[i + 1] is TaskModel)
    //            {
    //                nextChildTime = ((TaskModel)all[i + 1]).timeFrom;
    //            }
    //            if (all[i + 1] is EventTimeSlotModel)
    //            {
    //                nextChildTime = ((EventTimeSlotModel)all[i + 1]).timeFrom;
    //            }
    //            if (all[i + 1] is ReminderTimeSlotModel)
    //            {
    //                nextChildTime = ((ReminderTimeSlotModel)all[i + 1]).time;
    //            }

    //            if (DateTime.Compare(currentChildTime, nextChildTime) == 0)
    //            {
    //                GameObject rowGameObject = Instantiate(rowPrefab, columnGameObject.transform);
    //                GameObject child1Element = Helper.Instantiate<GameObject>(dayLayoutElementPrefab, rowGameObject.transform, (obj) =>
    //                {
    //                    if (all[i] is TaskModel)
    //                    {
    //                        obj.GetComponent<DayLayoutElementController>().currentTask = (TaskModel)all[i];
    //                    }
    //                    if (all[i] is EventTimeSlotModel)
    //                    {
    //                        obj.GetComponent<DayLayoutElementController>().currentEventTimeSlot = (EventTimeSlotModel)all[i];
    //                    }
    //                    if (all[i] is ReminderTimeSlotModel)
    //                    {
    //                        obj.GetComponent<DayLayoutElementController>().currentReminderTimeSlot = (ReminderTimeSlotModel)all[i];
    //                    }
    //                    obj.GetComponent<DayLayoutElementController>().currentPageType = currentPageType;
    //                    obj.GetComponent<DayLayoutElementController>().currentDate = currentDate;
    //                });
    //                while (DateTime.Compare(currentChildTime, nextChildTime) == 0 && i + 1 < currentEventTimeSlot.parentEvent.allEventsTimeSlots.Length)
    //                {
    //                    i++;
    //                    GameObject child2Element = Helper.Instantiate<GameObject>(dayLayoutElementPrefab, rowGameObject.transform, (obj) =>
    //                    {
    //                        if (all[i] is TaskModel)
    //                        {
    //                            obj.GetComponent<DayLayoutElementController>().currentTask = (TaskModel)all[i];
    //                        }
    //                        if (all[i] is EventTimeSlotModel)
    //                        {
    //                            obj.GetComponent<DayLayoutElementController>().currentEventTimeSlot = (EventTimeSlotModel)all[i];
    //                        }
    //                        if (all[i] is ReminderTimeSlotModel)
    //                        {
    //                            obj.GetComponent<DayLayoutElementController>().currentReminderTimeSlot = (ReminderTimeSlotModel)all[i];
    //                        }
    //                        obj.GetComponent<DayLayoutElementController>().currentPageType = currentPageType;
    //                        obj.GetComponent<DayLayoutElementController>().currentDate = currentDate;
    //                    });
    //                    if (all[i + 1] is TaskModel)
    //                    {
    //                        nextChildTime = ((TaskModel)all[i + 1]).timeFrom;
    //                    }
    //                    if (all[i + 1] is EventTimeSlotModel)
    //                    {
    //                        nextChildTime = ((EventTimeSlotModel)all[i + 1]).timeFrom;
    //                    }
    //                    if (all[i + 1] is ReminderTimeSlotModel)
    //                    {
    //                        nextChildTime = ((ReminderTimeSlotModel)all[i + 1]).time;
    //                    }
    //                }
    //                continue;
    //            }

    //        }

    //        GameObject childElement = Helper.Instantiate<GameObject>(dayLayoutElementPrefab, columnGameObject.transform, (obj) =>
    //        {
    //            if (all[i] is TaskModel)
    //            {
    //                obj.GetComponent<DayLayoutElementController>().currentTask = (TaskModel)all[i];
    //            }
    //            if (all[i] is EventTimeSlotModel)
    //            {
    //                obj.GetComponent<DayLayoutElementController>().currentEventTimeSlot = (EventTimeSlotModel)all[i];
    //            }
    //            if (all[i] is ReminderTimeSlotModel)
    //            {
    //                obj.GetComponent<DayLayoutElementController>().currentReminderTimeSlot = (ReminderTimeSlotModel)all[i];
    //            }
    //            obj.GetComponent<DayLayoutElementController>().currentPageType = currentPageType;
    //            obj.GetComponent<DayLayoutElementController>().currentDate = currentDate;
    //        });
    //    }
    //}

    private void OnTaskPageSelected()
    {
        DestroyAllChildren(dayLayoutContent);
        if (currentPage.Value == Pages.Tasks.Value)
        {
            TasksListModel list = currentPage.tasksList;
            TaskModel[] tasks = GetDaysTasksOrdered(currentDay);
            for (int i = 0; i < tasks.Length; i++)
            {
                if(DateTime.Compare(tasks[i-1].timeTo, tasks[i].timeFrom) < 0)
                {
                    float duration = (float)(tasks[i].timeFrom - tasks[i - 1].timeTo).TotalHours;
                    GameObject emptyPlaceHolder = Instantiate(emptyPanelPrefab, dayLayoutContent.transform);
                    emptyPlaceHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(540f, duration * 60f);
                }
                if (i + 1 < tasks.Length)
                {
                    if (
                        DateTime.Compare(
                            tasks[i].timeFrom,
                            tasks[i + 1].timeFrom
                            ) == 0
                            )
                    {
                        GameObject rowGameObject = Instantiate(rowPrefab, dayLayoutContent.transform);
                        GameObject child1Element = Helper.Instantiate<GameObject>(dayLayoutElementPrefab, rowGameObject.transform, (obj) =>
                        {
                            obj.GetComponent<DayLayoutElementController>().currentTask = tasks[i];
                            obj.GetComponent<DayLayoutElementController>().currentPageType = currentPage;
                            obj.GetComponent<DayLayoutElementController>().currentDate = currentDay;
                        });
                        while (
                            DateTime.Compare(
                            tasks[i].timeFrom,
                            tasks[i + 1].timeFrom
                            ) == 0 &&
                            i + 1 < tasks.Length
                            )
                        {
                            i++;
                            GameObject child2Element = Helper.Instantiate<GameObject>(dayLayoutElementPrefab, rowGameObject.transform, (obj) =>
                            {
                                obj.GetComponent<DayLayoutElementController>().currentTask = tasks[i];
                                obj.GetComponent<DayLayoutElementController>().currentPageType = currentPage;
                                obj.GetComponent<DayLayoutElementController>().currentDate = currentDay;
                            });
                        }
                        continue;
                    }
                }
                GameObject childElement = Helper.Instantiate<GameObject>(dayLayoutElementPrefab, dayLayoutContent.transform, (obj) =>
                {
                    obj.GetComponent<DayLayoutElementController>().currentTask = tasks[i];
                    obj.GetComponent<DayLayoutElementController>().currentPageType = currentPage;
                    obj.GetComponent<DayLayoutElementController>().currentDate = currentDay;
                });
            }

        }
    }

    private void OnRemindersPageSelected()
    {
        DestroyAllChildren(dayLayoutContent);
        ReminderTimeSlotModel[] timeSlots = GetDaysRemindersOrdered(currentDay);
        for (int i = 0; i < timeSlots.Length; i++)
        {
            if(DateTime.Compare(timeSlots[i-1].time, timeSlots[i].time) != 0)
            {
                float duration = (float)(timeSlots[i].time - timeSlots[i - 1].time).TotalHours;
                GameObject emptyPlaceHolder = Instantiate(emptyPanelPrefab, dayLayoutContent.transform);
                emptyPlaceHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(540f, duration * 60f - 15f);

            }

            if(i+1 < timeSlots.Length)
            {
                if (DateTime.Compare(timeSlots[i].time, timeSlots[i + 1].time) == 0)
                {
                    GameObject rowGameObject = Instantiate(rowPrefab, dayLayoutContent.transform);
                    GameObject child1Element = Helper.Instantiate<GameObject>(dayLayoutElementPrefab, rowGameObject.transform, (obj) =>
                    {
                        obj.GetComponent<DayLayoutElementController>().currentReminderTimeSlot = timeSlots[i];
                        obj.GetComponent<DayLayoutElementController>().currentPageType = currentPage;
                        obj.GetComponent<DayLayoutElementController>().currentDate = currentDay;
                    });
                    while(DateTime.Compare(timeSlots[i].time, timeSlots[i + 1].time) == 0 && i + 1 < timeSlots.Length)
                    {
                        i++;
                        GameObject child2Element = Helper.Instantiate<GameObject>(dayLayoutElementPrefab, rowGameObject.transform, (obj) =>
                        {
                            obj.GetComponent<DayLayoutElementController>().currentReminderTimeSlot = timeSlots[i];
                            obj.GetComponent<DayLayoutElementController>().currentPageType = currentPage;
                            obj.GetComponent<DayLayoutElementController>().currentDate = currentDay;
                        });
                    }
                    continue;
                }
                GameObject childElement = Helper.Instantiate<GameObject>(dayLayoutElementPrefab, dayLayoutContent.transform, (obj) =>
                {
                    obj.GetComponent<DayLayoutElementController>().currentReminderTimeSlot = timeSlots[i];
                    obj.GetComponent<DayLayoutElementController>().currentPageType = currentPage;
                    obj.GetComponent<DayLayoutElementController>().currentDate = currentDay;
                });
            }
        }
    }

    private void OnEventsPageSelected()
    {
        DestroyAllChildren(dayLayoutContent);
        EventTimeSlotModel[] eventTimeSlots = GetDaysEventsOrdered(currentDay);
        for(int i = 0; i< eventTimeSlots.Length; i++)
        {
            if (i + 1 < eventTimeSlots.Length)
            {
                if (DateTime.Compare(eventTimeSlots[i - 1].timeTo, eventTimeSlots[i].timeFrom) < 0)
                {
                    float duration = (float)(eventTimeSlots[i].timeFrom - eventTimeSlots[i - 1].timeTo).TotalHours;
                    GameObject emptyPlaceHolder = Instantiate(emptyPanelPrefab, dayLayoutContent.transform);
                    emptyPlaceHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(540f, duration * 60f);
                }
                if (
                    DateTime.Compare(
                        eventTimeSlots[i].timeFrom,
                        eventTimeSlots[i+1].timeFrom
                        ) == 0
                        )
                {
                    GameObject rowGameObject = Instantiate(rowPrefab, dayLayoutContent.transform);
                    GameObject child1Element = Helper.Instantiate<GameObject>(dayLayoutElementPrefab, rowGameObject.transform, (obj) =>
                    {
                        obj.GetComponent<DayLayoutElementController>().currentEventTimeSlot = eventTimeSlots[i];
                        obj.GetComponent<DayLayoutElementController>().currentPageType = currentPage;
                        obj.GetComponent<DayLayoutElementController>().currentDate = currentDay;
                    });
                    while (
                        DateTime.Compare(
                        eventTimeSlots[i].timeFrom,
                        eventTimeSlots[i + 1].timeFrom
                            ) == 0 &&
                        i + 1 < eventTimeSlots.Length
                        )
                    {
                        i++;
                        GameObject child2Element = Helper.Instantiate<GameObject>(dayLayoutElementPrefab, rowGameObject.transform, (obj) =>
                        {
                            obj.GetComponent<DayLayoutElementController>().currentEventTimeSlot = eventTimeSlots[i];
                            obj.GetComponent<DayLayoutElementController>().currentPageType = currentPage;
                            obj.GetComponent<DayLayoutElementController>().currentDate = currentDay;
                        });
                    }
                    continue;
                }
            }
            GameObject childElement = Helper.Instantiate<GameObject>(dayLayoutElementPrefab, dayLayoutContent.transform, (obj) =>
            {
                obj.GetComponent<DayLayoutElementController>().currentEventTimeSlot = eventTimeSlots[i];
                obj.GetComponent<DayLayoutElementController>().currentPageType = currentPage;
                obj.GetComponent<DayLayoutElementController>().currentDate = currentDay;
            });
        }
    }

    #endregion

    #region Change Day


    #endregion

    #endregion

    #region Event System

    private void OnChangePage(Pages page)
    {
        if(page.Value == Pages.Events.Value)
        {
            OnEventsPageSelected();
        }else if(page.Value == Pages.Reminders.Value)
        {
            OnRemindersPageSelected();
        }else if(page.Value == Pages.Tasks.Value)
        {
            OnTaskPageSelected();
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
        if (currentPage != null && currentDay != null)
        {
            if (currentPage.Value == Pages.Events.Value)
            {
                OnEventsPageSelected();
            }else if(currentPage.Value == Pages.Reminders.Value)
            {
                OnRemindersPageSelected();
            }else if(currentPage.Value == Pages.Tasks.Value)
            {
                OnTaskPageSelected();
            }
            else
            {
                Debug.LogError("You attached the wrong type of page");
            }

            dayInfo.GetComponentInChildren<TMP_Text>().text = currentDay.ToString("MMM dd, yyyy");
            if (dayInfo.GetComponentInChildren<TMP_Text>() == null)
            {
                Debug.LogError("The text component of the dayinfo is null");
            }
            if(DateTime.Compare(currentDay.Date, DateTime.Now.Date) == 0)
            {
                dayInfo.GetComponent<Image>().color = new Color(43/255f, 47 / 255f, 212 / 255f);
            }

        }
        else
        {
            Debug.LogError("You forgot to set the current page and the current day");
        }
    }

    #region Helpers

    private TaskModel[] GetDaysTasksOrdered(DateTime day, TasksListModel list = null)
    {
        TaskModel[] tasks = { };
        if(list != null)
        {
            tasks = TaskModel.GetListTasks(list);
        }
        else
        {
            tasks = TaskModel.GetTasks();
        }
        List<TaskModel> daysTasks = new List<TaskModel>();
        foreach(TaskModel task in tasks)
        {
            if(DateTime.Compare(task.timeFrom.Date, day.Date) == 0 || DateTime.Compare(task.timeTo.Date, day.Date) == 0)
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
            if(DateTime.Compare(timeSlots[i].dateFrom.Date, day.Date) == 0 || DateTime.Compare(timeSlots[i].dateTo.Date, day.Date) == 0)
            {
                orderedSlots.Add(timeSlots[i]);
            }
        }
        return EventTimeSlotModel.OrderTimeSlots(orderedSlots);
    }
    private ReminderTimeSlotModel[] GetDaysRemindersOrdered(DateTime day)
    {
        ReminderModel[] reminders = ReminderModel.GetReminders();
        ReminderTimeSlotModel[] timeSlots = ReminderModel.OrderRemindersTimeSlots(ref reminders);
        List<ReminderTimeSlotModel> orderedSlots = new List<ReminderTimeSlotModel>();
        for (int i = 0; i < timeSlots.Length; i++)
        {
            if (DateTime.Compare(timeSlots[i].date.Date, day.Date) == 0)
            {
                orderedSlots.Add(timeSlots[i]);
            }
        }
        return orderedSlots.ToArray();
    }

    #endregion

    #endregion

}
