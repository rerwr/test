using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using Game;
using System;

namespace Game
{
    public class DeltaStoreUnit
    {
        private int id;
        private int Deltacount;//b

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int Deltacount1
        {
            get { return Deltacount; }
            set { Deltacount = value; }
        }
    }
    public class StoreController : BaseController<StoreController>
    {
        public int currentSellID;//当前正在卖出的物品id
        public int currentSellNumber;//当前卖出的数量

        protected override Type GetEventType()
        {
            return typeof(StoreEvent);
        }

        public override void InitController()
        {
            //2-5
            _Proxy = new NetProxy(NetModules.FarmGameProtocal.ModuleId);
            SocketParser.Instance.RegisterParser(NetModules.FarmGameProtocal.ModuleId, NetModules.FarmGameProtocal.Farm_Game_StoreInfo_Req, Farm_Game_StoreInfo_Anw.ParseFrom);//注册对应消息（在服务器发过来时）的Proto解析器
            _Proxy.AddNetListenner(NetModules.FarmGameProtocal.Farm_Game_StoreInfo_Req, StoreInfoCallBack);

            //2-9
            _Proxy = new NetProxy(NetModules.FarmGameProtocal.ModuleId);
            SocketParser.Instance.RegisterParser(NetModules.FarmGameProtocal.ModuleId, NetModules.FarmGameProtocal.Farm_Game_Store_Update, Farm_Game_Store_Update.ParseFrom);//注册对应消息（在服务器发过来时）的Proto解析器
            _Proxy.AddNetListenner(NetModules.FarmGameProtocal.Farm_Game_Store_Update, StoreUpdateCallBack);

            //3-6
            _Proxy = new NetProxy(NetModules.GameAction.ModuleId);
            SocketParser.Instance.RegisterParser(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_buyOrSell_Req, Farm_Game_buyOrSell_Anw.ParseFrom);//注册对应消息（在服务器发过来时）的Proto解析器
            //_Proxy.AddNetListenner(NetModules.GameAction.Farm_Game_buyOrSell_Req, StoreSellCallBack);

        }
        private List<DeltaStoreUnit> GetDeltaData(IList<PMsg_StagePropUnit> list)
        {
            if (list.Count > 0)
            {
                List<DeltaStoreUnit> units = new List<DeltaStoreUnit>();
                for (int i = 0; i < list.Count; i++)
                {
                    //原始数据
                    BaseObject bo = Farm_Game_StoreInfoModel.Instance.GetData(list[i].Id);
                    if (bo != null)
                    {
                       
                            int deltaNum = list[i].Count - bo.ObjectNum;
                            DeltaStoreUnit deltaStoreUnit = new DeltaStoreUnit();
                            deltaStoreUnit.Id = list[i].Id;
                            deltaStoreUnit.Deltacount1 = deltaNum;
                            
                            units.Add(deltaStoreUnit);
//                        Debug.LogError(string.Format("<color=#ffffffff><---{0}-{1}----></color>", bo.Name, deltaNum));

                    }


                }
                return units;

            }
            return null;

        }
 

        //仓库信息回调2-9
        private void StoreUpdateCallBack(MsgRec msg)
        {

            Farm_Game_Store_Update p = (Farm_Game_Store_Update)msg._proto;
            if (p != null)
            {
                List<DeltaStoreUnit> ds=GetDeltaData(p.ObjectsList);
                Farm_Game_StoreInfoModel.Instance.SetData(p.ObjectsList);
             
                GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnStoreUnitsChange, ds);

            }
            
            GetDispatcher().Dispatch(StoreEvent.OnStoreUnits);

            FieldsController.ProtocalAction = ProtocalAction.None;

        }

        //仓库卖出动作回调3-6
        private void StoreSellCallBack(MsgRec msg)
        {
            FieldsController.ProtocalAction =ProtocalAction.None;

            Farm_Game_buyOrSell_Anw p = (Farm_Game_buyOrSell_Anw)msg._proto;
            BuyOrSellAtionModel.Instance.SetData(p);
            GetDispatcher().Dispatch(StoreEvent.OnSellSucc);
        }

        //仓库信息回调2-5
        private void StoreInfoCallBack(MsgRec msg)
        {
            Farm_Game_StoreInfo_Anw p = (Farm_Game_StoreInfo_Anw)msg._proto;

            if (p != null)
            {
                List<DeltaStoreUnit> ds= GetDeltaData(p.SuArrayList);
                Farm_Game_StoreInfoModel.Instance.SetData(p.SuArrayList);
                GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnStoreUnitsChange, ds);

            }
            //             GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnItemChange);
            GetDispatcher().Dispatch(StoreEvent.OnStoreUnits);
            FieldsController.ProtocalAction = ProtocalAction.None;

        }
        //2-5更新仓库信息
        public void ReqStoreInfo(int userID)
        {
            if (FieldsController.ProtocalAction != ProtocalAction.None) return;
            else
            {
                FieldsController.ProtocalAction = ProtocalAction.ReqStoreInfo;
            }

            Farm_Game_StoreInfo_Req.Builder builder = Farm_Game_StoreInfo_Req.CreateBuilder();
            builder.UserGameID = userID;

            _Proxy.SendMsg(NetModules.FarmGameProtocal.ModuleId, NetModules.FarmGameProtocal.Farm_Game_StoreInfo_Req, builder);
   
        }

        //仓库卖出请求
        public void SellItem(int UserGameID,int ShoppingItemID,int Count,int buyOrSell)
        {
            if (FieldsController.ProtocalAction != ProtocalAction.None) return;
            else
            {
                FieldsController.ProtocalAction = ProtocalAction.Sell;
            }

            Farm_Game_buyOrSell_Req.Builder builder = Farm_Game_buyOrSell_Req.CreateBuilder();
            builder.UserGameID = UserGameID;
            builder.ShoppingItemID = ShoppingItemID;
            builder.Count = Count;
            builder.BuyOrSell = 1;

            NetMsgListener _listener = NetMsgListenerMgr.Instance.GetListener(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_buyOrSell_Req);
            NetMsgListenerMgr.Instance.UnRegisterMsgListener(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_buyOrSell_Req,_listener);
            _Proxy.AddNetListenner(NetModules.GameAction.Farm_Game_buyOrSell_Req, StoreSellCallBack);

            _Proxy.SendMsg(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_buyOrSell_Req, builder);
            
        }

        //3-25卖出全部
        public void TotalSell(int type)
        {
            var builder = Farm_Game_TotalSell_Req.CreateBuilder();
            builder.Type = type;
            _Proxy.SendMsg(NetModules.GameAction.ModuleId,NetModules.GameAction.Farm_Game_TotalSell_Req,builder); 
        }

        //点击选择仓库中的物体
        public void SelectItem(int UserGameID)
        {
            GetDispatcher().Dispatch(StoreEvent.OnSelectItem, UserGameID);
        }

    }

    public class StoreEvent : EventDispatcher.BaseEvent
    {
        public static readonly int OnStoreUnits = ++id;
        public static readonly int OnRemoveUnits = ++id;
        public static readonly int OnSelectItem = ++id;
        public static readonly int OnSellSucc = ++id;
    }
}
