using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;

namespace Game
{
    public class ChantController:BaseController<ChantController>
    {
        protected override Type GetEventType()
        {
            return typeof(ChantControllerEvent);
        }

        public override void InitController()
        {
            _Proxy = new NetProxy(NetModules.GameAction.ModuleId);
            //module =3 ,sub = 17
            SocketParser.Instance.RegisterParser(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_Chant_Req, Farm_Game_Chat_Anw.ParseFrom);            

            //3-22 请求聊天记录
            _Proxy = new NetProxy(NetModules.GameAction.ModuleId);
            SocketParser.Instance.RegisterParser(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_ChatLog_Req, Farm_Game_ChatLog_Anw.ParseFrom);
            _Proxy.AddNetListenner(NetModules.GameAction.Farm_Game_ChatLog_Req, ChatLogCallBack);
        }

        //聊天记录回调
        private void ChatLogCallBack(MsgRec msg)
        {
            Farm_Game_ChatLog_Anw p = (Farm_Game_ChatLog_Anw)msg._proto;
            if (p!=null)
            {
                ChatModel.Instance.SetData(p);
                ChatModel.Instance.currentPage++;
            }
            GetDispatcher().Dispatch(ChantControllerEvent.OnReciveLog);
        }

        //发送聊天信息
        public void SendContent(string content)
        {
            var builder = Farm_Game_Chant_Req.CreateBuilder();
            builder.UserGameID = LoginModel.Instance.Uid;
            builder.VisitedGameID = ChatModel.Instance.ChatTarget.UserGameId;
            builder.Content = content;
            _Proxy.SendMsg(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_Chant_Req,builder);
        }

        //请求聊天记录(此id为对方的id)
        public void ReqChatLog(int page)
        {
            var builder = Farm_Game_ChatLog_Req.CreateBuilder();
            builder.FriendID = ChatModel.Instance.ChatTarget.UserGameId;
            builder.Page = page;
            _Proxy.SendMsg(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_ChatLog_Req, builder);
            
        }
    }
    public class ChantControllerEvent : EventDispatcher.BaseEvent
    {
        public static readonly int OnReciveContent = ++id;
        public static readonly int OnReciveLog = ++id;
        public static readonly int OnChat = ++id;
    }
}
