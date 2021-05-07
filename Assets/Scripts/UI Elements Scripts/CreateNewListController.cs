using DeadMosquito.AndroidGoodies;
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

    // Update is called once per frame
    void Update()
    {
        
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

    private void OnSaveBtnClicked()
    {
        if (listTitleInputField != null)
        {
            string str = listTitleInputField.text;
            if (string.IsNullOrEmpty(str))
            {
                AGUIMisc.ShowToast("List title is required.", AGUIMisc.ToastLength.Short);
                return;
            }
            listTitle = str;
        }

        TasksListModel newList = new TasksListModel(title: listTitle);
        TasksListModel.SaveList(newList);
        EventSystem.instance.AddNewTasksList();
        HideAddTasksListPanel();

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
