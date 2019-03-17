using System;

using Framework;
using UnityEngine;

namespace Game
{
    public class FriendsInfoController:BaseController<FriendsInfoController>
    {
		public int CurrentID;
        public int currentPage=1;

        protected override Type GetEventType()
        {
            return typeof(FriendsInfoEvent);
        }

        public override void InitController()
        {
            //2-4
            _Proxy = new NetProxy(NetModules.FarmGameProtocal.ModuleId);
            SocketParser.Instance.RegisterParser(NetModules.FarmGameProtocal.ModuleId, NetModules.FarmGameProtocal.Farm_Game_RankInfo_Req, Farm_Game_RankInfo_Anw.ParseFrom);
            _Proxy.AddNetListenner(NetModules.FarmGameProtocal.Farm_Game_RankInfo_Req, AnwRankListCallBack);

            //2-6
            _Proxy = new NetProxy(NetModules.FarmGameProtocal.ModuleId);
          SocketParser.Instance.RegisterParser(NetModules.FarmGameProtocal.ModuleId,NetModules.FarmGameProtocal.Farm_Game_FriendsInfo_Req,Farm_Game_FriendsInfo_Anw.ParseFrom);
			_Proxy.AddNetListenner(NetModules.FarmGameProtocal.Farm_Game_FriendsInfo_Req, OnFriendListCallBack);
            
            //  标识: module = 3,sub = 9
            _Proxy = new NetProxy(NetModules.GameAction.ModuleId);
            SocketParser.Instance.RegisterParser(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_SingleFriendInfo_Req, Farm_Game_SingleFriendInfo_Anw.ParseFrom);
            _Proxy.AddNetListenner(NetModules.GameAction.Farm_Game_SingleFriendInfo_Req, AnwFriendCallBack);
            // 标识: module = 3,sub = 11
            _Proxy.AddNetListenner(NetModules.GameAction.Farm_Game_AddFriend_Req, AnwFriendsCallBack);

            //3-21 搜索玩家
            _Proxy = new NetProxy(NetModules.GameAction.ModuleId);
            SocketParser.Instance.RegisterParser(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_SearchFriend_Req, Farm_Game_SearchFriend_Anw.ParseFrom);
            _Proxy.AddNetListenner(NetModules.GameAction.Farm_Game_SearchFriend_Req, SearchCallBack);


        }


        //3-11
        protected  void AnwFriendsCallBack(MsgRec msgRec)
        {
           
            SystemMsgView.SystemFunction(Function.Tip,Info.AddFriendSucc,2);
        }
		//2-6
        public void SendFriendsInfoReq(int currentPage)
        {
            var builder = Farm_Game_FriendsInfo_Req.CreateBuilder();
            builder.UserGameID = LoginModel.Instance.Uid;
            builder.CurrentPage = currentPage;
            _Proxy.SendMsg(NetModules.FarmGameProtocal.ModuleId,NetModules.FarmGameProtocal.Farm_Game_FriendsInfo_Req,builder);
        }

        private void OnFriendListCallBack(MsgRec msg)
        {
            Farm_Game_FriendsInfo_Anw p = (Farm_Game_FriendsInfo_Anw)msg._proto;
            if (p != null)
            {
                FriendsInfoModel.Instance.SetData(p);
                GetDispatcher().Dispatch(FriendsInfoEvent.OnReqFriendsInfo);
            }
         
        }

        //2-4
        /// <summary>
        /// 排行榜列表
        /// </summary>
        /// <param name="msgRec"></param>
        protected void AnwRankListCallBack(MsgRec msgRec)
        {
            Farm_Game_RankInfo_Anw msgRecPro = (Farm_Game_RankInfo_Anw)msgRec._proto;
            RankInfoModel.Instance.SetData(msgRecPro);

            GetDispatcher().Dispatch(FriendsInfoEvent.OnReqRankingList);
            FieldsController.ProtocalAction = ProtocalAction.None;

        }

        public void RankListReq(int userId,int currentPage)
        {
            var builder = Farm_Game_RankInfo_Req.CreateBuilder();
            builder.UserGameID = userId;
            builder.CurrentPage = currentPage;
            _Proxy.SendMsg(NetModules.FarmGameProtocal.ModuleId,NetModules.FarmGameProtocal.Farm_Game_RankInfo_Req,builder);
        }

        //3-9
        protected void AnwFriendCallBack(MsgRec msgRec)
        {
            Farm_Game_SingleFriendInfo_Anw msgRecPro = (Farm_Game_SingleFriendInfo_Anw)msgRec._proto;
            if (msgRecPro != null)
            {
                SingeFriendInfoModel.Instance.SetData(msgRecPro);
            }
            if (FieldsController.ProtocalAction==ProtocalAction.ReflashFields)
            {

            }
            else
            {
                GetDispatcher().Dispatch(FriendsInfoEvent.OnVisitedFriend);

            }
            FieldsController.ProtocalAction = ProtocalAction.None;
            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "visiting", "test1"));
                
        }

        public void SendSingleFriendInfoReq(int VisitedID)
        {
            var builder = Farm_Game_SingleFriendInfo_Req.CreateBuilder();
            builder.UserGameID = LoginModel.Instance.Uid;
            builder.VisitedGameID = VisitedID;
            _Proxy.SendMsg(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_SingleFriendInfo_Req, builder);
        }

        //3-11 请求添加好友
        public void AddFriend(int uid, int friendId)
        {
            var builder = Farm_Game_AddFriend_Req.CreateBuilder();
            builder.AddGameID = friendId;
            builder.UserGameID = uid;
            _Proxy.SendMsg(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_AddFriend_Req, builder);
        }

        //3-20 同意添加好友
        public void AgreeAddFriend(int uid,int friendId)
        {
            var builder = Farm_Game_AgreeAdd_Req.CreateBuilder();
            builder.AddGameID = friendId;
            builder.UserGameID = uid;
            _Proxy.SendMsg(NetModules.GameAction.ModuleId,NetModules.GameAction.Farm_Game_AgreeAdd_Req,builder);

            GetDispatcher().Dispatch(FriendsInfoEvent.onAgreeFriend);
        }

        //3-21 请求搜索玩家
        public void Search(string name)
        {
            var builder = Farm_Game_SearchFriend_Req.CreateBuilder();
            builder.Name = name;
            _Proxy.SendMsg(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_SearchFriend_Req, builder);
        }

        private void SearchCallBack(MsgRec msg)
        {    
            Farm_Game_SearchFriend_Anw anw= (Farm_Game_SearchFriend_Anw)msg._proto;
            if (anw == null)
            {
                
            }
            else
            {
                SearchInfoModel.Instance.SetData(anw);
            }
            GetDispatcher().Dispatch(FriendsInfoEvent.OnReqSearchList);
        }
    }
    public class FriendsInfoEvent : EventDispatcher.BaseEvent
    {
        public static readonly int OnReqFriendsInfo = ++id;
        public static readonly int OnNoFrendsInfo = ++id;
        public static readonly int OnReqRankingList = ++id;
        public static readonly int OnReqSearchList = ++id;
        public static readonly int onAgreeFriend = ++id;
        public static readonly int OnVisitedFriend = ++id;
        public static readonly int Gohome = ++id;
    }
}