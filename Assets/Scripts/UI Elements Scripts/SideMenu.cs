using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SideMenu : MonoBehaviour
{
    #region Main functions
    // Start is called before the first frame update
    void Start()
    {
        AddListners();
        OnStartUp();
    }

    #endregion

    #region Controls

    //private void CloseSideMenu()
    //{
    //    Debug.Log("Close side menu");
    //    GameObject shadowPanel = GameObject.FindGameObjectWithTag("MenusShadowPanel");
    //    if(shadowPanel != null)
    //    {
    //        shadowPanel.SetActive(false);
    //        if (gameObject != null)
    //        {
    //            gameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-2000f, -2355.483f), 0.25f);
    //            Destroy(gameObject);
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError("Shadow panel need to be attached");
    //    }
    //}

    #endregion

    #region Content

    #region UserInfo
    public TMP_Text userFNameLNameTxt;
    public TMP_Text userEmailTxt;
    #endregion

    #region All

    public Button allBtn;
    
    private void OnAllBtnClicked()
    {
        EventSystem.instance.ChangePage(Pages.All);
        
    }

    #endregion

    #region Tasks

    public GameObject sideMenuTasksListBtnPrefab;
    public GameObject listsBtnList;

    private void SetTasksLists()
    {
        TasksListModel[] lists = TasksListModel.GetLists();
        foreach(TasksListModel list in lists)
        {
            GameObject listGameObject = Instantiate(sideMenuTasksListBtnPrefab, listsBtnList.transform );
            listGameObject.GetComponentInChildren<TMP_Text>().text = list.listTitle;
            listGameObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                TasksListBtnClicked(list);
            });
        }
    }

    private void TasksListBtnClicked(TasksListModel list)
    {
        Pages page = Pages.Tasks;
        page.tasksList = list;
        EventSystem.instance.ChangePage(page);
        //CloseSideMenu();
    }

    public Button createNewListBtn;
    public GameObject createNewListPrefab;

    private void OnCreateNewListBtnClicked()
    {
        GameObject background = GameObject.FindGameObjectWithTag("MainBackground");
        Instantiate(createNewListPrefab, background.transform);
        EventSystem.instance.CloseSideMenuPanel();
    }

    #endregion

    #region Events

    public Button eventsBtn;
    private void OnEventsBtnClicked()
    {
        EventSystem.instance.ChangePage(Pages.Events);
    }

    #endregion

    #region Reminders

    public Button remindersBtn;
    private void OnRemindersBtnClicked()
    {
        EventSystem.instance.ChangePage(Pages.Reminders);
        
    }

    #endregion

    #endregion

    #region General

    private void AddListners()
    {
        if(allBtn != null)
        {
            allBtn.onClick.AddListener(OnAllBtnClicked);
        }

        if (eventsBtn != null)
        {
            eventsBtn.onClick.AddListener(OnEventsBtnClicked);
        }

        if (remindersBtn != null)
        {
            remindersBtn.onClick.AddListener(OnRemindersBtnClicked);
        }

        if(createNewListBtn != null)
        {
            createNewListBtn.onClick.AddListener(OnCreateNewListBtnClicked);
        }
    }

    private void OnStartUp()
    {
        userEmailTxt.text = UserModel.Instance.email;
        userFNameLNameTxt.text = UserModel.Instance.firstName + " " + UserModel.Instance.lastName;
        SetTasksLists();
    }

    #endregion

}
