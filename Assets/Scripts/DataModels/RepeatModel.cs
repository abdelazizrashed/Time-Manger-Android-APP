using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

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

    public string JSON()
    {
        return JsonConvert.SerializeObject(this);
    }
}

