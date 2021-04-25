﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DeadMosquito.AndroidGoodies;
using DG.Tweening;
using TMPro;
using System;

public class AddTaskController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        AddListnersToBtns();
        OnStartup();
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Cancel and Save functionalities

    #region UI Variables

    public Button cancelButton;
    public Button saveButton;

    #endregion

    private void OnCancelButtonClicked()
    {
        AGAlertDialog.ShowMessageDialog(
            "",
            "Discard this task?",
            "Keep editing", () => { },
            negativeButtonText: "Discard", onNegativeButtonClick: () =>
            {
                HideAddTaskPanel();
            },
            theme: AGDialogTheme.Dark
            );

    }

    private void OnSaveBtnClicked()
    {
        //Todo: Check that the required field are not empty
        //Todo: create an object of a task with the data inputted.
        //Todo: Close the add task panel
    }

    /// <summary>
    /// The HideAddTaskPanel method will close the add task panel by moving it downward and destroying it.
    /// </summary>
    private void HideAddTaskPanel()
    {
        if (gameObject != null)
        {
            gameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, -2000f), 0.25f);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    #endregion

    #region Input handling

    #region Data variables

    private TasksListModel choosenList = null;

    private DateTime dateFrom = DateTime.Now;
    private DateTime dateTo = DateTime.Now;
    private DateTime timeFrom = DateTime.Now;
    private DateTime timeTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour + 1, DateTime.Now.Minute, 0, 0);


    #endregion

    #region Choose List

    public Button chooseListBtn;

    private void OnChooseListBtnClicked()
    {
        TasksListModel[] lists = TasksListModel.GetLists();
        List<string> listsTitles = new List<string>();
        foreach (TasksListModel list in lists)
        {
            listsTitles.Add(list.listTitle);
        }

        AGAlertDialog.ShowChooserDialog("Choose List", listsTitles.ToArray(), listIndex =>
        {
            choosenList = lists[listIndex];
            if (chooseListBtn != null)
            {
                chooseListBtn.GetComponentInChildren<TMP_Text>().text = choosenList.listTitle;
            }
        });
    }


    #endregion

    #region Title and Description

    public TMP_InputField taskTitleInputField;
    public TMP_InputField taskDescriptionInputField;

    private string GetTheTaskTitle()
    {
        return taskTitleInputField.text;
    }

    private bool IsTaskTitleEmpty()
    {
        return String.IsNullOrEmpty(GetTheTaskTitle());
    }

    private string GetTheTaskDescription()
    {
        return taskDescriptionInputField.text;
    }

    private bool IsTaskDescriptionEmpty()
    {
        return String.IsNullOrEmpty(GetTheTaskDescription());
    }


    #endregion

    #region Time from and Time to

    public Button dateFromBtn;
    public Button timeFromBtn;
    public Button dateToBtn;
    public Button timeToBtn;

    private void OnTimeFromBtnClicked()
    {
        if (timeFrom != null)
        {
            AGDateTimePicker.ShowTimePicker(
                timeFrom.Hour,
                timeFrom.Minute,
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
                dateTo.Year,
                dateTo.Month,
                dateTo.Day,
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

    #endregion

    private void AddListnersToBtns()
    {
        if (cancelButton != null)
        {
            cancelButton.onClick.AddListener(OnCancelButtonClicked);
        }

        if (saveButton != null)
        {
            saveButton.onClick.AddListener(OnSaveBtnClicked);
        }

        if (chooseListBtn != null)
        {
            chooseListBtn.onClick.AddListener(OnChooseListBtnClicked);
        }

        if (timeFromBtn != null)
        {
            timeFromBtn.onClick.AddListener(OnTimeFromBtnClicked);
        }

        if (dateFromBtn != null)
        {
            dateFromBtn.onClick.AddListener(OnDateFromBtnClicked);
        }

        if (timeToBtn != null)
        {
            timeToBtn.onClick.AddListener(OnTimeToBtnClicked);
        }

        if (dateToBtn != null)
        {
            dateToBtn.onClick.AddListener(OnDateToBtnClicked);
        }

    }

    private void OnStartup()
    {
        SetTaskDateTimeOnStartup();
    }
}
