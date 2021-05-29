using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppMan : MonoBehaviour
{
    public bool doesPrintQuery = false;
    public bool doesPrintRowsAffected = false;
    public bool doesPrintDataReader = false;
    // Start is called before the first frame update
    void Start()
    {
        DBMan.Instance.doesPrintQuery = doesPrintQuery;
        DBMan.Instance.doesPrintRowsAffected = doesPrintRowsAffected;
        DBMan.Instance.doesPrintDataReader = doesPrintDataReader;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
