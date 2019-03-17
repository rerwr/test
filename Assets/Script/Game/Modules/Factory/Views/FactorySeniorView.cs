using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework;

namespace Game
{
    public class FactorySeniorView : BaseSubView
    {
        private Transform MaterialList;
        private Transform OilList;

        private Button BtnProduce;

        private GameObject fcItemPrefab;
        private UI_Control_ScrollFlow scrollF;
        
        private Transform NotEnoughView;
        private Button NotEnoughCloseBtn;
        private InputField CountInput;
        private Button AddBtn;
        private Button ReduceBtn;
        
        private GameObject SEOilItem;

        private bool isFrist;

        public FactorySeniorView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
        }

        public override void BuildUIContent()
        {
            base.BuildUIContent();

            MaterialList = TargetGo.transform.Find("MeterialsGrid/Materials");
            OilList = TargetGo.transform.Find("Oil");

            BtnProduce = TargetGo.transform.Find("ProduceBtn").GetComponent<Button>();
            BtnProduce.onClick.AddListener(OnClickProduceBtn);

            scrollF = OilList.GetComponent<UI_Control_ScrollFlow>();
            scrollF.NeedGrid = TargetGo.transform.Find("Need/NeedGrid");
            scrollF.ImageTr = TargetGo.transform.Find("Need/Image");

            NotEnoughView = TargetGo.transform.Find("NotEnoughView");
            NotEnoughView.gameObject.SetActive(false);
            NotEnoughCloseBtn = TargetGo.transform.Find("NotEnoughView/CloseBtn").GetComponent<Button>();
            NotEnoughCloseBtn.onClick.AddListener(OnClickNotEnoughCloseBtn);

            CountInput = TargetGo.transform.Find("CountInput").GetComponent<InputField>();
            CountInput.onEndEdit.AddListener(OnCountInputChange);
            AddBtn = TargetGo.transform.Find("CountInput/AddBtn").GetComponent<Button>();
            AddBtn.onClick.AddListener(delegate () { this.OnClickChangeCount(1); });
            ReduceBtn = TargetGo.transform.Find("CountInput/ReduceBtn").GetComponent<Button>();
            ReduceBtn.onClick.AddListener(delegate () { this.OnClickChangeCount(-1); });

            ResourceMgr.Instance.LoadResource("Prefab/FcItem", OnLoadPrefab);
            ResourceMgr.Instance.LoadResource("Prefab/POilItem", OnLoadOilPrefab);
            ResourceMgr.Instance.LoadResource("Prefab/NeedItem", (res,succ)=> 
            {
                if (succ)
                {
                    scrollF.NeedItem = (GameObject)res.UnityObj;
                    scrollF.RefreshNeedItems();
                }
            });
            isFrist = true;
        }

        public override void OnOpen()
        {
            base.OnOpen();
            CountInput.text = "0";
            TargetGo.SetActive(false);
            StoreController.Instance.GetDispatcher().AddListener(StoreEvent.OnStoreUnits, InitSFc);
        }

        //加载工厂item预制回调
        private void OnLoadPrefab(Resource res, bool succ)
        {
            if (succ)
            {
                fcItemPrefab = (GameObject)res.UnityObj;
                if (!isFrist)
                {
                    InitSFc(0, null);
                }
            }
        }

        private void OnLoadOilPrefab(Resource res, bool succ)
        {
            if (succ)
            {
                SEOilItem = (GameObject)res.UnityObj;
                InitOilPanel();
            }
        }

        //初始化高级工厂
        private bool InitSFc(int eventId,object arg)
        {
            if (fcItemPrefab == null)
            {
                isFrist = false;
                return false;
            }

            InitPOilMaterial();
            InitSOilList();
            return false;
        }

        //初始化初级精油的数量列表
        private void InitPOilMaterial()
        {
            for (int i = 0; i < MaterialList.childCount; i++)
            {
                GameObject go = MaterialList.GetChild(i).gameObject;
                GameObject.Destroy(go);
            }

            Dictionary<int, Oil> oils = Farm_Game_StoreInfoModel.storage.Oils;
            foreach (Oil o in oils.Values)
            {
                if (o.OilType == 1)//取到初级精油
                {
                    GameObject go = GameObject.Instantiate(fcItemPrefab) as GameObject;
                    FcItem fcitem = go.AddComponent<FcItem>();
                    fcitem.SetData(o);

                    go.transform.SetParent(MaterialList);
                    go.transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }

        //初始化要合成的高级精油列表
        private void InitSOilList()
        {
            for (int i = 0; i < OilList.childCount; i++)
            {
                Transform tran = OilList.GetChild(i);
                UI_Control_ScrollFlow_Item item = tran.GetComponent<UI_Control_ScrollFlow_Item>();
                if (item != null&&item.NeedIds.Count>0)
                {
                    int needId = item.NeedIds[0].id;
                    int count = 0;
                    if (Farm_Game_StoreInfoModel.storage.Oils.ContainsKey(needId))
                    {
                        Oil poil = Farm_Game_StoreInfoModel.storage.Oils[needId];
                        count = poil.ObjectNum / poil.CombinCount;
                    }
                    item.Count = count;
                    tran.Find("Count").GetComponent<Text>().text = "x " + item.Count.ToString();
                }
            }
        }

        private void InitOilPanel()
        {
            int _id = 401;
            UI_Control_ScrollFlow ui_control = OilList.GetComponent<UI_Control_ScrollFlow>();
            while (LoadObjctDateConfig.Instance.GetAtrribute(_id) != null)
            {
                GameObject oilItem = GameObject.Instantiate(SEOilItem) as GameObject;
                oilItem.transform.SetParent(OilList);
                oilItem.transform.localPosition = new Vector3(0, 65.75f, 0);
                UI_Control_ScrollFlow_Item item = oilItem.GetComponent<UI_Control_ScrollFlow_Item>();
                item.ID = _id;
                _id++;
            }
            
            OilList.GetChild(OilList.childCount - 2).SetSiblingIndex(0);
            OilList.GetChild(OilList.childCount - 1).SetSiblingIndex(1);
            ui_control.Refresh();
        }

        //点击加工按钮
        private void OnClickProduceBtn()
        {
            if (scrollF.Current.Count <= 0)
            {
                NotEnoughView.gameObject.SetActive(true);
                return;
            }

            int produceId;
            produceId = scrollF.Current.ID;

            int userId = LoginModel.Instance.Uid;
            int count;
            int.TryParse(CountInput.text,out count);
            if (count <= 0) return;
            FactoryController.Instance.pattern = 2;
            FactoryController.Instance.OilUpgradeReq(userId,produceId,count,1);

            MusicManager.Instance.Playsfx(AudioNames.OnClick4);
        }

        private void OnCountInputChange(string str)
        {
            int count;
            if (int.TryParse(str, out count))
            {
                if (count < 1 || count > scrollF.Current.Count)
                {
                    CountInput.text = "0";
                }
                else
                {
                    CountInput.text = count.ToString();
                }
            }
            else
            {
                CountInput.text = "0";
            }
        }
        //点击加减号更改合成数量
        private void OnClickChangeCount(int x)
        {
            int count;
            if (int.TryParse(CountInput.text, out count))
            {
                count += x;
                if (count < 1) count = 0;
                else if (count > scrollF.Current.Count) count = scrollF.Current.Count;
            }
            else
            {
                count = 0;
            }
            CountInput.text = count.ToString();

            MusicManager.Instance.Playsfx(AudioNames.OnClick3);
        }

        private void OnClickNotEnoughCloseBtn()
        {
            NotEnoughView.gameObject.SetActive(false);
        }

        public override void OnClose()
        {
            base.OnClose();
            StoreController.Instance.GetDispatcher().RemoveListener(StoreEvent.OnStoreUnits, InitSFc);
        }
    }
}
