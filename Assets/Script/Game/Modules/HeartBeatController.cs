using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

using Framework;
using UnityEngine;

namespace Game
{
    public enum ErrorInfo
    {
        NoneDispose,//无处理
        Succ,           //1表示成功
        Lose,           //2表示失败
        LessCoin,           //3表示缺少金币
        PhoneNumError,           //4表示号码不正确
        NonePhoneNum,           //5没有该号码
        CantPluck,           //6该地块不可翻地操作
        CantSeed,           //7该地块无法播种
        InviteCodeErrot,           //8为邀请码错误
        InviteCodeTakeEffect,           //9当前用户邀请推广的生效
        crudity,           //10表示未成熟不可采摘
        DogCatch,           //11采摘失败，被狗抓住
        ResultNumError,           //12果实数量不符合合成条件
        PrimaryOilLess,           //13初级精油不足一瓶，无法合成半成品精油
        WaterError,           //14浇水操作失败，
        WeedError,           //15除草操作失败
        DebugError,           //16除虫操作失败
        FertilizerError,           //17施肥操作失败
        ElixirLess,           //18仙丹数量不满足兑换条件
        SeniorOilError,           //19高级精油不满足兑换条件
        VeriCodeError,           ////20验证码失效
        ChatInfoSendError,           //21聊天信息发送失败
        QRCodeError,         //22为二维码错误
        ERROR_INFO ,        //操作延迟，重新刷新土地";


         ACCOUNT_NULL=100,            //22为账号为空
        PW_ERROR=101,            //22为密码错误
        LOGIN_OUTDATE=102,            //22为登陆超时
        UNKNOWN =103 ,           //22为未知错误
        ACCOUNT_REPEATED=104   ,          //22账号重复

         PARAMS_ERROR ,            //22账号重复
         LAND_NULLPLUCK ,            //22账号重复
         GOTOPLUCK ,            //22账号重复
         NO_SEED ,            //22账号重复
         ACCOUNT_EXIT ,            //22账号重复
        CANT_RECIE ,            //无法收获
        Land_PLUCKED,                //土地已经开垦
        Item_None ,               //物品数量不足
        Coin_None  ,              //金币数量不足
        Item_Null ,               //物品不存在
        Oil_NOTENOUGH,              //合成精油数量不足s
        TodayIsSign,
         isFriend ,     //= "已经是好友s";
         isSendReq ,     // = "已经发出申请，等待对方同意s";
         isLogined, //= "你的账号在别处登录s";
        pNOTFRIEND, //= "你们不是好友s";
        HasGet, //该土地已经摘取
        Copldown, //冷却中
        CodeNotCorrect, //验证码不正确
        CodeHasUsed, //二维码已经被使用
        WaterInOne,
        FertilizerIn3,
        HasFerti,
        NoBug,
        NoGrass

}
    public class HeartBeatController : BaseController<HeartBeatController>
    {
        private bool running = false;
        public ErrorInfo errorInfo;
        protected override Type GetEventType()
        {
            return typeof(HeartBeatEvent);
        }

        public override void InitController()
        {
            running = true;
            //0-1
            _Proxy = new NetProxy(NetModules.NetBase.ModuleId);
            SocketParser.Instance.RegisterParser(NetModules.NetBase.ModuleId, NetModules.NetBase.Beat, Farm_Tick_Anw.ParseFrom);
            _Proxy.AddNetListenner(NetModules.NetBase.Beat, AnwCallBack);

            //0-2
            _Proxy=new NetProxy(NetModules.NetBase.ModuleId);
            SocketParser.Instance.RegisterParser(NetModules.NetBase.ModuleId,NetModules.NetBase.ErrorInfo,Farm_CommAnswer.ParseFrom);
            _Proxy.AddNetListenner(NetModules.NetBase.ErrorInfo, AnwErrorInfoCallBack);
            //TODO心跳
            GlobalDispatcher.Instance.AddListener(LoginEvent.OnLoginSucc,((id, o) =>
            {
                MTRunner.Instance.StartRunner(HeartBeatSender());

                return false;
            } ));


//            Debug.Log((int)(ServerTime.Now - new DateTime(2000, 1, 1)).TotalSeconds);
        }

        //0-2
        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <param name="msg"></param>
        private void AnwErrorInfoCallBack(MsgRec msg)
        {
            FieldsController.ProtocalAction = ProtocalAction.None;

            var info = (Farm_CommAnswer)msg._proto;
       
            errorInfo = (ErrorInfo)info.Result;
            string temp=info.Content; 

            Debug.LogError(string.Format("<color=#ff0000ff><---{0}-{1}----></color>", "错误码", info.Result));
            if (temp != "")
            {
                SystemMsgView.SystemFunction(Function.Tip, temp, 2);
                return;
            }
            if (Info.Instance().infos.ContainsKey(info.Result))
            {
                string content = Info.Instance().infos[info.Result];

                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", content, "test1"));

                if (info.Result == 119)
                {
                    SocketMgr.Instance._isneed2loginview = false;
                    SystemMsgView.SystemFunction(Function.Quit, content, 2);

                }
                else if (info.Result == 110)
                {

                }
                else if (info.Result == 124)
                {
                    MTRunner.Instance.StartRunner(Wait(1.5f, content));

                }
                else if (info.Result == 121)
                {

                }
                else if (info.Result == 130)
                {
                    GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnDogCatch);
                    SystemMsgView.SystemFunction(Function.Tip, content, 2);

                    Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", FieldsController.Instance.currentFarmID, "test1"));

                    FieldsModel.Instance.DogCatch(FieldsController.Instance.currentFarmID);
                }
                else if (info.Result == 132)
                {
                    SystemMsgView.SystemFunction(Function.Tip, Info.BUYGAME, 2);
                    PayOrderInterfaceMgr.Instance.payfor = PayFor.Login;
                    ViewMgr.Instance.Open(ViewNames.PayChooseView);
                }
                else
                {
                    SystemMsgView.SystemFunction(Function.Tip, content, 2);

                }
            }
        

            //停止旋转
            LoadingImageManager.Instance.Ending();

        }


        IEnumerator Wait(float time,string content)
        {
            yield return time;
            SystemMsgView.SystemFunction(Function.Tip, content, 2);

        }
        //0-1
        private void AnwCallBack(MsgRec msg)
        {
            var info = (Farm_Tick_Anw)msg._proto;
            double t=info.Unixtime;


            ServerTime.Now=Clock.ConvertIntDateTime(t);
            //Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", t, "test1"));

            GlobalDispatcher.Instance.Dispatch(GlobalEvent.ServerTimeReflash);
            
        }


        private IEnumerator HeartBeatSender()
        {
            yield return MTRunner.TYPE_Asyn;
            while (running)
            {
                if (GameStarter.Instance.isOpenBeating==false)
                {
                    running = false;
                }
            
                    Debug.Log("------test------");
                    if (SocketMgr.Instance.Status == SocketMgr.status.Connected)
                    {
                        var builder = Farm_Tick_Req.CreateBuilder();

//                        builder.Unixtime = (int)Clock.ConvertDataTime2UnixTime(ServerTime.Now);
                        
                        _Proxy.SendMsg(NetModules.NetBase.ModuleId, NetModules.NetBase.Beat, builder);
                    }
                    yield return MTRunner.TYPE_Asyn;
                    Thread.Sleep(3000);
                
            }
        }

        internal void Stop()
        {
            running = false;
        }
        

        public class HeartBeatEvent : EventDispatcher.BaseEvent
        {

        }
    }


}
