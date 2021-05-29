using DeadMosquito.AndroidGoodies;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateNewListController : MonoBehaviour
{
    #region Main functions
    // Start is called before the first frame update
    void Start()
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

    #endregion

    #region Controller
    private void HideAddTasksListPanel()
    {
        Destroy(gameObject);
    }

    #region Save Cancel

    public Button cancelBtn;
    public Button saveBtn;

    private void OnCancelBtnClicked()
    {
        try
        {
            if(Application.platform == RuntimePlatform.Android)
            {
                Debug.Log("The current platform is android");
                AGAlertDialog.ShowMessageDialog(
                    "",
                    "Discard this list?",
                    "Keep editing", () => { },
                    negativeButtonText: "Discard", onNegativeButtonClick: () =>
                    {
                        HideAddTasksListPanel();
                    },
                    theme: AGDialogTheme.Dark
                    );
            }
            else
            {
                HideAddTasksListPanel();

            }
        }catch(Exception e)
        {
            if(Application.platform == RuntimePlatform.Android)
            {
                AGUIMisc.ShowToast(e.Message, AGUIMisc.ToastLength.Long);
            }
            else
            {
                Debug.LogError(e.Message);
            }
        }
    }

    private void OnSaveBtnClicked()
    {
        try
        {
            if (listTitleInputField != null)
            {
                string str = listTitleInputField.text;
                if (string.IsNullOrEmpty(str))
                {
#if UNITY_ANDROID && !UNITY_EDITOR
                    AGUIMisc.ShowToast("List title is required.", AGUIMisc.ToastLength.Short);
#else
                    Debug.LogError("List title is required");
#endif
                    return;
                }
                listTitle = str;
            }

            TasksListModel newList = new TasksListModel(title: listTitle);
            TasksListModel.SaveList(ref newList);
            EventSystem.instance.AddNewTasksList();
            HideAddTasksListPanel();
        }catch(Exception e)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                AGUIMisc.ShowToast(e.Message, AGUIMisc.ToastLength.Long);
            }
            else
            {
                Debug.LogError(e.Message);
            }
        }

    }

#endregion
#endregion

#region Input handling

#region Data vairables

    private string listTitle;

#endregion

    public TMP_InputField listTitleInputField;

#endregion

#region Event system

#endregion

#region General

#endregion

}
