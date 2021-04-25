using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    public static EventSystem instance;

    private void Awake()
    {
        instance = this;
    }

    public event Action<string> onChangePage;
    public void ChangePage(string pageTitle)
    {
        if (onChangePage != null)
        {
            onChangePage(pageTitle);
        }
    }
}
