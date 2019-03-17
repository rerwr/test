using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using Game;
using UnityEngine;


public class CommitController : BaseController<CommitController>
{
    protected override Type GetEventType()
    {
        return typeof(CommitControllerEvent);
    }

    public override void InitController()
    {
        _Proxy = new NetProxy(NetModules.GameAction.ModuleId);
        //module =3 ,sub = 28
        SocketParser.Instance.RegisterParser(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_Postage_Req, Farm_Game_Postage_Anw.ParseFrom);
        _Proxy.AddNetListenner(NetModules.GameAction.Farm_Game_Postage_Req, PostageCallBack);

        //  3-16
        _Proxy = new NetProxy(NetModules.GameAction.ModuleId);
        SocketParser.Instance.RegisterParser(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_OidExchange_Req, Farm_Game_OidExchange_Anw.ParseFrom);
        _Proxy.AddNetListenner(NetModules.GameAction.Farm_Game_OidExchange_Req, OidExchangeCallBack);

        //  3-30
        _Proxy = new NetProxy(NetModules.GameAction.ModuleId);
        SocketParser.Instance.RegisterParser(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_PaySucc_Req, Farm_Game_PaySucc_Anw.ParseFrom);
        _Proxy.AddNetListenner(NetModules.GameAction.Farm_Game_PaySucc_Req, PaySuccCallBack);
        //3-31
        _Proxy = new NetProxy(NetModules.GameAction.ModuleId);
        SocketParser.Instance.RegisterParser(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_CheckOrder_Req, Farm_Game_CheckOrder_Anw.ParseFrom);//注册对应消息（在服务器发过来时）的Proto解析器
        _Proxy.AddNetListenner(NetModules.GameAction.Farm_Game_CheckOrder_Req, CheckOrderCallBack);
    }

    public void CheckOrderReq()
    {
        var builder = Farm_Game_CheckOrder_REQ.CreateBuilder();

        _Proxy.SendMsg(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_CheckOrder_Req, builder);

    }
    private void CheckOrderCallBack(MsgRec msg)
    {
        if (msg._proto!=null)
        {
            var proto = (Farm_Game_CheckOrder_Anw)msg._proto;
            OrderController.Instance.SetDatas(proto);

            GetDispatcher().Dispatch(CommitControllerEvent.OnOrderCallback);
        }
        else
        {
            SystemMsgView.SystemFunction(Function.Tip, Info.NoOrder);
        }
    
    }


    //3-27兑换请求回调，可以去接口兑换
    private void OidExchangeCallBack(MsgRec msg)
    {
        FieldsController.ProtocalAction = ProtocalAction.None;

        
        GetDispatcher().Dispatch(CommitControllerEvent.OnExchange);
    }

    //3-27兑换请求
    public void OidExchangeReq(int userId, int pattern,
        string recieverName, string phoneNumber,string province,string city, string county,int pinpai, string adress, string beaty,int PayPattern)
    {
        if (FieldsController.ProtocalAction != ProtocalAction.None) return;
        else
        {
            FieldsController.ProtocalAction = ProtocalAction.Exchange;
        }
        Farm_Game_OidExchange_Req.Builder builder = Farm_Game_OidExchange_Req.CreateBuilder();
        builder.UserGameID = userId;
        builder.RecieverName = recieverName;
        builder.PhoneNum = phoneNumber;
        builder.Adress = adress;
        builder.Beaty = beaty;
        builder.City = city;
        builder.County = county=="请选择县级"?"没有填写，见联系地址":county;
        builder.PinPai = pinpai;
        builder.Pattern = pattern;
        builder.PayPattern = PayPattern;
        builder.Province = province;

        Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", builder.County,"hjhj"));

        List<ToggleAndObj> taos = CommitViewModel.Instance.taos;
        
        for (int i = 0; i < taos.Count; i++)
        {
            if (taos[i].T.isOn)
            {
                var a = PMsg_StagePropUnit.CreateBuilder();
                a.Count = taos[i].Num;
                a.Id = taos[i].Id;

                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", a.Count, a.Id));

                builder.AddObjs(a);
            }
        }

        Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", province, county));

        _Proxy.SendMsg(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_OidExchange_Req, builder);
    }

    //3-28
    public void SendPostageReq(List<ToggleAndObj> taos)
    {
        var builder = Farm_Game_Postage_Req.CreateBuilder();
        for (int i = 0; i < taos.Count; i++)
        {
            if (taos[i].T.isOn)
            {
                 var a = PMsg_StagePropUnit.CreateBuilder();
                a.Count = taos[i].Num;
                a.Id = taos[i].Id;
                builder.AddObjs(a);
            }
        }
        _Proxy.SendMsg(NetModules.GameAction.ModuleId,NetModules.GameAction.Farm_Game_Postage_Req,builder);
    }

    private void PostageCallBack(MsgRec msg)
    {
        var builder = (Farm_Game_Postage_Anw)msg._proto;
        CommitViewModel.Instance.Postage=builder.Postage;
        GetDispatcher().Dispatch(CommitControllerEvent.OnPostageCallback);
    }

    //3-30 支付成功服务器通知前端
    public void PaySuccCallBack(MsgRec msg)
    {
        var p = (Farm_Game_PaySucc_Anw) msg._proto;
        OrderController.Instance.SetData(p);
        if (p.Type == 1)
        {

//            MTRunner.Instance.StartRunner(Waitclose(0.3f));
            OnloginPayed();
            GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnFirstPaySucc);
            
            SystemMsgView.SystemFunction(Function.Tip, Info.WXPaySucc);
        }
        else if (p.Type == 2)
        {
//            MTRunner.Instance.StartRunner(Waitclose(0.3f));
            OnCommitPay();
            if (ViewMgr.Instance.isOpen(ViewNames.MsgListView))
            {
                GetDispatcher().Dispatch(CommitControllerEvent.OnOrderCallback);
            }
            SystemMsgView.SystemFunction(Function.Tip, Info.Exchange);

        }
    }
    private void OnCommitPay()
    {

        ViewMgr.Instance.Close(ViewNames.PayChooseView);

        SystemMsgView.SystemFunction(Function.CloseDialog, Info.Exchange);
        ViewMgr.Instance.Close(ViewNames.StoreView);
        ViewMgr.Instance.Close(ViewNames.CommitView);
        GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnCommitSucc);

    }
    private void OnloginPayed()
    {
        ViewMgr.Instance.Close(ViewNames.PayChooseView);
        SystemMsgView.SystemFunction(Function.CloseDialog, Info.Logined);

        PlayerSave.SetString("isFirstPay", "pay");

        GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnFirstPaySucc);
//        MTRunner.Instance.StartRunner(Wait(0.3f));
    }
//    IEnumerator Wait(float time)
//    {
//        yield return time;
//
//    }
//    IEnumerator Waitclose(float time)
//    {
//            yield return time;
//            
//    }
    public class CommitControllerEvent : EventDispatcher.BaseEvent
    {
        public static readonly int OnPostageCallback = ++id;
        public static readonly int OnExchange = ++id;

        public static readonly int OnOrderCallback = ++id;
    }
}


 
