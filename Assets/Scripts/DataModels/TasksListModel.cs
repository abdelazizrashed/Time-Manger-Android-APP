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
        //Todo: Get a list from the database
        //Todo: Remove this temperary return 
        return new TasksListModel(id: 1, title: "A new list");
    }

    public static TasksListModel[] GetLists()
    {
        //Todo: Get all the lists in the database
        //Todo: Remove this temperary return
        TasksListModel[] lists = { new TasksListModel(id: 1, title: "A new list"), new TasksListModel(id: 2, title: "list 2"), new TasksListModel(id: 3, title: "list 3") };
        return lists;
    }

    public static void SaveList(TasksListModel newList)
    {
        //Todo:implement this method
    }
}
