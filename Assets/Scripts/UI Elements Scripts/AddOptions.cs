using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class AddOptions : MonoBehaviour
{
    private void Awake()
    {
        addOptionsShadowPanel = GameObject.Find("AddOptionsShadowPanel");
        background = GameObject.Find("Background");
    }

    private void Start()
    {
        AddListenersToBtns();
    }

    private void Update()
    {
    }

    private void AddListenersToBtns()
    {
        if (addTaskBtn != null)
        {
            addTaskBtn.onClick.AddListener(OnAddTaskBtnClicked);
        }

        if (addReminderBtn != null)
        {
            addReminderBtn.onClick.AddListener(OnAddReminderBtnClicked);
        }

        if (addEventBtn != null)
        {
            addEventBtn.onClick.AddListener(OnAddEventBtnClicked);
        }
    }

    private GameObject background;

    public Button addTaskBtn;

    public GameObject addTaskPanelPrefab;

    public Button addReminderBtn;

    public GameObject addReminderPanelPrefab;

    public Button addEventBtn;

    public GameObject addEventPanelPrefab;

    private GameObject addOptionsShadowPanel;

    private void OnAddTaskBtnClicked()
    {
        ShowAddTaskPanel();
        HideAddOptionsBtns();
    }

    private void OnAddReminderBtnClicked()
    {
        ShowAddReminderPanel();
        HideAddOptionsBtns();
    }

    private void OnAddEventBtnClicked()
    {
        ShowAddEventPanel();
        HideAddOptionsBtns();
    }

    private void ShowAddTaskPanel()
    {
        if (addTaskPanelPrefab != null && background != null)
        {
            GameObject addTaskPanelGameObject = Instantiate(addTaskPanelPrefab, new Vector3(0f, -2000f, 0), background.transform.rotation, background.transform);
            addTaskPanelGameObject.SetActive(true);
            addTaskPanelGameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.25f);
        }
    }

    private void ShowAddReminderPanel()
    {
        if (addReminderPanelPrefab != null)
        {
            GameObject addReminderPanelGameObject = Instantiate(addReminderPanelPrefab, new Vector3(0, -2000f, 0), background.transform.rotation, background.transform);
            addReminderPanelGameObject.SetActive(true);
            addReminderPanelGameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.25f);
        }
    }

    private void HideAddReminderPanel()
    {
        if (gameObject != null)
        {
            gameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, -2000f), 0.25f);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void ShowAddEventPanel()
    {
        if (addEventPanelPrefab != null)
        {
            GameObject addEventPanelGameObject = Instantiate(addEventPanelPrefab, new Vector3(0, -2000f, 0), background.transform.rotation, background.transform);
            addEventPanelGameObject.SetActive(true);
            addEventPanelGameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.25f);
        }
    }

    private void HideAddOptionsBtns()
    {
        if (addOptionsShadowPanel != null)
        {
            addOptionsShadowPanel.SetActive(false);
        }

        if (gameObject != null)
        {
            gameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-438, -1495), 0.25f);
            Destroy(gameObject);
        }
    }
}
