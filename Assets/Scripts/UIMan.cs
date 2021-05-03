using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DeadMosquito.AndroidGoodies;

public class UIMan : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.fullScreen = false;
        #if ANDROID
            AGUIMisc.ShowStatusBar();
        #endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
