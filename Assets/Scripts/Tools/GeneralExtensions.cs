using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;
using UnityEngine.EventSystems;

public static class GeneralExtensions
{
    public static void DestroyAllChilds(this Transform tr)
    {
        for (int i = tr.childCount - 1; i >= 0; i--)
            Object.Destroy(tr.GetChild(i).gameObject);
    }

    public static void Reset(this Transform tr)
    {
        tr.localEulerAngles = Vector3.zero;
        tr.localScale = Vector3.one;
        tr.localPosition = Vector3.zero;
    }

    public static void SafeSetParent(this Transform tr, Transform parent)
    {
		tr.SetParent(parent, false);
    }

    public static string AsUTF8String(this byte[] content)
    {
        return Encoding.UTF8.GetString(content);
    }

	public static DateTime BeginOfday(this DateTime dt)
	{
		return new DateTime(dt.Year,dt.Month,dt.Day,0,0,0);
	}

	public static bool IsSameDay(this DateTime dt, DateTime otherDate)
	{
		return dt.BeginOfday().Ticks == otherDate.BeginOfday().Ticks;
	}

	public static DateTime NonSleepyTime(this DateTime dt, int workDayMinHours, int workDayMaxHours, int? minHours = null, int? maxHous = null)
	{
		//holydays
		if(minHours.HasValue && maxHous.HasValue)
		{
			var dayOfWeek = dt.DayOfWeek;

			if(dayOfWeek == DayOfWeek.Sunday || dayOfWeek == DayOfWeek.Saturday){
				var minDate = new DateTime(dt.Year,dt.Month, dt.Day, minHours.Value, 0, 0);
				var maxDate = new DateTime(dt.Year,dt.Month, dt.Day, maxHous.Value, 0, 0);

				if(dt >= minDate && dt <= maxDate){
					return dt;
				}

				if(dt < minDate){
					return dt.BeginOfday().AddHours(minHours.Value);
				}

				var newDay = dt.BeginOfday().AddDays(1);
				dayOfWeek = newDay.DayOfWeek;

				return newDay.AddHours((dayOfWeek == DayOfWeek.Sunday || dayOfWeek == DayOfWeek.Saturday) ? minHours.Value : workDayMinHours);
			}
			else
			{
				var minDate = new DateTime(dt.Year,dt.Month, dt.Day, workDayMinHours, 0, 0);
				var maxDate = new DateTime(dt.Year,dt.Month, dt.Day, workDayMaxHours, 0, 0);

				if(dt >= minDate && dt <= maxDate){
					return dt;
				}

				if(dt < minDate){
					return dt.BeginOfday().AddHours(workDayMinHours);
				}

				var newDay = dt.BeginOfday().AddDays(1);
				dayOfWeek = newDay.DayOfWeek;

				return newDay.AddHours((dayOfWeek == DayOfWeek.Sunday || dayOfWeek == DayOfWeek.Saturday) ? minHours.Value : workDayMinHours);
			}
		}
		else
		{
			var minDate = new DateTime(dt.Year,dt.Month, dt.Day, workDayMinHours, 0, 0);
			var maxDate = new DateTime(dt.Year,dt.Month, dt.Day, workDayMaxHours, 0, 0);

			if(dt >= minDate && dt <= maxDate){
				return dt;
			}

			if(dt < minDate){
				return dt.BeginOfday().AddHours(workDayMinHours);
			}

			return dt.BeginOfday().AddDays(1).AddHours(workDayMinHours);
		}
	}

	public static string ToPriceString(this double price)
	{
		return string.Format("{0:N2}", price);
	}
}
