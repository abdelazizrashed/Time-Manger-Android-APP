using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DeadMosquito.AndroidGoodies;
using DG.Tweening;

public class AddEventController : MonoBehaviour
{
    #region Main funcgtions
    // Start is called before the first frame update
    void Start()
    {
        AddListeners();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

        if(timeSlots.Count == 0)
        {
            AGUIMisc.ShowToast("You need to supply at least one time slot.", AGUIMisc.ToastLength.Long);
        }

        EventModel newEvent = new EventModel(
            _eventTitle: eventTitle,
            _eventDescription: eventDescription,
            _userID: UserModel.Instance.userID,
            _timeSlots: timeSlots.ToArray(),
            _color: color,
            _parent: parent
            );
        EventModel.SaveEvent(newEvent);
        EventSystem.instance.AddNewEvent();
        HideAddEventPanel();

    }

    #endregion

    #region InputHandling

    #region Data variables

    private string eventTitle;
    private string eventDescription;

    private List<EventTimeSlotModel> timeSlots = new List<EventTimeSlotModel>();

    private ColorModel color = null;

    private EventModel parent = null;

    #endregion

    #region Title and Description

    public TMP_InputField eventTitleInputField;
    public TMP_InputField eventDescriptionInputField;

    #endregion

    #region Time Slots

    //Add new time slot 

    #region Date time of the slot

    #endregion

    #region Repeat

    #endregion

    #region Location

    #endregion

    #region Reminder

    #endregion



    #endregion

    #region Choose Color

    #endregion

    #region Choose Parent

    #endregion

    #endregion

    #region Events

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
    }

    private void OnStartUp()
    {

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
