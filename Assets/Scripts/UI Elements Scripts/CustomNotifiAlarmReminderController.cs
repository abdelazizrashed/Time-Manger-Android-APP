using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DeadMosquito.AndroidGoodies;

public class CustomNotifiAlarmReminderController : MonoBehaviour
{
    private int parentPageNum;
    // Start is called before the first frame update
    void Start()
    {
        AttachListners();
        OnStartUp();
    }

    public Button backBtn;
    public Button doneBtn;

    public TMP_InputField beforeNumInputField;
    public Button beforePeriodTypeBtn;
    public Button reminderTypeOptionBtn;

    #region Data variables

    private int beforeNum;
    private ReminderType reminderType = ReminderType.Notification;
    private TimePeriodsType timePeriodType = TimePeriodsType.Minutes;

    #endregion

    private void OnBackBtnClicked()
    {
        ClosePage();
    }

    private void OnDoneBtnClicked()
    {
        if(beforeNumInputField != null)
        {
            string s = beforeNumInputField.text;
            if (string.IsNullOrEmpty(s))
            {
                AGUIMisc.ShowToast("You need to specify the number of " + beforePeriodTypeBtn.GetComponentInChildren<TMP_Text>().text, AGUIMisc.ToastLength.Long);
            }
        }

        NotifiAlarmReminderModel reminder = new NotifiAlarmReminderModel(reminderType, timePeriodType, beforeNum);

        EventSystem.instance.OnAddTaskCustomReminderSave(reminder, parentPageNum);
    }

    private void OnBeforePeriodTypeBtnClicked()
    {
        string[] options = { "Minutes", "Hours", "Days", "Weeks" };
        AGAlertDialog.ShowChooserDialog(
            "Choose the period type",
            options,
            index =>
            {
                if(beforePeriodTypeBtn != null)
                {
                    beforePeriodTypeBtn.GetComponentInChildren<TMP_Text>().text = options[index];
                }
                switch (index)
                {
                    case 0:
                        timePeriodType = TimePeriodsType.Minutes;
                        break;
                    case 1:
                        timePeriodType = TimePeriodsType.Hours;
                        break;
                    case 2:
                        timePeriodType = TimePeriodsType.Days;
                        break;
                    case 3:
                        timePeriodType = TimePeriodsType.Weeks;
                        break;
                }
            },
            AGDialogTheme.Dark
            );
    }

    private void OnReminderTypeOptionBtnClicked()
    {
        string[] options = { "Alarm", "Notification" };
        AGAlertDialog.ShowChooserDialog(
            "Choose reminder type",
            options,
            index =>
            {
                if(reminderTypeOptionBtn != null)
                {
                    reminderTypeOptionBtn.GetComponentInChildren<TMP_Text>().text = options[index];
                }
                switch (index)
                {
                    case 0:
                        reminderType = ReminderType.Alarm;
                        break;
                    case 1:
                        reminderType = ReminderType.Notification;
                        break;
                }
            },
            AGDialogTheme.Dark
            );
    }

    private void ClosePage()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);

    }

    private void AttachListners()
    {
        if(backBtn != null)
        {
            backBtn.onClick.AddListener(OnBackBtnClicked);
        }

        if (doneBtn != null)
        {
            doneBtn.onClick.AddListener(OnDoneBtnClicked);
        }

        if(beforePeriodTypeBtn != null)
        {
            beforePeriodTypeBtn.onClick.AddListener(OnBeforePeriodTypeBtnClicked);
        }

        if (reminderTypeOptionBtn != null)
        {
            reminderTypeOptionBtn.onClick.AddListener(OnReminderTypeOptionBtnClicked);
        }
    }

    private void OnStartUp()
    {
        beforePeriodTypeBtn.GetComponentInChildren<TMP_Text>().text = "Minutes";
        reminderTypeOptionBtn.GetComponentInChildren<TMP_Text>().text = "Notification";
    }
}
