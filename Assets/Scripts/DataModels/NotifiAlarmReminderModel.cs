﻿public class NotifiAlarmReminderModel
{
    public ReminderType reminderType;
    public TimePeriodsType timePeriodType;
    public int timePeriodsNum;

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