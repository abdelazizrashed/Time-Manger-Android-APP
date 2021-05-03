using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotifAlarmInfoController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        cancelBtn.onClick.AddListener(OnCancelBtnClicked);
    }

    public Button cancelBtn;

    private void OnCancelBtnClicked()
    {
        Destroy(gameObject);
    }
}
