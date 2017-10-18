using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDF.Module.Scheduler.Core.DateTimeExtensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 当前时间是否在分钟范围内
        /// </summary>
        /// <param name="time">当前时间</param>
        /// <param name="lastTime">上次的时间</param>
        /// <param name="span">从当前小时的几秒为起始时间范围</param>
        /// <returns>是否在范围内</returns>
        public static bool IsMinuteTime(this DateTime time, DateTime? lastTime, TimeSpan span)
        {
            var min = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 0).Add(span);
            var max = min.AddMinutes(1);
            if (lastTime == null)
            {
                if (time >= min && time < max)
                {
                    return true;
                }
            }
            else
            {
                if (time >= min && time < max && min > lastTime.Value)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 当前时间是否在小时范围内
        /// </summary>
        /// <param name="time">当前时间</param>
        /// <param name="lastTime">上次的时间</param>
        /// <param name="span">从当前小时的几分几秒为起始时间范围</param>
        /// <returns>是否在范围内</returns>
        public static bool IsHourlyTime(this DateTime time, DateTime? lastTime, TimeSpan span)
        {
            var min = new DateTime(time.Year, time.Month, time.Day, time.Hour, 0, 0).Add(span);
            var max = min.AddHours(1);
            if (lastTime == null)
            {
                if (time >= min && time < max)
                {
                    return true;
                }
            }
            else
            {
                if (time >= min && time < max && min > lastTime.Value)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 当前时间是否在天范围内
        /// </summary>
        /// <param name="time">当前时间</param>
        /// <param name="lastTime">上次的时间</param>
        /// <param name="span">从当前天的几时几分几秒为起始时间范围</param>
        /// <returns>是否在范围内</returns>
        public static bool IsDailyTime(this DateTime time, DateTime? lastTime, TimeSpan span)
        {
            var min = time.Date.Add(span);
            var max = min.AddDays(1);
            if (lastTime == null)
            {
                if (time >= min && time < max)
                {
                    return true;
                }
            }
            else
            {
                if (time >= min && time < max && min > lastTime.Value)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 当前时间是否在周范围内
        /// </summary>
        /// <param name="time">当前时间</param>
        /// <param name="lastTime">上次的时间</param>
        /// <param name="span">从当前天的几时几分几秒为起始时间范围</param>
        /// <param name="week">第几周</param>
        /// <returns>是否在范围内</returns>
        public static bool IsWeeklyTime(this DateTime time, DateTime? lastTime, TimeSpan span, DayOfWeek week)
        {
            var dayNow = time.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)time.DayOfWeek;
            var dayWeek = week == DayOfWeek.Sunday ? 7 : (int)week;
            var min = time.Date.AddDays(dayWeek - dayNow).Add(span);
            var max = min.AddDays(7);
            if (lastTime == null)
            {
                if (time >= min && time < max)
                {
                    return true;
                }
            }
            else
            {
                if (time >= min && time < max && min > lastTime.Value)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 当前时间是否在月范围内
        /// </summary>
        /// <param name="time">当前时间</param>
        /// <param name="lastTime">上次的时间</param>
        /// <param name="span">从当前月的几天几时几分几秒为起始时间范围</param>
        /// <returns>是否在范围内</returns>
        public static bool IsMonthlyTime(this DateTime time, DateTime? lastTime, TimeSpan span)
        {
            var min = time.Date.AddDays(1 - time.Date.Day).Add(span);
            var max = min.AddMonths(1);
            if (lastTime == null)
            {
                if (time >= min && time < max)
                {
                    return true;
                }
            }
            else
            {
                if (time >= min && time < max && min > lastTime.Value)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
