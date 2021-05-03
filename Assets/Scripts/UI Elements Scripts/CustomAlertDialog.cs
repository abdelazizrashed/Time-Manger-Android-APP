using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomAlertDialog : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void ShowColorPickerDialog(ColorModel[] colors, GameObject colorBtnPrefab, Action<int> onClickCallback)
    {
        GameObject background = GameObject.FindGameObjectWithTag("MainBackground");
        if (background != null)
        {
            GameObject alertDialogPrefab = Resources.Load<GameObject>("Prefabs/CustomAlertDialog");
            if (alertDialogPrefab != null)
            {
                GameObject alertDialogGameObject = Instantiate(alertDialogPrefab, background.transform);
                GameObject alertDialogContent = alertDialogGameObject.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
                for (int index = 0; index < colors.Length; index++)
                {
                    GameObject colorBtn = Instantiate(colorBtnPrefab, alertDialogContent.transform);
                    colorBtn.GetComponentInChildren<TMP_Text>().text = colors[index].colorName;
                    colorBtn.GetComponentInChildren<Image>().color = new Color32(
                        Helper.StringToByteArray(colors[index].colorValue)[0],
                        Helper.StringToByteArray(colors[index].colorValue)[1],
                        Helper.StringToByteArray(colors[index].colorValue)[2],
                        0xFF
                        );

                    colorBtn.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        int i = FindIndexByColorName(colorBtn.GetComponentInChildren<TMP_Text>().text, colors);
                        onClickCallback(i);
                        Destroy(alertDialogGameObject);
                    });
                }
            }
        }
    }

    public static int FindIndexByColorName(string name, ColorModel[] colors)
    {
        for (int i = 0; i < colors.Length; i++)
        {
            if(colors[i].colorName == name)
            {
                return i;
            }
        }
        return -1;
    }
}
