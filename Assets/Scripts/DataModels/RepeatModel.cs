using System;

public class RepeatModel: System.Object
{
    public RepeatPeriod repeatPeriod { get; set; }
    public int repeatEveryNum { get; set; }

    public WeekDays[] repeatDays { get; set; }

    public DateTime? endDate { get; set; }

    public RepeatModel(RepeatPeriod period, WeekDays[] days, int every = 1, DateTime? end = null)
    {
        repeatPeriod = period;
        repeatEveryNum = every;
        repeatDays = days;
        endDate = end;
    }
}

public enum WeekDays
{
    Sunday,
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saterday
}

public enum RepeatPeriod
{
    Day,
    Week,
    Month,
    Year
}