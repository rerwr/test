using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;

namespace Game
{
    public class AnnouncementController:BaseController<AnnouncementController>
    {
        protected override Type GetEventType()
        {
            return typeof(AnnouncementEvent);
        }

        public override void InitController()
        {
            //  标识: module = 2,sub = 8
            _Proxy = new NetProxy(NetModules.FarmGameProtocal.ModuleId);
            SocketParser.Instance.RegisterParser(NetModules.FarmGameProtocal.ModuleId,NetModules.FarmGameProtocal.Farm_Game_AnnouncementInfo_Req,Farm_Game_AnnouncementInfo_Anw.ParseFrom);
            _Proxy.AddNetListenner(NetModules.FarmGameProtocal.Farm_Game_AnnouncementInfo_Req,AnwCallBack);
            //3-26
            _Proxy = new NetProxy(NetModules.GameAction.ModuleId);
            SocketParser.Instance.RegisterParser(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_InfoLog_Req, Farm_Game_InfoLog_Anw.ParseFrom);
            _Proxy.AddNetListenner(NetModules.GameAction.Farm_Game_InfoLog_Req, AnwCallBack);
        }
        //3-26
        protected void AnwInfoCallBack(MsgRec msgRec)
        {
            Farm_Game_InfoLog_Anw announcement = (Farm_Game_InfoLog_Anw)msgRec._proto;
            AnnouncementModel a=AnnouncementModel.Instance;
            a.Info = announcement.Info;
            a.CompanyInfo1 = announcement.CompanyInfo;
            a.LoginInfo1 = announcement.LoginInfo;
            a.ExpressInfo = announcement.ExpressInfo;
            a.CompanyInfo1 = announcement.CompanyInfo;
         
            a.Strategy = announcement.Strategy;
            a.Other2 = announcement.Other2;
            a.Other3 = announcement.Other3;
            
            GetDispatcher().Dispatch(AnnouncementEvent.OnAnn, announcement.Info);
        }

        public void ReqInfo()
        {
            var builder = Farm_Game_InfoLog_Req.CreateBuilder();
            builder.Uid = LoginModel.Instance.Uid;
            _Proxy.SendMsg(NetModules.GameAction.ModuleId,NetModules.GameAction.Farm_Game_InfoLog_Req,builder);
        }


        //2-8
        protected  void AnwCallBack(MsgRec msgRec)
        {
            Farm_Game_AnnouncementInfo_Anw announcement = (Farm_Game_AnnouncementInfo_Anw) msgRec._proto;
            AnnouncementModel.Instance.SetData(announcement);
            GetDispatcher().Dispatch(AnnouncementEvent.OnAnn);
        }

        public  void ReqSendAnnouncementAction()
        {
            var builder = Farm_Game_AnnouncementInfo_Req.CreateBuilder();
            builder.UserGameID = LoginModel.Instance.Uid;
            _Proxy.SendMsg(NetModules.FarmGameProtocal.ModuleId,NetModules.FarmGameProtocal.Farm_Game_AnnouncementInfo_Req,builder);
            
        }

        
    }
    public class AnnouncementEvent : EventDispatcher.BaseEvent
    {
        public static readonly int OnAnn = ++id;
        public static readonly int OnAnnHori = ++id;
    }
}
