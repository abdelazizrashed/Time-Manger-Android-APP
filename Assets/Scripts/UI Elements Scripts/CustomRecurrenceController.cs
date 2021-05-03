using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DeadMosquito.AndroidGoodies;
using System;

public class CustomRecurrenceController : MonoBehaviour
{
    [HideInInspector]
    public int parentPageNumber;
    //public Button 
    // Start is called before the first frame update
    void Start()
    {
        AddListnersToBtns();

        OnStartup();
    }

    public Button backBtn;
    public Button doneBtn;

    public TMP_InputField repeatEveryNumInputField;
    public Button repeatEveryPeriodBtn;

    public Button sunBtn;
    public Button monBtn;
    public Button tuesBtn;
    public Button wednesBtn;
    public Button thursBtn;
    public Button friBtn;
    public Button saterBtn;

    public Button chooseEndsOptionbtn;

    #region Data Variables

    private int repeatEveryNum = 0;
    private RepeatPeriod? repeatPeriodSelectedOption = null;

    private bool isSunBtnSelected = false;
    private bool isMonBtnSelected = false;
    private bool isTuesBtnSelected = false;
    private bool isWednesBtnSelected = false;
    private bool isThursBtnSelected = false;
    private bool isFriBtnSelected = false;
    private bool isSaterBtnSelected = false;

    private DateTime? endDate = null;

    #endregion

    private void OnCancelBtnClicked()
    {
        EventSystem.instance.OnAddTaskCustomRepeatSave(null, parentPageNumber);
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void OnDoneBtnClicked()
    {
        if(repeatEveryNumInputField != null)
        {
            string repeatNumberStr = repeatEveryNumInputField.text;
            if (String.IsNullOrEmpty(repeatNumberStr) || repeatNumberStr == "0")
            {
                if (repeatEveryPeriodBtn != null)
                {
                    AGUIMisc.ShowToast("You need to supply the number of " + repeatEveryPeriodBtn.GetComponentInChildren<TMP_Text>().text, AGUIMisc.ToastLength.Long);
                }
                return;
            }
            repeatEveryNum = int.Parse(repeatNumberStr);
        }

        if (!isSaterBtnSelected && !isSunBtnSelected && !isMonBtnSelected && !isTuesBtnSelected && !isWednesBtnSelected && !isThursBtnSelected && !isFriBtnSelected)
        {
            AGUIMisc.ShowToast("You need to choose a day.", AGUIMisc.ToastLength.Long);
            return;
        }

        List<WeekDays> repeatDays = new List<WeekDays>();
        if (isSaterBtnSelected)
        {
            repeatDays.Add(WeekDays.Saterday);
        }
        if (isSunBtnSelected)
        {
            repeatDays.Add(WeekDays.Sunday);
        }
        if (isMonBtnSelected)
        {
            repeatDays.Add(WeekDays.Monday);
        }
        if (isTuesBtnSelected)
        {
            repeatDays.Add(WeekDays.Tuesday);
        }
        if (isWednesBtnSelected)
        {
            repeatDays.Add(WeekDays.Wednesday);
        }
        if (isThursBtnSelected)
        {
            repeatDays.Add(WeekDays.Thursday);
        }
        if (isFriBtnSelected)
        {
            repeatDays.Add(WeekDays.Friday);
        }

        RepeatModel customRepeat = new RepeatModel(repeatPeriodSelectedOption ?? RepeatPeriod.Week, repeatDays.ToArray(), repeatEveryNum, endDate);

        EventSystem.instance.OnAddTaskCustomRepeatSave(customRepeat, parentPageNumber);
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void OnRepeatEveryPeriodBtnClicked()
    {
        string[] repeatPeriodOptions = {"weeks", "months", "years" };

        AGAlertDialog.ShowChooserDialog("", repeatPeriodOptions, index =>
        {
            if (repeatEveryPeriodBtn != null)
            {
                repeatEveryPeriodBtn.GetComponentInChildren<TMP_Text>().text = repeatPeriodOptions[index];
            }
            switch (index)
            {
                case 0:
                    repeatPeriodSelectedOption = RepeatPeriod.Week;
                    break;
                case 1:
                    repeatPeriodSelectedOption = RepeatPeriod.Month;
                    break;
                case 2:
                    repeatPeriodSelectedOption = RepeatPeriod.Year;
                    break;

            }
        }, AGDialogTheme.Dark);
    }

    #region Repeat Days

    private void OnSunBtnClicked()
    {
        Debug.Log(isSunBtnSelected);
        if (isSunBtnSelected)
        {
            DeselectDayBtn(sunBtn);
            isSunBtnSelected = false;
        }
        else
        {
            SelectDayBtn(sunBtn);
            isSunBtnSelected = true;
        }
    }
    private void OnMonBtnClicked()
    {
        if (isMonBtnSelected)
        {
            DeselectDayBtn(monBtn);
            isMonBtnSelected = false;
        }
        else
        {
            SelectDayBtn(monBtn);
            isMonBtnSelected = true;
        }
    }
    private void OnTuesBtnClicked()
    {
        if (isTuesBtnSelected)
        {
            DeselectDayBtn(tuesBtn);
            isTuesBtnSelected = false;
        }
        else
        {
            SelectDayBtn(tuesBtn);
            isTuesBtnSelected = true;
        }
    }
    private void OnWednesBtnClicked()
    {
        if (isWednesBtnSelected)
        {
            DeselectDayBtn(wednesBtn);
            isWednesBtnSelected = false;
        }
        else
        {
            SelectDayBtn(wednesBtn);
            isWednesBtnSelected = true;
        }
    }
    private void OnThursBtnClicked()
    {
        if (isThursBtnSelected)
        {
            DeselectDayBtn(thursBtn);
            isThursBtnSelected = false;
        }
        else
        {
            SelectDayBtn(thursBtn);
            isThursBtnSelected = true;
        }
    }
    private void OnFriBtnClicked()
    {
        if (isFriBtnSelected)
        {
            DeselectDayBtn(friBtn);
            isFriBtnSelected = false;
        }
        else
        {
            SelectDayBtn(friBtn);
            isFriBtnSelected = true;
        }
    }
    private void OnSaterBtnClicked()
    {
        if (isSaterBtnSelected)
        {
            DeselectDayBtn(saterBtn);
            isSaterBtnSelected = false;
        }
        else
        {
            SelectDayBtn(saterBtn);
            isSaterBtnSelected = true;
        }
    }

    private void SelectDayBtn(Button button)
    {

        if (button != null)
        {
            button.GetComponent<Image>().color = new Color(43/255f, 47/255f, 212/255f);
        }
    }

    private void DeselectDayBtn(Button button)
    {
        if (button != null)
        {
            button.GetComponent<Image>().color = new Color(85/255f, 95/255f, 115/255f);
        }
    }

    #endregion


    private void OnChooseEndsOptionBtnClicked()
    {
        string[] options = { "Never", "On day" };
        AGAlertDialog.ShowChooserDialog("Repeat End", options, index =>
        {
            if (index == 0)
            {
                endDate = null;
                if (chooseEndsOptionbtn != null)
                {
                    chooseEndsOptionbtn.GetComponentInChildren<TMP_Text>().text = "Never";
                }
            }
            else
            {
                AGDateTimePicker.ShowDatePicker(
                    DateTime.Now.Year,
                    DateTime.Now.Month,
                    DateTime.Now.Day,
                    (year, month, day) =>
                    {
                        endDate = new DateTime(year, month, day);
                        if(chooseEndsOptionbtn != null)
                        {
                            string s = new DateTime(year, month, day).ToString("MMM d, yyyy");
                            chooseEndsOptionbtn.GetComponentInChildren<TMP_Text>().text = s;
                        }
                    },
                    () => { },
                    AGDialogTheme.Dark
                );
            }
        }, AGDialogTheme.Dark);
    }

    private void AddListnersToBtns()
    {
        if (repeatEveryPeriodBtn != null)
        {
            repeatEveryPeriodBtn.onClick.AddListener(OnRepeatEveryPeriodBtnClicked);
        }

        if (sunBtn != null)
        {
            sunBtn.onClick.AddListener(OnSunBtnClicked);
        }

        if (monBtn != null)
        {
            monBtn.onClick.AddListener(OnMonBtnClicked);
        }

        if (tuesBtn != null)
        {
            tuesBtn.onClick.AddListener(OnTuesBtnClicked);
        }

        if (wednesBtn != null)
        {
            wednesBtn.onClick.AddListener(OnWednesBtnClicked);
        }

        if (thursBtn != null)
        {
            thursBtn.onClick.AddListener(OnThursBtnClicked);
        }

        if (friBtn != null)
        {
            friBtn.onClick.AddListener(OnFriBtnClicked);
        }

        if (saterBtn != null)
        {
            saterBtn.onClick.AddListener(OnSaterBtnClicked);
        }

        if (chooseEndsOptionbtn != null)
        {
            chooseEndsOptionbtn.onClick.AddListener(OnChooseEndsOptionBtnClicked);
        }

        if (backBtn != null)
        {
            backBtn.onClick.AddListener(OnCancelBtnClicked);
        }

        if(doneBtn != null)
        {
            doneBtn.onClick.AddListener(OnDoneBtnClicked);
        }
    }

    private void OnStartup()
    {
        if (repeatEveryPeriodBtn != null)
        {
            repeatEveryPeriodBtn.GetComponentInChildren<TMP_Text>().text = "weeks";
            repeatPeriodSelectedOption = RepeatPeriod.Week;

        }
    }
}
