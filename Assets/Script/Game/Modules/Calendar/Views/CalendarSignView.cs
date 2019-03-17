using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;
using Game.monos;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class CalendarSignView : ShopView
    {
        private int continuedays = 0;
        private Button closeButton;
        private Button signButton;
        private bool isFristOpen;
        private Resource ItemPrefab;

        private GridLayoutGroup container;
        private List<SignItem> items = new List<SignItem>();
        public CalendarSignView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {

        }

        public int Continuedays
        {
            get
            {
                return continuedays;
            }
            set
            {
                continuedays = value;
            }
        }


        public override void BuildUIContent()
        {

            signButton = TargetGo.transform.Find("SignBtn").GetComponent<Button>();
            closeButton = TargetGo.transform.Find("closeBtn").GetComponent<Button>();

            container = TargetGo.transform.Find("Container").GetComponent<GridLayoutGroup>();
            closeButton.onClick.AddListener(Close);
            signButton.onClick.AddListener(Sign);
            ResourceMgr.Instance.LoadResource("Prefab/SignItem", OnLoadShopItem);

        }

        public override void OnClose()
        {
            base.OnClose();
            GlobalDispatcher.Instance.RemoveListener(GlobalEvent.OnStoreUnitsChange, Reflash);

        }
        public override void OnOpen()
        {

            MusicManager.Instance.Playsfx(AudioNames.OnClick6);

            GlobalDispatcher.Instance.AddListener(GlobalEvent.OnStoreUnitsChange, Reflash);
            //            MTRunner.Instance.StartRunner(Wait(3));
            InitShopGrid(0, null);

        }

        //
        //        IEnumerator Wait(float time)
        //        {
        //            yield return time;
        //            Reflash(1, null);
        //        }
        private bool Reflash(int id, object arg)
        {

            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", FieldsController.ProtocalAction.ToString(), "test1"));

            LoginModel.Instance.ContinueDays++;
            Continuedays++;
            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", LoginModel.Instance.ContinueDays, "test1"));

            InitShopGrid(0, null);

            return false;
        }
        //初始化商店各商品
        private bool InitShopGrid(int eventId, object arg)
        {
          

            if (ItemPrefab == null)
            {
                return false;
            }
        
            AddGrid(container.transform, SignModel.Instance.awardUnits);
  
            return false;
        }

        //根据不同类型添加商品item，显示名字、价格等
        private void AddGrid(Transform gridTr, List<AwardUnit> list)
        {
            foreach (Transform VARIABLE in gridTr)
            {
                GameObject.Destroy(VARIABLE.gameObject);
            }
            
            for (int i = 0; i < 7; i++)
            {
                GameObject go = GameObject.Instantiate(ItemPrefab.UnityObj) as GameObject;
                SignItem item = go.AddComponent<SignItem>();
                item.SetData(list[i]);
                
                items.Add(item);

                go.transform.SetParent(gridTr);
                go.transform.localScale = new Vector3(1, 1, 1);

                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", i, Continuedays-1));

                if (i <= Continuedays-1)
                {
                    item.ShowGetMask();
                }
            }

        }

        //加载商品item的prefab
        private void OnLoadShopItem(Resource res, bool succ)
        {
            if (succ)
            {
                    ItemPrefab = res;
                    Continuedays = LoginModel.Instance.ContinueDays;
                    InitShopGrid(0, SignModel.Instance.awardUnits);
                
            }
            else
            {
                Debug.LogError("加载商店物品item的prefab失败！");
            }
        }

        private void Sign()
        {
            
            SignController.Instance.SignReq();

        }

        bool LoadData(int id, object arg)
        {

            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", id, "test1"));

            return false;
        }
        private void Close()
        {
            ViewMgr.Instance.Close(ViewNames.CalendarView);

        }
    }
}
