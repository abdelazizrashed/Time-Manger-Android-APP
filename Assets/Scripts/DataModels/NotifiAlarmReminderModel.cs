public class NotifiAlarmReminderModel
{
    ReminderType reminderType;
    TimePeriodsType timePeriodType;
    int timePeriodsNum;

    public NotifiAlarmReminderModel(ReminderType type, TimePeriodsType tPeriodsType, int tPeriodsNum)
    {
        reminderType = type;
        timePeriodType = tPeriodsType;
        timePeriodsNum = tPeriodsNum;
    }
}

public enum ReminderType
{
    Alarm,
    Notification
}

public enum TimePeriodsType
{
    Minutes,
    Hours,
    Days,
    Weeks
}