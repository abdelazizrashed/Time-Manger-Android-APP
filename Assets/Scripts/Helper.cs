using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Helper : MonoBehaviour
{
    //static public Helper instance;
    //private void Awake()
    //{
    //    instance = this;
    //}

    #region Instantiate function overloads

    static public T Instantiate<T>(T unityObject, System.Action<T> beforeAwake = null) where T : UnityEngine.Object
    {
        //Find prefab gameObject
        var gameObject = unityObject as GameObject;
        var component = unityObject as Component;

        if (gameObject == null && component != null)
            gameObject = component.gameObject;

        //Save current prefab active state
        var isActive = false;
        if (gameObject != null)
        {
            isActive = gameObject.activeSelf;
            //Deactivate
            gameObject.SetActive(false);
        }

        //Instantiate
        var obj = UnityEngine.Object.Instantiate(unityObject) as T;
        if (obj == null)
            throw new Exception("Failed to instantiate Object " + unityObject);

        //This funciton will be executed before awake of any script inside
        if (beforeAwake != null)
            beforeAwake(obj);

        //Revert prefab active state
        if (gameObject != null)
            gameObject.SetActive(isActive);

        //Find instantiated GameObject
        gameObject = obj as GameObject;
        component = obj as Component;

        if (gameObject == null && component != null)
            gameObject = component.gameObject;

        //Set active state to prefab state
        if (gameObject != null)
            gameObject.SetActive(isActive);

        return obj;
    }

    static public T Instantiate<T>(T unityObject, Transform parent, System.Action<T> beforeAwake = null) where T : UnityEngine.Object
    {
        //Find prefab gameObject
        var gameObject = unityObject as GameObject;
        var component = unityObject as Component;

        if (gameObject == null && component != null)
            gameObject = component.gameObject;

        //Save current prefab active state
        var isActive = false;
        if (gameObject != null)
        {
            isActive = gameObject.activeSelf;
            //Deactivate
            gameObject.SetActive(false);
        }

        //Instantiate
        var obj = UnityEngine.Object.Instantiate(unityObject, parent) as T;
        if (obj == null)
            throw new Exception("Failed to instantiate Object " + unityObject);

        //This funciton will be executed before awake of any script inside
        if (beforeAwake != null)
            beforeAwake(obj);

        //Revert prefab active state
        if (gameObject != null)
            gameObject.SetActive(isActive);

        //Find instantiated GameObject
        gameObject = obj as GameObject;
        component = obj as Component;

        if (gameObject == null && component != null)
            gameObject = component.gameObject;

        //Set active state to prefab state
        if (gameObject != null)
            gameObject.SetActive(isActive);

        return obj;
    }
    static public T Instantiate<T>(T unityObject, Transform parent, bool worldPositionStays, System.Action<T> beforeAwake = null) where T : UnityEngine.Object
    {
        //Find prefab gameObject
        var gameObject = unityObject as GameObject;
        var component = unityObject as Component;

        if (gameObject == null && component != null)
            gameObject = component.gameObject;

        //Save current prefab active state
        var isActive = false;
        if (gameObject != null)
        {
            isActive = gameObject.activeSelf;
            //Deactivate
            gameObject.SetActive(false);
        }

        //Instantiate
        var obj = UnityEngine.Object.Instantiate(unityObject, parent, worldPositionStays) as T;
        if (obj == null)
            throw new Exception("Failed to instantiate Object " + unityObject);

        //This funciton will be executed before awake of any script inside
        if (beforeAwake != null)
            beforeAwake(obj);

        //Revert prefab active state
        if (gameObject != null)
            gameObject.SetActive(isActive);

        //Find instantiated GameObject
        gameObject = obj as GameObject;
        component = obj as Component;

        if (gameObject == null && component != null)
            gameObject = component.gameObject;

        //Set active state to prefab state
        if (gameObject != null)
            gameObject.SetActive(isActive);

        return obj;
    }
    static public T Instantiate<T>(T unityObject, Vector3 position, Quaternion rotation, System.Action<T> beforeAwake = null) where T : UnityEngine.Object
    {
        //Find prefab gameObject
        var gameObject = unityObject as GameObject;
        var component = unityObject as Component;

        if (gameObject == null && component != null)
            gameObject = component.gameObject;

        //Save current prefab active state
        var isActive = false;
        if (gameObject != null)
        {
            isActive = gameObject.activeSelf;
            //Deactivate
            gameObject.SetActive(false);
        }

        //Instantiate
        var obj = UnityEngine.Object.Instantiate(unityObject, position, rotation) as T;
        if (obj == null)
            throw new Exception("Failed to instantiate Object " + unityObject);

        //This funciton will be executed before awake of any script inside
        if (beforeAwake != null)
            beforeAwake(obj);

        //Revert prefab active state
        if (gameObject != null)
            gameObject.SetActive(isActive);

        //Find instantiated GameObject
        gameObject = obj as GameObject;
        component = obj as Component;

        if (gameObject == null && component != null)
            gameObject = component.gameObject;

        //Set active state to prefab state
        if (gameObject != null)
            gameObject.SetActive(isActive);

        return obj;
    }
    static public T Instantiate<T>(T unityObject, Vector3 position, Quaternion rotation, Transform parent, System.Action<T> beforeAwake = null) where T : UnityEngine.Object
    {
        //Find prefab gameObject
        var gameObject = unityObject as GameObject;
        var component = unityObject as Component;

        if (gameObject == null && component != null)
            gameObject = component.gameObject;

        //Save current prefab active state
        var isActive = false;
        if (gameObject != null)
        {
            isActive = gameObject.activeSelf;
            //Deactivate
            gameObject.SetActive(false);
        }

        //Instantiate
        var obj = UnityEngine.Object.Instantiate(unityObject, position, rotation, parent) as T;
        if (obj == null)
            throw new Exception("Failed to instantiate Object " + unityObject);

        //This funciton will be executed before awake of any script inside
        if (beforeAwake != null)
            beforeAwake(obj);

        //Revert prefab active state
        if (gameObject != null)
            gameObject.SetActive(isActive);

        //Find instantiated GameObject
        gameObject = obj as GameObject;
        component = obj as Component;

        if (gameObject == null && component != null)
            gameObject = component.gameObject;

        //Set active state to prefab state
        if (gameObject != null)
            gameObject.SetActive(isActive);

        return obj;
    }
    #endregion

    public static byte[] StringToByteArray(string hex)
    {
        return Enumerable.Range(0, hex.Length)
                         .Where(x => x % 2 == 0)
                         .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                         .ToArray();
    }

    public static Color32 StringToColor(string hex)
    {
        return new Color32(
                        Helper.StringToByteArray(hex)[0],
                        Helper.StringToByteArray(hex)[1],
                        Helper.StringToByteArray(hex)[2],
                        0xFF
                        );
    }

    /// <summary>
    /// This method takes two object of type TaskModel, EventModel, or Reminder Model and sort them
    /// </summary>
    /// <returns></returns>
    public static System.Object[] SortTwoByDateTime<T, R>(T[] object1, R[] object2) 
    {
        int m = object1.Length;
        int n = object2.Length;

        List<object> sortedList = new List<object>();


    }
}
