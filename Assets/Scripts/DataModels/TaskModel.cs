using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class TaskModel: System.Object
{
    public int taskID { get; set; }
    public string taskTitle { get; set; }
    public string taskDescription { get; set; }
    public DateTime timeFrom { get; set; }
    public DateTime timeTo { get; set; }
    public DateTime timeStarted { get; set; }
    public DateTime timeFinished { get; set; }
    public bool isCompleted { get; set; }
    public RepeatModel repeat { get; set; }
    public NotifiAlarmReminderModel[] reminders { get; set; }
    public ColorModel color { get; set; }
    public int userID { get; set; }
    public EventModel parentEvent { get; set; }
    public TaskModel parentTask { get; set; }
    public TasksListModel taskList { get; set; }
    public TaskModel[] childrenTasks { get; set; }
    public bool isChildrenTasksOrdered { get; set; }

    public TaskModel(
        int _taskID = 0,
        string _taskTitle = "",
        string _taskDescription = "",
        DateTime? _timeFrom = null,
        DateTime? _timeTo = null,
        DateTime? _timeStarted = null,
        DateTime? _timeFinished = null,
        bool _isCompleted = false,
        RepeatModel _repeat = null,
        NotifiAlarmReminderModel[] _reminders = null,
        ColorModel _color = null,
        int _userID = 0,
        EventModel _parentEvent = null,
        TaskModel _parentTask = null,
        TasksListModel _taskList = null
        )
    {

        taskID = _taskID;
        taskTitle = _taskTitle;
        taskDescription = _taskDescription;
        timeFrom = _timeFrom ?? timeFrom;
        timeTo = _timeTo ?? timeTo;
        timeStarted = _timeStarted ?? timeStarted;
        timeFinished = _timeFinished ?? timeFinished;
        isCompleted = _isCompleted;
        repeat = _repeat;
        reminders = _reminders;
        color = _color;
        userID = _userID;
        parentEvent = _parentEvent;
        parentTask = _parentTask;
        taskList = _taskList;
        isChildrenTasksOrdered = false;

    }

    public static TaskModel[] GetTasks()
    {
        string query = "SELECT * FROM TASKS;";
        IDataReader reader = DBMan.Instance.ExecuteQueryAndReturnDataReader(query);
        List<TaskModel> tasks = new List<TaskModel>();
        while (reader.Read())
        {
            tasks.Add(new TaskModel(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                DateTime.Parse(reader.GetString(3)),
                DateTime.Parse(reader.GetString(4)),
                DateTime.Parse(reader.GetString(5)),
                DateTime.Parse(reader.GetString(6)),
                reader.GetInt32(7) == 1,
                (RepeatModel)JsonConvert.DeserializeObject(reader.GetString(8)),
                (NotifiAlarmReminderModel[])JsonConvert.DeserializeObject(reader.GetString(9)),
                ColorModel.GetColorByColorID(reader.GetInt32(11)),
                reader.GetInt32(12),
                EventModel.GetEventByEventID(reader.GetInt32(13)),
                TaskModel.GetTaskByTaskID(reader.GetInt32(14)),
                TasksListModel.GetList(reader.GetInt32(10))
                ));
        }
        return tasks.ToArray();
    }

    public static TaskModel GetTaskByTaskID(int id)
    {
        string query = "SELECT * FROM TASKS WHERE task_id = " + id + ";";
        IDataReader reader = DBMan.Instance.ExecuteQueryAndReturnDataReader(query);
        while (reader.Read())
        {

            return new TaskModel(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                DateTime.Parse(reader.GetString(3)),
                DateTime.Parse(reader.GetString(4)),
                DateTime.Parse(reader.GetString(5)),
                DateTime.Parse(reader.GetString(6)),
                reader.GetInt32(7) == 1,
                (RepeatModel)JsonConvert.DeserializeObject(reader.GetString(8)),
                (NotifiAlarmReminderModel[])JsonConvert.DeserializeObject(reader.GetString(9)),
                ColorModel.GetColorByColorID(reader.GetInt32(11)),
                reader.GetInt32(12),
                EventModel.GetEventByEventID(reader.GetInt32(13)),
                TaskModel.GetTaskByTaskID(reader.GetInt32(14)),
                TasksListModel.GetList(reader.GetInt32(10))
                );
        }
        return null;
    }

    public static string[] GetTasksTitles(TaskModel[] tasks)
    {
        List<string> tasksTitles = new List<string>();
        foreach (TaskModel t in tasks)
        {
            tasksTitles.Add(t.taskTitle);
        }
        return tasksTitles.ToArray();
    }


    public static void SaveTask(ref TaskModel task)
    {
        string query =
            "INSERT INTO Tasks " +
            "VALUES ( NULL, " +
            task.taskTitle + ", " +
            task.taskDescription + ", " +
            task.timeFrom.ToString() + ", " +
            task.timeTo.ToString() + ", " +
            task.timeStarted.ToString() + ", " +
            task.timeFinished.ToString() + ", " +
            (task.isCompleted ? 1 : 0).ToString() + ", " +
            JsonConvert.SerializeObject(task.repeat) + ", " +
            JsonConvert.SerializeObject(task.reminders) + ", " +
            task.taskList.listID + ", " +
            task.color.colorID + ", " +
            task.userID + ", " +
            task.parentEvent.eventID + ", " +
            task.parentTask.taskID + "); ";
        task.taskID = Convert.ToInt32(DBMan.Instance.ExecuteQueryAndReturnTheRowID(query));
    }

    public static TaskModel[] GetTaskChildrenOrderedByStartTime(ref TaskModel parentTask)
    {
        if (parentTask.isChildrenTasksOrdered)
        {
            return parentTask.childrenTasks;
        }
        TaskModel[] tasks = TaskModel.GetTasks();
        List<TaskModel> children = new List<TaskModel>();
        for(int i = 0; i< tasks.Length; i++)
        {
            if (tasks[i].parentTask.taskID == parentTask.taskID)
            {
                tasks[i].childrenTasks = TaskModel.GetTaskChildrenOrderedByStartTime(ref tasks[i]);
                children.Add(tasks[i]);
            }
        }
        List<TaskModel> orderedChildren = new List<TaskModel>();
        foreach (TaskModel child in children)
        {
            TaskModel earlestChild = child;
            foreach(TaskModel child2 in children)
            {
                if(DateTime.Compare(earlestChild.timeFrom, child2.timeFrom) > 0)
                {
                    earlestChild = child2;
                }else if(DateTime.Compare(earlestChild.timeFrom, child2.timeFrom) == 0)
                {
                    if(DateTime.Compare(earlestChild.timeTo, child2.timeTo) > 0)
                    {
                        earlestChild = child2;
                    }
                }
            }
            orderedChildren.Add(earlestChild);
            children.Remove(earlestChild);
        }
        parentTask.isChildrenTasksOrdered = true;
        return orderedChildren.ToArray();
    }

    public static TaskModel[] OrderTasks(ref TaskModel[] tasks)
    {
        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i].childrenTasks = TaskModel.GetTaskChildrenOrderedByStartTime(ref tasks[i]);
        }
        List<TaskModel> orderedTasks = new List<TaskModel>();
        foreach (TaskModel task in tasks)
        {
            TaskModel earlestTask = task;
            foreach (TaskModel task2 in tasks)
            {
                if (DateTime.Compare(earlestTask.timeFrom, task2.timeFrom) > 0)
                {
                    earlestTask = task2;
                }
                else if (DateTime.Compare(earlestTask.timeFrom, task2.timeFrom) == 0)
                {
                    if (DateTime.Compare(earlestTask.timeTo, task2.timeTo) > 0)
                    {
                        earlestTask = task2;
                    }
                }
            }
            orderedTasks.Add(earlestTask);
        }
        return orderedTasks.ToArray();
    }

    public static TaskModel[] GetListTasks(TasksListModel list)
    {
        string query = "SELECT * FROM TASKS WHERE list_id = " + list.listID + ";";
        IDataReader reader = DBMan.Instance.ExecuteQueryAndReturnDataReader(query);
        List<TaskModel> tasks = new List<TaskModel>();
        while (reader.Read())
        {
            tasks.Add(new TaskModel(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                DateTime.Parse(reader.GetString(3)),
                DateTime.Parse(reader.GetString(4)),
                DateTime.Parse(reader.GetString(5)),
                DateTime.Parse(reader.GetString(6)),
                reader.GetInt32(7) == 1,
                (RepeatModel)JsonConvert.DeserializeObject(reader.GetString(8)),
                (NotifiAlarmReminderModel[])JsonConvert.DeserializeObject(reader.GetString(9)),
                ColorModel.GetColorByColorID(reader.GetInt32(11)),
                reader.GetInt32(12),
                EventModel.GetEventByEventID(reader.GetInt32(13)),
                TaskModel.GetTaskByTaskID(reader.GetInt32(14)),
                TasksListModel.GetList(reader.GetInt32(10))
                ));
        }
        return tasks.ToArray();
    }

    public static void StartTask(TaskModel task, DateTime time)
    {
        string query = 
            "UPDATE Tasks " +
            "SET time_started = " + time.ToString() + 
            "WHERE task_id = " + task.taskID + "; ";
        DBMan.Instance.ExecuteQueryAndReturnRowsAffected(query);
    }

    public static void FinishTask(TaskModel task, DateTime time)
    {
        string query =
            "UPDATE Tasks " +
            "SET time_finished = " + time.ToString() +
            "is_completed = " + 1 + " " +
            "WHERE task_id = " + task.taskID + "; ";
        DBMan.Instance.ExecuteQueryAndReturnRowsAffected(query);
    }
}
