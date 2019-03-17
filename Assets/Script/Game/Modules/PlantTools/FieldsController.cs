using System;
using System.Collections;
using System.Collections.Generic;

using Framework;
using UnityEngine;

namespace Game
{
  
    public enum GameAction
    {
        /// <summary>
        /// 无操作
        /// </summary>
        None,
        /// <summary>
        /// 浇水
        /// </summary>
        Water,
        /// <summary>
        /// 施肥
        /// </summary>
        Fertitlize,
        /// <summary>
        /// 除虫
        /// </summary>
        Debug,
        /// <summary>
        /// 除草
        /// </summary>
        weed,
        /// <summary>
        /// 翻地
        /// </summary>
        Plow,
        
    }
    //表示当前正在进行的协议发送，当没有协议操作时，才可以进行下一个协议操作
    public enum ProtocalAction
    {
        /// <summary>
        /// 无操作
        /// </summary>
        None,
        /// <summary>
        /// 浇水
        /// </summary>
        Water,
        /// <summary>
        /// 施肥
        /// </summary>
        Fertitlize,
        /// <summary>
        /// 除虫
        /// </summary>
        Debug,
        /// <summary>
        /// 除草
        /// </summary>
        weed,
        /// <summary>
        /// 翻地
        /// </summary>
        Plow,

        OneKey,//一键操作

        GetOne,//得到一个

        Seed,//播种操作
        RelashMap,//地图更新
        ReClaim,//开垦
        
        ReqStoreInfo,//请求背包信息
        Sell,//卖出物品
        Buy,//购买物品
        Produce,//合成物品
        Exchange,//兑换物品
        Shop,//商店信息
        Sign,//签到
        ReflashFields,//刷新土地
        QRcode,//二维码
    }
    public class FieldsController : BaseController<FieldsController>
    {
        public static ProtocalAction ProtocalAction=ProtocalAction.None;
        public int currentFarmID ;
        
        protected override Type GetEventType()
        {
            return typeof(FieldsEvent);
        }

        public override void InitController()
        {
            // 2-2
            _Proxy = new NetProxy(NetModules.FarmGameProtocal.ModuleId);
            SocketParser.Instance.RegisterParser(NetModules.FarmGameProtocal.ModuleId, NetModules.FarmGameProtocal.Farm_Game_MapInfo_Req, Farm_Game_MapInfo_Anw.ParseFrom);
            _Proxy.AddNetListenner(NetModules.FarmGameProtocal.Farm_Game_MapInfo_Req, AnwFieldsCallBack);

            //          SocketParser.Instance.RegisterParser(NetModules.FarmGameProtocal.ModuleId, NetModules.FarmGameProtocal.Farm_Game_FarmLand_Req, Farm_Game_FarmLand_Anw.ParseFrom);
            //          _Proxy.AddNetListenner(NetModules.FarmGameProtocal.Farm_Game_FarmLand_Req, AnwFarmLandCallBack);

           
            //3-5
            _Proxy = new NetProxy(NetModules.GameAction.ModuleId);
            SocketParser.Instance.RegisterParser(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_Action_Anw, Farm_Game_Action_Anw.ParseFrom);

            _Proxy.AddNetListenner(NetModules.GameAction.Farm_Game_Action_Anw, AnwPlantActionCallBack);
            
            //3-1
            SocketParser.Instance.RegisterParser(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_Action_Fandi_Req, Farm_Game_Action_Fandi_Anw.ParseFrom);
            _Proxy.AddNetListenner(NetModules.GameAction.Farm_Game_Action_Fandi_Req, AnwFanDiCallBack);
            
            //3-4
            SocketParser.Instance.RegisterParser(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_Action_Pluck_Req, Farm_Game_Action_Pluck_Anw.ParseFrom);
            _Proxy.AddNetListenner(NetModules.GameAction.Farm_Game_Action_Pluck_Req, AnwPluckCallBack);
          
            //3-2
            SocketParser.Instance.RegisterParser(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_Seed_Req, Farm_Game_Seed_Anw.ParseFrom);
            _Proxy.AddNetListenner(NetModules.GameAction.Farm_Game_Seed_Req, AnwSeedCallBack);
           
            //3-3
            SocketParser.Instance.RegisterParser(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_Action_WF_Req, Farm_Game_Action_WF_Anw.ParseFrom);
            _Proxy.AddNetListenner(NetModules.GameAction.Farm_Game_Action_WF_Req, AnwWFCallBack);
     
            //3-14
            SocketParser.Instance.RegisterParser(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_ReClaim_Req, Farm_Game_ReClaim_Anw.ParseFrom);
            _Proxy.AddNetListenner(NetModules.GameAction.Farm_Game_ReClaim_Req, AnwReClaimCallBack);

            //收获到的植物
            GlobalDispatcher.Instance.AddListener(GlobalEvent.OnStoreUnitsChange, PluckAndGet);


           
        }
   
        /// 地图更新发送2-2
        protected void AnwFieldsCallBack(MsgRec msgRec)
        {
            //            Debug.LogError("hahah");
            Farm_Game_MapInfo_Anw farmGameMapInfoAnw = (Farm_Game_MapInfo_Anw)msgRec._proto;

            FieldsModel.Instance.SetData(farmGameMapInfoAnw.MapArrayList);
            GetDispatcher().Dispatch(FieldsEvent.FieldAllRefresh,true);
            FieldsController.ProtocalAction = ProtocalAction.None;

        }
        /// <summary>
        /// 地图更新发送2-2
        /// </summary>
        public void SendFieldsReflashAction()
        {
            //            Debug.Log("reflash");
            Farm_Game_MapInfo_Req.Builder builder = Farm_Game_MapInfo_Req.CreateBuilder();
            builder.UserGameID = LoginModel.Instance.Uid;
            _Proxy.SendMsg(NetModules.FarmGameProtocal.ModuleId, NetModules.FarmGameProtocal.Farm_Game_MapInfo_Req, builder);
            FieldsController.ProtocalAction = ProtocalAction.RelashMap;
        }

        //  标识: module = 3,sub = 5
        protected void AnwPlantActionCallBack(MsgRec msgRec)
        {
            Farm_Game_Action_Anw msgRecPro = (Farm_Game_Action_Anw)msgRec._proto;
            FarmUnit farm;
            if (FieldsModel.Instance.farms.TryGetValue(msgRecPro.FieldsID, out farm))
            {
                if (farm.Plant!=null)
                {
                    switch (msgRecPro.ActionID)
                    {
                        case 1:
                            //状态1才能浇水
                            //                        if (farm.Plant.CurrentType<=1)
                        {
                            farm.Plant.IsWater = 1;

                        }
                            break;
                        case 2:
                            farm.Plant.IsWorm = 1;
                            break;
                        case 3:
                            farm.Plant.IsGrass = 1;

                            break;
                    }
                }
         
            }
            
            FieldsController.ProtocalAction = ProtocalAction.None;

        }
        
        //3-1翻地

        protected void AnwFanDiCallBack(MsgRec msgRec)
        {
            Farm_Game_Action_Fandi_Anw msgRecPro = (Farm_Game_Action_Fandi_Anw)msgRec._proto;
            FieldsModel.Instance.farms[msgRecPro.FieldsID].RemovePlant();
            FieldsController.ProtocalAction = ProtocalAction.None;

        }

        public void ReqSenPlowdAction(int FieldsID)
        {
            var builder = Farm_Game_Action_Fandi_Req.CreateBuilder();
            builder.UserGameID = LoginModel.Instance.Uid;
            builder.FieldsID = FieldsID;
            _Proxy.SendMsg(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_Action_Fandi_Req, builder);
            FieldsController.ProtocalAction = ProtocalAction.Plow;

        }

       
        //返回收获数量和返回成功的顺序不一定
        private bool  PluckAndGet(int  id, object arg)
        {
            List<DeltaStoreUnit> ld = arg as List<DeltaStoreUnit>;

            if (FieldsController.ProtocalAction==ProtocalAction.GetOne&&arg!=null)
            {
                if (ld.Count>0&&currentFarmID>0)
                {
                    Dictionary<int, FarmUnit> farms = FieldsModel.Instance.farms;
                    
                    farms[currentFarmID].PluckPlant(ld[0].Deltacount1);
                    //当已经摘完了 刷新一次土地
                   
                }

            }
            FieldsController.ProtocalAction = ProtocalAction.None;
            return false;
        }
        //3-4一键收获
        protected void AnwPluckCallBack(MsgRec msgRec)
        {
            FieldsController.ProtocalAction = ProtocalAction.None;

        }

        public void SendPluckReq(int FieldsID, int visitID)
        {
            //ID
            if (FieldsID==0)
            {
                return;
            }
            currentFarmID = FieldsID;
            FieldsController.ProtocalAction = ProtocalAction.GetOne;

//            ids.Enqueue(FieldsID);
            
            Farm_Game_Action_Pluck_Req.Builder builder = Farm_Game_Action_Pluck_Req.CreateBuilder();
            builder.UserGameID = LoginModel.Instance.Uid;
            builder.FieldsID = FieldsID;
            builder.VisitedGameID = visitID;
            _Proxy.SendMsg(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_Action_Pluck_Req, builder);

        }

        //  标识: 3-2
        protected void AnwSeedCallBack(MsgRec msgRec)
        {
            Farm_Game_Seed_Anw msgRecPro = (Farm_Game_Seed_Anw)msgRec._proto;
            SeedActionModel.Instance.SetData(msgRecPro);
            FieldsController.ProtocalAction = ProtocalAction.None;

        }
        /// <summary>
        /// 请求播种
        /// </summary>
        public void ReqSeedAction(int FieldsID, int seedID)
        {
            Farm_Game_Seed_Req.Builder builder = new Farm_Game_Seed_Req.Builder();
            builder.UserGameID = LoginModel.Instance.Uid;
            builder.FieldsID = FieldsID;
            builder.SeedID = seedID;
            _Proxy.SendMsg(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_Seed_Req, builder);
            FieldsController.ProtocalAction = ProtocalAction.Seed;

        }

        protected void AnwReClaimCallBack(MsgRec msgRec)
        {
            var anw = (Farm_Game_ReClaim_Anw)msgRec._proto;
            FieldsModel.Instance.farms[anw.FieldsID].EnablePlant = 1;
            //开垦版放在下一个
            Brand.Instance.SelectId = anw.FieldsID + 1;
            (ViewMgr.Instance.views[ViewNames.PlowNeedView].Viewlist[0] as PlowView).CloseView();
            (ViewMgr.Instance.views[ViewNames.PlowNeedView].Viewlist[1] as PlowSuccessView).ShowView();
            FieldsController.ProtocalAction = ProtocalAction.None;

        }
        //3-14农田开垦请求

        public void SendReclaimReq(int id)
        {
            var builder = Farm_Game_ReClaim_Req.CreateBuilder();
            builder.FieldsID = id;
            builder.UserGameID = LoginModel.Instance.Uid;
            _Proxy.SendMsg(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_ReClaim_Req, builder);
            FieldsController.ProtocalAction = ProtocalAction.ReClaim;

        }


        //3-3
        protected void AnwWFCallBack(MsgRec msgRec)
        {
            
            switch (FieldsController.ProtocalAction)
            {
                case ProtocalAction.Debug:
                    FieldsModel.Instance.farms[FarmUnit.SeletedFarmID].Plant.IsWorm=0;

                    Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "debug", "test1"));

                    break;
                case ProtocalAction.Fertitlize:

                    Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "Fertitlize", "test1"));
                    FieldsModel.Instance.farms[FarmUnit.SeletedFarmID].Plant.GrothTime-=3600;

                    break;
                case ProtocalAction.weed:
                    FieldsModel.Instance.farms[FarmUnit.SeletedFarmID].Plant.IsGrass = 0;
                    Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "weed", "test1"));


                    break;
                case ProtocalAction.Water:
                    FieldsModel.Instance.farms[FarmUnit.SeletedFarmID].Plant.IsWater = 0;
                    Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "Water", "test1"));

                    break;;
                default:

                    break;
            }
            FieldsController.ProtocalAction = ProtocalAction.None;

        }

        public void SendWFActionReq(int FieldsID, GameAction action, int FertilizersID)
        {
            Farm_Game_Action_WF_Req.Builder builder = Farm_Game_Action_WF_Req.CreateBuilder();
            builder.UserGameID = LoginModel.Instance.Uid;
            builder.ActionID = (int)action;
            builder.FieldsID = FieldsID;
            builder.FertilizersID = FertilizersID;
            _Proxy.SendMsg(NetModules.GameAction.ModuleId, NetModules.GameAction.Farm_Game_Action_WF_Req, builder);
        }
        
    }

    public class FieldsEvent : EventDispatcher.BaseEvent
    {
        public static readonly int FieldAllRefresh = ++id;
        public static readonly int FieldRefreshList = ++id;
        /// <summary>
        /// 长草
        /// </summary>
        public static readonly int BadAction = ++id;

    }

}
