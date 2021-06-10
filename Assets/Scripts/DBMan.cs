using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;
using System;

public class DBMan
{
    private static DBMan instance = null;
    private static readonly object padlock = new object();


    private string connectionStr = "";
    //private IDbConnection dbConnection;
    //private IDbCommand dbCommand;
    private string dbFileName = "/database.db";
    
    public bool doesPrintQuery { get; set; }
    public bool doesPrintRowsAffected { get; set; }
    public bool doesPrintDataReader { get; set; }

    DBMan()
    {
        StartDB();
        //doesPrintQuery = false;
        //doesPrintRowsAffected = false;
        //doesPrintDataReader = false;
    }

    //~DBMan()
    //{
    //    CloseDB();
    //}

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
        SetConnectionStr();

        //dbConnection = new SqliteConnection(connectionStr);

        //dbConnection.Open();

        //dbCommand = dbConnection.CreateCommand();
        CreateTables();

    }

    public void SetConnectionStr()
    {
        string filepath = Application.persistentDataPath + "/" + dbFileName;

        if (!File.Exists(filepath))

        {

            Debug.LogWarning("File \"" + filepath + "\" does not exist. Attempting to create from \"" +

            Application.dataPath + "!/assets/" + dbFileName);

            // if it doesn't ->

            // open StreamingAssets directory and load the db ->

            WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + "data.sqlite");

            while (!loadDB.isDone) { }

            // then save to Application.persistentDataPath

            File.WriteAllBytes(filepath, loadDB.bytes);

        }

        //open db connection

        connectionStr = "URI=file:" + filepath;
    }

    public IDbConnection CreateDBConnection()
    {
        if (string.IsNullOrEmpty(connectionStr))
        {
            SetConnectionStr();
        }
        Debug.Log("Stablishing connection to: " + connectionStr);

        IDbConnection  dbConnection = new SqliteConnection(connectionStr);

        dbConnection.Open();

        return dbConnection;
    }

    public void DisposeConnectionNCommand(ref IDbConnection dbConnection, ref IDbCommand dbCommand)
    {
        dbCommand?.Dispose();
        dbCommand = null;
        dbConnection?.Close();
        dbConnection = null;
    }

    public void CreateTables()
    {
        IDbConnection dbConnection = CreateDBConnection();
        IDbCommand dbCommand = dbConnection.CreateCommand();

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
                        time_slot_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        time_from TEXT NOT NULL,
                        time_to TEXT NOT NULL,
                        time_started TEXT DEFAULT NULL,
                        time_finished TEXT DEFAULT NULL,
                        is_completed INTEGER DEFAULT 0,
                        location TEXT DEFAULT NULL,
                        repeat TEXT DEFAULT NULL,
                        reminder TEXT DEFAULT NULL,
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
                        is_completed INTEGER DEFAULT 0,
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

        DisposeConnectionNCommand(ref dbConnection, ref dbCommand);
    }

    public void DropTables()
    {
        IDbConnection dbConnection = CreateDBConnection();
        IDbCommand dbCommand = dbConnection.CreateCommand();
        
        string query = @"
                        select 'drop table ' || name || ';' from sqlite_master
                        where type = 'table';
                        ";
        dbCommand.CommandText = query;
        PrintQuery(query);
        PrintRowAffected(dbCommand.ExecuteNonQuery());
        
        DisposeConnectionNCommand(ref dbConnection, ref dbCommand);
    }

    public int ExecuteQueryAndReturnRowsAffected(string query)
    {
        IDbConnection dbConnection = CreateDBConnection();
        IDbCommand dbCommand = dbConnection.CreateCommand();

        dbCommand.CommandText = query;
        PrintQuery(query);
        int rowsAffected = dbCommand.ExecuteNonQuery();
        PrintRowAffected(rowsAffected);

        DisposeConnectionNCommand(ref dbConnection, ref dbCommand);
        return rowsAffected;
    }

    public long ExecuteQueryAndReturnTheRowID(string query)
    {
        IDbConnection dbConnection = CreateDBConnection();
        IDbCommand dbCommand = dbConnection.CreateCommand();

        dbCommand.CommandText = query + " SELECT last_insert_rowid();";
        PrintQuery(query);
        if(dbCommand == null)
        {
            Debug.Log("dbCommand is null");
        }
        long rowID = (long)dbCommand?.ExecuteScalar();
        Debug.Log(rowID);

        DisposeConnectionNCommand(ref dbConnection, ref dbCommand);
        return rowID;
    }

    public IEnumerable<T> ExecuteQueryAndReturnRows<T>(string query, Func<IDataRecord, T> copyRow)
    {
        IDbConnection dbConnection = CreateDBConnection();
        IDbCommand dbCommand = dbConnection.CreateCommand();

        dbCommand.CommandText = query;
        PrintQuery(query: query);
        using (var rdr = dbCommand.ExecuteReader())
        {
            while (rdr.Read())
            {
                yield return copyRow(rdr);
            }
            rdr.Close();
        }

        DisposeConnectionNCommand(ref dbConnection, ref dbCommand);
    }

    //private IEnumerable<T> GetRows<T>(string sql, Action<SqlParameterCollection> addParameters, Func<IDataRecord, T> copyRow)
    //{
    //    using (var cn = new SqlConnection("Connection string here"))
    //    using (var cmd = new SqlCommand(sql, cn)
    //    {
    //        cmd.CommandType = CommandType.StoredProcedure;
    //    addParameters(cmd.Parameters);
    //    cn.Open();
    //    using (var rdr = cmd.ExecuteReader())
    //    {
    //        while (rdr.Read())
    //        {
    //            yield return copyRow(rdr);
    //        }
    //        rdr.Close();
    //    }
    //}
//}

//void CloseDB()
//{
//    dbCommand?.Dispose();
//    dbCommand = null;
//    dbConnection?.Close();
//    dbConnection = null;
//}
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

    public void PrintDataReader(IDataReader reader)
    {
        if (doesPrintDataReader)
        {
            string readerData = "";
            for (int i = 0; i < reader.FieldCount; i++)
            {
                readerData += reader.GetName(i) + ": " + reader.GetValue(i).ToString() + "\n";
            }
            Debug.Log(readerData);
        }
    }
}
