using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
        
    }


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

    private void OnStartUp()
    {
        HideAddOptionsBtns();
        if (addBtn != null)
        {
            addBtn.gameObject.SetActive(true);
        }

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
