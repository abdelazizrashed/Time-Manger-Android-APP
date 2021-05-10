using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NavBar : MonoBehaviour
{
    #region Main Unity main methods
    // Start is called before the first frame update
    void Start()
    {
        AttachActionsToEvents();
        AddListenerToBtns();
        CloseAllMenus();
        OnStartUp();
    }

    private void OnDestroy()
    {
        DeattachActionsToEvents();
    }
    #endregion

    #region Data variables

    private Pages currentPage;

    #endregion 

    #region UI Variables
    public Button sideMenuBtn;
    public GameObject sideMenuPrefab;
    private GameObject sideMenuGameObject = null;

    public Button moreOptionsBtn;
    public GameObject moreOptionsMenuPrefab;
    private GameObject moreOptionsMenuGameObject = null;

    public TMPro.TMP_Text pageTitleTxt;

    public GameObject shadowPanel;

    #endregion

    #region Nav bar manager methods

    private void OnStartUp()
    {
        Pages page = Pages.Events;
        if (!IsNull(pageTitleTxt) && page != null)
        {
            CloseAllMenus();
            currentPage = page;
            if (page.Value == Pages.Tasks.Value)
            {
                pageTitleTxt.text = page.tasksList.listTitle;
            }
            else
            {
                pageTitleTxt.text = page.Value;
            }
        }
    }

    private void AddListenerToBtns()
    {
        if (!IsNull(sideMenuBtn))
        {
            sideMenuBtn.onClick.AddListener(OnSideMenuBtnClicked);
        }

        if (!IsNull(moreOptionsBtn))
        {
            moreOptionsBtn.onClick.AddListener(OnMoreOptionsBtnClicked);
        }

        if(shadowPanel != null)
        {
            shadowPanel.GetComponent<Button>().onClick.AddListener(OnShadowPanelClicked);
        }
        else
        {
            Debug.LogError("Shadow panel need to be attached");
        }
    }

    #region Side Menu
    private bool isSideMenuOpen;
    private void OnSideMenuBtnClicked()
    {
        Debug.Log("Side menu btn is invoked");
        if (!isSideMenuOpen)
        {
            OpenSideMenu();
        }
    }

    public void OnShadowPanelClicked()
    {
        Debug.Log("Side menu shadow btn got clicked");
        sideMenuBtn.onClick.Invoke();
        if (isSideMenuOpen)
        {
            sideMenuBtn.onClick.Invoke();
            CloseSideMenu();
        }

        if (isMoreOptionsMenuOpen)
        {
            CloseMoreOptionsMenu();
        }
    }

    private void OpenSideMenu()
    {
        if (! IsNull(sideMenuPrefab)){
            if (shadowPanel == null)
            {
                Debug.LogError("Shadow panel need to be attached");
            }
            shadowPanel.SetActive(true);
            sideMenuGameObject = Instantiate(sideMenuPrefab, shadowPanel.transform);
            sideMenuGameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-1111, 0);
            sideMenuGameObject.SetActive(true);
            sideMenuGameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(799.98f, 0), 0.25f);
            isSideMenuOpen = true;
        }
    }

    private void CloseSideMenu()
    {
        shadowPanel.SetActive(false);
        if (sideMenuGameObject != null)
        {
            if (shadowPanel == null)
            {
                Debug.LogError("Shadow panel need to be attached");
            }
            sideMenuGameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-2000f, -2355.483f), 0.25f);
            Destroy(sideMenuGameObject);
            sideMenuGameObject = null;
            
            isSideMenuOpen = false;
        }
    }

    #endregion
    #region More Options menu

    private bool isMoreOptionsMenuOpen = false;

    private void OnMoreOptionsBtnClicked()
    {
        
        OpenMoreOptionsMenu();
        
    }

    private void OpenMoreOptionsMenu()
    {
        if (!IsNull(moreOptionsMenuPrefab))
        {
            if (shadowPanel == null)
            {
                Debug.LogError("Shadow panel need to be attached");
            }
            shadowPanel.SetActive(true);
            moreOptionsMenuGameObject = Instantiate(moreOptionsMenuPrefab, shadowPanel.transform);
            moreOptionsMenuGameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-418.98f, 744);
            moreOptionsMenuGameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-418.98f, -300.9f), 0.25f);

            moreOptionsMenuGameObject.SetActive(true);
            isMoreOptionsMenuOpen = true;
        }
    }

    private void CloseMoreOptionsMenu()
    {
        shadowPanel.SetActive(false);
        if (moreOptionsMenuGameObject != null)
        {
            if (shadowPanel == null)
            {
                Debug.LogError("Shadow panel need to be attached");
            }
            moreOptionsMenuGameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-418.98f, 744), 0.25f);
            Destroy(moreOptionsMenuGameObject);
            moreOptionsMenuGameObject = null;

            isMoreOptionsMenuOpen = false;
        }

    }

    #endregion 

    private void CloseAllMenus()
    {
        CloseSideMenu();
        CloseMoreOptionsMenu();
    }

    #endregion

    #region Events related methods
    
    private void OnChagePage(Pages page)
    {
        if (!IsNull(pageTitleTxt) && page != null){
            CloseAllMenus();
            currentPage = page;
            if(page.Value == Pages.Tasks.Value)
            {
                pageTitleTxt.text = page.tasksList.listTitle;
            }
            else
            {
                pageTitleTxt.text = page.Value;
            }
            //Todo: update the content
        }
    }

    private void AttachActionsToEvents()
    {
        EventSystem.instance.onChangePage += OnChagePage;
    }

    private void DeattachActionsToEvents()
    {
        EventSystem.instance.onChangePage -= OnChagePage;
    }

    #endregion

    #region Helper functions

    private bool IsNull(object toCheck)
    {
        if (toCheck == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion
}