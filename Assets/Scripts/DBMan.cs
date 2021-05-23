using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;

public class DBMan
{
    private static DBMan instance = null;
    private static readonly object padlock = new object();


    private string connectionStr;
    private IDbConnection dbConnection;
    private IDbCommand dbCommand;
    private string dbFileName = "/MainDB.db";
    
    public bool doesPrintQuery { get; set; }
    public bool doesPrintRowsAffected { get; set; }

    DBMan()
    {
        StartDB();
        doesPrintQuery = false;
        doesPrintRowsAffected = false;
    }

    ~DBMan()
    {
        CloseDB();
    }

    public static DBMan Instance
    {
        get
        {
            lock (padlock)
            {
                if(instance == null)
                {
                    instance = new DBMan();
                }
                return instance;
            }
        }
    }

    public void StartDB()
    {
        connectionStr = "URI=file:" + Application.dataPath + dbFileName;
        dbConnection = (IDbConnection)new SqliteConnection(connectionStr);
        dbConnection.Open(); //Open connection to the database.
        dbCommand = dbConnection.CreateCommand();
        CreateTables();

    }

    public void CreateTables()
    {
        //string query = @"
        //            CREATE TABLE IF NOT EXISTS Users(
        //                user_id INTEGER PRIMARY KEY AUTOINCREMENT,
        //                username TEXT UNIQUE NOT NULL,
        //                is_admin INTEGER DEFAULT 0,
        //                password TEXT NOT NULL,
        //                email TEXT UNIQUE NOT NULL,
        //                first_name TEXT NOT NULL,
        //                last_name TEXT NOT NULL
        //            );
        //            ";
        //dbCommand.CommandText = query;
        //PrintQuery(query);
        //PrintRowAffected(dbCommand.ExecuteNonQuery());

        string query = @"
                    CREATE TABLE IF NOT EXISTS Colors(
                        color_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        color_name TEXT NOT NULL,
                        color_value TEXT NOT NULL
                    );
                ";
        dbCommand.CommandText = query;
        PrintQuery(query);
        PrintRowAffected(dbCommand.ExecuteNonQuery());

        query = @"
                    CREATE TABLE IF NOT EXISTS TasksLists(
                        list_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        list_title TEXT NOT NULL,
                        user_id INTEGER NOT NULL
                    );
                ";
        dbCommand.CommandText = query;
        PrintQuery(query);
        PrintRowAffected(dbCommand.ExecuteNonQuery());

        query = @"
                    CREATE TABLE IF NOT EXISTS Events(
                        event_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        event_title TEXT NOT NULL,
                        event_description TEXT DEFAULT NULL,
                        user_id INTEGER NOT NULL,
                        color_id INTEGER NOT NULL,
                        parent_event_id INTEGER DEFAULT NULL,
                        FOREIGN KEY(color_id) REFERENCES Colors(color_id),
                        FOREIGN KEY(parent_event_id) REFERENCES Events(event_id)
                    );
                ";
        dbCommand.CommandText = query;
        PrintQuery(query);
        PrintRowAffected(dbCommand.ExecuteNonQuery());

        query = @"
                    CREATE TABLE IF NOT EXISTS EventsTimeSlots(
                        time_slot_id INT PRIMARY KEY AUTOINCREMENT,
                        time_from TEXT NOT NULL,
                        time_to TEXT NOT NULL,
                        time_started TEXT DEFAULT NULL,
                        time_finished TEXT DEFAULT NULL,
                        is_completed INTE DEFAULT NULL,
                        location TEXT DEFAULT NULL,
                        repeat TEXT DEFAULT NULL,
                        reminder TEXT DEFAULT NULL,
                        is_completed INT DEFAULT 0,
                        event_id INTEGER DEFAULT NULL,
                        FOREIGN KEY (event_id) REFERENCES Events(event_id)
                    );
                ";
        dbCommand.CommandText = query;
        PrintQuery(query);
        PrintRowAffected(dbCommand.ExecuteNonQuery());

        query = @"
                    CREATE TABLE IF NOT EXISTS Tasks(
                        task_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        task_title TEXT NOT NULL,
                        task_description TEXT,
                        time_from TEXT NOT NULL,
                        time_to TEXT NOT NULL,
                        time_started TEXT,
                        time_finished TEXT,
                        is_completed INTEGER DEFAULT 0,
                        repeat TEXT DEFAULT NULL,
                        reminder TEXT DEFAULT NULL,
                        list_id INTEGER NOT NULL,
                        color_id INTEGER NOT NULL,
                        user_id INTEGER NOT NULL,
                        parent_event_id INTEGER DEFAULT NULL,
                        parent_task_id INTEGER DEFAULT NULL,
                        FOREIGN KEY(list_id) REFERENCES TasksList(list_id),
                        FOREIGN KEY(color_id) REFERENCES Colors(color_id),
                        FOREIGN KEY(parent_event_id) REFERENCES Events(event_id),
                        FOREIGN KEY(parent_task_id) REFERENCES Tasks(task_id)
                    );
                ";
        dbCommand.CommandText = query;
        PrintQuery(query);
        PrintRowAffected(dbCommand.ExecuteNonQuery());

        query = @"
                    CREATE TABLE IF NOT EXISTS Reminders(
                        reminder_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        reminder_title TEXT NOT NULL,
                        reminder_description TEXT DEFAULT NULL,
                        user_id INTEGER NOT NULL,
                        color_id INTEGER NOT NULL,
                        parent_event_id INTEGER DEFAULT NULL,
                        FOREIGN KEY(color_id) REFERENCES Colors(color_id),
                        FOREIGN KEY(parent_event_id) REFERENCES Event(event_id)
                    );
                ";
        dbCommand.CommandText = query;
        PrintQuery(query);
        PrintRowAffected(dbCommand.ExecuteNonQuery());

        query = @"
                    CREATE TABLE IF NOT EXISTS RemindersTimeSlots(
                        time_slot_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        time TEXT NOT NULL,
                        time_done TEXT DEFAULT NULL,
                        is_completed INT DEFAULT 0,
                        repeat TEXT DEFAULT NULL,
                        reminder TEXT DEFAULT NULL,
                        reminder_id INTEGER NOT NULL,
                        FOREIGN KEY (reminder_id) REFERENCES Reminders(reminder_id)
                    );
                ";
        dbCommand.CommandText = query;
        PrintQuery(query);
        PrintRowAffected(dbCommand.ExecuteNonQuery());

        query = @"
                    CREATE TABLE IF NOT EXISTS Reports(
                        report_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        time_started TEXT DEFAULT NULL,
                        time_finished TEXT DEFAULT NULL,
                        event_id INTEGER DEFAULT NULL,
                        task_id INTEGER DEFAULT NULL,
                        reminder_id INTEGER DEFAULT NULL,
                        user_id INTEGER NOT NULL,
                        FOREIGN KEY (event_id) REFERENCES Events(event_id),
                        FOREIGN KEY (task_id) REFERENCES Tasks(task_id),
                        FOREIGN KEY (reminder_id) REFERENCES Reminders(reminder_id),
                        FOREIGN KEY (user_id) REFERENCES Users(user_id)
                    );
                ";
        dbCommand.CommandText = query;
        PrintQuery(query);
        PrintRowAffected(dbCommand.ExecuteNonQuery());
    }

    public void DropTables()
    {

        string query = @"
                        select 'drop table ' || name || ';' from sqlite_master
                        where type = 'table';
                        ";
        dbCommand.CommandText = query;
        PrintQuery(query);
        PrintRowAffected(dbCommand.ExecuteNonQuery());
    }

    public int ExecuteQueryAndReturnRowsAffected(string query)
    {
        dbCommand.CommandText = query;
        PrintQuery(query);
        int rowsAffected = dbCommand.ExecuteNonQuery();
        PrintRowAffected(rowsAffected);
        return rowsAffected;
    }

    public long ExecuteQueryAndReturnTheRowID(string query)
    {
        dbCommand.CommandText = query;
        PrintQuery(query);
        return (long)dbCommand.ExecuteScalar();

    }

    public IDataReader ExecuteQueryAndReturnDataReader(string query)
    {
        dbCommand.CommandText = query;
        PrintQuery(query);
        return dbCommand.ExecuteReader();
    }

    void CloseDB()
    {
        dbCommand?.Dispose();
        dbCommand = null;
        dbConnection?.Close();
        dbConnection = null;
    }
    void PrintQuery(string query)
    {
        if (doesPrintQuery)
        {
            Debug.Log(query);
        }
    }

    void PrintRowAffected(int noOFRow)
    {
        if (doesPrintRowsAffected)
        {
            Debug.Log("Rows Affected: " + noOFRow.ToString());
        }
    }
}
