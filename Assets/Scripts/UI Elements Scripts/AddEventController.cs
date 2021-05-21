using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DeadMosquito.AndroidGoodies;
using DG.Tweening;
using System;

public class AddEventController : MonoBehaviour
{

    #region Main funcgtions
    // Start is called before the first frame update
    void Start()
    {
        AddListeners();

        OnStartUp();
    }

    #endregion

    #region Control

    #region Edit Event

    private EventModel currentEvent;

    public TMP_Text addEventTitleTxt;
    public Button markAsStartedBtn;
    public Button markAsFinishedBtn;

    public void SetUpFieldsForEdit(EventModel editEvent)
    {
        markAsFinishedBtn.gameObject.SetActive(true);
        markAsStartedBtn.gameObject.SetActive(true);
        if (addEventTitleTxt != null)
        {
            addEventTitleTxt.text = editEvent.eventTitle;
        }
        if(eventTitleInputField != null)
        {
            eventTitleInputField.text = editEvent.eventTitle;
            eventTitle = editEvent.eventTitle;
        }
        if(eventDescriptionInputField != null)
        {
            eventDescriptionInputField.text = editEvent.eventDescription;
            eventDescription = editEvent.eventDescription;
        }

        timeSlots = new List<EventTimeSlotModel>(editEvent.timeSlots);

        foreach(EventTimeSlotModel timeSlot in timeSlots)
        {
            if (addEventPanelContent != null && eventTimeSlotPrefab != null)
            {
                GameObject newTimeSlot = Instantiate(eventTimeSlotPrefab, addEventPanelContent.transform);
                newTimeSlot.transform.SetSiblingIndex(3);
                newTimeSlot.GetComponent<EventTimeSlotController>().SetUpFieldsForEdit(timeSlot);
                timeSlotsGameObjects.Add(newTimeSlot);
            }
        }
        color = editEvent.color;
        SetChooseColorBtn(color);
        currentEvent = editEvent;
    }

    #region Start Finish

    private void OnMarkStartedBtnClicked()
    {
        string[] titles = { "Now", "Pick specific time" };
        AGAlertDialog.ShowChooserDialog(
            "Choose start time",
            titles,
            index =>
            {
                if(index == 0)
                {

                }
            },
            AGDialogTheme.Dark
            );
    }

    private void OnMarkFinishedBtnClicked()
    {

    }

    #endregion

    #endregion

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
                HideAddEventPanel();
            },
            theme: AGDialogTheme.Dark
            );
    }

    private void OnSaveBtnClicked()
    {
        if(eventTitleInputField != null)
        {
            string str = eventTitleInputField.text;
            if (string.IsNullOrEmpty(str))
            {
                AGUIMisc.ShowToast("Event title is required.", AGUIMisc.ToastLength.Short);
                return;
            }
            eventTitle = str;
        }
        
        if(eventDescriptionInputField != null)
        {
            eventDescription = eventDescriptionInputField.text;
        }

        if(timeSlotsGameObjects.Count == 0)
        {
            AGUIMisc.ShowToast("You need to supply at least one time slot.", AGUIMisc.ToastLength.Long);
            return;
        }

        foreach(GameObject timeSlotGameObject in timeSlotsGameObjects)
        {
            if(timeSlotGameObject != null)
            {
                timeSlots.Add(timeSlotGameObject.GetComponent<EventTimeSlotController>().GetTimeSlot());
            }
        }

        if(parentEvent != null)
        {
            foreach(EventTimeSlotModel timeSlot in timeSlots)
            {
                bool isWithinParentTimeRange = false;
                foreach(EventTimeSlotModel parentTimeSlot in parentEvent.timeSlots)
                {
                    if(DateTime.Compare(timeSlot.timeFrom, parentTimeSlot.timeFrom) < 0 && DateTime.Compare(timeSlot.timeTo, parentTimeSlot.timeTo) > 0)
                    {
                        isWithinParentTimeRange = true;
                        break;
                    }
                }

                if(!isWithinParentTimeRange)
                {
                    AGUIMisc.ShowToast("Event time frame must be with its parent.", AGUIMisc.ToastLength.Long);
                    return;
                }

            }
        }

        EventModel newEvent = new EventModel(
            _eventTitle: eventTitle,
            _eventDescription: eventDescription,
            _userID: UserModel.Instance.userID,
            _timeSlots: timeSlots.ToArray(),
            _color: color,
            _parent: parentEvent
            );
        EventModel.SaveEvent(ref newEvent);
        EventSystem.instance.AddNewEvent();
        HideAddEventPanel();

    }

    #endregion

    #endregion

    #region InputHandling

    #region Data variables

    private string eventTitle;
    private string eventDescription;

    private List<GameObject> timeSlotsGameObjects = new List<GameObject>();
    private List<EventTimeSlotModel> timeSlots = new List<EventTimeSlotModel>();

    private ColorModel color = null;

    private EventModel parentEvent = null;

    #endregion

    #region Title and Description

    public TMP_InputField eventTitleInputField;
    public TMP_InputField eventDescriptionInputField;

    #endregion

    #region Time Slots

    public Button addTimeSlotBtn;
    public GameObject eventTimeSlotPrefab;
    public GameObject addEventPanelContent;

    private void OnAddTimeSlotBtnClicked()
    {
        if (addEventPanelContent != null && eventTimeSlotPrefab != null)
        {
            GameObject newTimeSlot = Instantiate(eventTimeSlotPrefab, addEventPanelContent.transform);
            newTimeSlot.transform.SetSiblingIndex(3);
            timeSlotsGameObjects.Add(newTimeSlot);
        }
    }

    #endregion

    #region Choose Color

    public Button chooseColorBtn;
    public GameObject colorBtnPrefab;

    private void OnChooseColorBtnClicked()
    {
        ColorModel[] colors = ColorModel.GetColors();
        CustomAlertDialog.ShowColorPickerDialog(colors, colorBtnPrefab, index =>
        {
            color = colors[index];
            SetChooseColorBtn(colors[index]);
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
        EventModel[] events = EventModel.GetEvents();
        string chooseParentBtnTxt = "Event: ";

        string[] eventsTitles = EventModel.GetEventsTitles(events);
        AGAlertDialog.ShowChooserDialog(
            "Choose Event",
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

    #region General methods

    private void AddListeners()
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

        if(markAsStartedBtn != null)
        {
            markAsStartedBtn.onClick.AddListener(OnMarkStartedBtnClicked);
        }

        if(markAsFinishedBtn != null)
        {
            markAsFinishedBtn.onClick.AddListener(OnMarkFinishedBtnClicked);
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

    private void HideAddEventPanel()
    {
        if (gameObject != null)
        {
            gameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, -2000f), 0.25f);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    #endregion

}
