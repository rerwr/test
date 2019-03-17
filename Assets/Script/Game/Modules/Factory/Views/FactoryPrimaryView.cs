using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using UnityEngine.UI;

namespace Game
{
    public class FactoryPrimaryView : BaseSubView
    {
        private Transform MaterialList;
        private Transform OilList;
        private Transform NotEnoughView;
        private Button NotEnoughCloseBtn;

        private Button BtnProduce;
        private InputField CountInput;
        private Button AddBtn;
        private Button ReduceBtn;

        private GameObject fcItemPrefab;
        private GameObject POilItem;
        private UI_Control_ScrollFlow scrollF;

        private bool isFrist;

        public FactoryPrimaryView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
        }

        public override void BuildUIContent()
        {
            base.BuildUIContent();

            MaterialList = TargetGo.transform.Find("MeterialsGrid/Materials");
            OilList = TargetGo.transform.Find("Oil");
            NotEnoughView = TargetGo.transform.Find("NotEnoughView");
            NotEnoughView.gameObject.SetActive(false);
            NotEnoughCloseBtn = TargetGo.transform.Find("NotEnoughView/CloseBtn").GetComponent<Button>();
            NotEnoughCloseBtn.onClick.AddListener(OnClickNotEnoughCloseBtn);

            BtnProduce = TargetGo.transform.Find("ProduceBtn").GetComponent<Button>();
            BtnProduce.onClick.AddListener(OnClickProduceBtn);
            CountInput = TargetGo.transform.Find("CountInput").GetComponent<InputField>();
            CountInput.onEndEdit.AddListener(OnCountInputChange);
            AddBtn = TargetGo.transform.Find("CountInput/AddBtn").GetComponent<Button>();
            AddBtn.onClick.AddListener(delegate() {this.OnClickChangeCount(1); });
            ReduceBtn = TargetGo.transform.Find("CountInput/ReduceBtn").GetComponent<Button>();
            ReduceBtn.onClick.AddListener(delegate () { this.OnClickChangeCount(-1); });

            scrollF = OilList.GetComponent<UI_Control_ScrollFlow>();
            ResourceMgr.Instance.LoadResource("Prefab/FcItem",OnLoadPrefab);
            ResourceMgr.Instance.LoadResource("Prefab/POilItem", OnLoadOilPrefab);
            
            isFrist = true;
        }

        public override void OnOpen()
        {

            base.OnOpen();
            CountInput.text = "0";
            StoreController.Instance.ReqStoreInfo(LoginModel.Instance.Uid);
            StoreController.Instance.GetDispatcher().AddListener(StoreEvent.OnStoreUnits, InitFactory);
        }

        //加载工厂item预制回调
        private void OnLoadPrefab(Resource res,bool succ)
        {
            if (succ)
            {
                fcItemPrefab = (GameObject)res.UnityObj;
                if(!isFrist) InitFactory(0,null);
            }
        }

        private void OnLoadOilPrefab(Resource res, bool succ)
        {
            if (succ)
            {
                POilItem = (GameObject)res.UnityObj;
                InitOilPanel();
            }
        }

        //初始化工厂
        private bool InitFactory(int eventId,object arg)
        {
            LoadingImageManager.Instance.StartLoading(TargetGo.transform, TargetGo.transform.position);

            if (fcItemPrefab == null)
            {
                isFrist = false;
                return false;
            }
            InitMaterials();
            InitOils();
            LoadingImageManager.Instance.StopLoading();

            return false;
        }

        //初始化果实材料
        private void InitMaterials()
        {
            for (int i = 0; i < MaterialList.childCount; i++)
            {
                GameObject go = MaterialList.GetChild(i).gameObject;
                GameObject.Destroy(go);
            }


            Dictionary<int, Result> results = Farm_Game_StoreInfoModel.storage.Results;
            foreach (Result r in results.Values)
            {
                GameObject go = GameObject.Instantiate(fcItemPrefab) as GameObject;
                FcItem fcitem = go.AddComponent<FcItem>();
                fcitem.SetData(r);

                go.transform.SetParent(MaterialList);
                go.transform.localScale = new Vector3(1, 1, 1);
            }

        }
        
        //初始化要合成的精油(计算数量)
        private void InitOils()
        {
            for (int i = 0; i < OilList.childCount; i++)
            {
                Transform tran = OilList.GetChild(i);
                UI_Control_ScrollFlow_Item item = tran.GetComponent<UI_Control_ScrollFlow_Item>();
                if (item != null&&item.NeedIds.Count>0)
                {
                    int needId = item.NeedIds[0].id;
                    int count = 0;
                    if (Farm_Game_StoreInfoModel.storage.Results.ContainsKey(needId))
                    {
                        Result result=Farm_Game_StoreInfoModel.storage.Results[needId];
                        count = result.ObjectNum / result.UpGradeToOilNum;
                        item.Count = count;
                    }
                    tran.Find("Count").GetComponent<Text>().text="x "+count.ToString();
                }
            }
        }

        //初始化要加工的精油的面板信息，只初始化一次
        private void InitOilPanel()
        {
            int _id = 301;
            UI_Control_ScrollFlow ui_control = OilList.GetComponent<UI_Control_ScrollFlow>();
            while (LoadObjctDateConfig.Instance.GetAtrribute(_id) != null)
            {
                GameObject oilItem = GameObject.Instantiate(POilItem) as GameObject;
                oilItem.transform.SetParent(OilList);
                oilItem.transform.localPosition = new Vector3(0, 23.5f, 0);
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

            int userId = LoginModel.Instance.Uid;
            int produceId = scrollF.Current.ID;
            int pattern = 1;
            int count=0;
            int.TryParse(CountInput.text,out count);
            if (count <= 0) return;
            FactoryController.Instance.pattern = 1;
            FactoryController.Instance.OilUpgradeReq(userId, produceId,count, pattern);

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
                if (count <0) count = 0;
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
            StoreController.Instance.GetDispatcher().RemoveListener(StoreEvent.OnStoreUnits, InitFactory);
        }
    }
}
