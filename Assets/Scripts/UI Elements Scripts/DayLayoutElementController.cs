using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayLayoutElementController : MonoBehaviour
{
    #region Main functions
    // Start is called before the first frame update
    void Start()
    {
        AddListners();
        OnStartUp();
    }

    #endregion

    #region Data variables
    [HideInInspector]
    public TaskModel currentTask = null;

    [HideInInspector]
    public EventTimeSlotModel currentEventTimeSlot = null;

    [HideInInspector]
    public ReminderTimeSlotModel currentReminderTimeSlot = null;

    [HideInInspector]
    public Pages currentPageType;

    [HideInInspector]
    public DateTime currentDate;

    #endregion

    #region UI Variables

    public TMP_Text elementTitle;

    #endregion

    private void OnClicked()
    {
        GameObject background = GameObject.FindGameObjectWithTag("MainBackground");
        if(background != null)
        {
            if (currentEventTimeSlot != null)
            { 
                GameObject addEventPanelPrefab = Resources.Load<GameObject>("Prefabs/AddEventPanel");
                GameObject editEventGameObject = Instantiate(addEventPanelPrefab, background.transform);
                editEventGameObject.GetComponent<AddEventController>().SetUpFieldsForEdit(currentEventTimeSlot.parentEvent);
            }else if(currentTask != null)
            {
                GameObject addTaskPanelPrefab = Resources.Load<GameObject>("Prefabs/AddTaskPanel");
                GameObject editTaskGameObject = Instantiate(addTaskPanelPrefab, background.transform);
                editTaskGameObject.GetComponent<AddTaskController>().SetUpFieldsForEdit(currentTask);
            }else if(currentReminderTimeSlot != null)
            {
                GameObject addReminderPanelPrefab = Resources.Load<GameObject>("Prefabs/AddReminderPanel");
                GameObject editReminderGO = Instantiate(addReminderPanelPrefab, background.transform);
                editReminderGO.GetComponent<AddReminderController>().SetUpFieldsForEdit(currentReminderTimeSlot.parentReminder);
            }

        }
    }

    private void AddListners()
    {
        if (elementTitle != null)
        {
            Button elementTitleBtnComponent = elementTitle.GetComponent<Button>();
            if (elementTitleBtnComponent != null)
            {
                elementTitleBtnComponent.onClick.AddListener(OnClicked);
            }
        }
        if (gameObject != null)
        {
            Button btnComponent = gameObject.GetComponent<Button>();
            if (btnComponent != null)
            {
                btnComponent.onClick.AddListener(OnClicked);
            }
        }
    }

    public GameObject dayLayoutElementPrefab;
    public GameObject dayLayoutElementChildrenPrefab;
    public GameObject rowPrefab;
    public GameObject columnPrefab;

    private void OnStartUp()
    {
        if (elementTitle != null)
        {
            if (currentEventTimeSlot != null)
            {
                elementTitle.text = currentEventTimeSlot.parentEvent.eventTitle;
                gameObject.GetComponent<Image>().color = Helper.StringToColor(currentEventTimeSlot.parentEvent.color.colorValue);
                if (DateTime.Compare(currentEventTimeSlot.dateFrom.Date, currentEventTimeSlot.dateTo.Date) == 0 && DateTime.Compare(currentDate.Date, currentEventTimeSlot.dateTo.Date) == 0)
                {
                    float duration = (float)(currentEventTimeSlot.timeTo - currentEventTimeSlot.timeFrom).TotalMinutes;
                    gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(540, duration / 60 * 60f);
                }
                else if (DateTime.Compare(currentDate.Date, currentEventTimeSlot.dateTo.Date) < 0 && DateTime.Compare(currentDate.Date, currentEventTimeSlot.dateFrom.Date) == 0)
                {
                    float duration = (float)(
                        currentEventTimeSlot.timeFrom.Date.AddDays(1).AddSeconds(-1) 
                        -
                        currentEventTimeSlot.timeFrom).TotalMinutes;
                    gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(540, duration / 60 * 60f);
                }
                else if(DateTime.Compare(currentDate.Date, currentEventTimeSlot.dateTo.Date) == 0 && DateTime.Compare(currentDate.Date, currentEventTimeSlot.dateFrom.Date) > 0)
                {
                    float duration = (float)(
                        currentEventTimeSlot.timeTo
                        -
                        currentEventTimeSlot.timeTo.Date
                        ).TotalMinutes;
                    gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(540, duration / 60 * 60f);
                }
                else
                {
                    gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(540, 24 * 60f);
                }
                if (
                    currentEventTimeSlot.parentEvent.childrenEventsTimeSlots.Length != 0 ||
                    currentEventTimeSlot.parentEvent.childrenTasks.Length != 0 ||
                    currentEventTimeSlot.parentEvent.childrenRemindersTimeSlots.Length != 0
                    )
                {
                    GameObject dayLayoutElementChildrenGameObject = Instantiate(dayLayoutElementChildrenPrefab, gameObject.transform);
                    GameObject columnGameObject = Instantiate(columnPrefab, dayLayoutElementChildrenGameObject.transform);
                    if (currentPageType.Value == Pages.Events.Value && currentEventTimeSlot.parentEvent.childrenEventsTimeSlots.Length != 0)
                    {
                        for (int i = 0; i < currentEventTimeSlot.parentEvent.childrenEventsTimeSlots.Length; i++)
                        {
                            if(i + 1 < currentEventTimeSlot.parentEvent.childrenEventsTimeSlots.Length)
                            {
                                if (
                                    DateTime.Compare(
                                        currentEventTimeSlot.parentEvent.childrenEventsTimeSlots[i].timeFrom,
                                        currentEventTimeSlot.parentEvent.childrenEventsTimeSlots[i + 1].timeFrom
                                        ) == 0 
                                        )
                                {
                                    GameObject rowGameObject = Instantiate(rowPrefab, columnGameObject.transform);
                                    GameObject child1Element = Helper.Instantiate<GameObject>(dayLayoutElementPrefab, rowGameObject.transform, (obj) =>
                                    {
                                        obj.GetComponent<DayLayoutElementController>().currentEventTimeSlot = currentEventTimeSlot.parentEvent.childrenEventsTimeSlots[i];
                                        obj.GetComponent<DayLayoutElementController>().currentPageType = currentPageType;
                                        obj.GetComponent<DayLayoutElementController>().currentDate = currentDate;
                                    });
                                    while (
                                        DateTime.Compare(
                                            currentEventTimeSlot.parentEvent.childrenEventsTimeSlots[i].timeFrom,
                                            currentEventTimeSlot.parentEvent.childrenEventsTimeSlots[i + 1].timeFrom
                                            ) == 0 &&
                                        i + 1 < currentEventTimeSlot.parentEvent.childrenEventsTimeSlots.Length
                                        )
                                    {
                                        i++;
                                        GameObject child2Element = Helper.Instantiate<GameObject>(dayLayoutElementPrefab, rowGameObject.transform, (obj) =>
                                        {
                                            obj.GetComponent<DayLayoutElementController>().currentEventTimeSlot = currentEventTimeSlot.parentEvent.childrenEventsTimeSlots[i];
                                            obj.GetComponent<DayLayoutElementController>().currentPageType = currentPageType;
                                            obj.GetComponent<DayLayoutElementController>().currentDate = currentDate;
                                        });
                                    }
                                    continue;
                                }
                            }
                            GameObject childElement = Helper.Instantiate<GameObject>(dayLayoutElementPrefab, columnGameObject.transform, (obj) =>
                            {
                                obj.GetComponent<DayLayoutElementController>().currentEventTimeSlot = currentEventTimeSlot.parentEvent.childrenEventsTimeSlots[i];
                                obj.GetComponent<DayLayoutElementController>().currentPageType = currentPageType;
                                obj.GetComponent<DayLayoutElementController>().currentDate = currentDate;
                            });
                        }
                    }
                    if (currentPageType.Value == Pages.All.Value)
                    {
                        object[] children = Helper.MergeSort3ArraysByTime(
                            currentEventTimeSlot.parentEvent.childrenTasks,
                            currentEventTimeSlot.parentEvent.childrenEventsTimeSlots,
                            currentEventTimeSlot.parentEvent.childrenRemindersTimeSlots
                            );
                        for (int i = 0; i < children.Length; i++)
                        {
                            DateTime currentChildTime = new DateTime();
                            if (children[i] is TaskModel)
                            {
                                currentChildTime = ((TaskModel)children[i]).timeFrom;
                            }
                            if (children[i] is EventTimeSlotModel)
                            {
                                currentChildTime = ((EventTimeSlotModel)children[i]).timeFrom;
                            }
                            if (children[i] is ReminderTimeSlotModel)
                            {
                                currentChildTime = ((ReminderTimeSlotModel)children[i]).time;
                            }
                            if (i + 1 < children.Length)
                            {
                                DateTime nextChildTime = new DateTime();
                                if (children[i + 1] is TaskModel)
                                {
                                    nextChildTime = ((TaskModel)children[i + 1]).timeFrom;
                                }
                                if (children[i + 1] is EventTimeSlotModel)
                                {
                                    nextChildTime = ((EventTimeSlotModel)children[i + 1]).timeFrom;
                                }
                                if (children[i + 1] is ReminderTimeSlotModel)
                                {
                                    nextChildTime = ((ReminderTimeSlotModel)children[i + 1]).time;
                                }

                                if (DateTime.Compare(currentChildTime, nextChildTime) == 0)
                                {
                                    GameObject rowGameObject = Instantiate(rowPrefab, columnGameObject.transform);
                                    GameObject child1Element = Helper.Instantiate<GameObject>(dayLayoutElementPrefab, rowGameObject.transform, (obj) =>
                                    {
                                        if (children[i] is TaskModel)
                                        {
                                            obj.GetComponent<DayLayoutElementController>().currentTask = (TaskModel)children[i];
                                        }
                                        if (children[i] is EventTimeSlotModel)
                                        {
                                            obj.GetComponent<DayLayoutElementController>().currentEventTimeSlot = (EventTimeSlotModel)children[i];
                                        }
                                        if (children[i] is ReminderTimeSlotModel)
                                        {
                                            obj.GetComponent<DayLayoutElementController>().currentReminderTimeSlot = (ReminderTimeSlotModel)children[i];
                                        }
                                        obj.GetComponent<DayLayoutElementController>().currentPageType = currentPageType;
                                        obj.GetComponent<DayLayoutElementController>().currentDate = currentDate;
                                    });
                                    while (DateTime.Compare(currentChildTime, nextChildTime) == 0 && i + 1 < currentEventTimeSlot.parentEvent.childrenEventsTimeSlots.Length)
                                    {
                                        i++;
                                        GameObject child2Element = Helper.Instantiate<GameObject>(dayLayoutElementPrefab, rowGameObject.transform, (obj) =>
                                        {
                                            if (children[i] is TaskModel)
                                            {
                                                obj.GetComponent<DayLayoutElementController>().currentTask = (TaskModel)children[i];
                                            }
                                            if (children[i] is EventTimeSlotModel)
                                            {
                                                obj.GetComponent<DayLayoutElementController>().currentEventTimeSlot = (EventTimeSlotModel)children[i];
                                            }
                                            if (children[i] is ReminderTimeSlotModel)
                                            {
                                                obj.GetComponent<DayLayoutElementController>().currentReminderTimeSlot = (ReminderTimeSlotModel)children[i];
                                            }
                                            obj.GetComponent<DayLayoutElementController>().currentPageType = currentPageType;
                                            obj.GetComponent<DayLayoutElementController>().currentDate = currentDate;
                                        });
                                        if (children[i + 1] is TaskModel)
                                        {
                                            nextChildTime = ((TaskModel)children[i + 1]).timeFrom;
                                        }
                                        if (children[i + 1] is EventTimeSlotModel)
                                        {
                                            nextChildTime = ((EventTimeSlotModel)children[i + 1]).timeFrom;
                                        }
                                        if (children[i + 1] is ReminderTimeSlotModel)
                                        {
                                            nextChildTime = ((ReminderTimeSlotModel)children[i + 1]).time;
                                        }
                                    }
                                    continue;
                                }

                            }

                            GameObject childElement = Helper.Instantiate<GameObject>(dayLayoutElementPrefab, columnGameObject.transform, (obj) =>
                            {
                                if (children[i] is TaskModel)
                                {
                                    obj.GetComponent<DayLayoutElementController>().currentTask = (TaskModel)children[i];
                                }
                                if (children[i] is EventTimeSlotModel)
                                {
                                    obj.GetComponent<DayLayoutElementController>().currentEventTimeSlot = (EventTimeSlotModel)children[i];
                                }
                                if (children[i] is ReminderTimeSlotModel)
                                {
                                    obj.GetComponent<DayLayoutElementController>().currentReminderTimeSlot = (ReminderTimeSlotModel)children[i];
                                }
                                obj.GetComponent<DayLayoutElementController>().currentPageType = currentPageType;
                                obj.GetComponent<DayLayoutElementController>().currentDate = currentDate;
                            });
                        }

                    }


                }

            } 
            else if (currentTask != null)
            {
                elementTitle.text = currentTask.taskTitle;
                gameObject.GetComponent<Image>().color = Helper.StringToColor(currentTask.color.colorValue);
                if (DateTime.Compare(currentTask.timeFrom.Date, currentTask.timeTo.Date) == 0 && DateTime.Compare(currentDate.Date, currentTask.timeFrom.Date) == 0)
                {
                    float duration = (float)(currentTask.timeTo - currentTask.timeFrom).TotalMinutes;
                    gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(540, duration / 60 * 60f);
                }
                else if (DateTime.Compare(currentDate.Date, currentTask.timeTo.Date) < 0 && DateTime.Compare(currentDate.Date, currentTask.timeFrom.Date) == 0)
                {
                    float duration = (float)(
                        currentTask.timeFrom.Date.AddDays(1).AddSeconds(-1)
                        -
                        currentTask.timeFrom).TotalMinutes;
                    gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(540, duration / 60 * 60f);
                }
                else if (DateTime.Compare(currentDate.Date, currentTask.timeTo.Date) == 0 && DateTime.Compare(currentDate.Date, currentTask.timeFrom.Date) > 0)
                {
                    float duration = (float)(
                        currentTask.timeTo
                        -
                        currentTask.timeTo.Date
                        ).TotalMinutes;
                    gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(540, duration / 60 * 60f);
                }
                else
                {
                    gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(540, 24 * 60f);
                }

                if (currentTask.childrenTasks.Length != 0)
                {
                    GameObject dayLayoutElementChildrenGameObject = Instantiate(dayLayoutElementChildrenPrefab, gameObject.transform);
                    GameObject columnGameObject = Instantiate(columnPrefab, dayLayoutElementChildrenGameObject.transform);
                    for (int i = 0; i < currentTask.childrenTasks.Length; i++)
                    {
                        if(i+1 < currentTask.childrenTasks.Length)
                        {
                            if (
                                DateTime.Compare(
                                    currentTask.childrenTasks[i].timeFrom,
                                    currentTask.childrenTasks[i+1].timeFrom
                                    ) == 0)
                            {
                                GameObject rowGameObject = Instantiate(rowPrefab, columnGameObject.transform);
                                GameObject child1Element = Helper.Instantiate<GameObject>(dayLayoutElementPrefab, rowGameObject.transform, (obj) =>
                                {
                                    obj.GetComponent<DayLayoutElementController>().currentTask = currentTask.childrenTasks[i];
                                    obj.GetComponent<DayLayoutElementController>().currentPageType = currentPageType;
                                    obj.GetComponent<DayLayoutElementController>().currentDate = currentDate;
                                });
                                while (
                                    DateTime.Compare(
                                    currentTask.childrenTasks[i].timeFrom,
                                    currentTask.childrenTasks[i + 1].timeFrom
                                    ) == 0 &&
                                    i + 1 < currentEventTimeSlot.parentEvent.childrenEventsTimeSlots.Length
                                    )
                                {
                                    i++;
                                    GameObject child2Element = Helper.Instantiate<GameObject>(dayLayoutElementPrefab, rowGameObject.transform, (obj) =>
                                    {
                                        obj.GetComponent<DayLayoutElementController>().currentTask = currentTask.childrenTasks[i+1];
                                        obj.GetComponent<DayLayoutElementController>().currentPageType = currentPageType;
                                        obj.GetComponent<DayLayoutElementController>().currentDate = currentDate;
                                    });
                                }
                                continue;
                            }
                        }
                        GameObject childElement = Helper.Instantiate<GameObject>(dayLayoutElementPrefab, columnGameObject.transform, (obj) =>
                        {
                            obj.GetComponent<DayLayoutElementController>().currentTask = currentTask.childrenTasks[i];
                            obj.GetComponent<DayLayoutElementController>().currentPageType = currentPageType;
                            obj.GetComponent<DayLayoutElementController>().currentDate = currentDate;
                        });
                    }
                }
            }
               
            else if (currentReminderTimeSlot != null)
            {
                elementTitle.text = currentReminderTimeSlot.parentReminder.reminderTitle;
                gameObject.GetComponent<Image>().color = Helper.StringToColor(currentReminderTimeSlot.parentReminder.color.colorValue);
                gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(540, 15f);

            }
            else
            {
                Debug.LogError("current element is not attached");
            }
        }
        else
        {
            Debug.LogError("Element title is not attached");
        }
    }
}
