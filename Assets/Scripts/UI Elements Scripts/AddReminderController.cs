using DeadMosquito.AndroidGoodies;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddReminderController : MonoBehaviour
{
    #region Main functions   
    // Start is called before the first frame update
    void Start()
    {
        AddListners();
        OnStartUp();
    }

    #endregion

    #region Controll methods

    private void HideAddReminderPanel()
    {
        Destroy(gameObject);
    }

    private ReminderModel currentReminder;
    public TMP_Text addReminderTitleTxt;
    public void SetUpFieldsForEdit(ReminderModel reminder)
    {
        markDoneBtn.gameObject.SetActive(true);
        //title
        title = reminder.reminderTitle;
        if(reminderTitleInputField != null)
        {
            reminderTitleInputField.text = title;
        }
        if(addReminderTitleTxt != null)
        {
            addReminderTitleTxt.text = title;
        }

        //description
        description = reminder.reminderDescription;
        if(reminderDescriptionInputField != null)
        {
            reminderDescriptionInputField.text = description;
        }

        //time slots
        timeSlots = new List<ReminderTimeSlotModel>(reminder.timeSlots);

        foreach (ReminderTimeSlotModel timeSlot in timeSlots)
        {
            if (addReminderPanelContent != null && reminderTimeSlotPrefab != null)
            {
                GameObject newTimeSlot = Instantiate(reminderTimeSlotPrefab, addReminderPanelContent.transform);
                newTimeSlot.transform.SetSiblingIndex(3);
                newTimeSlot.GetComponent<ReminderTimeSlotController>().SetUpFieldsForEdit(timeSlot);
                timeSlotsGameObjects.Add(newTimeSlot);
            }
        }

        //color
        color = reminder.color;
        SetChooseColorBtn(color);

        //parent
        parentEvent = reminder.parentEvent;
        chooseParentBtn.GetComponentInChildren<TMP_Text>().text = "Event: " + parentEvent.eventTitle;

        currentReminder = reminder;
    }

    public Button markDoneBtn;

    private void OnMarkDoneBtnClicked()
    {
        List<string> timeSlotsTimesTitles = new List<string>();
        for (int i = 0; i < currentReminder.timeSlots.Length; i++)
        {
            timeSlotsTimesTitles.Add(timeSlots[i].time.ToString("t, MMM dd, yyyy"));
        }
#if UNITY_ANDROID && !UNITY_EDITOR
        AGAlertDialog.ShowChooserDialog(
            "Choose time slot to start",
            timeSlotsTimesTitles.ToArray(),
            index =>
            {
                ReminderTimeSlotModel chosenTimeSlot = currentReminder.timeSlots[index];
                string[] titles = { "Now", "Choose specific time" };
                AGAlertDialog.ShowChooserDialog(
                    "Choose time",
                    titles,
                    i =>
                    {
                        if (i == 0)
                        {
                            ReminderModel.MarkReminderDone(chosenTimeSlot, DateTime.Now);
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
                                            ReminderModel.MarkReminderDone(chosenTimeSlot, new DateTime(year, month, day, hour, minute, 0));
                                        },
                                        () =>
                                        {
                                            AGUIMisc.ShowToast("No time was selected so the reminder wasn't registed as done", AGUIMisc.ToastLength.Long);
                                        },
                                        AGDialogTheme.Dark
                                        );
                                },
                                () =>
                                {
                                    AGUIMisc.ShowToast("No date was selected so the reminder wasn't registed as done", AGUIMisc.ToastLength.Long);
                                },
                                AGDialogTheme.Dark
                                );
                        }
                    },
                    AGDialogTheme.Dark
                    );
            },
            AGDialogTheme.Dark
            );
#else
        ReminderModel.MarkReminderDone(timeSlots[0], DateTime.Now);
#endif
    }

#region Save Cancel

    public Button cancelBtn;
    public Button saveBtn;

    private void OnCancelBtnClicked()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AGAlertDialog.ShowMessageDialog(
            "",
            "Discard this task?",
            "Keep editing", () => { },
            negativeButtonText: "Discard", onNegativeButtonClick: () =>
            {
                HideAddReminderPanel();
            },
            theme: AGDialogTheme.Dark
            );
#else
        HideAddReminderPanel();
#endif
    }

    private void OnSaveBtnClicked()
    {
        if (reminderTitleInputField != null)
        {
            string str = reminderTitleInputField.text;
            if (string.IsNullOrEmpty(str))
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                AGUIMisc.ShowToast("Reminder title is required.", AGUIMisc.ToastLength.Short);
#else
                Debug.LogError("Reminder title is required.");
#endif
                return;
            }
            title = str;
        }

        if (reminderDescriptionInputField != null)
        {
            description = reminderDescriptionInputField.text;
        }

        foreach (GameObject timeSlotGameObject in timeSlotsGameObjects)
        {
            if (timeSlotGameObject != null)
            {
                timeSlots.Add(timeSlotGameObject.GetComponent<ReminderTimeSlotController>().GetTimeSlot());

            }
        }

        if (timeSlots.Count == 0)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            AGUIMisc.ShowToast("You need to supply at least one time slot.", AGUIMisc.ToastLength.Long);
#else
            Debug.LogError("You need to supply at least one time slot.");
#endif
            return;
        }

        if (parentEvent != null)
        {
            foreach (ReminderTimeSlotModel timeSlot in timeSlots)
            {
                bool isWithinParentTimeRange = false;
                foreach (EventTimeSlotModel parentTimeSlot in parentEvent.timeSlots)
                {
                    if (DateTime.Compare(timeSlot.time, parentTimeSlot.timeFrom) < 0 && DateTime.Compare(timeSlot.time, parentTimeSlot.timeTo) > 0)
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
        }

        ReminderModel newReminder = new ReminderModel(
            _reminderTitle: title,
            _description: description,
            _userID: UserModel.Instance.userID,
            _timeSlots: timeSlots.ToArray(),
            _color: color,
            _parentEvent: parentEvent
            );
        ReminderModel.SaveReminder(ref newReminder);
        EventSystem.instance.AddNewReminder();
        HideAddReminderPanel();

    }

#endregion

#endregion

#region Input handling

#region Data variables

    private string title;
    private string description;
    private List<GameObject> timeSlotsGameObjects = new List<GameObject>();
    private List<ReminderTimeSlotModel> timeSlots = new List<ReminderTimeSlotModel>();
    private ColorModel color;
    private EventModel parentEvent;

#endregion

#region Title and Description

    public TMP_InputField reminderTitleInputField;
    public TMP_InputField reminderDescriptionInputField;

#endregion

#region Time slots

    public Button addTimeSlotBtn;
    public GameObject reminderTimeSlotPrefab;
    public GameObject addReminderPanelContent;

    private void OnAddTimeSlotBtnClicked()
    {
        if (addReminderPanelContent != null && reminderTimeSlotPrefab != null)
        {
            GameObject newTimeSlot = Instantiate(reminderTimeSlotPrefab, addReminderPanelContent.transform);
            newTimeSlot.transform.SetSiblingIndex(3);
            timeSlotsGameObjects.Add(newTimeSlot);
        }
    }

#endregion

#region Choose color

    public Button chooseColorBtn;
    public GameObject colorBtnPrefab;

    private void OnChooseColorBtnClicked()
    {
        ColorModel[] colors = ColorModel.GetColors();
        CustomAlertDialog.ShowColorPickerDialog(colors, colorBtnPrefab, index =>
        {
            color = colors[index];
            SetChooseColorBtn(color);
        });

    }

    private void SetChooseColorBtn(ColorModel color)
    {
        if (chooseColorBtn != null)
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

#region Choose parent

    public Button chooseParentBtn;
    private void OnChooseParentBtnClicked()
    {
        EventModel[] events = EventModel.GetEvents();
        string chooseParentBtnTxt = "Event: ";

        string[] eventsTitles = EventModel.GetEventsTitles(events);
#if UNITY_ANDROID && !UNITY_EDITOR
        AGAlertDialog.ShowChooserDialog(
            "Choose Parent Event",
            eventsTitles,
            i =>
            {
                parentEvent = events[i];
                chooseParentBtnTxt += parentEvent.eventTitle;
            },
            AGDialogTheme.Dark
            );
#else
        parentEvent = events[0];
        chooseParentBtnTxt += parentEvent.eventTitle;
#endif

        chooseParentBtn.GetComponentInChildren<TMP_Text>().text = chooseParentBtnTxt;
    }

#endregion

#endregion

#region General
    private void AddListners()
    {
        if(cancelBtn != null)
        {
            cancelBtn.onClick.AddListener(OnCancelBtnClicked);
        }

        if(saveBtn != null)
        {
            saveBtn.onClick.AddListener(OnSaveBtnClicked);
        }

        if(addTimeSlotBtn != null)
        {
            addTimeSlotBtn.onClick.AddListener(OnAddTimeSlotBtnClicked);
        }

        if(chooseColorBtn != null)
        {
            chooseColorBtn.onClick.AddListener(OnChooseColorBtnClicked);
        }

        if(chooseParentBtn != null)
        {
            chooseParentBtn.onClick.AddListener(OnChooseParentBtnClicked);
        }

        if(markDoneBtn != null)
        {
            markDoneBtn.onClick.AddListener(OnMarkDoneBtnClicked);
        }
    }

    private void OnStartUp()
    {
        ColorModel[] colors = ColorModel.GetColors();
        color = colors[0];
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
