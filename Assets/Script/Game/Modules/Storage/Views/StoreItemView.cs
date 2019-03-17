using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Framework;

namespace Game
{

    public enum StoreType {
        Seed,
        Result,
        Oil,
        Fertilizer

    }
    public class StoreItemView : BaseSubView
    {
        private Button Btnclose;
        private Button BtnTotalSell_Seed;
        private Button BtnTotalSell_Fruit;
        private Button BtnTotalSell_Poil;
        private Button BtnTotalSell_Seoil;
        private Button BtnTotalSell_Soil;
        private Button BtnTotalExchange_Oil;

        private Transform StoreGrid;
        private Transform SeedGrid;
        private Transform FruitGrid;
        private Transform OilGrid_Primary;
        private Transform OilGrid_Semi;
        private Transform OilGrid_Senior;
//        private static Toggle OilGrid_SeniorTg;
        private Transform PlantFoodGrid;

        private Transform ConfirmView;
        private Button ConfirmView_SellBtn;
        private Button ConfirmView_CancelBtn;
        private Text ConfirmView_Text;

        private  Toggle seedTg;
        private  Toggle OilTg;
       
        private Resource ItemPrefab;
        private bool isFrist;

        private int CurrentTotalSell;   //全部卖出的东西：1为果实，2为精油

        public StoreItemView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {

        }


        
        public override void BuildUIContent()
        {
            base.BuildUIContent();

            Btnclose = TargetGo.transform.Find("CloseBtn").GetComponent<Button>();
            Btnclose.onClick.AddListener(OnClickCloseBtn);

            BtnTotalSell_Seed= TargetGo.transform.Find("Items/StoreGrid/Seed/TotalSell").GetComponent<Button>();
            BtnTotalSell_Fruit = TargetGo.transform.Find("Items/StoreGrid/Fruit/TotalSell").GetComponent<Button>();
            BtnTotalSell_Poil = TargetGo.transform.Find("Items/StoreGrid/Oil/ToggleGroup/primarySub/TotalSell").GetComponent<Button>();
            BtnTotalSell_Seoil= TargetGo.transform.Find("Items/StoreGrid/Oil/ToggleGroup/SemiSub/TotalSell").GetComponent<Button>();
            BtnTotalSell_Soil= TargetGo.transform.Find("Items/StoreGrid/Oil/ToggleGroup/SeniorSub/TotalSell").GetComponent<Button>();
            BtnTotalSell_Seed.onClick.AddListener(delegate() {this.OnClickTotalSellBtn(1);});
            BtnTotalSell_Fruit.onClick.AddListener(delegate () { this.OnClickTotalSellBtn(2); });
            BtnTotalSell_Poil.onClick.AddListener(delegate () { this.OnClickTotalSellBtn(3); });
            BtnTotalSell_Seoil.onClick.AddListener(delegate () { this.OnClickTotalSellBtn(4); });
            BtnTotalSell_Soil.onClick.AddListener(delegate () { this.OnClickTotalSellBtn(5); });

            BtnTotalExchange_Oil = TargetGo.transform.Find("Items/StoreGrid/PlantFood/TotalExchangeBtn").GetComponent<Button>(); 
            BtnTotalExchange_Oil.onClick.AddListener(OnClickTotalExchange);

            StoreGrid= TargetGo.transform.Find("Items/StoreGrid");
            SeedGrid = TargetGo.transform.Find("Items/StoreGrid/Seed/SeedGrid");
            FruitGrid = TargetGo.transform.Find("Items/StoreGrid/Fruit/FruitGrid");
            OilGrid_Primary = TargetGo.transform.Find("Items/StoreGrid/Oil/ToggleGroup/primarySub/POils/POilGrid");
            OilGrid_Semi= TargetGo.transform.Find("Items/StoreGrid/Oil/ToggleGroup/SemiSub/SEOils/SEmiGrid");
            OilGrid_Senior = TargetGo.transform.Find("Items/StoreGrid/Oil/ToggleGroup/SeniorSub/SOils/SOilGrid");
            PlantFoodGrid = TargetGo.transform.Find("Items/StoreGrid/PlantFood/GameObject/PlantFoodGrid");

//            OilGrid_SeniorTg = TargetGo.transform.Find("Items/StoreGrid/Oil/ToggleGroup/Senior").GetComponent<Toggle>();



            ConfirmView = TargetGo.transform.Find("TotalSellConfirm");
            ConfirmView_SellBtn = TargetGo.transform.Find("TotalSellConfirm/TotalSell").GetComponent<Button>();
            ConfirmView_SellBtn.onClick.AddListener(OnClickConfirmBtn);
            ConfirmView_CancelBtn = TargetGo.transform.Find("TotalSellConfirm/Cancel").GetComponent<Button>();
            ConfirmView_CancelBtn.onClick.AddListener(OnClickCancelBtn);
            ConfirmView_Text =TargetGo.transform.Find("TotalSellConfirm/Text").GetComponent<Text>();
//            OilGrid_Primary = TargetGo.transform.Find("Items/StoreGrid/Oil/ToggleGroup").getco;

            OilTg = TargetGo.transform.Find("Items/ToggleGroup/PlantFood").GetComponent<Toggle>();
            
//            seedTg = TargetGo.transform.Find("Items/ToggleGroup/Seeds").GetComponent<Toggle>();
            seedTg = TargetGo.transform.Find("Items/ToggleGroup/Seeds").GetComponent<Toggle>();
//            seedTg = TargetGo.transform.Find("Items/ToggleGroup/Oil").GetComponent<Toggle>();
//            seedTg = TargetGo.transform.Find("Items/ToggleGroup/Oil").GetComponent<Toggle>();
            ResourceMgr.Instance.LoadResource("Prefab/StoreItem", OnLoadItem);

            isFrist = true;
        }

        private static bool isflag = false;
        public static void Show()
        {
            
            isflag = true;
            ViewMgr.Instance.Open(ViewNames.StoreView);

        }

        public override void OnOpen()
        {
            base.OnOpen();
            if (!isflag)
            {
                seedTg.isOn = true;
                OilTg.isOn = false;
//                OilGrid_SeniorTg.isOn = false;
            }
            else
            {
                seedTg.isOn = false;
                OilTg.isOn = true;
//                OilGrid_SeniorTg.isOn = true;
            }
            
            ConfirmView.gameObject.SetActive(false);
            StoreController.Instance.GetDispatcher().AddListener(StoreEvent.OnStoreUnits, LoadItems);
            StoreController.Instance.ReqStoreInfo(LoginModel.Instance.Uid);
            MusicManager.Instance.Playsfx(AudioNames.OnClick1);
        }

        //点击关闭仓库按钮
        private void OnClickCloseBtn()
        {
            MusicManager.Instance.Playsfx(AudioNames.CloseBtn);
            ViewMgr.Instance.Close(ViewNames.StoreView);
        }

        //点击全部卖出按钮
        private void OnClickTotalSellBtn(int type)
        {
            MusicManager.Instance.Playsfx(AudioNames.OnClick4);

            CurrentTotalSell = type;
            switch (type)
            {
                case 1: ConfirmView_Text.text = "是否确认卖出全部种子?";break;
                case 2: ConfirmView_Text.text = "是否确认卖出全部果实?"; break;
                case 3: ConfirmView_Text.text = "是否确认卖出全部初级精油?"; break;
                case 4: ConfirmView_Text.text = "是否确认卖出全部高级精油半成品?"; break;
                case 5: ConfirmView_Text.text = "是否确认卖出全部高级精油?"; break;
                default:break;
            }
            ConfirmView.gameObject.SetActive(true);
        }

        //点击确认全部卖出按钮
        private void OnClickConfirmBtn()
        {
            MusicManager.Instance.Playsfx(AudioNames.OnClick4);
            StoreController.Instance.TotalSell(CurrentTotalSell);

            ConfirmView.gameObject.SetActive(false);
        }

        //点击取消全部卖出按钮
        private void OnClickCancelBtn()
        {
            MusicManager.Instance.Playsfx(AudioNames.CloseBtn);

            ConfirmView.gameObject.SetActive(false);
        }

        //点击精油全部合成按钮
        private void OnClickTotalExchange()
        {
            ViewMgr.Instance.Open(ViewNames.CommitView);
        }
        
        //加载仓库item的prefab
        private void OnLoadItem(Resource res, bool succ)
        {
            if (succ)
            {
                ItemPrefab = res;
                if (!isFrist)
                {
                    LoadItems(0, null);
                }
            }
            else {
                Debug.Log("加载仓库item的prefab失败！");
            }
        }

        //加载仓库物品
        private bool LoadItems (int eventId,object arg)
        {
            if (ItemPrefab == null)
            {
                isFrist = false;
                return false;
            }
            LoadingImageManager.Instance.StartLoading(TargetGo.transform, TargetGo.transform.position);

            StorageDeltaList store = Farm_Game_StoreInfoModel.storage;
            SetGrid<Seed>(SeedGrid, store.Seeds);
            SetGrid<Result>(FruitGrid, store.Results);
            SetOilGrid(OilGrid_Primary, OilGrid_Semi, OilGrid_Senior, store.Oils,store.Formulas);
            SetGrid<Fertilizer>(PlantFoodGrid, store.Fertilizers);
            SetGrid<Elixir>(PlantFoodGrid, store.Elixirs);
            LoadingImageManager.Instance.StopLoading();

            return false;
        }

        //精油初级高级分类
        private void SetOilGrid(Transform OilGrid_Primary,Transform OilGrid_Semi, Transform OilGrid_Senior, Dictionary<int, Oil> list, Dictionary<int, Formula> fs)
        {
            //清空格子
            for (int i = 0; i < OilGrid_Primary.childCount; i++)
            {
                GameObject go = OilGrid_Primary.GetChild(i).gameObject;
                if (go != null)GameObject.Destroy(go);
                
            }

            for (int i = 0; i < OilGrid_Semi.childCount; i++)
            {
                GameObject go = OilGrid_Semi.GetChild(i).gameObject;
                if (go != null) GameObject.Destroy(go);
            }

            for (int i = 0; i < OilGrid_Senior.childCount; i++)
            {
                GameObject go = OilGrid_Senior.GetChild(i).gameObject;
                if (go != null) GameObject.Destroy(go);
            }
                
            //分类放入格子
            foreach (Oil t in list.Values)
            {
                GameObject go = GameObject.Instantiate(ItemPrefab.UnityObj) as GameObject;
                storeItem item = go.AddComponent<storeItem>();
                    
                item.SetData(t);
                if (t.OilType == 1)
                {
                    go.transform.SetParent(OilGrid_Primary);
                }
                else if (t.OilType == 2)
                {
                    go.transform.SetParent(OilGrid_Semi);
                }
                else if (t.OilType == 3)
                {
                    go.transform.SetParent(OilGrid_Senior);
                }
                go.transform.localScale = new Vector3(1, 1, 1);
            }

            //将神秘配方放入背包高级精油格子
            foreach (Formula f in fs.Values)
            {
                GameObject go = GameObject.Instantiate(ItemPrefab.UnityObj) as GameObject;
                storeItem item = go.AddComponent<storeItem>();
                item.SetData(f);
                go.transform.SetParent(OilGrid_Senior);
                go.transform.localScale = new Vector3(1, 1, 1);
            }
        }

        //根据数据，生成storeItem，加入对应的grid
        private void SetGrid<T>(Transform gridTr,Dictionary<int,T> list) where T:BaseObject
        {
            if (typeof(T) != typeof(Elixir))
            {
                for (int i = 0; i < gridTr.childCount; i++)
                {
                    GameObject go = gridTr.GetChild(i).gameObject;
                    if (go != null) GameObject.Destroy(go);
                }
            }

            foreach (T t in list.Values)
            {
                GameObject go = GameObject.Instantiate(ItemPrefab.UnityObj) as GameObject;
                storeItem item = go.AddComponent<storeItem>();
                item.SetData(t);
                    
                go.transform.SetParent(gridTr);
                go.transform.localScale = new Vector3(1, 1, 1);
            }
        }

        public override void OnClose()
        {
            base.OnClose();
            isflag = false;
            StoreController.Instance.GetDispatcher().RemoveListener(StoreEvent.OnStoreUnits,LoadItems);
        }
    }
}
