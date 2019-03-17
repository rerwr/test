using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Framework;
using UnityEngine;

namespace Game
{
    class CommonController : BaseController<CommonController>
    {
        protected override Type GetEventType()
        {
            return typeof(CommonEvent);
        }

        public override void InitController()
        {
           
            
            _Proxy = new NetProxy(NetModules.GameAction.ModuleId);
            //  3-15

            _Proxy.AddNetListenner(NetModules.GameAction.Farm_Game_ScanQRcodeOrRecommand_Req, AnwScanQRcodeOrRecommandCallBack);
            //  3-13
            SocketParser.Instance.RegisterParser(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_DogUpgrad_Req,
                Farm_Game_DogUpgrad_Req.ParseFrom);
            _Proxy.AddNetListenner(NetModules.GameAction.Farm_Game_DogUpgrad_Req, PersonInfoCallBack);

            //3-18
            
            SocketParser.Instance.RegisterParser(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_CoinOrExpChange_Anw, Farm_Game_CoinOrExpChange_Anw.ParseFrom);
            _Proxy.AddNetListenner(NetModules.GameAction.Farm_Game_CoinOrExpChange_Anw, ReFlashCoin);
        }

        private void PersonInfoCallBack(MsgRec msg)
        {
            var builder = (Farm_Game_DogUpgrad_Req)msg._proto;
            LoginModel.Instance.Lv = builder.DogLevel;
            LoginModel.Instance.Exp = builder.CurrentEXP;
            LoginModel.Instance.LevelMaxExp = builder.MaxEXP;
            GetDispatcher().Dispatch(CommonEvent.OnPersonReflash);
        }

        private void ReFlashCoin(MsgRec msg)
        {
           var info=(Farm_Game_CoinOrExpChange_Anw) msg._proto;
            LoginModel loginModel=LoginModel.Instance;
            loginModel.Gold=info.Coin;
//            loginModel.LevelMaxExp=info.MaxEXP;
            loginModel.Exp = info.EXP;
            loginModel.Lv = info.LV;
            GlobalDispatcher.Instance.Dispatch(GlobalEvent.onPlayerPanelReflash);
        }
       

        private void AnwScanQRcodeOrRecommandCallBack(MsgRec msgRec)
        {

        }
        //1为扫描二维码，2为邀请码
        public void SendScanQRcodeOrRecommandReq(string info,int pattern)
        {
            if (pattern==1)
            {
                FieldsController.ProtocalAction = ProtocalAction.QRcode;
            }
            var builder = Farm_Game_ScanQRcodeOrRecommand_Req.CreateBuilder();
            builder.Info = info;
            builder.Pattern = pattern;
            builder.UserGameID = LoginModel.Instance.Uid;
            _Proxy.SendMsg(NetModules.GameAction.ModuleId,NetModules.GameAction.Farm_Game_ScanQRcodeOrRecommand_Req,builder);

        }
        

        public class CommonEvent : EventDispatcher.BaseEvent
        {
            public static readonly int OnPersonReflash = ++id;

        }
    }
}
