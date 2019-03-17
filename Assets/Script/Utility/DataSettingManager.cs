using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Framework;
using Game;
using Google.ProtocolBuffers;
using UnityEngine;

namespace Game {
    public class DataSettingManager
    {
		static bool isSeed;
        //仓库信息输入
        public static void SetStoreAnwData(IList<PMsg_StagePropUnit> list)
        {
            StorageDeltaList storageDeltaList = Farm_Game_StoreInfoModel.storage;

            if (list != null)
            {
              
                for (int i = 0; i < list.Count; i++)
                {
                    
                    //移除该id的信息，后面重新输入
                    storageDeltaList.Seeds.Remove(list[i].Id);
                    storageDeltaList.Results.Remove(list[i].Id);
                    storageDeltaList.Oils.Remove(list[i].Id);
                    storageDeltaList.DogFoods.Remove(list[i].Id);
                    storageDeltaList.Fertilizers.Remove(list[i].Id);
                    storageDeltaList.Elixirs.Remove(list[i].Id);
                    storageDeltaList.Formulas.Remove(list[i].Id);
                    //若数量为0，则表示该物品已从仓库移除

                    BaseAtrribute item= LoadObjctDateConfig.Instance.GetAtrribute(list[i].Id);

                    if (list[i].Count == 0)
                    {
                        switch (item.Type)
                        {
                            case ObjectType.Seed:
                                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", list[i].Id, "test1"));

                                storageDeltaList.Seeds.Remove(list[i].Id);
                                Seed seed;
                                if (storageDeltaList.Seeds.TryGetValue(list[i].Id,out seed))
                                {

                                    Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", seed.Name, "test1"));

                                }

                                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "test", "test1"));

                                break;
                            case ObjectType.Result:
                                storageDeltaList.Results.Remove(list[i].Id);
                                break;
                            case ObjectType.SeniorOil:
                            case ObjectType.PrimaryOil:
                            case ObjectType.SemiOil:
                                storageDeltaList.Oils.Remove(list[i].Id);
                                break;
                            case ObjectType.DogFood:
                                storageDeltaList.DogFoods.Remove(list[i].Id);
                                break;
                            case ObjectType.Fertilizer:
                                storageDeltaList.Fertilizers.Remove(list[i].Id);
                                break;
                            case ObjectType.elixir:
                                storageDeltaList.Elixirs.Remove(list[i].Id);
                                break;
                            case ObjectType.formula:
                                storageDeltaList.Formulas.Remove(list[i].Id);
                                break;
                            default:
                                Debug.LogError("后台返回了不该在仓库的东西:" + item.Name);
                                break;
                        }
                       
                    }
                    else
                    {
                        if (item!=null)
                        {
                            switch (item.Type)
                            {
                                case ObjectType.Seed:

                                    storageDeltaList.Seeds.Add(list[i].Id, SetSeedAnwData(list[i]));
                                    //                                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", storageDeltaList.Seeds[list[i].Id].Name, storageDeltaList.Seeds[list[i].Id].ObjectNum));

                                    break;
                                case ObjectType.Result:
                                    storageDeltaList.Results.Add(list[i].Id, SetResultAnwData(list[i]));
                                    break;
                                case ObjectType.SeniorOil:
                                case ObjectType.PrimaryOil:
                                case ObjectType.SemiOil:
                                    storageDeltaList.Oils.Add(list[i].Id, SetOilAnwData(list[i]));
                                    break;
                                case ObjectType.DogFood:
                                    storageDeltaList.DogFoods.Add(list[i].Id, SetDogFoodAnwData(list[i]));
                                    break;
                                case ObjectType.Fertilizer:
                                    storageDeltaList.Fertilizers.Add(list[i].Id, SetFertilizerAnwData(list[i]));
                                    break;
                                case ObjectType.elixir:
                                    storageDeltaList.Elixirs.Add(list[i].Id, SetElixirAnwData(list[i]));
                                    break;
                                case ObjectType.formula:
                                    storageDeltaList.Formulas.Add(list[i].Id, SetFormulaAnwData(list[i]));
                                    break;
                                default:
                                    Debug.LogError("后台返回了不该在仓库的东西:" + item.Name);
                                    break;
                            }
                        }
              
                    }



                }

                if (storageDeltaList.Seeds.Count == 0)
                {
                    if (ViewMgr.Instance.isOpen(ViewNames.SeedBarView))
                    {
                        SystemMsgView.SystemFunction(Function.OpenDialog, Info.SeedNumNotEngouth, ViewNames.ShopView, (
                            () =>
                            {
                                SeedBarView.PlayBack();
                            }));

                    }
                    
                }
              
            }
            Farm_Game_StoreInfoModel.storage = storageDeltaList;
        }
        
        //商店信息输入
        public static void SetAnwData(out Dictionary<int,Seed> seeds, out Dictionary<int, DogFood> dogfoods, out Dictionary<int, Fertilizer> Fertilizer,out Dictionary<int, Result> results, out Dictionary<int, Formula> formulas, Farm_Game_ShopInfo_Anw GenerateAnw)
        {
            IList<Shop_MessageUnit> list = GenerateAnw.ObjectsList;
            Dictionary<int, Seed> _seeds = new Dictionary<int, Seed>();
            Dictionary<int, DogFood> _dogfoods = new Dictionary<int, DogFood>();
            Dictionary<int, Fertilizer> _fertilizer = new Dictionary<int, Fertilizer>();
            Dictionary<int, Result> _results = new Dictionary<int, Result>();
            Dictionary<int, Formula> _formulas = new Dictionary<int, Formula>();

            for (int i = 0; i < list.Count; i++)
            {
                BaseAtrribute ba = LoadObjctDateConfig.Instance.GetAtrribute(list[i].Object.Id);
                if (ba!=null)
                {
                    switch (ba.Type)
                    {
                        case ObjectType.Seed:
                            _seeds.Add(list[i].Object.Id, SetSeedAnwData(list[i].Object));
                            _seeds[list[i].Object.Id].Price = list[i].Price;
                            break;
                        case ObjectType.DogFood:
                            _dogfoods.Add(list[i].Object.Id, SetDogFoodAnwData(list[i].Object));
                            _dogfoods[list[i].Object.Id].Price = list[i].Price;
                            break;
                        case ObjectType.Fertilizer:
                            _fertilizer.Add(list[i].Object.Id, SetFertilizerAnwData(list[i].Object));
                            _fertilizer[list[i].Object.Id].Price = list[i].Price;
                            break;

                        //测试用果实
                        case ObjectType.Result:
                            _results.Add(list[i].Object.Id, SetResultAnwData(list[i].Object));
                            break;
                        case ObjectType.formula:
                            _formulas.Add(list[i].Object.Id, SetFormulaAnwData(list[i].Object));
                            _formulas[list[i].Object.Id].Price = list[i].Price;
                            break;
                        default:
                            break;
                    }
                }
       
            }

            //测试配方
            //Formula f = new Formula();
            //f.ID = 1001;
            //f.Name = "神秘配方";
            //f.Price = 100;
            //f.ShopTag = 2;
            //_formulas.Add(f.ID, f);
            
            seeds = _seeds;
            dogfoods = _dogfoods;
            Fertilizer = _fertilizer;
            results = _results;
            formulas = _formulas;
        }

        //玩家信息输入
        public static Dictionary<int, PlayerInfo> SetAnwData(IList<Farm_Game_UserInfo_Anw> infoList)
        {
            Dictionary<int, PlayerInfo> infos = new Dictionary<int, PlayerInfo>();
            for (int i = 0; i < infoList.Count; i++)
            {
                Farm_Game_UserInfo_Anw GenerateAnw = infoList[i];
                PlayerInfo info = new PlayerInfo();
                info.UserGameId = GenerateAnw.UserGameID;
                info.GameName = GenerateAnw.GameName;

                info.HeaderIcon = GenerateAnw.HeaderIcon;
                info.GameMoney = GenerateAnw.GameMoney;

                info.UserLevel = GenerateAnw.UserLevel;

                info.UserExp = GenerateAnw.UserExp;
                info.LevelMaxExp = GenerateAnw.LevelMaxExp;
                info.Aciton = GenerateAnw.Aciton;
                //          info.Url = GenerateAnw.Url;
                info.Rank = GenerateAnw.Rank;

                info.DogUpgradMaxExp = GenerateAnw.DogUpgradeMaxEXP;
                info.DogCurrentExp = GenerateAnw.DogCurrentEXP;

                infos.Add(info.UserGameId, info);
            }

            return infos;
        }
  
        //消息消息输入
        public static Dictionary<int, MsgUnit> SetAnwData(IList<PMsg_MessageUnit> list)
        {
            Dictionary<int, MsgUnit> MsgList = MessageModel.Instance.MsgList;
            bool isChange = false;
            //Debug.LogError(list.Count);
            for(int i = 0; i < list.Count; i++)
            {
                if (MsgList.ContainsKey(list[i].MsgID))
                    continue;

                if (list[i].FromUid == LoginModel.Instance.Uid)
                {
                    MessageController.Instance.DelMsg(list[i].MsgID);
                    continue;
                }

                isChange = true;
                MsgUnit unit = new MsgUnit();
                unit.id = list[i].MsgID;
                unit.type = list[i].MsgType;
                unit.content = list[i].Content;
                unit.SendTime = list[i].SendTime;

                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
                DateTime dt = startTime.AddSeconds(list[i].SendTime);
                //System.Debug.Log(dt.ToString("yyyy/MM/dd HH:mm:ss:ffff"));
                unit.sendTime = dt.ToString("HH:mm");

                unit.PlayerUid = list[i].FromUid;
                unit.PlayerHead = list[i].FromHead;
                unit.PlayerName = list[i].FromName;

                int _id = -1;
                foreach (var msg in MsgList.Values)
                {
                    if (msg.type == 2 && msg.PlayerUid == list[i].FromUid&&msg.content!=list[i].Content)
                    {
                        _id = msg.id;
                        break;
                    }
                }
                if (_id != -1)
                {
                    ChatLog c = new ChatLog();
                    c.SendPlayer = 2;
                    c.Content = MsgList[_id].content;
                    c.sendTime = dt.ToString("yyyy/MM/dd HH:mm");
                    ChatLogManager.Instance.SaveData(MsgList[_id].PlayerUid, c);

                    MessageController.Instance.DelMsg(MsgList[_id].id);
                    MsgList.Remove(_id);
                }
                MsgList.Add(unit.id, unit);
                //bool isHas = false;
                //if (PlayerSave.HasKey(list[i].FromUid.ToString()))
                //{
                //    string s=PlayerSave.GetString(list[i].FromUid.ToString());
                //    string[] contents= s.Split(',');
                //    for (int j = 0; j < contents.Length; j++)
                //    {
                //        //如果内容ID已经存在则跳出循环
                //       if (contents[j]== list[i].MsgID.ToString())
                //        {
                //            isHas = true;
                //            break;
                //        }
                //    }
                //    //如果没有出现过则添加进去
                //    if (!isHas)
                //    {
                //        StringBuilder sb=new StringBuilder(s);
                //        sb.Append("," + list[i].MsgID.ToString());
                //        PlayerSave.SetString(list[i].FromUid.ToString(),sb.ToString());
                //
                //        MsgList.Add(unit.id, unit);
                //    }
                // 
                //}
                //else
                //{
                //    PlayerSave.SetString(list[i].FromUid.ToString(), list[i].MsgID.ToString());
                //
                //    MsgList.Add(unit.id, unit);
                //
                //}

            }

            if (isChange)
            {
                MessageController.Instance.MsgCount = MsgList.Count;
                GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnMsgChange, MsgList.Count);
            }
            //Debug.LogError(MsgList.Count);
            return MsgList;
        }

        //聊天记录输入
        public static List<ChatLog> SetAnwData(IList<PMsg_ChatLog> chatLog)
        {
            List<ChatLog> list = ChatModel.Instance.chatLog;//new List<ChatLog>();
            for(int i=0;i<chatLog.Count;i++)
            {
                ChatLog c = new ChatLog();
                c.Content = chatLog[i].Content;
                if (chatLog[i].Uid == LoginModel.Instance.Uid)
                {
                    c.SendPlayer = 1;
                }
                else if (chatLog[i].Uid == ChatModel.Instance.ChatTarget.UserGameId)
                {
                    c.SendPlayer = 2;
                }
                //list.Add(c);
                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
                DateTime dt = startTime.AddSeconds(chatLog[i].SendTime);
                //System.Debug.Log(dt.ToString("yyyy/MM/dd HH:mm:ss:ffff"));
                c.sendTime = dt.ToString("yyyy/MM/dd HH:mm");
                list.Insert(0,c);
            }

            return list;
        }

        //地块信息输入？？
        /*      public static Dictionary<int, FarmUnit> SetAnwData(IList<PMsg_FarmUnit> infoList)
                {
                    Dictionary<int, FarmUnit> infos = new Dictionary<int, FarmUnit>();
                    for (int i = 0; i < infoList.Count; i++)
                    {
                        PMsg_FarmUnit GenerateAnw = infoList[i];
                        FarmUnit info = new FarmUnit();
                        info.EnablePlant = GenerateAnw.EnablePlant;
                        info.Xposi = GenerateAnw.Xposi;

                        info.Yposi = GenerateAnw.Yposi;
                        info.ReclaimCoin = GenerateAnw.ReclaimCoin;

                        info.StartPlant = GenerateAnw.StartPlant;
                        info.Plant = SetAnwData(GenerateAnw.Plant);
                        SerializeProto(info,GenerateAnw.FarmField);


                        infos.Add(info.ID,info);
                    }

                    return infos;
                }
        */



        /// <summary>
        /// 测试使用函数
        /// </summary>
        /// <param name="infoList"></param>
        /// <returns></returns>
        public static Dictionary<int, FarmUnit> SetAnwData(List<FarmUnit> infoList)
        {
            Dictionary<int, FarmUnit> infos = new Dictionary<int, FarmUnit>();
            for (int i = 0; i < infoList.Count; i++)
            {
                FarmUnit GenerateAnw = infoList[i];
                FarmUnit info = new FarmUnit();
                info.EnablePlant = GenerateAnw.EnablePlant;
                info.Xposi = GenerateAnw.Xposi;

                info.Yposi = GenerateAnw.Yposi;
                info.ReclaimCoin = GenerateAnw.ReclaimCoin;

                info.StartPlant = GenerateAnw.StartPlant;
                info.Plant = GenerateAnw.Plant;
                info.ID = GenerateAnw.ID;
                info.Name = GenerateAnw.Name;
                info.ObjectNum = GenerateAnw.ObjectNum;
                info.Price = GenerateAnw.Price;
                info.Url = GenerateAnw.Url;
                info.ShopTag = GenerateAnw.ShopTag;
                info.StoreShowTag = GenerateAnw.StoreShowTag;
                info.Des = GenerateAnw.Des;
                infos.Add(info.ID, info);
            }

            return infos;
        }
        public static Plant SetAnwData(PMsg_Plant _plant)
        {
            if (_plant != null)
            {
                //查找本地配置表
                BaseAtrribute ba = LoadObjctDateConfig.Instance.GetAtrribute(_plant.Id);

                Plant p = new Plant();
                p.GrothTime = ba.GrothTime;
                p.ID = ba.Id;
                p.Name = ba.Name;
                p.Des = ba.Des;

                return p;
            }
            else
            {
                Debug.LogError(string.Format("the plant is null"));
                return null;
            }
        }

      
        public static PlayerInfo SetAnwData(Farm_Game_UserInfo_Anw infoList)
        {
           

            Farm_Game_UserInfo_Anw GenerateAnw = infoList;
            PlayerInfo info = new PlayerInfo();
            info.UserGameId = GenerateAnw.UserGameID;
            info.GameName = GenerateAnw.GameName;

            info.HeaderIcon = GenerateAnw.HeaderIcon;
            info.GameMoney = GenerateAnw.GameMoney;

            info.UserLevel = GenerateAnw.UserLevel;

            info.UserExp = GenerateAnw.UserExp;
            info.LevelMaxExp = GenerateAnw.LevelMaxExp;
            info.Aciton = GenerateAnw.Aciton;
            //          info.Url = GenerateAnw.Url;
            info.Rank = GenerateAnw.Rank;

            info.DogUpgradMaxExp = GenerateAnw.DogUpgradeMaxEXP;
            info.DogCurrentExp = GenerateAnw.DogCurrentEXP;

            return info;
        }
        public static Seed SetSeedAnwData(PMsg_StagePropUnit obj)
        {
            if (obj != null)
            {
                //查找本地配置表
                BaseAtrribute ba = LoadObjctDateConfig.Instance.GetAtrribute(obj.Id);

                Seed p = new Seed();
                p.GrothTime = ba.GrothTime;
                p.StoreShowTag = 1;
                p.Url = "Sprites/Seeds/Seed_" + ba.Id;
                p.Des = ba.Des;
                
                DataSettingManager.SerializeProto(p, obj);
                return p;
            }
            else
            {

                Debug.LogError(string.Format("the SeedObject is null"));
                return null;
            }

        }
        public static Result SetResultAnwData(PMsg_StagePropUnit obj)
        {
            if (obj != null)
            {
                //查找本地配置表
                BaseAtrribute ba = LoadObjctDateConfig.Instance.GetAtrribute(obj.Id);

                Result p = new Result();
                p.UpGradeToOilNum = ba.CombineCount;
                p.StoreShowTag = 2;
                p.Url = "Sprites/Results/Result_" + ba.Id;
                p.Des = ba.Des;

                DataSettingManager.SerializeProto(p, obj);
                return p;
            }
            else
            {
                Debug.LogError(string.Format("the ResultObject is null"));
                return null;
            }

        }
        public static Oil SetOilAnwData(PMsg_StagePropUnit obj)
        {
            if (obj != null)
            {
                //查找本地配置表
                BaseAtrribute ba = LoadObjctDateConfig.Instance.GetAtrribute(obj.Id);

                Oil p = new Oil();
                if (ba.Type == ObjectType.PrimaryOil)
                {
                    p.OilType = 1;//初级精油   ？合成数量？
                    p.CombinCount = ba.CombineCount;
                    p.Url = "Sprites/Oils/PrimaryOil/POil_" + ba.Id;
                }
                else if (ba.Type == ObjectType.SemiOil)
                {
                    p.OilType = 2;//半成品精油
                    p.Url = "Sprites/Oils/SemiOil/SEOil_" + ba.Id;
                }
                else if (ba.Type == ObjectType.SeniorOil)
                {
                    p.OilType = 3;//高级精油
                    p.Url = "Sprites/Oils/SeniorOil/SOil_" + ba.Id;
                }
                //p.GainExp = obj.GainEXP;
                //p.OnceLackResult = obj.OnceLackResult;
                p.StoreShowTag = 3;
                p.Des = ba.Des;

                DataSettingManager.SerializeProto(p, obj);
                return p;
            }
            else
            {
                Debug.LogError(string.Format("the OilObject is null"));
                return null;
            }

        }
        public static Fertilizer SetFertilizerAnwData(PMsg_StagePropUnit obj)
        {
            if (obj != null)
            {
                //查找本地配置表
                BaseAtrribute ba = LoadObjctDateConfig.Instance.GetAtrribute(obj.Id);

                Fertilizer p = new Fertilizer();
                p.Url = "Sprites/Fertilizers/Fertilizer_"+ba.Id;
                p.SpeedUp = ba.SpeedUp;
                p.StoreShowTag = 4;
                p.Des = ba.Des;

                DataSettingManager.SerializeProto(p, obj);
                return p;
            }
            else
            {
                Debug.LogError(string.Format("the FertilizerObject is null"));
                return null;
            }

        }

        public static Elixir SetElixirAnwData(PMsg_StagePropUnit obj)
        {
            if (obj != null)
            {
                //查找本地配置表
                BaseAtrribute ba = LoadObjctDateConfig.Instance.GetAtrribute(obj.Id);

                Elixir p = new Elixir();
                //p.Url = "Sprites/Fertilizers/Fertilizer_" + ba.Id;
                p.StoreShowTag = 4;
                p.Des = ba.Des;

                DataSettingManager.SerializeProto(p, obj);
                return p;
            }
            else
            {
                Debug.LogError(string.Format("the FertilizerObject is null"));
                return null;
            }

        }

        public static Formula SetFormulaAnwData(PMsg_StagePropUnit obj)
        {
            if (obj != null)
            {
                //查找本地配置表
                BaseAtrribute ba = LoadObjctDateConfig.Instance.GetAtrribute(obj.Id);

                Formula p = new Formula();
                //p.Url = "Sprites/Fertilizers/Fertilizer_" + ba.Id;
                p.StoreShowTag = 4;
                p.ShopTag = 2;
                p.Des = ba.Des;

                DataSettingManager.SerializeProto(p, obj);
                return p;
            }
            else
            {
                //Debug.LogError(string.Format("the FertilizerObject is null"));
                return null;
            }

        }

        public static DogFood SetDogFoodAnwData(PMsg_StagePropUnit obj)
        {
            if (obj != null)
            {
                //查找本地配置表
                BaseAtrribute ba = LoadObjctDateConfig.Instance.GetAtrribute(obj.Id);

                DogFood p = new DogFood();
                p.Speedup = ba.SpeedUp;
                p.Url = "Sprites/DogFoods/DogFood_" + ba.Id;
                p.Des = ba.Des;

                DataSettingManager.SerializeProto(p, obj);
                return p;
            }
            else
            {
                Debug.LogError(string.Format("the DogFoodObject is null"));
                return null;
            }

        }
        
        
        
        /// <summary>
        /// 给公用属性赋值
        /// </summary>
        /// <param name="baseObject"></param>
        /// <param name="stagePropUnit"></param>
        public static void SerializeProto(BaseObject baseObject, PMsg_StagePropUnit stagePropUnit)
        {
            //服务器的信息：id+count
            baseObject.ID = stagePropUnit.Id;
            baseObject.ObjectNum = stagePropUnit.Count;

            //查找本地配置表
            BaseAtrribute ba = LoadObjctDateConfig.Instance.GetAtrribute(stagePropUnit.Id);

            baseObject.Name = ba.Name;
            baseObject.Price = ba.Price;
            baseObject.Des = ba.Des;
        }
    }
}