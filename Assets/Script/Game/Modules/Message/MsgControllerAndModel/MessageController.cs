using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using System;

namespace Game
{
    public class MessageController : BaseController<MessageController>
    {
        public int MsgCount=0;

        protected override Type GetEventType()
        {
            return typeof(MessageEvent);
        }

        public override void InitController()
        {
            //3-19  请求消息列表
            _Proxy = new NetProxy(NetModules.GameAction.ModuleId);
            SocketParser.Instance.RegisterParser(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_MessageSend_Req, Farm_Game_MessageSend_Anw.ParseFrom);//注册对应消息（在服务器发过来时）的Proto解析器
            _Proxy.AddNetListenner(NetModules.GameAction.Farm_Game_MessageSend_Req, MsgListCallBack);
         
        }

     
        //请求消息列表回调
        private void MsgListCallBack(MsgRec msg)
        {
            Farm_Game_MessageSend_Anw p = (Farm_Game_MessageSend_Anw)msg._proto;
            if (p != null)
            {
                MessageModel.Instance.SetData(p);
            }
            GetDispatcher().Dispatch(MessageEvent.OnGetMsgList);
        }

        //请求消息列表
        public void MsgListReq()
        {
            var builder = Farm_Game_MessageSend_Req.CreateBuilder();
            builder.UserGameID = LoginModel.Instance.Uid;

            _Proxy.SendMsg(NetModules.GameAction.ModuleId,NetModules.GameAction.Farm_Game_MessageSend_Req,builder);
        }

        //删除已读消息
        public void DelMsg(int msgId)
        {
            var builder = Farm_Game_DeleteMsg_Req.CreateBuilder();
            builder.MsgId = msgId;
            _Proxy.SendMsg(NetModules.GameAction.ModuleId,NetModules.GameAction.Farm_Game_DeleteMsg_Req,builder);
            Dictionary<int, MsgUnit> msgList = MessageModel.Instance.MsgList;
            if (msgList.ContainsKey(msgId))
            {
                msgList.Remove(msgId);
            }
            MessageModel.Instance.MsgList = msgList;
            MsgCount -=1 ;
            GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnMsgChange, MsgCount);

        }

    }

    public class MessageEvent : EventDispatcher.BaseEvent
    {
        public static readonly int OnGetMsgList = ++id;

        public static readonly int OnClickSystemMsg = ++id;
    }
}
