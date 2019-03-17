using System;
using System.Collections;
using System.Collections.Generic;

using Framework;
using UnityEngine;

namespace Game
{
    //********************************************************************************************************************//
    //  名称: 农田信息回应
    //  描述: 服务器收到地图信息请求后，根据角色ID查询并组织地图数据，返回App。请求完后，app和服务器一同开始计算服务器长虫，干燥，缺肥，长草周期，开始计时
    //  	  地图信息结构组织格式，为数组结构格式，具体格式组成，请查看文档上部分的消息结构。
    //  标识: module = 2 ,sub = 2
    //  方向: Server To App
    public class FieldsModel : BaseModel<FieldsModel>
    {
        public int Width { set; get; }
        public int Height { set; get; }
        private GameObject root;
        private GameObject building;
        private GameObject dog;
        public Dictionary<int, FarmUnit> farms = new Dictionary<int, FarmUnit>();
        public Dictionary<string, WorldObject> otherObjs = new Dictionary<string, WorldObject>();

        /// <summary>
        /// init最先执行
        /// </summary>
        public override void InitModel()
        {
            root = GameObject.Find("Panel2D/Fields");
            building = GameObject.Find("Panel2D/Buildings");

            Dog dog = new Dog();
            dog.Go = GameObject.Find("Panel2D/Buildings/Dog").gameObject;
            otherObjs.Add("Dog", dog);

            SeedFriendBrand brand = new SeedFriendBrand();
            brand.Go = GameObject.Find("Panel2D/Buildings/Seefriend").gameObject;
            otherObjs.Add("Seefriend", brand);

            DogFoodTip dogFoodTip = new DogFoodTip(GameObject.Find("Panel2D/Buildings/bowl/dogfood").gameObject);

            otherObjs.Add("dogfood", dogFoodTip);
            GetFields();
        }
        /// <summary>
        /// 获得unity中场景中所有创建的物体
        /// </summary>
        public void GetFields()
        {
            foreach (Transform childTran in root.transform)
            {
                //此处作为地块ID
                int id = int.Parse(childTran.name.Substring(4));
                SpriteRenderer LandRenderer = childTran.gameObject.AddComponent<SpriteRenderer>();

                FarmUnit unit = new FarmUnit();
                unit.FarmID = id;
                unit.Renderer = LandRenderer;

                farms.Add(unit.FarmID, unit);
            }
            int index = 0;
            //初始化位置，为层级计算做好准备
            for (int i = 1; i < 7; i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    index++;
                    (farms[index] as FarmUnit).Xposi = i;
                    (farms[index] as FarmUnit).Yposi = j;
                }
            }
        }
        /// <summary>
        /// 狗抓住，删除收获图标
        /// </summary>
        /// <param name="farmid"></param>
        public void DogCatch(int farmid)
        {
            if (farms.ContainsKey(farmid))
            {
                Plant p = farms[farmid].Plant;
                p.IsSteal = 1;
                p.RemoveCatch();
            }
        }

        public void SetData(PMsg_Plant plant)
        {
            FarmUnit fm = farms[FarmUnit.SeletedFarmID] as FarmUnit;
            Plant plant1;
            plant1 = DataSettingManager.SetAnwData(plant);
            GameObject plantGO = new GameObject("plant" + fm.FarmID);
            plantGO.transform.SetParent(fm.Renderer.transform, false);
            SpriteRenderer plantR = plantGO.AddComponent<SpriteRenderer>();
            plant1.FarmID = FarmUnit.SeletedFarmID;
            plant1.Renderer = plantR;
            plant1.StartTime = plant.StartTime;
            plant1.IsSteal = plant.IsSteal;

            plant1.IsGrass = plant.IsGrass;
            plant1.IsWorm = plant.IsWorm;
            plant1.ID = plant.Id;


            fm.Plant = plant1;

            fm.Plant.Renderer.sortingLayerName = "plant";
            fm.Plant.Renderer.sortingOrder = fm.Renderer.sortingOrder + 10;

        }

        private int index = 0;
        /// <summary>
        /// 初始化地图上的所有数据
        /// </summary>
        /// <param name="GenerateAnw"></param>
        public void SetData(IList<PMsg_MapUnit> GenerateAnw)
        {
            int _brandId = 1;

            for (int i = 0; i < GenerateAnw.Count; i++)
            {
                PMsg_MapUnit mapUnit = GenerateAnw[i];
                //                Debug.Log(mapUnit.Id);
                FarmUnit fm = farms[mapUnit.Id] as FarmUnit;

                fm.EnablePlant = mapUnit.EnablePlant;


                if (mapUnit.EnablePlant == 1 && mapUnit.Id > _brandId)
                {
                    _brandId = mapUnit.Id;
                }

                fm.FarmID = mapUnit.Id;

                SpriteRenderer sr = fm.Renderer;

                int baseOder = 100 * fm.Xposi + 50 * fm.Yposi;
                sr.sortingLayerName = "land";

                sr.sortingOrder = baseOder;
                //传回来的地上植物不为空
                if (mapUnit.Plant.ToString() != "")
                {
                    if (fm.Plant != null)
                    {
                        fm.Plant.ID = mapUnit.Plant.Id;
                        fm.Plant.FarmID = fm.FarmID;

                        fm.Plant.IsSteal = mapUnit.Plant.IsSteal;
                        fm.Plant.StartTime = mapUnit.Plant.StartTime;
                        fm.Plant.IsGrass = mapUnit.Plant.IsGrass;
                        fm.Plant.IsWorm = mapUnit.Plant.IsWorm;
                        fm.Plant.IsWater = mapUnit.Plant.IsLessWater;
                        
                        BaseAtrribute ba = LoadObjctDateConfig.Instance.GetAtrribute(mapUnit.Plant.Id);
                        fm.Plant.Name=ba.Name;

                    }
                    else
                    {
                        Plant plant = DataSettingManager.SetAnwData(mapUnit.Plant);

                        GameObject plant1 = new GameObject("plant" + fm.FarmID);
                        plant1.transform.SetParent(fm.Renderer.transform, false);
                        SpriteRenderer plantR = plant1.AddComponent<SpriteRenderer>();
                        plant.FarmID = fm.FarmID;

                        plant.ID = mapUnit.Plant.Id;
                        plant.IsSteal = mapUnit.Plant.IsSteal;
                        plant.StartTime = mapUnit.Plant.StartTime;
                        plant.Renderer = plantR;

                        plant.IsGrass = mapUnit.Plant.IsGrass;
                        plant.IsWorm = mapUnit.Plant.IsWorm;
                        plant.IsWater = mapUnit.Plant.IsLessWater;
                        fm.Plant = plant;
                        fm.Plant.Renderer.sortingLayerName = "plant";
                        fm.Plant.Renderer.sortingOrder = baseOder + 10;
                    }

                }
                else
                {
                    for (int j = 0; j < fm.Go.transform.childCount; j++)
                    {
                        if (fm.Go.transform.GetChild(j) && fm.Go.transform.GetChild(j).name != "brand(Clone)")
                        {
                            GameObject.Destroy(fm.Go.transform.GetChild(j).gameObject);

                        }
                    }
                    //这块地上原来有植物
                    if (fm.Plant != null)
                    {
                        fm.Plant.destroyPlant();
                        fm.Plant.DisposeBadIcon();

                    }
                    fm.Plant = null;

                }
            }
//            GC.Collect();
            Brand brand = Brand.Instance;
            brand.SelectId = _brandId + 1;

        }




    }


}
