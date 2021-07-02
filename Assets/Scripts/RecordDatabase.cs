using UnityEngine;
using System.IO;
using System;

public class RecordDatabase
{
    public long AllTimeBest => record.allTimeBest;
    public long TodaysBest => record.todaysBest[3];

    public bool NewAllTimeBest;
    public bool NewTodaysBest;

    private const string DataFileName = "/data.dat";
    private RecordsData record = new RecordsData();

    public void WriteRecord(long runMeters)
    {
        bool shouldUpdate = false;
        NewTodaysBest = NewAllTimeBest = false;

        if (runMeters > record.allTimeBest)
        {
            record.allTimeBest = runMeters;
            shouldUpdate = true;
            NewAllTimeBest = true;
        }

        if (runMeters > record.todaysBest[3])
        {
            record.todaysBest[3] = runMeters;
            shouldUpdate = true;
            NewTodaysBest = true;
        }

        if (shouldUpdate)
        {
            var json = JsonUtility.ToJson(record).Base64Encode();
            File.WriteAllText(Directory.GetCurrentDirectory() + DataFileName, json);
        }
    }

    public void LoadRecord()
    {
        try
        {
            var json = File.ReadAllText(Directory.GetCurrentDirectory() + DataFileName).Base64Decode();
            var recordFromFile = JsonUtility.FromJson<RecordsData>(json);

            var currentYear = DateTime.Today.Year;
            var currentMonth = DateTime.Today.Month;
            var currentDay = DateTime.Today.Day;

            if (recordFromFile.todaysBest[0] != currentYear || recordFromFile.todaysBest[1] != currentMonth || recordFromFile.todaysBest[2] != currentDay)
                recordFromFile.todaysBest = new long[] { DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0 };

            record = recordFromFile;
        }
        catch
        {
            Debug.LogWarning("Data file is corrupt!");
        }
    }
}