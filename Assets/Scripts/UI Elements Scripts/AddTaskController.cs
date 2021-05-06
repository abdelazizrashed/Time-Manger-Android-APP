using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DeadMosquito.AndroidGoodies;
using DG.Tweening;
using TMPro;
using System;

public class AddTaskController : MonoBehaviour
{
    public GameObject background;
    private int pageNumber;


    // Start is called before the first frame update
    void Start()
    {
        System.Random random = new System.Random();
        pageNumber = random.Next(1000000000);
        background = GameObject.Find("Background");
        AddListnersToBtns();
        AttachMethodsToEvents();

        OnStartup();
    }

    private void OnDestroy()
    {
        DeattachMethodsFromEvents();
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
        if (IsTaskTitleEmpty())
        {
            AGUIMisc.ShowToast("Task title is required.", AGUIMisc.ToastLength.Long);
            return;
        }
        taskTitle = GetTheTaskTitle();

        taskDescription = GetTheTaskDescription();

        if(choosenList == null)
        {
            AGUIMisc.ShowToast("Tasks list is required.", AGUIMisc.ToastLength.Short);
        }

        TaskModel newTask = new TaskModel(
            _taskTitle: taskTitle,
            _taskDescription: taskDescription,
            _timeFrom: timeFrom,
            _timeTo: timeTo,
            _repeat: choosenRepeatOption,
            _reminders: chosenReminders.ToArray(),
            _color: chosenColor,
            _userID: UserModel.Instance.userID,
            _parentEvent: parentEvent,
            _parentTask: parentTask,
            _taskList: choosenList
            );
        TaskModel.SaveTask(newTask);

        HideAddTaskPanel();
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

    private string taskTitle = "";
    private string taskDescription = "";

    private TasksListModel choosenList = null;
    private TasksListModel[] tasksListsList = { };

    private DateTime dateFrom;
    private DateTime dateTo;
    private DateTime timeFrom;
    private DateTime timeTo;

    private RepeatModel choosenRepeatOption;

    private List<NotifiAlarmReminderModel> chosenReminders;

    private ColorModel chosenColor = null;

    private TaskModel parentTask = null;
    private EventModel parentEvent = null;


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
        },
        theme: AGDialogTheme.Dark
        );
    }


    #endregion

    #region Title and Description

    public TMP_InputField taskTitleInputField;
    public TMP_InputField taskDescriptionInputField;

    private string GetTheTaskTitle()
    {
        if (taskTitleInputField != null)
        {
            return taskTitleInputField.text;
        }
        return "";
    }

    private bool IsTaskTitleEmpty()
    {
        return String.IsNullOrEmpty(GetTheTaskTitle());
    }

    private string GetTheTaskDescription()
    {
        if (taskDescriptionInputField != null)
        {
            return taskDescriptionInputField.text;
        }
        return "";
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
                    choosenRepeatOption = null;
                    break;
                case 1:
                    choosenRepeatOption = new RepeatModel(RepeatPeriod.Day, new WeekDays[] { });
                    break;
                case 2:
                    choosenRepeatOption = new RepeatModel(RepeatPeriod.Week, new WeekDays[] { });
                    break;
                case 3:
                    choosenRepeatOption = new RepeatModel(RepeatPeriod.Month, new WeekDays[] { });
                    break;
                case 4:
                    choosenRepeatOption = new RepeatModel(RepeatPeriod.Year, new WeekDays[] { });
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

    #region Reminder

    public Button addReminderBtn;
    public GameObject customReminderPrefab;
    public GameObject remindersList;
    public GameObject notfiAlarmInfoPrefab;


    private void OnAddReminderBtnClicked()
    {
        if(customReminderPrefab!= null && background != null)
        {
            GameObject gb = Instantiate(customReminderPrefab, background.transform);
            gb.SetActive(true);
            gb.GetComponent<CustomNotifiAlarmReminderController>().parentPageNum = pageNumber;
        }
    }

    #endregion

    #region Color
    public Button chooseColorBtn;
    public GameObject colorBtnPrefab;

    private void OnChooseColorBtnClicked()
    {
        ColorModel[] colors = ColorModel.GetColors();
        CustomAlertDialog.ShowColorPickerDialog(colors, colorBtnPrefab, index =>
        {
            chosenColor = colors[index];
            chooseColorBtn.GetComponentInChildren<TMP_Text>().text = colors[index].colorName;
            chooseColorBtn.GetComponentInChildren<Image>().color = new Color32(
                Helper.StringToByteArray(colors[index].colorValue)[0],
                Helper.StringToByteArray(colors[index].colorValue)[1],
                Helper.StringToByteArray(colors[index].colorValue)[2],
                0xFF
                );
        });

    }
    #endregion

    #region Choose Parent
    public Button chooseParentBtn;
    private void OnChooseParentBtnClicked()
    {
        TaskModel[] tasks = TaskModel.GetTasks();
        EventModel[] events = EventModel.GetEvents();
        string[] eventTaskTitles = { "Event", "Task" };

        AGAlertDialog.ShowChooserDialog(
            "Choose the type of parent",
            eventTaskTitles,
            index =>
            {
            switch (index)
            {
                case 0:
                    string[] eventsTitles = EventModel.GetEventsTitles(events);
                    AGAlertDialog.ShowChooserDialog(
                        "Choose Event",
                        eventsTitles,
                        i =>
                        {
                            parentEvent = events[i];
                        }
                            );
                        break;
                    case 1:
                        string[] tasksTitles = TaskModel.GetTasksTitles(tasks);
                        AGAlertDialog.ShowChooserDialog(
                            "Choose Task",
                            tasksTitles,
                            i =>
                            {
                                parentTask = tasks[i];
                            }
                                );
                        break;

                }
            },
            AGDialogTheme.Dark
            );

    }

    #endregion

    #endregion


    #region Events

    //private void SetTasksLists(TasksListModel[] lists)
    //{
    //    tasksListsList = lists;
    //}

    private void SetCustomRepeat(RepeatModel repeatModel, int pNum)
    {
        if (pNum == pageNumber)
        {
            choosenRepeatOption = repeatModel;
        }
    }

    private void SetCustomReminder(NotifiAlarmReminderModel reminder, int pNum)
    {
        if(pNum == pageNumber)
        {
            chosenReminders.Add(reminder);
            GameObject r = Instantiate(notfiAlarmInfoPrefab, remindersList.transform);
            string txt = reminder.reminderType == ReminderType.Notification ? "Notification: " : "Alarm: ";
            txt += "before " + reminder.timePeriodsNum.ToString() + " ";
            if(reminder.timePeriodType == TimePeriodsType.Minutes)
            {
                txt += "minutes.";
            }
            else if(reminder.timePeriodType == TimePeriodsType.Hours)
            {
                txt += "hours.";
            }else if (reminder.timePeriodType == TimePeriodsType.Days)
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
        //EventSystem.instance.getTasksLists += SetTasksLists;
    }

    private void DeattachMethodsFromEvents()
    {
        EventSystem.instance.onAddTaskCustomRepeatSave -= SetCustomRepeat;
        EventSystem.instance.onAddTaskCustomReminderSave -= SetCustomReminder;
        //EventSystem.instance.getTasksLists -= SetTasksLists;

    }
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

        if (repeatBtn != null)
        {
            repeatBtn.onClick.AddListener(OnRepeatBtnClicked);
        }

        if(addReminderBtn != null)
        {
            addReminderBtn.onClick.AddListener(OnAddReminderBtnClicked);
        }

        if(chooseColorBtn != null)
        {
            chooseColorBtn.onClick.AddListener(OnChooseColorBtnClicked);
        }

        if(chooseParentBtn != null)
        {
            chooseParentBtn.onClick.AddListener(OnChooseParentBtnClicked);
        }
    }

    public void OnStartup()
    {
        dateFrom = DateTime.Now;
        timeFrom = DateTime.Now;
        dateTo = DateTime.Now;
        timeTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour + 1, DateTime.Now.Minute, 0, 0);
        SetTaskDateTimeOnStartup();
        ColorModel[] colors = ColorModel.GetColors();
        chosenColor = colors[0];
        chooseColorBtn.GetComponentInChildren<TMP_Text>().text = colors[0].colorName;
        chooseColorBtn.GetComponentInChildren<Image>().color = new Color32(
            Helper.StringToByteArray(colors[0].colorValue)[0],
            Helper.StringToByteArray(colors[0].colorValue)[1],
            Helper.StringToByteArray(colors[0].colorValue)[2],
            0xFF
            );
    }

}
