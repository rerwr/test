using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;

namespace Game
{
    public class PayActionController:BaseController<PayActionController>
    {
        protected override Type GetEventType()
        {
            return typeof(PayActionEvent);
        }

        public override void InitController()
        {
            //  标识: module = 3,sub = 8
            _Proxy = new NetProxy(NetModules.FarmGameProtocal.ModuleId);
            SocketParser.Instance.RegisterParser(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_pay_Req, Farm_Game_pay_Anw.ParseFrom);
            _Proxy.AddNetListenner(NetModules.GameAction.Farm_Game_pay_Req, AnwCallBack);
        }

        protected  void AnwCallBack(MsgRec msgRec)
        {
            Farm_Game_pay_Anw msgRecPro = (Farm_Game_pay_Anw) msgRec._proto;
            PayAtionModel.Instance.SetData(msgRecPro);
        }

        public void SendPayReq()
        {
            //TODO
        }

        public class PayActionEvent:EventDispatcher.BaseEvent
        {
            
        }
    }
}
