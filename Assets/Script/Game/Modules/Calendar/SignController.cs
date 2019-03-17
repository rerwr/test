using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;

namespace Game
{
    public class SignController:BaseController<SignController>
    {
        protected override Type GetEventType()
        {
            return typeof(SignControllerEvent);
        }

        public override void InitController()
        {
            //  标识: module =3 ,sub = 12
            _Proxy = new NetProxy(NetModules.FarmGameProtocal.ModuleId);
            SocketParser.Instance.RegisterParser(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_Sign_Req, Farm_Game_Sign_Awn.ParseFrom);
            _Proxy.AddNetListenner(NetModules.GameAction.Farm_Game_Sign_Req, SignCallBack);

        }

     
        private void SignCallBack(MsgRec msg)
        {
            
        }

        public void SignReq()
        {
            FieldsController.ProtocalAction = ProtocalAction.Sign;
            var builder = Farm_Game_Sign_Req.CreateBuilder();
            builder.UserGameID = LoginModel.Instance.Uid;
            _Proxy.SendMsg(NetModules.GameAction.ModuleId,NetModules.GameAction.Farm_Game_Sign_Req,builder);
            
        }

        public class SignControllerEvent : EventDispatcher.BaseEvent
        {
            
        }
    }
}
