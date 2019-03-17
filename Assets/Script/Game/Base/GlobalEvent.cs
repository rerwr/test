using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;

public class GlobalEvent : EventDispatcher.BaseEvent
{
    //Game Life Circle
    public static readonly int OnEnterGame = ++id;
    public static readonly int OnLogin = ++id;
    public static readonly int OnLogout = ++id;

    public static readonly int OnDisconnect = ++id;
    public static readonly int OnConnect = ++id;
    public static readonly int OnReconnect = ++id;


    //Clock
    public static readonly int OnTenMinute = ++id;
    //线程一秒
    public static readonly int OnOneSecond = ++id;
    //线程3秒
    public static readonly int OnThreeSecond = ++id;
    public static readonly int OnThreadOneSecond = ++id;
    public static readonly int OnHour = ++id;
    public static readonly int OnDay = ++id;
    public static readonly int ServerTimeReflash = ++id;


    public static readonly int OnRegionsConfigLoadDone = ++id;
    public static readonly int OnLoginConfigLoadDone = ++id;
    //加载收获手势
    public static readonly int OnLoadingGetIcon = ++id;

    public static readonly int OnDataConfigLoadDone = ++id;
    public static readonly int OnLoadURLConfig = ++id;
    public static readonly int onPlayerPanelReflash = ++id;
    public static readonly int OnPlantTools = ++id;//工具ID
    public static readonly int OnDogCatch = ++id;//被狗狗抓住



    public static readonly int OnSelectSeed = ++id;//选中某个种子

    public static readonly int OnFerItemChange = ++id;//肥料改变
    public static readonly int OnResultItemChange = ++id;//果实改变
    public static readonly int OnOilItemChange = ++id;//精油物品发生改变

    
    public static readonly int OnIconLoaded = ++id;//头像加载完毕
    public static readonly int OnStoreUnitsChange = ++id;


    public static readonly int OnFarmUnitClick = ++id;//农田点击

    public static readonly int OnViewLoadFinished = ++id;//农田点击

    public static readonly int OnAvatarEnd = ++id;//相册点击结束
    public static readonly int OnCommitSucc = ++id;//支付宝支付成功
    public static readonly int OnAliPayfaid = ++id;//支付宝支付失败

    public static readonly int OnWXPaySucc = ++id;//支付宝支付成功
    public static readonly int OnWXPayfaid = ++id;//支付宝支付失败
    public static readonly int OnPayEnded = ++id;//支付结束 跳回unity界面时 不管是否成功
    public static readonly int OnPluckSucc = ++id;//摘取成功

    public static readonly int Onfocus = ++id;//屏幕启动聚焦
    public static readonly int OnPause = ++id;//游戏暂停
    public static readonly int OnFirstPaySucc = ++id;//第一次游戏支付
    public static readonly int OnCommitPaySucc = ++id;//兑换游戏支付
 



    /// <summary>
    /// 场地任何物体ID
    /// </summary>
    public static readonly int OnAnyObj = ++id;

    public static readonly int OnLine = ++id;

    public static readonly int OnUpdateEnd = ++id;

    public static readonly int OnLoadingApk = ++id;

    public static readonly int OnMsgChange = ++id;
}
