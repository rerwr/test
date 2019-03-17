using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using System;

namespace Game
{
    public class DogInfoController : BaseController<DogInfoController>
    {
        protected override Type GetEventType()
        {
            return typeof(DogInfoEvent);
        }

        public override void InitController()
        {
            //3-24 喂狗
            _Proxy = new NetProxy(NetModules.GameAction.ModuleId);
            SocketParser.Instance.RegisterParser(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_FeedDog_Req, Farm_Game_FeedDog_Anw.ParseFrom);
            _Proxy.AddNetListenner(NetModules.GameAction.Farm_Game_FeedDog_Req, FeedDogCallBack);

        }

        private void FeedDogCallBack(MsgRec msg)
        {
            Farm_Game_FeedDog_Anw p = (Farm_Game_FeedDog_Anw)msg._proto;
            if (p != null)
            {
                LoginModel.Instance.DogUpgradeMaxExp = p.DogUpgradeMaxEXP;
                LoginModel.Instance.DogCurrentExp = p.DogCurrentEXP;
                LoginModel.Instance.DogLv = p.DogLv;
                LoginModel.Instance.Chance = p.Chance;

            }
            GetDispatcher().Dispatch(DogInfoEvent.OnDogChange);
        }

        public void FeedDog()
        {
            var builder = Farm_Game_FeedDog_Req.CreateBuilder();
            builder.UserGameID = LoginModel.Instance.Uid;
            _Proxy.SendMsg(NetModules.GameAction.ModuleId,NetModules.GameAction.Farm_Game_FeedDog_Req,builder);
        }
    }

    public class DogInfoEvent : EventDispatcher.BaseEvent
    {
        public static readonly int OnDogChange = ++id;
    }
}
