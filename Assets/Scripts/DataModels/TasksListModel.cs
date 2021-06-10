using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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
        TasksListModel[] lists = Enumerable.ToArray<TasksListModel>(DBMan.Instance.ExecuteQueryAndReturnRows<TasksListModel>(query, reader =>
        {
            return new TasksListModel(reader.GetInt32(0), reader.GetString(1));
        }));
        return lists[0];
        //IDataReader reader = DBMan.Instance.ExecuteQueryAndReturnDataReader(query);
        //while (reader.Read())
        //{
        //    DBMan.Instance.PrintDataReader(reader);
        //    TasksListModel newList =  new TasksListModel(reader.GetInt32(0), reader.GetString(1));
        //    reader?.Close();
        //    reader = null;
        //    return newList;
        //}
        //reader?.Close();
        //reader = null;
        //return null;
    }

    public static TasksListModel[] GetLists()
    {
        string query = "SELECT * FROM TasksLists;";
        TasksListModel[] lists = Enumerable.ToArray<TasksListModel>(DBMan.Instance.ExecuteQueryAndReturnRows<TasksListModel>(query, reader =>
        {
            return new TasksListModel(reader.GetInt32(0), reader.GetString(1));
        })); 
        return lists;
        //while (reader.Read())
        //{
        //    DBMan.Instance.PrintDataReader(reader);
        //    lists.Add(new TasksListModel(reader.GetInt32(0), reader.GetString(1)));
        //}
        //reader?.Close();
        //reader = null;
        //return lists.ToArray();
    }

    public static void SaveList(ref TasksListModel newList)
    {
        string query = "INSERT INTO TasksLists VALUES (NULL, \"" + newList.listTitle + "\", " + UserModel.Instance.userID + ");";
        newList.listID = Convert.ToInt32(DBMan.Instance.ExecuteQueryAndReturnTheRowID(query));
    }
}
