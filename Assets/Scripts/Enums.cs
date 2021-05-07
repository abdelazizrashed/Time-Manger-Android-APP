public class Pages
{
    private Pages(string value) { Value = value; }

    public string Value { get; set; }
    public TasksListModel tasksList { get; set; }

    public static Pages All { get { return new Pages("All"); } }
    public static Pages Tasks { get { return new Pages("Tasks"); } }
    public static Pages Events { get { return new Pages("Events"); } }
    public static Pages Reminders { get { return new Pages("Reminders"); } }
}
