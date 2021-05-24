using System;
using System.Collections.Generic;
using System.Data;

public class TasksListModel
{
    public int listID { get; set; }

    public string listTitle { get; set; }

    public TasksListModel(int id = 0, string title = "")
    {
        listID = id;
        listTitle = title;
    }

    public static TasksListModel GetList(int id)
    {
        string query = "SELECT * FROM TasksLists WHERE list_id = " + id + ";";
        IDataReader reader = DBMan.Instance.ExecuteQueryAndReturnDataReader(query);
        while (reader.Read())
        {
            return new TasksListModel(reader.GetInt32(0), reader.GetString(1));
        }
        return null;
    }

    public static TasksListModel[] GetLists()
    {
        string query = "SELECT * FROM TasksLists;";
        List<TasksListModel> lists = new List<TasksListModel>();
        IDataReader reader = DBMan.Instance.ExecuteQueryAndReturnDataReader(query);
        while (reader.Read())
        {
            lists.Add(new TasksListModel(reader.GetInt32(0), reader.GetString(1)));
        }
        reader?.Close();
        reader = null;
        return lists.ToArray();
    }

    public static void SaveList(ref TasksListModel newList)
    {
        string query = "INSERT INTO TasksLists(list_title) VALUES (" + newList.listTitle + ");";
        newList.listID = Convert.ToInt32(DBMan.Instance.ExecuteQueryAndReturnTheRowID(query));
    }
}
