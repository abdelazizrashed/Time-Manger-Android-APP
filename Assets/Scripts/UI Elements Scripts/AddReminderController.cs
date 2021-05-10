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

    public void SetUpFieldsForEdit(ReminderModel reminder)
    {

    }

    #region Save Cancel

    public Button cancelBtn;
    public Button saveBtn;

    private void OnCancelBtnClicked()
    {
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
    }

    private void OnSaveBtnClicked()
    {
        if (reminderTitleInputField != null)
        {
            string str = reminderTitleInputField.text;
            if (string.IsNullOrEmpty(str))
            {
                AGUIMisc.ShowToast("Reminder title is required.", AGUIMisc.ToastLength.Short);
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
            AGUIMisc.ShowToast("You need to supply at least one time slot.", AGUIMisc.ToastLength.Long);
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
                    AGUIMisc.ShowToast("Event time frame must be with its parent.", AGUIMisc.ToastLength.Long);
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
        ReminderModel.SaveReminder(newReminder);
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

    #region Choose parent

    public Button chooseParentBtn;
    private void OnChooseParentBtnClicked()
    {
        EventModel[] events = EventModel.GetEvents();
        string chooseParentBtnTxt = "Event: ";

        string[] eventsTitles = EventModel.GetEventsTitles(events);
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
