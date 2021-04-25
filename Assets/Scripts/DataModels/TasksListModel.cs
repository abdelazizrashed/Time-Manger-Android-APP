using System.Collections.Generic;
/// <summary>
/// Defines the <see cref="TasksListModel" />.
/// </summary>
public class TasksListModel
{
    /// <summary>
    /// Gets or sets the listID.
    /// The listID is an id uniquely identifing the list in the database.
    /// </summary>
    public int listID { get; set; }

    /// <summary>
    /// Gets or sets the listTitle.
    /// The listTitle is the title of the list.
    /// </summary>
    public string listTitle { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TasksListModel"/> class.
    /// </summary>
    /// <param name="id">The id of the list which uniquely identify it in the database<see cref="int"/>.</param>
    /// <param name="title">The title of the list<see cref="string"/>.</param>
    public TasksListModel(int id, string title)
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
        TasksListModel[] list = { new TasksListModel(id: 1, title: "A new list") };
        return list;
    }
}
