//*************************************************************************
//	创建日期:	2015-9-24
//	文件名称:	JarCallback.cs
//  创建作者:	Rect 	
//	版权所有:	shadowkong.com
//	相关说明:	
//*************************************************************************


//-------------------------------------------------------------------------


using UnityEngine;
using System.Collections;
using Framework;
using Game;
/// <summary>
/// 所有jar包的回调方法
/// </summary>
public class JarCallback : SingletonMonoBehaviour<JarCallback>
{
    public ENUM_AVATAR_RESULT result;
    //-------------------------------------------------------------------------
    private Texture m_texture;
    private string m_LogMessage;
    //-------------------------------------------------------------------------

    /// <summary>
    /// 微信支付0为成功，-1错误，-2取消
    /// </summary>
    /// <param name="errCode"></param>
    void OnWXPayCallback(string strResult)
    {
        //        SystemMsgView.SystemFunction(Function.Tip, strResult);

        switch (strResult)
        {
            case "0":
                    PaySucc();
                
//                else if(PayOrderInterfaceMgr.Instance.payfor == PayFor.Exchange)
//                {
//                    OnCommitPay();
//                }

                break;
            case "-1":

                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", ENUM_AVATAR_RESULT.eResult_Cancel.ToString(), "支付失败"));
                GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnWXPayfaid);
                SystemMsgView.SystemFunction(Function.Tip, Info.WXPayCancel);

                break;
            case "-2":
                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "支付失败", "test1"));
                break;

        }
        GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnPayEnded);

    }

    void PaySucc()
    {
        LoadingImageManager.Instance.AddLoadingItem();

        if (PayOrderInterfaceMgr.Instance.payfor == PayFor.Login)
        {
//            PlayerSave.SetInt("Login", 1);
              ViewMgr.Instance.Close(ViewNames.PayChooseView);
        }
    }
    void OnIOSWeChatPayReslut(string argement)
    {
        //        SystemMsgView.SystemFunction(Function.Tip, argement);
        switch (argement)
        {
            case "0":

                    PaySucc();
            
//                else if (PayOrderInterfaceMgr.Instance.payfor == PayFor.Exchange)
//                {
//                    OnCommitPay();
//                }

                SystemMsgView.SystemFunction(Function.Tip, Info.WXPaySucc);

                break;
            case "-1":
                SystemMsgView.SystemFunction(Function.Tip, Info.WXPayCancel);
                GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnWXPayfaid);

                break;
            case "-2":
                SystemMsgView.SystemFunction(Function.Tip, Info.WXPayCancel);

                break;
            default:

                break;

        }
        GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnPayEnded);

    }
    /// <summary>
    /// 支付宝回调
    /// </summary>
    void OnAlipayCallBack(string strResult)
    {
        //        SystemMsgView.SystemFunction(Function.Tip, strResult);

        if (strResult.Equals(ENUM_AVATAR_RESULT.eResult_Success.ToString()))
        {
            result = ENUM_AVATAR_RESULT.eResult_Success;
            PaySucc();

            //            if (PayOrderInterfaceMgr.Instance.payfor == PayFor.Login)
            //            {
            //                OnloginPayed();
            //
            //            }
            //            else if (PayOrderInterfaceMgr.Instance.payfor == PayFor.Exchange)
            //            {
            //                OnCommitPay();
            //            }

            GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnCommitSucc);
        }
        else if (strResult.Equals(ENUM_AVATAR_RESULT.eResult_Cancel.ToString()))
        {
            result = ENUM_AVATAR_RESULT.eResult_Cancel;

            GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnAliPayfaid);
            SystemMsgView.SystemFunction(Function.Tip, Info.WXPayCancel);

            // 取消
        }
        else if (strResult.Equals(ENUM_AVATAR_RESULT.eResult_Failed.ToString()))
        {
            result = ENUM_AVATAR_RESULT.eResult_Failed;

            // 失败
        }
        GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnPayEnded);

    }
    /// <summary>
    /// 支付宝ios回调
    /// </summary>
    /// <param name="strResult"></param>
    void OnIOSAlipayReslut(string strResult)
    {
        //        SystemMsgView.SystemFunction(Function.Tip, strResult);

        if (strResult.Equals("9000"))
        {
            PaySucc();

            result = ENUM_AVATAR_RESULT.eResult_Success;
//            if (PayOrderInterfaceMgr.Instance.payfor == PayFor.Login)
//            {
//                OnloginPayed();
//            }
//            else if (PayOrderInterfaceMgr.Instance.payfor == PayFor.Exchange)
//            {
//                OnCommitPay();
//            }
        }
        else
        {
            result = ENUM_AVATAR_RESULT.eResult_Failed;
            SystemMsgView.SystemFunction(Function.Tip, Info.WXPayCancel);

            GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnAliPayfaid);

            // 取消
        }
        GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnPayEnded);

    }

    //    private void OnCommitPay()
    //    {
    //        MTRunner.Instance.StartRunner(Wait(0.3f));
    //        SystemMsgView.SystemFunction(Function.CloseDialog, Info.Exchange);
    //        ViewMgr.Instance.Close(ViewNames.StoreView);
    //        ViewMgr.Instance.Close(ViewNames.CommitView);
    //        GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnCommitSucc);
    //        
    //    }
    //    private void OnloginPayed()
    //    {
    //        PlayerSave.SetString("isFirstPay", "pay");
    //
    //        GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnFirstPaySucc);
    //        MTRunner.Instance.StartRunner(Wait(0.3f));
    //    }
    //
    //    IEnumerator Wait(float time)
    //    {
    //        yield return time;
    //        ViewMgr.Instance.Close(ViewNames.PayChooseView);
    //
    //    }
    /// <summary>
    /// 相机回调
    /// </summary>
    /// <param name="strResult"></param>
    void OnAvaterCallBack(string strResult)
    {
        m_LogMessage += " - " + strResult;

        if (strResult.Equals(ENUM_AVATAR_RESULT.eResult_Success.ToString()))
        {
            result = ENUM_AVATAR_RESULT.eResult_Success;

            GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnAvatarEnd);
        }
        else if (strResult.Equals(ENUM_AVATAR_RESULT.eResult_Cancel.ToString()))
        {
            result = ENUM_AVATAR_RESULT.eResult_Cancel;

        }
        else if (strResult.Equals(ENUM_AVATAR_RESULT.eResult_Failed.ToString()))
        {
            result = ENUM_AVATAR_RESULT.eResult_Failed;

        }
    }



}





























































