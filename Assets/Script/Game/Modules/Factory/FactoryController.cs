using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;

namespace Game
{
    public class FactoryController:BaseController<FactoryController>
    {
        public int pattern = 1; //当前打开的窗口：1为初级工厂，2为高级工厂

        protected override Type GetEventType()
        {
            return typeof(FactoryControllerEvent);
        }

        public override void InitController()
        {
            //  3-10
            _Proxy = new NetProxy(NetModules.GameAction.ModuleId);
            SocketParser.Instance.RegisterParser(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_OilUpgrade_Req, Farm_Game_OilUpgrade_Anw.ParseFrom);
            _Proxy.AddNetListenner(NetModules.GameAction.Farm_Game_OilUpgrade_Req, OilUpgradeCallBack);

           
        }




     


        private void OilUpgradeCallBack(MsgRec msg)
        {
            FieldsController.ProtocalAction =ProtocalAction.None;

            Farm_Game_OilUpgrade_Anw p=(Farm_Game_OilUpgrade_Anw)msg._proto;

            GetDispatcher().Dispatch(FactoryControllerEvent.OnUpgrade);
        }


        public void OilUpgradeReq(int userId,int produceID,int count, int pattern)
        {
            if (FieldsController.ProtocalAction != ProtocalAction.None) return;
            else
            {
                FieldsController.ProtocalAction = ProtocalAction.Produce;
            }

            Farm_Game_OidUpgrade_Req.Builder builder = Farm_Game_OidUpgrade_Req.CreateBuilder();
            builder.UserGameID = userId;
            builder.ProduceID = produceID;
            builder.Number = count;
            builder.Pattern = pattern;

            _Proxy.SendMsg(NetModules.GameAction.ModuleId,NetModules.GameAction.Farm_Game_OilUpgrade_Req,builder);
            GetDispatcher().Dispatch(FactoryControllerEvent.OnUpgrading, produceID);
        }
        
    }

    public class FactoryControllerEvent : EventDispatcher.BaseEvent
    {
        public static readonly int OnUpgrading = ++id;
        public static readonly int OnUpgrade = ++id;

    }
}
