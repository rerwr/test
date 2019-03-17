using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;

namespace Game
{
    class NewPlayerCreateController:BaseController<NewPlayerCreateController>
    {
        private string name;
        protected override Type GetEventType()
        {
            return typeof(CreateEvent);
        }

        public override void InitController()
        {
            //  标识: module = 2 ,sub = 3
            _Proxy = new NetProxy(NetModules.FarmGameProtocal.ModuleId);
            SocketParser.Instance.RegisterParser(NetModules.FarmGameProtocal.ModuleId, NetModules.FarmGameProtocal.Farm_Game_NewUser_Req, Farm_Game_NewUser_Anw.ParseFrom);
            _Proxy.AddNetListenner(NetModules.FarmGameProtocal.Farm_Game_NewUser_Req, AnwCallBack);

        }

        protected void AnwCallBack(MsgRec msgRec)
        {
            Farm_Game_NewUser_Anw msgRecPro = (Farm_Game_NewUser_Anw)msgRec._proto;
            LoginModel.Instance.Uid = msgRecPro.UserGameID;
            LoginModel.Instance.Nickname = name;
            GetDispatcher().Dispatch(CreateEvent.OnCreateSucc);
        }

        public void CreateNewUserReq(string gameName,int sex,string headIcon)
        {
            name=gameName;
            Farm_Game_NewUser_Req.Builder builder = Farm_Game_NewUser_Req.CreateBuilder();
            builder.GameName = gameName;
            builder.Sex = sex;
            builder.HeaderIcon = headIcon;
            _Proxy.SendMsg(NetModules.FarmGameProtocal.ModuleId,NetModules.FarmGameProtocal.Farm_Game_NewUser_Req,builder);
        }

        
    }
    public class CreateEvent : EventDispatcher.BaseEvent
    {
        public static readonly int OnCreateSucc = ++id;
    }
}
