using System;

public class RepeatModel
{
    RepeatPeriod repeatPeriod;
    int repeatEveryNum;

    WeekDays[] repeatDays;

    DateTime? endDate;

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