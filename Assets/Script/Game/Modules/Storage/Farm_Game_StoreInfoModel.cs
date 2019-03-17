using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using System;

namespace Game
{
    //********************************************************************************************************************//
    //  名称: 仓库信息回应
    //  描述: 服务器接收仓库信息请求，根据附带角色ID，返回仓库存储信息。
    //  标识: module = 2 ,sub = 9
    //  方向: Server To App
    public class Farm_Game_StoreInfoModel : BaseModel<Farm_Game_StoreInfoModel>
    {
        public static StorageDeltaList storage = new StorageDeltaList();

        public override void InitModel()
        {

        }

        public bool ContainType(ObjectType ba)
        {
            switch (ba)
            {
                case ObjectType.Seed:
                    if (storage.Seeds.Count > 0)
                    {
                        return true;
                    }
                    else return false;
                   
                case ObjectType.Result:
                    if (storage.Results.Count > 0)
                    {
                        return true;
                    }
                    else return false;
                    
                case ObjectType.PrimaryOil:
                    if (storage.Oils.Count > 0)
                    {
                        Dictionary<int, Oil>.Enumerator t= storage.Oils.GetEnumerator();
                        for (int i=0; i < storage.Oils.Count; i++)
                        {
                           
                            if (t.Current.Value.OilType == (int)ObjectType.PrimaryOil)
                            {
                                return true;
                            }
                            t.MoveNext();
                        }
                        t.Dispose();

                        return false;
                    }
                    else return false;

                case ObjectType.SemiOil:
                    if (storage.Oils.Count > 0)
                    {
                        Dictionary<int, Oil>.Enumerator t = storage.Oils.GetEnumerator();
                        for (int i = 0; i < storage.Oils.Count; i++)
                        {

                            if (t.Current.Value.OilType == (int)ObjectType.SemiOil)
                            {
                                return true;
                            }
                            t.MoveNext();
                        }
                        t.Dispose();

                        return false;
                    }
                    else return false;
                case ObjectType.SeniorOil:
                    if (storage.Oils.Count > 0)
                    {
                        Dictionary<int, Oil>.Enumerator t = storage.Oils.GetEnumerator();

                        Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "大于0", "test1"));

                        for (int i = 0; i < storage.Oils.Count; i++)
                        {
                            t.MoveNext();
//                            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", t.Current.Value.OilType, t.Current.Key));

                            if (t.Current.Value.OilType == (int)ObjectType.SeniorOil)
                            {
                                return true;
                            }
                        }
                        Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "test", "test1"));

                        t.Dispose();

                        return false;
                    }
                    else return false;
                case ObjectType.DogFood:
                    if (storage.DogFoods.Count > 0)
                    {
                        return true;
                    }
                    else return false;
                    
                case ObjectType.Fertilizer:
                    if (storage.Fertilizers.Count > 0)
                    {
                        return true;
                    }
                    else return false;
                default:
                    return false;
                    
            }
        }
      
        //输入数据（如果当前已有改列表中id对应的物品，这刷新改物品数量信息，若数量为0，则删除该物品）
        public void SetData(IList<PMsg_StagePropUnit> list)
        {
            DataSettingManager.SetStoreAnwData(list);
        }
        public BaseObject GetData(int id)
        {
            BaseAtrribute ba = LoadObjctDateConfig.Instance.GetAtrribute(id);
            if (ba == null) return null;
            BaseObject bo;
            switch (ba.Type) {
                case ObjectType.Seed:
                    if (storage.Seeds.ContainsKey(id))
                    {
                        bo = storage.Seeds[id];
                    }
                    else bo = new BaseObject()
                    {
                        Name = ba.Name,
                        Des = ba.Des,
                        Price = ba.Price,
                        
                        ID = id,
                        ObjectNum = 0,
                    };
                    break;
                case ObjectType.Result:
                    if (storage.Results.ContainsKey(id))
                    {
                        bo = storage.Results[id];
                    }
                    else bo = new BaseObject()
                    {
                        Name = ba.Name,
                        Des = ba.Des,
                        Price = ba.Price,

                        ID = id,
                        ObjectNum = 0,
                    }; ;
                    break;
                case ObjectType.PrimaryOil:
                case ObjectType.SemiOil:
                case ObjectType.SeniorOil:
                    if (storage.Oils.ContainsKey(id))
                    {
                       
                        bo = storage.Oils[id];
                    }
                    else bo = new BaseObject()
                    {
                        Name = ba.Name,
                        Des = ba.Des,
                        Price = ba.Price,

                        ID = id,
                        ObjectNum = 0,
                    }; ;
                    break;
                case ObjectType.DogFood:
                    if (storage.DogFoods.ContainsKey(id))
                    {
                        bo = storage.DogFoods[id];
                    }
                    else bo = new BaseObject()
                    {
                        Name = ba.Name,
                        Des = ba.Des,
                        Price = ba.Price,

                        ID = id,
                        ObjectNum = 0,
                    }; ;
                    break;
                case ObjectType.Fertilizer:
                    if (storage.Fertilizers.ContainsKey(id))
                    {
                        bo = storage.Fertilizers[id];
                    }
                    else bo = new BaseObject()
                    {
                        Name = ba.Name,
                        Des = ba.Des,
                        Price = ba.Price,

                        ID = id,
                        ObjectNum = 0,
                    }; 
                    break;
                case ObjectType.elixir:
                    if (storage.Elixirs.ContainsKey(id))
                    {
                        bo = storage.Elixirs[id];
                    }
                    else bo = new BaseObject()
                    {
                        Name = ba.Name,
                        Des = ba.Des,
                        Price = ba.Price,

                        ID = id,
                        ObjectNum = 0,
                    };
                    break;
                default:
                    bo = null;
                    break;
            }
            return bo;
        }

    }
}