using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Framework;
using UnityEngine;

namespace Game
{
    public interface LoadPage
    {
        
    }
    public class LoadObjctDateConfig : BaseConfig<LoadObjctDateConfig>
    {
        private string url;

        public Dictionary<int ,BaseAtrribute> BaseAtrributes=new Dictionary<int, BaseAtrribute>();
        //targetID,needIDs
        public Dictionary<int,List<NeedClass>> needs=new Dictionary<int, List<NeedClass>>();
        public override void InitConfig()
        {
            //if (!GameStarter.Instance.isDebug)
            //{
            //   ResourceMgr.Instance.LoadResource("Config/ID", (delegate(Resource resource, bool b)
            //    {
            //        TextAsset txt = resource.UnityObj as TextAsset;
            //        string content = txt.text;
            
            //                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", content, "test1"));

            //        SetData(content);
            
                    //                GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnDataConfigLoadDone);
            //    }));
            //}
            //else
            //{
            //   ResourceMgr.Instance.LoadResource("Config/ID1", (delegate (Resource resource, bool b)
            //    {
            //        TextAsset txt = resource.UnityObj as TextAsset;
            //        string content = txt.text;

            //                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", content, "test1"));

            //        SetData(content);
            
            //                GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnDataConfigLoadDone);
            //    }));
            //}
            SetData();
            GlobalDispatcher.Instance.DispathDelay(GlobalEvent.OnDataConfigLoadDone);
        }

        private void SetData(string content)
        {
            string[] ContentLines = content.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < ContentLines.Length; i++)
            {
                string[] infos = ContentLines[i].Split('\t');

                if (infos[0]!="")
                {
                    BaseAtrribute baseAtrribute = new BaseAtrribute();
                    baseAtrribute.Name = infos[2];
                    baseAtrribute.CombineCount = int.Parse(infos[7] != "" ? infos[7] : "0");
                    baseAtrribute.Des = infos[4];
                    baseAtrribute.GrothTime = int.Parse(infos[5] != "" ? infos[5] : "0");
                    baseAtrribute.Price = int.Parse(infos[3] != "" ? infos[3] : "0");
                    baseAtrribute.SpeedUp = int.Parse(infos[6] != "" ? infos[6] : "0");
                    baseAtrribute.Type = (ObjectType)int.Parse(infos[1] != "" ? infos[1] : "0");
                    
                    baseAtrribute.Id = int.Parse(infos[0] != "" ? infos[0] : "0");
                    baseAtrribute.ProduceId = int.Parse(infos[8] != "" ? infos[8] : "0");
                    baseAtrribute.ExchangedName = infos[9]!=""?infos[9]:"";

//                    Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", baseAtrribute.ToString(), "test1"));


                    BaseAtrributes.Add(baseAtrribute.Id, baseAtrribute);
                }
                
            }
        }

        private void SetData()
        {
                        //url = @"http://39.108.134.200:8080/publiccms/api/gameGoodsData";
            url = @"http://119.23.48.181:8080/publiccms/api/gameGoodsData";
            //            url = "http:// "+ LoginConfig.Instance.serverIP+ ":"+ 8080 + "/api/gameGoodsData";

            Debug.LogError(string.Format("<color=#ff0000ff><---{0}-{1}----></color>", url, "test1"));

            string json=VersionUpdateManager.Instance.GetPage(url);
            GameDataResult r = JsonUtility.FromJson<GameDataResult>(json);
            if (r == null || r.result == null || r.result.Length == 0) return;
            for (int i = 0; i < r.result.Length; i++)
            {
                GameData data = r.result[i];
                BaseAtrribute baseAtrribute = new BaseAtrribute();
                baseAtrribute.Name = data.name.Trim();
                //baseAtrribute.CombineCount = data.combineCount;
                baseAtrribute.Des = data.desc.Trim() ;
                baseAtrribute.GrothTime = data.growTime;
                baseAtrribute.Price = data.price;
                baseAtrribute.SpeedUp = data.speedUp;
                baseAtrribute.Type = (ObjectType)data.type;

                baseAtrribute.Id = data.id;
                //baseAtrribute.ProduceId = data.combineId;
                baseAtrribute.ExchangedName =data.exchangeName.Trim() ;

                //                    Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", baseAtrribute.ToString(), "test1"));


                BaseAtrributes.Add(baseAtrribute.Id, baseAtrribute);
            }
            for (int i = 0; i < r.result.Length; i++)
            {
                GameData data = r.result[i];
                if (data.combineId != 0)
                {
                    BaseAtrributes[data.combineId].ProduceId = data.id;
                    BaseAtrributes[data.combineId].CombineCount = data.combineCount;
                }
            }

            for (int i = 0; i < r.exchange.Length; i++)
            {
                ID data = r.exchange[i];
                if (needs.ContainsKey(data.targetId))
                {
                    List<NeedClass> ids= needs[data.targetId];
                    bool isHas=false;
                    for (int j = 0; j < ids.Count; j++)
                    {
                        if (ids[j].id == data.needId)
                        {
                            isHas = true;
                            break;
                        }
                    }
                    if (isHas==false)
                    {
                        NeedClass m = new NeedClass(data.needId,data.count);
                        ids.Add(m);
                    }
                }
                else
                {
                    List<NeedClass> needlist=new List<NeedClass>();
                    NeedClass m = new NeedClass(data.needId, data.count);
                    needlist.Add(m);
                    needs.Add(data.targetId, needlist);
                }
            }

            needs.Add(401, new List<NeedClass>() { new NeedClass(301, 1) });
            needs.Add(402, new List<NeedClass>() { new NeedClass(302, 1) });
            needs.Add(403, new List<NeedClass>() { new NeedClass(303, 1) });
            needs.Add(404, new List<NeedClass>() { new NeedClass(304, 1) });
            needs.Add(405, new List<NeedClass>() { new NeedClass(305, 1) });
            needs.Add(406, new List<NeedClass>() { new NeedClass(306, 1) });
            needs.Add(407, new List<NeedClass>() { new NeedClass(307, 1) });

            needs.Add(301, new List<NeedClass>() { new NeedClass(101, 1) });
            needs.Add(302, new List<NeedClass>() { new NeedClass(102, 1) });
            needs.Add(303, new List<NeedClass>() { new NeedClass(103, 1) });
            needs.Add(304, new List<NeedClass>() { new NeedClass(104, 1) });
            needs.Add(305, new List<NeedClass>() { new NeedClass(105, 1) });
            needs.Add(306, new List<NeedClass>() { new NeedClass(106, 1) });
            needs.Add(307, new List<NeedClass>() { new NeedClass(107, 1) });

            //测试配方
            //BaseAtrribute b = new BaseAtrribute();
            //b.Name = "神秘配方";
            //b.Price = 10000;
            //b.Type = ObjectType.formula;
            //b.Id = 1001;
            //BaseAtrributes.Add(b.Id, b);


        }

        public BaseAtrribute GetAtrribute(int ID)
        {
            if (BaseAtrributes.ContainsKey(ID))
            {
                //Debug.LogError(BaseAtrributes[ID]+" "+ ID);
                return BaseAtrributes[ID];
            }
            else {

                Debug.LogError(string.Format("<color=#ff0000ff><---{0}-{1}----></color>", "不存在这个id:", ID));

                return null;
            }
        }
    }
    public enum ObjectType
    {
        None,
        Seed = 1,
        Result,
        Plant,
        PrimaryOil,//初级
        SemiOil,//半成品
        SeniorOil,//高级
        DogFood,
        Fertilizer,
        elixir,//仙丹
        formula//配方
    }
    public class BaseAtrribute
    {
        private int id;
        private ObjectType type;

        public override string ToString()
        {
            return string.Format("Id: {0}, Type: {1}, Price: {2}, GrothTime: {3}, SpeedUp: {4}, CombineCount: {5}, Name: {6}, Des: {7}, ProduceId: {8}, Id: {9}, Type: {10}, Price: {11}, GrothTime: {12}, SpeedUp: {13}, CombineCount: {14}, Name: {15}, Des: {16}, ProduceId: {17}", id, type, price, _grothTime, _speedUp, _combineCount, name, des, produceId, Id, Type, Price, GrothTime, SpeedUp, CombineCount, Name, Des, ProduceId);
        }

        private int price;
        private int _grothTime;
        private int _speedUp;
        private int _combineCount;
        private string name;
        private string des;
        private int produceId;
        private string _exchangedName;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public ObjectType Type
        {
            get { return type; }
            set { type = value; }
        }

        public int Price
        {
            get { return price; }
            set { price = value; }
        }

        public int GrothTime
        {
            //返回的是小时
            get { return _grothTime; }
            set { _grothTime = value; }
        }

        public int SpeedUp
        {
            get { return _speedUp; }
            set { _speedUp = value; }
        }

        public int CombineCount
        {
            get { return _combineCount; }
            set { _combineCount = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Des
        {
            get { return des; }
            set { des = value; }
        }

        public int ProduceId
        {
            get { return produceId; }
            set { produceId = value; }
        }

        public string ExchangedName
        {
            get { return _exchangedName; }
            set { _exchangedName = value; }
        }
    }

    [Serializable]
    public class GameDataResult
    {
        public GameData[] result;
        public ID[] exchange;
    }

    [Serializable]
    public class GameData
    {
        public int id;
        public string name;
        public int type;
        public int plantId;
        public int fruit;
        public int price;
        public int combineId;
        public int combineCount;
        public int growTime;
        public int countPluck;
        public string desc;
        public int speedUp;  //肥料加速的时间
        public string exchangeName;//精油对应兑换的物品名
    }
    [Serializable]
    public class ID
    {
        public int targetId;
        public int needId;
        public int count;
    }

    public class NeedClass
    {
        public int id;
        public int count;
        public NeedClass()
        {
        }
        public NeedClass(int id,int count)
        {
            this.id = id;
            this.count = count;
        }
    }
}
