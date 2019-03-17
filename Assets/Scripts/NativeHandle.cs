//*************************************************************************
//	创建日期:	2015-10-12
//	文件名称:	NativeHandle.cs
//  创建作者:	Rect 	
//	版权所有:	shadowkong.com
//	相关说明:	
//*************************************************************************


//-------------------------------------------------------------------------


using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using Game;

public class NativeHandle
{
    //-------------------------------------------------------------------------
    private object m_NativeObj;

    private AndroidJavaClass androidCall;
    //-------------------------------------------------------------------------
    public NativeHandle()
    {


#if UNITY_ANDROID&&!UNITY_EDITOR
        AndroidJavaClass androidActivityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        m_NativeObj = androidActivityClass.GetStatic<AndroidJavaObject>("currentActivity");
        androidCall = new AndroidJavaClass("com.Util.Component.CallMethod");
        androidCall.CallStatic("init", m_NativeObj);
#endif
    }


    public void InstallApk(string path)
    {
        androidCall.CallStatic("OpenApk", path);

    }

    public void WechatPay(string appid,string parterid,string prepayid,string nonstr,string timestamp,string packagestr,string sign)
    {
#if UNITY_ANDROID
    
        androidCall.CallStatic("WXPay",appid, parterid, prepayid, nonstr,timestamp,packagestr, sign);

#elif UNITY_IPHONE
        _wxpay(appid, parterid, prepayid, packagestr, nonstr, timestamp, sign);

#endif
    }

    public void Alipay(string signpayinfo)
    {

#if UNITY_ANDROID
       
        androidCall.CallStatic("Alipay", signpayinfo);
#elif UNITY_IPHONE
        IOSAlipay(signpayinfo);
#endif
    }
    //    public void Alipay(int paycount,string signpayinfo)
    //    {
    //#if UNITY_ANDROID
    //        androidCall.CallStatic("Alipay");
    //#elif UNITY_IPHONE
    //#endif
    //    }

    //-------------------------------------------------------------------------
    /// <summary>
    /// 头像设置调用 !!Warnning!! : 图片文件统一保存在路径 Application.persistentDataPath (For Unity)中
    /// </summary>
    /// <param name="strObjectName">Unity回调对象名字</param>
    /// <param name="strFuncName">Unity回调挂在对象上的Mono函数名</param>
    /// <param name="strFileName">Unity传过来的 图片保存名字 (仅仅是文件名,并非全路径)</param>
    public void SettingAvaterFormMobile(string strObjectName, string strFuncName, string strFileName)
    {
        if (
            string.IsNullOrEmpty(strObjectName) ||
            string.IsNullOrEmpty(strFuncName) ||
            string.IsNullOrEmpty(strFileName)
            )
        {
            Debug.Log("NativeHandle::SettingAvaterFormMobile params is invalid");
            return;
        }
        
#if UNITY_ANDROID
        
        {
            if (null == m_NativeObj)
            {
                return;
            }
//            AndroidJavaClass obj = new AndroidJavaClass("com.rect.avatar.UserAvatarActivity");
//
//            obj.CallStatic("OpenPic", strObjectName, strFuncName, strFileName);
            androidCall.CallStatic("SettingAvaterFormMobile", strObjectName, strFuncName, strFileName);
        }
        

#elif UNITY_IPHONE

        {
            SettingAvaterFormiOS(strObjectName, strFuncName, strFileName);
        }
#else

        Debug.Log("NativeHandle::SettingAvaterFormMobile no handle at this Modlue");

#endif

    }
    //-------------------------------------------------------------------------
    [DllImport("__Internal")]
    private static extern void SettingAvaterFormiOS(string strObjectName, string strFuncName, string strFileName);
    //-------------------------------------------------------------------------
    [DllImport("__Internal")]
    private static extern void _wxpay(string appid, string mchid, string prepayid, string package, string nonceStr, string timeStamp, string sign);
    //-------------------------------------------------------------------------
    [DllImport("__Internal")]
    private static extern void IOSAlipay(string orderstr);
    //-------------------------------------------------------------------------
  
}

















