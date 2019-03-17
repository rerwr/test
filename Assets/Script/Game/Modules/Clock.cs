using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Framework;
using Game;
using UnityEngine;

public class Clock : SingletonMonoBehaviour<Clock>
{
    private int time = 0;

    public void Init()
    {
        
    }
      public static System.DateTime ConvertIntDateTime(double d)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            DateTime dt = startTime.AddSeconds(d);
            return dt;
        }

        public static DateTime ConvertStringToDateTime(string timeStamp)
        {
          
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }


    //把当前的时间转换成时间戳

    public static long ConvertDataTime2UnixTime(DateTime datatime)
    {
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
        long timeStamp = (long)(datatime - startTime).TotalSeconds; // 相差秒数

        Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", timeStamp, Clock.ConvertIntDateTime(timeStamp)));

        return timeStamp;
    }

    private int i = 0;
    void FixedUpdate()
    {
        if (SocketMgr.Instance&&SocketMgr.Instance.Status==SocketMgr.status.Connected)
        {
            DateTime now = ServerTime.Now;
            int minut = now.Minute;
            if (time != now.Second)
            {
                GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnOneSecond);
                i++;
            }
            if (i==2)
            {
                i = 0;
                GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnThreeSecond);
            }
            time = now.Second;

            if (minut % 10 == 0)
            {
                GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnTenMinute);
                if (minut == 0)
                {
                    GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnHour);
                    if (now.Hour == 0)
                    {
                        GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnDay);
                    }
                }
            }
        }
     
    }
}
