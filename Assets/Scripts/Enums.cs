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
public class WeekDays
{
    private WeekDays(string value) { Value = value; }
    public string Value { get;}
    public static WeekDays Sunday { get { return new WeekDays("Sunday"); } }
    public static WeekDays Monday { get { return new WeekDays("Monday"); } }
    public static WeekDays Tuesday { get { return new WeekDays("Tuesday"); } }
    public static WeekDays Wednesday { get { return new WeekDays("Wednesday"); } }
    public static WeekDays Thursday { get { return new WeekDays("Thursday"); } }
    public static WeekDays Friday { get { return new WeekDays("Friday"); } }
    public static WeekDays Saterday { get { return new WeekDays("Saterday"); } }
}

public class RepeatPeriod
{
    private RepeatPeriod(string value) { Value = value; }
    public string Value { get;}
    public static RepeatPeriod Day { get { return new RepeatPeriod("Day"); } }
    public static RepeatPeriod Week { get { return new RepeatPeriod("Week"); } }
    public static RepeatPeriod Month { get { return new RepeatPeriod("Month"); } }
    public static RepeatPeriod Year { get { return new RepeatPeriod("Year"); } }
}