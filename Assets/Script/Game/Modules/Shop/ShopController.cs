using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;

namespace Game
{
    class ShopController:BaseController<ShopController>
    {
        public int Model = 1;   //1为果实，2为肥料，3为狗粮

        protected override Type GetEventType()
        {
            return typeof(ShopEvent);
        }

        public override void InitController()
        {
            //2-7
            _Proxy = new NetProxy(NetModules.FarmGameProtocal.ModuleId);
            SocketParser.Instance.RegisterParser(NetModules.FarmGameProtocal.ModuleId,NetModules.FarmGameProtocal.Farm_Game_ShopInfo_Req,Farm_Game_ShopInfo_Anw.ParseFrom);
            _Proxy.AddNetListenner(NetModules.FarmGameProtocal.Farm_Game_ShopInfo_Req, AnwShopInfoCallBack);
            //3-6
            _Proxy = new NetProxy(NetModules.GameAction.ModuleId);
            SocketParser.Instance.RegisterParser(NetModules.GameAction.ModuleId,NetModules.GameAction.Farm_Game_buyOrSell_Req, Farm_Game_buyOrSell_Anw.ParseFrom);
            _Proxy.AddNetListenner(NetModules.GameAction.Farm_Game_buyOrSell_Req, AnwBuyOrSellCallBack);

        }

        //商店信息2-7

        protected  void AnwShopInfoCallBack(MsgRec msgRec)
        {
            Farm_Game_ShopInfo_Anw msgRecPro = (Farm_Game_ShopInfo_Anw) msgRec._proto;
            ShopModel.Instance.SetData(msgRecPro);
            GetDispatcher().Dispatch(ShopEvent.OnShopItemChanged);
            FieldsController.ProtocalAction = ProtocalAction.None;

        }

        public  void ReqShopSend()
        {
            var builder = Farm_Game_ShopInfo_Req.CreateBuilder();
            builder.UserGameID = LoginModel.Instance.Uid;
            _Proxy.SendMsg(NetModules.FarmGameProtocal.ModuleId,NetModules.FarmGameProtocal.Farm_Game_ShopInfo_Req,builder);
            FieldsController.ProtocalAction = ProtocalAction.Shop;

        }


        //购买or卖出
        //3-6
        protected void AnwBuyOrSellCallBack(MsgRec msgRec)
        {
            FieldsController.ProtocalAction =ProtocalAction.None;

            Farm_Game_buyOrSell_Anw msgRecPro = (Farm_Game_buyOrSell_Anw)msgRec._proto;
            BuyOrSellAtionModel.Instance.SetData(msgRecPro);
        }

        public void SendBuyOrSellReq(int ShoppingItemID, int Count,int buyOrSell)
        {
            if (FieldsController.ProtocalAction != ProtocalAction.None) return;
            else
            {
                FieldsController.ProtocalAction =ProtocalAction.Buy;
            }

            var builder = Farm_Game_buyOrSell_Req.CreateBuilder();
            builder.UserGameID = LoginModel.Instance.Uid;
            builder.ShoppingItemID = ShoppingItemID;
            builder.Count = Count;
            builder.BuyOrSell = buyOrSell;

            NetMsgListener _listener = NetMsgListenerMgr.Instance.GetListener(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_buyOrSell_Req);
            NetMsgListenerMgr.Instance.UnRegisterMsgListener(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_buyOrSell_Req, _listener);
            _Proxy.AddNetListenner(NetModules.GameAction.Farm_Game_buyOrSell_Req, AnwBuyOrSellCallBack);

            _Proxy.SendMsg(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_buyOrSell_Req, builder);

            GetDispatcher().Dispatch(ShopEvent.OnBuying,Count);
        }

        public void OnSelectItem(int ShopItemId)
        {
            GetDispatcher().Dispatch(ShopEvent.OnSelectShopItem, ShopItemId);
        }
    }
    public class ShopEvent:EventDispatcher.BaseEvent
    {
        public static readonly int OnShopItemChanged = ++id;
        public static readonly int OnSelectShopItem = ++id;
        public static readonly int OnBuying = ++id;
        public static readonly int OnBuySucc = ++id;
    }
}
