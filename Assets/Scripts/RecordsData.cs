using System;

[Serializable]
public class RecordsData
{
    public long allTimeBest = 0;
    public long[] todaysBest = new long[] { DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0 };
}
