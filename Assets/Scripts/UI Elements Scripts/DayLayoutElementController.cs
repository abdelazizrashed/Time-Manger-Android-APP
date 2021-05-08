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

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region Data variables
    [HideInInspector]
    public TaskModel currentTask = null;

    [HideInInspector]
    public EventTimeSlotModel currentEventTimeSlot = null;

    [HideInInspector]
    public ReminderTimeSlotModel currentReminderTimeSlot = null;

    #endregion

    #region UI Variables

    public TMP_Text elementTitle;

    #endregion

    private void OnClicked()
    {
        if (currentEventTimeSlot != null)
        {

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
        if(elementTitle != null)
        {
            if(currentEventTimeSlot != null)
            {
                elementTitle.text = currentEventTimeSlot.parentEvent.eventTitle;
                gameObject.GetComponent<Image>().color = Helper.StringToColor(currentEventTimeSlot.parentEvent.color.colorValue);
                if(currentEventTimeSlot.dateFrom.Year == currentEventTimeSlot.dateTo.Year && currentEventTimeSlot.dateFrom.Month == currentEventTimeSlot.dateTo.Month && currentEventTimeSlot.dateFrom.Day == currentEventTimeSlot.dateTo.Day)
                {
                    float duration = (float)(currentEventTimeSlot.timeTo - currentEventTimeSlot.timeFrom).TotalMinutes;
                    gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(540, duration / 60 * 60f);
                }
                else
                {
                    float duration = (float)(new DateTime(
                        currentEventTimeSlot.timeFrom.Year,
                        currentEventTimeSlot.timeFrom.Month,
                        currentEventTimeSlot.timeFrom.Day,
                        24, 0, 0
                        ) -
                        currentEventTimeSlot.timeFrom).TotalMinutes;
                    gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(540, duration / 60 * 60f);
                }
                if(
                    currentEventTimeSlot.parentEvent.childrenEventsTimeSlots.Length != 0 || 
                    currentEventTimeSlot.parentEvent.childrenTasks.Length != 0 || 
                    currentEventTimeSlot.parentEvent.childrenRemindersTimeSlots.Length != 0
                    )
                {
                    List<System.Object> children = new List<object>();
                }

            }else if(currentTask != null)
            {
                elementTitle.text = currentTask.taskTitle;
                gameObject.GetComponent<Image>().color = Helper.StringToColor(currentTask.color.colorValue);
                if (currentTask.timeFrom.Year == currentTask.timeTo.Year && currentTask.timeFrom.Month == currentTask.timeTo.Month && currentTask.timeFrom.Day == currentTask.timeTo.Day)
                {
                    float duration = (float)(currentTask.timeTo - currentTask.timeFrom).TotalMinutes;
                    gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(540, duration / 60 * 60f);
                }
                else
                {
                    float duration = (float)(new DateTime(
                        currentTask.timeFrom.Year,
                        currentTask.timeFrom.Month,
                        currentTask.timeFrom.Day,
                        24, 0, 0
                        ) -
                        currentTask.timeFrom).TotalMinutes;
                    gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(540, duration / 60 * 60f);
                }
            }
            else if(currentReminder)
        }
    }
}
