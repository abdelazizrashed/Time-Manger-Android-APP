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

    #region Main Methods
    // Start is called before the first frame update
    void Start()
    {
        System.Random random = new System.Random();
        pageNumber = random.Next(1000000000);
        background = GameObject.FindGameObjectWithTag("MainBackground");
        AddListnersToBtns();
        AttachMethodsToEvents();

        OnStartup();
    }

    private void OnDestroy()
    {
        DeattachMethodsFromEvents();
    }

    #endregion

    private TaskModel currentTask;
    public TMP_Text addTaskTitleTxt;
    private bool isEdit = false;
    public void SetUpFieldsForEdit(TaskModel editTask)
    {
        isEdit = true;
        markFinishedBtn.gameObject.SetActive(true);
        markStartedBtn.gameObject.SetActive(true);
        //choose list

        choosenList = editTask.taskList;
        if (chooseListBtn != null)
        {
            chooseListBtn.GetComponentInChildren<TMP_Text>().text = choosenList.listTitle;
        }

        //Task title
        taskTitle = editTask.taskTitle;
        if(taskTitleInputField != null)
        {
            taskTitleInputField.text = taskTitle;
        }
        if(addTaskTitleTxt != null)
        {
            addTaskTitleTxt.text = taskTitle;
        }

        //Task description
        taskDescription = editTask.taskDescription;
        if(taskDescriptionInputField != null)
        {
            taskDescriptionInputField.text = taskDescription;
        }

        //time date from 
        timeFrom = editTask.timeFrom;
        if (timeFromBtn != null)
        {
            SetTextOfTimeBtn(ref timeFromBtn, timeFrom.ToString("hh:mm tt"));
        }

        dateFrom = editTask.timeFrom.Date;
        if (dateFromBtn != null)
        {
            SetTextOfTimeBtn(ref dateFromBtn, dateFrom.ToString("ddd, dd MMM, yyy"));
        }

        //time date to
        timeTo = editTask.timeTo;
        if (timeToBtn != null)
        {
            SetTextOfTimeBtn(ref timeToBtn, timeTo.ToString("hh:mm tt"));
        }

        dateTo = editTask.timeTo.Date;
        if (dateToBtn != null)
        {
            SetTextOfTimeBtn(ref dateToBtn, dateTo.ToString("ddd, dd MMM, yyy"));
        }

        //color
        chosenColor = editTask.color;
        SetChooseColorBtn(chosenColor);
        //parent
        parentEvent = editTask.parentEvent;
        parentTask = editTask.parentTask;
        if(parentTask != null)
        {
            chooseParentBtn.GetComponentInChildren<TMP_Text>().text = "Task: " + parentTask?.taskTitle;
        }
        else
        {
            chooseParentBtn.GetComponentInChildren<TMP_Text>().text = "Event: " + parentEvent?.eventTitle;
        }

        currentTask = editTask;
    }

    #region Start Finish

    public Button markStartedBtn;
    public Button markFinishedBtn;

    private void OnMarkStartedBtnClicked()
    {

        string[] titles = { "Now", "Choose specific time" };
#if UNITY_ANDROID && !UNITY_EDITOR
#else
#endif
        AGAlertDialog.ShowChooserDialog(
            "Choose time",
            titles,
            i =>
            {
                if (i == 0)
                {
                    TaskModel.StartTask(currentTask, DateTime.Now);
                }
                else
                {
#if UNITY_ANDROID && !UNITY_EDITOR
#else
#endif
                    AGDateTimePicker.ShowDatePicker(
                        DateTime.Now.Year,
                        DateTime.Now.Month,
                        DateTime.Now.Day,
                        (year, month, day) =>
                        {
                            AGDateTimePicker.ShowTimePicker(
                                DateTime.Now.Hour,
                                DateTime.Now.Minute,
                                (hour, minute) =>
                                {
                                    TaskModel.StartTask(currentTask, new DateTime(year, month, day, hour, minute, 0));
                                    
                                },
                                () =>
                                {
                                    AGUIMisc.ShowToast("No time was selected so the task wasn't registed as started", AGUIMisc.ToastLength.Long);
                                },
                                AGDialogTheme.Dark
                                );
                        },
                        () =>
                        {
                            AGUIMisc.ShowToast("No date was selected so the task wasn't registed as started", AGUIMisc.ToastLength.Long);
                        },
                        AGDialogTheme.Dark
                        );
                }
            },
            AGDialogTheme.Dark
            );
    }

    private void OnMarkFinishedBtnClicked()
    {
        string[] titles = { "Now", "Choose specific time" };
#if UNITY_ANDROID && !UNITY_EDITOR
#else
#endif
        AGAlertDialog.ShowChooserDialog(
            "Choose time",
            titles,
            i =>
            {
                if (i == 0)
                {
                    TaskModel.FinishTask(currentTask, DateTime.Now);
                }
                else
                {
                    AGDateTimePicker.ShowDatePicker(
                        DateTime.Now.Year,
                        DateTime.Now.Month,
                        DateTime.Now.Day,
                        (year, month, day) =>
                        {
                            AGDateTimePicker.ShowTimePicker(
                                DateTime.Now.Hour,
                                DateTime.Now.Minute,
                                (hour, minute) =>
                                {
                                    TaskModel.FinishTask(currentTask, new DateTime(year, month, day, hour, minute, 0));
                                },
                                () =>
                                {
                                    AGUIMisc.ShowToast("No time was selected so the task wasn't registed as finished", AGUIMisc.ToastLength.Long);
                                },
                                AGDialogTheme.Dark
                                );
                        },
                        () =>
                        {
                            AGUIMisc.ShowToast("No date was selected so the task wasn't registed as finished", AGUIMisc.ToastLength.Long);
                        },
                        AGDialogTheme.Dark
                        );
                }
            },
            AGDialogTheme.Dark
            );
    }

    #endregion

    #region Cancel and Save functionalities

    #region UI Variables

    public Button cancelButton;
    public Button saveButton;

    #endregion

    private void OnCancelButtonClicked()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
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
#else
        HideAddTaskPanel();
#endif

    }

    private void OnSaveBtnClicked()
    {
        if(choosenList == null)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            AGUIMisc.ShowToast("Tasks list is required.", AGUIMisc.ToastLength.Short);
#else
            Debug.LogError("Tasks list is required.");
#endif
            return;
        }

        if (IsTaskTitleEmpty())
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            AGUIMisc.ShowToast("Task title is required.", AGUIMisc.ToastLength.Long);
#else
            Debug.LogError("field cannot be empty");
#endif
            return;
        }
        taskTitle = GetTheTaskTitle();

        taskDescription = GetTheTaskDescription();

        timeFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, timeFrom.Hour, timeFrom.Minute, timeFrom.Second);
        timeTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, timeTo.Hour, timeTo.Minute, timeTo.Second);

        if(DateTime.Compare(timeFrom, timeTo)> 0)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            AGUIMisc.ShowToast("The start time must be earlier than the finish time", AGUIMisc.ToastLength.Long);
#else
            Debug.LogError("The start time must be earlier than the finish time");
#endif

            return;
        }

        if (parentEvent != null)
        {
            bool isWithinParentTimeRange = false;
            foreach (EventTimeSlotModel parentTimeSlot in parentEvent.timeSlots)
            {
                if (DateTime.Compare(timeFrom, parentTimeSlot.timeFrom) < 0 && DateTime.Compare(timeTo, parentTimeSlot.timeTo) > 0)
                {
                    isWithinParentTimeRange = true;
                    break;
                }
            }

            if (!isWithinParentTimeRange)
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                AGUIMisc.ShowToast("Event time frame must be with its parent.", AGUIMisc.ToastLength.Long);
#else
                Debug.LogError("Event time frame must be with its parent.");
#endif
                return;
            }
        }

        if (parentTask != null)
        {
            if (DateTime.Compare(timeFrom, parentTask.timeFrom) < 0 && DateTime.Compare(timeTo, parentTask.timeTo) > 0)
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                AGUIMisc.ShowToast("Event time frame must be with its parent.", AGUIMisc.ToastLength.Long);
#endif

                return;
            }
        }

        if (isEdit)
        {
            currentTask.taskTitle = taskTitle;
            currentTask.taskDescription = taskDescription;
            currentTask.timeFrom = timeFrom;
            currentTask.timeTo = timeTo;
            currentTask.taskList = choosenList;
            currentTask.repeat = choosenRepeatOption;
            currentTask.reminders = chosenReminders.ToArray();
            currentTask.color = chosenColor;
            currentTask.parentEvent = parentEvent;
            currentTask.parentTask = parentTask;
            TaskModel.UpdateTask(ref currentTask);
        }
        else
        {
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
            TaskModel.SaveTask(ref newTask);

        }
        EventSystem.instance.NewTaskAdded();
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

    private DateTime dateFrom;
    private DateTime dateTo;
    private DateTime timeFrom;
    private DateTime timeTo;

    private RepeatModel choosenRepeatOption = null;

    private List<NotifiAlarmReminderModel> chosenReminders = new List<NotifiAlarmReminderModel>();

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
#if UNITY_ANDROID && !UNITY_EDITOR
            Debug.Log("The platform is android");
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
#else
            Debug.Log("Choose list is clicked and chosen the first option");
            choosenList = lists[0];
            chooseListBtn.GetComponentInChildren<TMP_Text>().text = choosenList.listTitle;
#endif
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
#if UNITY_ANDROID && !UNITY_EDITOR
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
#else
            Debug.Log("Pretend to do something but nothing will happen because it is not android");
#endif
        }
    }

    private void OnDateFromBtnClicked()
    {
        if (dateFromBtn != null)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
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
#else
            Debug.Log("Pretend to do something but every thing will be the same because this is not an android device");
#endif
        }
    }

    private void OnTimeToBtnClicked()
    {

#if UNITY_ANDROID && !UNITY_EDITOR
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
#else
        Debug.Log("Pretend to do something but every thing will be the same because this is not an android device");
#endif

    }

    private void OnDateToBtnClicked()
    {
        if (dateTo != null)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
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
#else
            Debug.Log("Pretend to do something but every thing will be the same because this is not an android device");
#endif
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

#if UNITY_ANDROID && !UNITY_EDITOR
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
#else
        Debug.Log("Pretend to do something but every thing will be the same because this is not an android device");
        choosenRepeatOption = new RepeatModel(RepeatPeriod.Day, new WeekDays[] { });
#endif
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
            SetChooseColorBtn(chosenColor);
        });

    }

    private void SetChooseColorBtn(ColorModel color)
    {
        if(chooseColorBtn != null)
        {
            chooseColorBtn.GetComponentInChildren<TMP_Text>().text = color.colorName;
            chooseColorBtn.GetComponentInChildren<Image>().color = new Color32(
                Helper.StringToByteArray(color.colorValue)[0],
                Helper.StringToByteArray(color.colorValue)[1],
                Helper.StringToByteArray(color.colorValue)[2],
                0xFF
                );
        }
    }
#endregion

#region Choose Parent
    public Button chooseParentBtn;
    private void OnChooseParentBtnClicked()
    {
        TaskModel[] tasks = TaskModel.GetTasks();
        EventModel[] events = EventModel.GetEvents();
        string[] eventTaskTitles = { "Event", "Task" };

        string chooseParentBtnTxt = "";

#if UNITY_ANDROID && !UNITY_EDITOR
#else
#endif
        AGAlertDialog.ShowChooserDialog(
            "Choose the type of parent",
            eventTaskTitles,
            index =>
            {
            switch (index)
            {
                case 0:
                        chooseParentBtnTxt += "Event: ";
                        string[] eventsTitles = EventModel.GetEventsTitles(events);
#if UNITY_ANDROID && !UNITY_EDITOR
#else
#endif
                        AGAlertDialog.ShowChooserDialog(
                            "Choose Event",
                            eventsTitles,
                            i =>
                            {
                                parentEvent = events[i];
                                chooseParentBtnTxt += parentEvent.eventTitle;
                            }
                                );
                        break;
                    case 1:
                        chooseParentBtnTxt += "Task: ";
                        string[] tasksTitles = TaskModel.GetTasksTitles(tasks);
#if UNITY_ANDROID && !UNITY_EDITOR
#else
#endif
                        AGAlertDialog.ShowChooserDialog(
                            "Choose Task",
                            tasksTitles,
                            i =>
                            {
                                parentTask = tasks[i];
                                chooseParentBtnTxt += parentTask.taskTitle;
                            }
                                );
                        break;

                }
            },
            AGDialogTheme.Dark
            );
        chooseParentBtn.GetComponentInChildren<TMP_Text>().text = chooseParentBtnTxt;
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

    #region General

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

        if(markStartedBtn != null)
        {
            markStartedBtn.onClick.AddListener(OnMarkStartedBtnClicked);
        }

        if(markFinishedBtn != null)
        {
            markFinishedBtn.onClick.AddListener(OnMarkFinishedBtnClicked);
        }
    }

    public void OnStartup()
    {
        dateFrom = DateTime.Now.Date;
        timeFrom = DateTime.Now;
        dateTo = DateTime.Now.Date;
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

    #endregion
}
