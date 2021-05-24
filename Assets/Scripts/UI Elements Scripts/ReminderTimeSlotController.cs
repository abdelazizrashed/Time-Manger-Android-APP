using DeadMosquito.AndroidGoodies;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReminderTimeSlotController : MonoBehaviour
{
    private int pageNumber;
    #region Main functions
    // Start is called before the first frame update
    void Start()
    {
        System.Random random = new System.Random();
        pageNumber = random.Next(1000000000);
        AddListners();
        AttachMethodsToEvents();
        OnStartUp();
    }

    private void OnDestroy()
    {
        DeattachMethodsFromEvents();
    }

    #endregion

    #region Control

    public void SetUpFieldsForEdit(ReminderTimeSlotModel timeSlot)
    {
        date = timeSlot.date;
        if (dateBtn != null)
        {
            SetTextOfTimeBtn(ref dateBtn, date.ToString("ddd, dd MMM, yyy"));
        }

        time = timeSlot.time;

        if (timeBtn != null)
        {
            SetTextOfTimeBtn(ref timeBtn, time.ToString("hh:mm tt"));
        }
    }

    #region Cancel and Get time slot

    public Button cancelBtn;

    private void OnCancelBtnClicked()
    {
        Destroy(gameObject);
    }

    public ReminderTimeSlotModel GetTimeSlot()
    {
        return new ReminderTimeSlotModel(_time: time, _date: date, _repeat: repeat, _reminders:reminders.ToArray());
    }

    #endregion

    #endregion

    #region Input handling

    #region Data Variables

    private DateTime time;
    private DateTime date;
    private RepeatModel repeat;
    private List<NotifiAlarmReminderModel> reminders = new List<NotifiAlarmReminderModel>();

    #endregion

    #region Time

    public Button dateBtn;
    public Button timeBtn;
    
    private void OnTimeBtnClicked()
    {
        if (time != null)
        {
            AGDateTimePicker.ShowTimePicker(
                time.Hour,
                time.Minute,
                (hour, minute) =>
                {
                    time = new DateTime(time.Year, time.Month, time.Day, hour, minute, 0);

                    if (timeBtn != null)
                    {
                        SetTextOfTimeBtn(ref timeBtn, time.ToString("hh:mm tt"));
                    }
                },
                () => { },
                AGDialogTheme.Dark,
                false
                );
        }
    }

    private void OnDateFromBtnClicked()
    {
        if (date != null)
        {
            AGDateTimePicker.ShowDatePicker(
                date.Year,
                date.Month,
                date.Day,
                (year, month, day) =>
                {
                    date = new DateTime(year, month, day);

                    if (dateBtn != null)
                    {
                        SetTextOfTimeBtn(ref dateBtn, date.ToString("ddd, dd MMM, yyy"));
                    }
                },
                () => { },
                AGDialogTheme.Dark
                );
        }
    }

    private void SetTextOfTimeBtn(ref Button button, string text)
    {
        button.GetComponentInChildren<TMP_Text>().text = text;
    }

    private void SetTaskDateTimeOnStartup()
    {
        if (timeBtn != null)
        {
            SetTextOfTimeBtn(ref timeBtn, DateTime.Now.ToString("hh:mm tt"));

        }

        if (dateBtn != null)
        {
            SetTextOfTimeBtn(ref dateBtn, DateTime.Now.ToString("ddd, dd MMM, yyy"));
        }
    }

    #endregion

    #region Repeat

    public Button repeatBtn;
    public GameObject customRecurrancePanelPrefab;

    private void OnRepeatBtnClicked()
    {
        string[] repeatOptionsTitles =
        {
            "Does not repeat",
            "Every day",
            "Every week",
            "Every month",
            "Every year",
            "Custom"
        };

        AGAlertDialog.ShowChooserDialog("", repeatOptionsTitles, optionIndex =>
        {
            switch (optionIndex)
            {
                case 0:
                    repeat = null;
                    break;
                case 1:
                    repeat = new RepeatModel(RepeatPeriod.Day, new WeekDays[] { });
                    break;
                case 2:
                    repeat = new RepeatModel(RepeatPeriod.Week, new WeekDays[] { });
                    break;
                case 3:
                    repeat = new RepeatModel(RepeatPeriod.Month, new WeekDays[] { });
                    break;
                case 4:
                    repeat = new RepeatModel(RepeatPeriod.Year, new WeekDays[] { });
                    break;
                case 5:
                    OnCustomRepeatChoosen();
                    break;

            }
        }, AGDialogTheme.Dark);
    }

    private void OnCustomRepeatChoosen()
    {
        GameObject background = GameObject.FindGameObjectWithTag("MainBackground");
        if (customRecurrancePanelPrefab != null && background != null)
        {
            GameObject gb = Instantiate(customRecurrancePanelPrefab, background.transform);
            gb.SetActive(true);
            gb.GetComponent<CustomRecurrenceController>().parentPageNumber = pageNumber;
        }
    }

    #endregion

    #region Reminder

    public Button addReminderBtn;
    public GameObject customReminderPrefab;
    public GameObject remindersList;
    public GameObject notfiAlarmInfoPrefab;


    private void OnAddReminderBtnClicked()
    {
        GameObject background = GameObject.FindGameObjectWithTag("MainBackground");
        if (customReminderPrefab != null && background != null)
        {
            GameObject gb = Instantiate(customReminderPrefab, background.transform);
            gb.SetActive(true);
            gb.GetComponent<CustomNotifiAlarmReminderController>().parentPageNum = pageNumber;
        }
    }

    #endregion

    #endregion

    #region Events

    private void SetCustomRepeat(RepeatModel repeatModel, int pNum)
    {
        if (pNum == pageNumber)
        {
            repeat = repeatModel;
        }
    }

    private void SetCustomReminder(NotifiAlarmReminderModel reminder, int pNum)
    {
        if (pNum == pageNumber)
        {
            reminders.Add(reminder);
            GameObject r = Instantiate(notfiAlarmInfoPrefab, remindersList.transform);
            string txt = reminder.reminderType == ReminderType.Notification ? "Notification: " : "Alarm: ";
            txt += "before " + reminder.timePeriodsNum.ToString() + " ";
            if (reminder.timePeriodType == TimePeriodsType.Minutes)
            {
                txt += "minutes.";
            }
            else if (reminder.timePeriodType == TimePeriodsType.Hours)
            {
                txt += "hours.";
            }
            else if (reminder.timePeriodType == TimePeriodsType.Days)
            {
                txt += "days.";
            }
            else
            {
                txt += "weeks.";
            }
            r.GetComponentInChildren<TMP_Text>().text = txt;
        }
    }

    private void AttachMethodsToEvents()
    {
        EventSystem.instance.onAddTaskCustomRepeatSave += SetCustomRepeat;
        EventSystem.instance.onAddTaskCustomReminderSave += SetCustomReminder;

    }

    private void DeattachMethodsFromEvents()
    {
        EventSystem.instance.onAddTaskCustomRepeatSave -= SetCustomRepeat;
        EventSystem.instance.onAddTaskCustomReminderSave -= SetCustomReminder;


    }

    #endregion

    #region General

    private void AddListners()
    {
        if(cancelBtn != null)
        {
            cancelBtn.onClick.AddListener(OnCancelBtnClicked);
        }

        if(timeBtn != null)
        {
            timeBtn.onClick.AddListener(OnTimeBtnClicked);
        }

        if(dateBtn != null)
        {
            dateBtn.onClick.AddListener(OnDateFromBtnClicked);
        }

        if(repeatBtn != null)
        {
            repeatBtn.onClick.AddListener(OnRepeatBtnClicked);
        }

        if(addReminderBtn != null)
        {
            addReminderBtn.onClick.AddListener(OnAddReminderBtnClicked);
        }
    }
    private void OnStartUp()
    {
        date = DateTime.Now;
        time = DateTime.Now;
        SetTaskDateTimeOnStartup();
    }
    #endregion


}
