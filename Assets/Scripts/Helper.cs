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
        if(hex != null)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        return null;
    }

    public static Color32 StringToColor(string hex)
    {
        if (!String.IsNullOrEmpty(hex))
        {
            return new Color32(
                            Helper.StringToByteArray(hex)[0],
                            Helper.StringToByteArray(hex)[1],
                            Helper.StringToByteArray(hex)[2],
                            0xFF
                            );
        }
        return Color.blue;
    }

    /// <summary>
    /// This method takes two object of type TaskModel, EventModel, or Reminder Model and sort them
    /// </summary>
    /// <returns></returns>
    public static System.Object[] MergeSort3ArraysByTime(TaskModel[] tasks, EventTimeSlotModel[] eventTimeSlots, ReminderTimeSlotModel[] reminderTimeSlots) 
    {
        List<object> sortedList = new List<object>();
        int i = 0;
        int j = 0; 
        int k = 0;
        while(i< tasks.Length && j< eventTimeSlots.Length && k < reminderTimeSlots.Length)
        {
            DateTime tTimeFrom = tasks[i].timeFrom;
            DateTime eTimeFrom = eventTimeSlots[j].timeFrom;
            DateTime rTime = reminderTimeSlots[k].time;
            if(DateTime.Compare(tTimeFrom, eTimeFrom) <= 0 && DateTime.Compare(tTimeFrom, rTime) <= 0)
            {
                sortedList.Add(tasks[i]);
                i++;
            }
            else if(DateTime.Compare(eTimeFrom, tTimeFrom) <= 0 && DateTime.Compare(eTimeFrom, rTime) <= 0)
            {
                sortedList.Add(eventTimeSlots[j]);
                j++;
            }
            else
            {
                sortedList.Add(reminderTimeSlots[k]);
                k++;
            }
        }

        while(i <tasks.Length && j < eventTimeSlots.Length)
        {
            if(DateTime.Compare(tasks[i].timeFrom, eventTimeSlots[j].timeFrom) < 0)
            {
                sortedList.Add(tasks[i]);
                i++;
            }
            else
            {
                sortedList.Add(eventTimeSlots[j]);
                j++;
            }
        }

        while (k < reminderTimeSlots.Length && j < eventTimeSlots.Length)
        {
            if (DateTime.Compare(reminderTimeSlots[k].time, eventTimeSlots[j].timeFrom) < 0)
            {
                sortedList.Add(reminderTimeSlots[k]);
                k++;
            }
            else
            {
                sortedList.Add(eventTimeSlots[j]);
                j++;
            }
        }

        while (i < tasks.Length && k < reminderTimeSlots.Length)
        {
            if (DateTime.Compare(tasks[i].timeFrom, reminderTimeSlots[k].time) < 0)
            {
                sortedList.Add(tasks[i]);
                i++;
            }
            else
            {
                sortedList.Add(reminderTimeSlots[k]);
                k++;
            }
        }

        while (i < tasks.Length)
        {
            sortedList.Add(tasks[i]);
            i++;
        }

        while (j < eventTimeSlots.Length)
        {
            sortedList.Add(eventTimeSlots[j]);
            j++;
        }

        while (k < reminderTimeSlots.Length)
        {
            sortedList.Add(reminderTimeSlots[k]);
            k++;
        }

        return sortedList.ToArray();
    }
}
