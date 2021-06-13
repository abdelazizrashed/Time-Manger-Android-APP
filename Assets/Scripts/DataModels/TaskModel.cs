using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
    public int isCompleted { get; set; }
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
        int _isCompleted = 0,
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
        TaskModel[] tasks = Enumerable.ToArray < TaskModel > (DBMan.Instance.ExecuteQueryAndReturnRows<TaskModel>(query, reader =>
        {
            return new TaskModel(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                DateTime.Parse(reader.GetString(3)),
                DateTime.Parse(reader.GetString(4)),
                DateTime.Parse(reader.GetString(5)),
                DateTime.Parse(reader.GetString(6)),
                reader.GetInt32(7),
                new RepeatModel(RepeatPeriod.Day, new WeekDays[] { }),
                new NotifiAlarmReminderModel[] { },
                new ColorModel(reader.GetInt32(11), "", ""),
                reader.GetInt32(12),
                !String.IsNullOrEmpty(reader.GetValue(13).ToString()) ? EventModel.GetEventByEventID(reader.GetInt32(13)) : null,
                !String.IsNullOrEmpty(reader.GetValue(14).ToString()) ? TaskModel.GetTaskByTaskID(reader.GetInt32(14)) : null,
                new TasksListModel(reader.GetInt32(10))
                );
        }));
        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i].taskList = TaskModel.SetTasksList(tasks[i].taskList);
            tasks[i].color = TaskModel.SetTaskColorModel(tasks[i].color);
        }
        return tasks;
    }

    public static TaskModel GetTaskByTaskID(int id)
    {
        string query = "SELECT * FROM TASKS WHERE task_id = " + id + ";";
        TaskModel[] tasks = Enumerable.ToArray<TaskModel>(DBMan.Instance.ExecuteQueryAndReturnRows<TaskModel>(query, reader =>
        {
            return new TaskModel(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                DateTime.Parse(reader.GetString(3)),
                DateTime.Parse(reader.GetString(4)),
                DateTime.Parse(reader.GetString(5)),
                DateTime.Parse(reader.GetString(6)),
                reader.GetInt32(7),
                new RepeatModel(RepeatPeriod.Day, new WeekDays[] { }),
                new NotifiAlarmReminderModel[] { },
                new ColorModel(reader.GetInt32(11), "", ""),
                reader.GetInt32(12),
                !String.IsNullOrEmpty(reader.GetValue(13).ToString()) ? EventModel.GetEventByEventID(reader.GetInt32(13)) : null,
                !String.IsNullOrEmpty(reader.GetValue(14).ToString()) ? TaskModel.GetTaskByTaskID(reader.GetInt32(14)) : null,
                new TasksListModel(reader.GetInt32(10))
                );
        }));
        TaskModel newTask = tasks[0];
        if(newTask != null)
        {
            newTask.taskList = TaskModel.SetTasksList(newTask.taskList);
            newTask.color = TaskModel.SetTaskColorModel(newTask.color);
            return newTask;
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
            "VALUES ( NULL, \"" +
            task.taskTitle + "\", \"" +
            task.taskDescription + "\", \"" +
            task.timeFrom.ToString() + "\", \"" +
            task.timeTo.ToString() + "\", \"" +
            task.timeStarted.ToString() + "\", \"" +
            task.timeFinished.ToString() + "\", " +
            (task.isCompleted).ToString() + ", \"" +
            JsonConvert.SerializeObject(task.repeat) + "\", \"" +
            JsonConvert.SerializeObject(task.reminders) + "\", " +
            task.taskList?.listID + ", " +
            task.color?.colorID + ", " +
            task.userID + ", ";
        if(task.parentEvent != null)
        {
            query += task.parentEvent?.eventID + ", ";
        }
        else
        {
            query += "NULL, ";
        }
        if(task.parentTask != null)
        {
            query += task.parentTask?.taskID + "); ";
        }
        else
        {
            query += "NULL);";
        }
        task.taskID = Convert.ToInt32(DBMan.Instance.ExecuteQueryAndReturnTheRowID(query));
    }

    public static void UpdateTask(ref TaskModel task)
    {
        string query =
            "UPDATE Tasks " +
            "SET " +
            "task_title = " + "\""+ task.taskTitle + "\", " +
            "task_description = " + "\"" + task.taskDescription + "\", " +
            "time_from = " + "\"" + task.timeFrom.ToString() + "\", " +
            "time_to = " + "\"" + task.timeTo.ToString() + "\", " +
            "time_started = " + "\"" + task.timeStarted.ToString() + "\", " +
            "time_finished = " + "\"" + task.timeFinished.ToString() + "\", " +
            "is_completed = " + (task.isCompleted).ToString() + ", " +
            "repeat = " + "\"" + JsonConvert.SerializeObject(task.repeat) + "\", " +
            "reminder = " + "\"" + JsonConvert.SerializeObject(task.reminders) + "\", " +
            "list_id = " + task.taskList?.listID + ", " +
            "color_id = " + task.color?.colorID + ", " +
            "user_id = " + task.userID + " ";
        if (task.parentEvent != null)
        {
            query += ", parent_event_id = " + task.parentEvent?.eventID + ", ";
        }
        if (task.parentTask != null)
        {
            query += ", parent_task_id = " + task.parentTask?.taskID + " ";
        }
        query += "WHERE task_id = " + task.taskID + ";";
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
            if (tasks[i].parentTask?.taskID == parentTask?.taskID)
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
        List<TaskModel> tasksList = new List<TaskModel>(tasks);
        tasksList.Sort((x, y) => DateTime.Compare(x.timeFrom, y.timeFrom));
        return tasksList.ToArray();
    }

    public static TaskModel[] GetListTasks(TasksListModel list)
    {
        string query = "SELECT * FROM Tasks WHERE list_id = " + list.listID + ";";
        TaskModel[] tasks = Enumerable.ToArray<TaskModel>(DBMan.Instance.ExecuteQueryAndReturnRows<TaskModel>(query, reader =>
        {
            return new TaskModel(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                DateTime.Parse(reader.GetString(3)),
                DateTime.Parse(reader.GetString(4)),
                DateTime.Parse(reader.GetString(5)),
                DateTime.Parse(reader.GetString(6)),
                reader.GetInt32(7),
                new RepeatModel(RepeatPeriod.Day, new WeekDays[] { }),
                new NotifiAlarmReminderModel[] { },
                new ColorModel(reader.GetInt32(11), "", ""),
                reader.GetInt32(12),
                !String.IsNullOrEmpty(reader.GetValue(13).ToString()) ? EventModel.GetEventByEventID(reader.GetInt32(13)) : null,
                !String.IsNullOrEmpty(reader.GetValue(14).ToString()) ? TaskModel.GetTaskByTaskID(reader.GetInt32(14)) : null,
                new TasksListModel(reader.GetInt32(10))
                );
        }));
        //IDataReader reader = DBMan.Instance.ExecuteQueryAndReturnDataReader(query);
        //List<TaskModel> tasks = new List<TaskModel>();
        //while (reader.Read())
        //{
        //    DBMan.Instance.PrintDataReader(reader);
        //    //for(int i = 0; i<15; i++)
        //    //{
        //    //    Debug.Log(reader.GetName(i) + ": " + reader.GetValue(i).ToString());
        //    //}
        //    tasks.Add(new TaskModel(
        //        reader.GetInt32(0),
        //        reader.GetString(1),
        //        reader.GetString(2),
        //        DateTime.Parse(reader.GetString(3)),
        //        DateTime.Parse(reader.GetString(4)),
        //        DateTime.Parse(reader.GetString(5)),
        //        DateTime.Parse(reader.GetString(6)),
        //        reader.GetInt32(7) == 1,
        //        new RepeatModel(RepeatPeriod.Day, new WeekDays[] { }),
        //        new NotifiAlarmReminderModel[] { },
        //        new ColorModel(reader.GetInt32(11), "", ""),
        //        reader.GetInt32(12),
        //        !String.IsNullOrEmpty(reader.GetValue(13).ToString()) ? EventModel.GetEventByEventID(reader.GetInt32(13)) : null,
        //        !String.IsNullOrEmpty(reader.GetValue(14).ToString()) ? TaskModel.GetTaskByTaskID(reader.GetInt32(14)) : null,
        //        new TasksListModel(reader.GetInt32(10))
        //        ));
        //}
        //reader?.Close();
        //reader = null;
        for (int i = 0; i<tasks.Length; i++)
        {
            tasks[i].taskList = TaskModel.SetTasksList(tasks[i].taskList);
            tasks[i].color = TaskModel.SetTaskColorModel(tasks[i].color);
        }
        return tasks;
    }

    public static  TasksListModel SetTasksList(TasksListModel list)
    {
        return TasksListModel.GetList(list.listID);
    }
    public static ColorModel SetTaskColorModel(ColorModel c)
    {
        return ColorModel.GetColorByColorID(c.colorID);
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
