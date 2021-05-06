using DeadMosquito.AndroidGoodies;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReminderTimeSlotController : MonoBehaviour
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

    #region Control

    #region Cancel and Get time slot

    public Button cancelBtn;

    private void OnCancelBtnClicked()
    {
        Destroy(gameObject);
    }

    public ReminderTimeSlotModel GetTimeSlot()
    {
        return new ReminderTimeSlotModel(time, date, repeat, reminders.ToArray());
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
        if (dateFrom != null)
        {
            AGDateTimePicker.ShowDatePicker(
                dateFrom.Year,
                dateFrom.Month,
                dateFrom.Day,
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

    private void SetTextOfTimeBtn(ref Button button, string text)
    {
        button.GetComponentInChildren<TMP_Text>().text = text;
    }

    #endregion

    #region Repeat

    #endregion

    #region Location

    #endregion

    #region Reminder

    #endregion

    #endregion

    #region Events

    #endregion

    #region General

    private void AddListners()
    {

    }
    private void OnStartUp()
    {

    }
    #endregion


}
