using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DeadMosquito.AndroidGoodies;

public class EventTimeSlotController : MonoBehaviour
{
    private int pageNumber { get; set; }
    public int parentPageNumber { get; set; }
    private GameObject background;
    // Start is called before the first frame update
    void Start()
    {
        System.Random random = new System.Random();
        pageNumber = random.Next(1000000000);
        background = GameObject.FindGameObjectWithTag("MainBackground");
        AddListners();
        AttachMethodsToEvents();
        OnStartup();
    }

    private void OnDestroy()
    {
        DeattachMethodsFromEvents();
    }

    public void SetUpFieldsForEdit(EventTimeSlotModel timeSlot)
    {
        if(timeFromBtn != null)
        {
            SetTextOfTimeBtn(ref timeFromBtn, timeSlot.timeFrom.ToString("hh:mm tt"));
            timeFrom = timeSlot.timeFrom;
        }
        if(dateFromBtn != null)
        {
            SetTextOfTimeBtn(ref dateFromBtn, timeSlot.timeFrom.ToString("ddd, dd MMM, yyy"));
            dateFrom = timeSlot.timeFrom.Date;
        }
        if(timeToBtn != null)
        {
            SetTextOfTimeBtn(ref timeToBtn, timeSlot.timeTo.ToString("hh:mm tt"));
            timeTo = timeSlot.timeTo;
        }
        if(dateToBtn != null)
        {
            SetTextOfTimeBtn(ref dateToBtn, timeSlot.timeTo.Date.ToString("ddd, dd MMM, yyy"));
            dateTo = timeSlot.timeTo.Date;
        }

        location = timeSlot.location;
        locationInputField.text = timeSlot.location;
    }

    #region Cancel Get time slot

    public Button cancelBtn;

    private void OnCancelBtnClicked()
    {
        Destroy(gameObject);
    }

    public EventTimeSlotModel GetTimeSlot()
    {
        if (locationInputField != null)
        {
            location = locationInputField.text;
        }

        if (DateTime.Compare(dateTo, dateFrom) > 0)
        {
            AGUIMisc.ShowToast("The start date must be ealier thant the finish date", AGUIMisc.ToastLength.Long);
            return null;
        }
        if(timeFrom.Hour > timeTo.Hour)
        { 
            AGUIMisc.ShowToast("The start time must be ealier thant the finish time", AGUIMisc.ToastLength.Long);
            return null;
        }
        if (timeFrom.Minute > timeTo.Minute)
        {
            AGUIMisc.ShowToast("The start time must be ealier thant the finish time", AGUIMisc.ToastLength.Long);
            return null;
        }

        return new EventTimeSlotModel(_timeFrom: timeFrom, _timeTo: timeTo, _repeat: repeat, _location: location, _reminders: reminders.ToArray());

    }

    #endregion

    #region Data variables

    private DateTime dateFrom;
    private DateTime dateTo;
    private DateTime timeFrom;
    private DateTime timeTo; 

    private RepeatModel repeat;
    private string location;
    private List<NotifiAlarmReminderModel> reminders = new List<NotifiAlarmReminderModel>();

    #endregion

    #region Date time of the slot

    public Button dateFromBtn;
    public Button timeFromBtn;
    public Button dateToBtn;
    public Button timeToBtn;

    private void OnTimeFromBtnClicked()
    {
        if (timeFrom != null)
        {
            AGDateTimePicker.ShowTimePicker(
                DateTime.Now.Hour,
                DateTime.Now.Minute,
                (hour, minute) =>
                {
                    timeFrom = new DateTime(timeFrom.Year, timeFrom.Month, timeFrom.Day, hour, minute, 0);

                    if (timeFromBtn != null)
                    {
                        SetTextOfTimeBtn(ref timeFromBtn, timeFrom.ToString("hh:mm tt"));
                    }
                    if (timeTo.Hour <= timeFrom.Hour)
                    {
                        if (timeTo.Minute <= timeFrom.Hour)
                        {
                            timeTo = new DateTime(timeTo.Year, timeTo.Month, timeTo.Day, timeFrom.Hour + 1, timeTo.Minute, 0);
                        }
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
        if (dateFrom != null)
        {
            AGDateTimePicker.ShowDatePicker(
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                (year, month, day) =>
                {
                    dateFrom = new DateTime(year, month, day);

                    if (dateFromBtn != null)
                    {
                        SetTextOfTimeBtn(ref dateFromBtn, dateFrom.ToString("ddd, dd MMM, yyy"));
                    }

                    if (dateTo.Year <= dateFrom.Year)
                    {
                        if (dateTo.Month <= dateFrom.Month)
                        {
                            if (dateTo.Day < dateFrom.Day)
                            {
                                dateTo = new DateTime(dateTo.Year, dateTo.Month, dateFrom.Day);
                            }
                        }
                    }
                },
                () => { },
                AGDialogTheme.Dark
                );
        }
    }

    private void OnTimeToBtnClicked()
    {
        DateTime now = DateTime.Now;
        AGDateTimePicker.ShowTimePicker(
            DateTime.Now.Hour,
            DateTime.Now.Minute,
            (hour, minute) =>
            {
                timeTo = new DateTime(timeTo.Year, timeTo.Month, timeTo.Day, hour, minute, 0);

                if (timeToBtn != null)
                {
                    SetTextOfTimeBtn(ref timeToBtn, timeTo.ToString("hh:mm tt"));
                }
            },
            () => { },
            AGDialogTheme.Dark,
            false
            );

    }

    private void OnDateToBtnClicked()
    {
        if (dateTo != null)
        {
            AGDateTimePicker.ShowDatePicker(
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                (year, month, day) =>
                {
                    dateTo = new DateTime(year, month, day);

                    if (dateToBtn != null)
                    {
                        SetTextOfTimeBtn(ref dateToBtn, dateTo.ToString("ddd, dd MMM, yyy"));
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
        if (timeFromBtn != null)
        {
            SetTextOfTimeBtn(ref timeFromBtn, DateTime.Now.ToString("hh:mm tt"));

        }

        if (dateFromBtn != null)
        {
            SetTextOfTimeBtn(ref dateFromBtn, DateTime.Now.ToString("ddd, dd MMM, yyy"));
        }

        if (timeToBtn != null)
        {
            SetTextOfTimeBtn(ref timeToBtn, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour + 1, DateTime.Now.Minute, 0, 0).ToString("hh:mm tt"));
        }

        if (dateToBtn != null)
        {
            SetTextOfTimeBtn(ref dateToBtn, DateTime.Now.ToString("ddd, dd MMM, yyy"));
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
        if (customRecurrancePanelPrefab != null && background != null)
        {
            GameObject gb = Instantiate(customRecurrancePanelPrefab, background.transform);
            gb.SetActive(true);
            gb.GetComponent<CustomRecurrenceController>().parentPageNumber = pageNumber;
        }
    }

    #endregion

    #region Location

    public TMP_InputField locationInputField;

    #endregion

    #region Reminder

    public Button addReminderBtn;
    public GameObject customReminderPrefab;
    public GameObject remindersList;
    public GameObject notfiAlarmInfoPrefab;


    private void OnAddReminderBtnClicked()
    {
        if (customReminderPrefab != null && background != null)
        {
            GameObject gb = Instantiate(customReminderPrefab, background.transform);
            gb.SetActive(true);
            gb.GetComponent<CustomNotifiAlarmReminderController>().parentPageNum = pageNumber;
        }
    }

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
        if (addReminderBtn != null)
        {
            addReminderBtn.onClick.AddListener(OnAddReminderBtnClicked);
        }

        if (repeatBtn != null)
        {
            repeatBtn.onClick.AddListener(OnRepeatBtnClicked);
        }

        if(timeFromBtn != null)
        {
            timeFromBtn.onClick.AddListener(OnTimeFromBtnClicked);
        }

        if (timeToBtn != null)
        {
            timeToBtn.onClick.AddListener(OnTimeToBtnClicked);
        }

        if (dateFromBtn != null)
        {
            dateFromBtn.onClick.AddListener(OnDateFromBtnClicked);
        }

        if (dateToBtn != null)
        {
            dateToBtn.onClick.AddListener(OnDateToBtnClicked);
        }
    }


    public void OnStartup()
    {
        dateFrom = DateTime.Now;
        timeFrom = DateTime.Now;
        dateTo = DateTime.Now;
        timeTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour + 1, DateTime.Now.Minute, 0, 0);
        SetTaskDateTimeOnStartup();
    }

    #endregion
}
