using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class UtilitySystems
{
    /// <summary>
    /// DateTimeをタイムスタンプ値に変換
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static int ConvertDateTimeToTimeStamp(DateTime dateTime)
    {
        var timeStamp = TimeZoneInfo.ConvertTimeToUtc(dateTime) - new DateTime(year: 1970, month: 1, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc);
        return (int)timeStamp.TotalSeconds;
    }

    /// <summary>
    /// 重み付けの配列からランダムでインデックスを返す
    /// </summary>
    /// <param name="priorities"></param>
    /// <returns></returns>
    public static int GetRandomValueByPriority(params int[] priorities)
    {
        int index = 0;

        if (priorities.Length <= 1)
        {
            return index;
        }

        int sumValue = priorities.Sum();
        int rndValue = UnityEngine.Random.Range(0, sumValue + 1);

        foreach (var priority in priorities)
        {
            if (priority >= rndValue)
            {
                break;
            }

            index++;
            rndValue -= priority;
        }

        return index;
    }
}
