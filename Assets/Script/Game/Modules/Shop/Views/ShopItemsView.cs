using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using UnityEngine.UI;

namespace Game
{
    public class ShopItemsView : BaseSubView
    {
        private Transform ShopGrid;
        private Transform SeedGrid;
        private Transform FertilizerGrid;
        private Transform DogFoodGrid;
        private Button Btnclose;
        private Resource ItemPrefab;
        private Toggle seedtg;
        private Toggle dogFoodtg;

        private bool isFristOpen;

        public ShopItemsView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
        }

        public override void BuildUIContent()
        {
            
            base.BuildUIContent();
            ShopGrid= TargetGo.transform.Find("Goods/ShopGrids");
            SeedGrid = TargetGo.transform.Find("Goods/ShopGrids/Seeds/ScrollView/Viewport/SeedGrid");
            FertilizerGrid = TargetGo.transform.Find("Goods/ShopGrids/SpAttributes/ScrollView/Viewport/SpAttributeGrid");
            DogFoodGrid = TargetGo.transform.Find("Goods/ShopGrids/Pets/ScrollView/Viewport/PetGrid");

            Btnclose = TargetGo.transform.Find("CloseBtn").GetComponent<Button>();
            Btnclose.onClick.AddListener(OnClickCloseBtn);
            
            seedtg = TargetGo.transform.Find("Goods/ToggleGroup/Seeds").GetComponent<Toggle>();
            dogFoodtg = TargetGo.transform.Find("Goods/ToggleGroup/Pets").GetComponent<Toggle>();

            ResourceMgr.Instance.LoadResource("Prefab/ShopItem", OnLoadShopItem);

            isFristOpen = true;
        }

        public override void OnOpen()
        {
            base.OnOpen();
            int model = ShopController.Instance.Model;
            if (model == 1)
            {
                seedtg.isOn = true;
            }
            else if (model == 3)
            {
                dogFoodtg.isOn = true;
            }

            ShopController.Instance.ReqShopSend();
            ShopController.Instance.GetDispatcher().AddListener(ShopEvent.OnShopItemChanged,InitShopGrid);

            MusicManager.Instance.Playsfx(AudioNames.OnClick1);
        }

        
        //初始化商店各商品
        private bool InitShopGrid(int eventId,object arg)
        {
            LoadingImageManager.Instance.StartLoading(TargetGo.transform, TargetGo.transform.position);

            isFristOpen = false;
            if (ItemPrefab == null)
            {
                return false;
            }

            AddGrid<Seed>(SeedGrid,ShopModel.Instance.seeds);
            AddGrid<DogFood>(DogFoodGrid, ShopModel.Instance.DogFoods);
            AddGrid<Fertilizer>(FertilizerGrid, ShopModel.Instance.Fertilizers);
            AddGrid<Formula>(FertilizerGrid, ShopModel.Instance.Formulas);
            LoadingImageManager.Instance.StopLoading();

            //测试用果实
            //AddGrid<Result>(DogFoodGrid, ShopModel.Instance.Results);

            return false;
        }

        //根据不同类型添加商品item，显示名字、价格等
        private void AddGrid<T>(Transform gridTr, Dictionary<int,T> list) where T : BaseObject
        {
            if (typeof(T)!=typeof(Formula))
            {
                for (int i = 0; i < gridTr.childCount; i++)
                {
                    GameObject go = gridTr.GetChild(i).gameObject;
                    GameObject.Destroy(go);
                }
            }
            foreach (T t in list.Values)
            {
                GameObject go = GameObject.Instantiate(ItemPrefab.UnityObj) as GameObject;
                ShopItem item = go.AddComponent<ShopItem>();
                item.SetData(t);

                go.transform.SetParent(gridTr);
                go.transform.localScale = new Vector3(1, 1, 1);
            }
        }

        //加载商品item的prefab
        private void OnLoadShopItem(Resource res, bool succ)
        {
            if (succ)
            {
                ItemPrefab = res;
                if (!isFristOpen)
                {
                    InitShopGrid(0, null);
                }
            }
            else{
                Debug.LogError("加载商店物品item的prefab失败！");
            }
        }

        //点击关闭商店按钮
        private void OnClickCloseBtn()
        {
            MusicManager.Instance.Playsfx(AudioNames.CloseBtn);
            ViewMgr.Instance.Close(ViewNames.ShopView);
        }

        public override void OnClose()
        {
            base.OnClose();
            ShopController.Instance.Model = 1;
            ShopController.Instance.GetDispatcher().RemoveListener(ShopEvent.OnShopItemChanged,InitShopGrid);
        }
    }
}