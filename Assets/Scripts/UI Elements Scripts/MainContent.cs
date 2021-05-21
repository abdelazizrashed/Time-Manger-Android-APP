using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Threading;

public class MainContent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AddListenersToBtns();
        OnStartUp();
    }

    // Update is called once per frame
    void Update()
    {
        CheckSwipeDirectionAlongX();
    }

    private DateTime currentDay;
    private Pages currentPage;
    #region Gesture Control


    private Vector2 fingerDown = new Vector2();
    private Vector2 fingerUp = new Vector2();

    /// <summary>
    /// This method checks the directions along the x axis.
    /// </summary>
    /// <returns></returns>
    private void CheckSwipeDirectionAlongX()
    {
        float SWIPE_THRESHOLD = 10f;
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                Debug.Log("Touch began");
                fingerUp = touch.position;
                fingerDown = touch.position;
            }

            //if(touch.phase == TouchPhase.Moved)
            //{
            //    fingerDown = touch.position;
            //    Debug.Log("The horizontal destance: " + (fingerDown.x - fingerUp.x).ToString());
            //    if (Mathf.Abs(fingerDown.x - fingerUp.x) > SWIPE_THRESHOLD && Mathf.Abs(fingerDown.x - fingerUp.x) > Mathf.Abs(fingerDown.y - fingerUp.y))
            //    {
            //        Debug.Log("The horizontal destance: " + (fingerDown.x - fingerUp.x).ToString());
            //        //Debug.Log("Horizontal");
            //        if (fingerDown.x - fingerUp.x > 0)//Right swipe
            //        {
            //            Debug.Log("Swiped Right");
            //            OnSwipeRight();
            //        }
            //        else if (fingerDown.x - fingerUp.x < 0)//Left swipe
            //        {
            //            Debug.Log("Swiped Left");
            //            OnSwipeLeft();
            //        }
            //        fingerUp = fingerDown;

            //    }
            //}
            //else 
            //Detects swipe after finger is released
            if (touch.phase == TouchPhase.Ended)
            {
                fingerDown = touch.position;
                Debug.Log("The horizontal destance: " + (fingerDown.x - fingerUp.x).ToString());
                if (Mathf.Abs(fingerDown.x - fingerUp.x) > SWIPE_THRESHOLD && Mathf.Abs(fingerDown.x - fingerUp.x) > Mathf.Abs(fingerDown.y - fingerUp.y))
                {
                    Debug.Log("The horizontal destance: " + (fingerDown.x - fingerUp.x).ToString());
                    //Debug.Log("Horizontal");
                    if (fingerDown.x - fingerUp.x > 0)//Right swipe
                    {
                        Debug.Log("Swiped Right");
                        OnSwipeRight();
                    }
                    else if (fingerDown.x - fingerUp.x < 0)//Left swipe
                    {
                        Debug.Log("Swiped Left");
                        OnSwipeLeft();
                    }
                    fingerUp = fingerDown;

                }
            }

        }
    }

    private GameObject currentDayLayoutPage;

    private void OnSwipeRight()
    {
        currentDay = currentDay.AddDays(-1);

        GameObject newLayoutPage = Helper.Instantiate<GameObject>(dayLayoutPagePrefab, gameObject.transform, (obj) =>
        {
            obj.GetComponent<DayLayoutController>().currentPage = currentPage;
            obj.GetComponent<DayLayoutController>().currentDay = currentDay;
        });
        var pos = newLayoutPage.GetComponent<RectTransform>().anchoredPosition;
        newLayoutPage.GetComponent<RectTransform>().anchoredPosition = new Vector2(pos.x - 2000, pos.y);
        newLayoutPage.transform.SetSiblingIndex(0);
        currentDayLayoutPage.GetComponent<RectTransform>().DOAnchorPos3DX(2000, 0.1f);
        newLayoutPage.GetComponent<RectTransform>().DOAnchorPos3DX(pos.x, 0.1f);
        //Thread.Sleep(250);
        Destroy(currentDayLayoutPage);
        currentDayLayoutPage = newLayoutPage;
        newLayoutPage = null;
    }

    private void OnSwipeLeft()
    {
        currentDay = currentDay.AddDays(1);

        GameObject newLayoutPage = Helper.Instantiate<GameObject>(dayLayoutPagePrefab, gameObject.transform, (obj) =>
        {
            obj.GetComponent<DayLayoutController>().currentPage = currentPage;
            obj.GetComponent<DayLayoutController>().currentDay = currentDay;
        });
        var pos = newLayoutPage.GetComponent<RectTransform>().anchoredPosition;
        newLayoutPage.GetComponent<RectTransform>().anchoredPosition = new Vector2(pos.x + 2000, pos.y);
        newLayoutPage.transform.SetSiblingIndex(0);
        currentDayLayoutPage.GetComponent<RectTransform>().DOAnchorPos3DX(-2000, 0.1f);
        newLayoutPage.GetComponent<RectTransform>().DOAnchorPos3DX(pos.x, 0.1f);
        //Thread.Sleep(250);
        Destroy(currentDayLayoutPage);
        currentDayLayoutPage = newLayoutPage;
        newLayoutPage = null;

    }

    #endregion
    private void AddListenersToBtns()
    {
        if (addBtn != null)
        {
            addBtn.onClick.AddListener(OnAddBtnClicked);
        }
        if (shadowPanel != null)
        {
            Button shadowPanelBtn = shadowPanel.GetComponent<Button>();
            if (shadowPanelBtn != null)
            {
                shadowPanelBtn.onClick.AddListener(OnShadowPanelClicked);
            }
        }
    }

    public GameObject dayLayoutPagePrefab;

    private void OnStartUp()
    {
        currentDay = DateTime.Now.Date;
        currentPage = Pages.Events;
        HideAddOptionsBtns();
        if (addBtn != null)
        {
            addBtn.gameObject.SetActive(true);
        }

        currentDayLayoutPage = Helper.Instantiate<GameObject>(dayLayoutPagePrefab, gameObject.transform, (obj) =>
        {
            obj.GetComponent<DayLayoutController>().currentPage = currentPage;
            obj.GetComponent<DayLayoutController>().currentDay = currentDay;
        });
        currentDayLayoutPage.transform.SetSiblingIndex(0);
        EventSystem.instance.ChangePage(Pages.Events);
    }
    
    #region Add menu

    public Button addBtn;
    public GameObject shadowPanel;
    public GameObject addOptionsMenuPanelPrefab;

    public GameObject background;


    private void OnShadowPanelClicked()
    {
        HideAddOptionsBtns();
        ShowAddBtn();
    }

    private void OnAddBtnClicked()
    {
        ShowAddOptionsBtns();
        HideAddBtn();
    }

    private void ShowAddBtn()
    {
        if (addBtn != null)
        {
            addBtn.gameObject.SetActive(true);
        }

    }

    private void HideAddBtn()
    {
        if (addBtn != null)
        {
            addBtn.gameObject.SetActive(false);
        }
        
    }

    private GameObject addOptionsMenuPanelGameObject = null;

    private void ShowAddOptionsBtns()
    {
        if (shadowPanel != null)
        {
            shadowPanel.SetActive(true);
        }

        if (addOptionsMenuPanelPrefab != null)
        {
            addOptionsMenuPanelGameObject = Instantiate(addOptionsMenuPanelPrefab, background.transform);
            addOptionsMenuPanelGameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-438, -1495);
            addOptionsMenuPanelGameObject.SetActive(true);
            addOptionsMenuPanelGameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-438, 478), 0.25f);
        }
    }

    private void HideAddOptionsBtns()
    {
        if (shadowPanel != null)
        {
            shadowPanel.SetActive(false);
        }

        if (addOptionsMenuPanelGameObject != null)
        {
            addOptionsMenuPanelGameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-438, -1495), 0.25f);
            Destroy(addOptionsMenuPanelGameObject);
            addOptionsMenuPanelGameObject = null; //For good luck
        }

    }

    #endregion
}
