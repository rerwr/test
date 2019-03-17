using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
public class ServerTime
{
    public static long unixtime;
    private static TimeSpan offset = TimeSpan.Zero;
    public static DateTime Now
    {
        get { return DateTime.Now + offset; }
        set { offset = value - DateTime.Now; }
    }
}
}
