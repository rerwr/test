using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class SeedBarView:BaseSubView
    {
        private bool isFristOpen;
        private Resource ItemPrefab;
        private Transform SeedGrid;
     
        private ToggleGroup toggleGroup;
      
        private static Tweener t;
        public SeedBarView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {

        }

        public override void BuildSubViews()
        {
//            base.BuildSubViews();
            subViews=new List<BaseSubView>();
            subViews.Add(this);
        }

        public override void BuildUIContent()
        {
            SeedGrid = TargetGo.transform.Find("ScrollView/Viewport/SeedGrid");
            toggleGroup = TargetGo.transform.Find("ScrollView/Viewport/SeedGrid").GetComponent<ToggleGroup>();
            ResourceMgr.Instance.LoadResource("Prefab/SeedItem", OnLoadShopItem);
            isFristOpen = true;
            t = TargetGo.transform.DOMoveX(-0.42f, 0.3f).SetEase(Ease.Flash).SetAutoKill(false);
            t.onRewind += () =>
            {
                ViewMgr.Instance.Close(ViewNames.SeedBarView);
            };
          
        }

        public override void OnOpen()
        {
            t.PlayForward();
            StoreController.Instance.ReqStoreInfo(LoginModel.Instance.Uid);

            GlobalDispatcher.Instance.AddListener(GlobalEvent.OnStoreUnitsChange, InitShopGrid);



//            StoreController.Instance.GetDispatcher().AddListener(StoreEvent.OnStoreUnits, InitShopGrid);
           

            MusicManager.Instance.Playsfx(AudioNames.OnClick1);
        
        }

        public override void OnClose()
        {
            GlobalDispatcher.Instance.RemoveListener(GlobalEvent.OnStoreUnitsChange, InitShopGrid);

            //            base.OnClose();
            //            StoreController.Instance.GetDispatcher().RemoveListener(StoreEvent.OnStoreUnits, InitShopGrid);

        }

        public static void PlayBack()
        {
            if (t!=null)
            {
                t.PlayBackwards();
            }
         
        }

        //private bool isLoading = false;
        //初始化商店各商品
        private bool InitShopGrid(int eventId, object arg)
        {
            isFristOpen = false;
            if (ItemPrefab == null)
            {
                return false;
            }
            if (arg!=null)
            {
                List<DeltaStoreUnit> deltas = arg as List<DeltaStoreUnit>;
                //变化集是否有种子
                bool hasSeed = false;
                for (int i = 0; i < deltas.Count; i++)
                {


                    if (LoadObjctDateConfig.Instance.GetAtrribute(deltas[i].Id).Type == ObjectType.Seed)
                    {
                        Debug.Log(string.Format("<color=#ffffffff><-有种子变化--{0}-{1}-{2}---></color>", LoadObjctDateConfig.Instance.GetAtrribute(deltas[i].Id).Name,deltas[i].Deltacount1, deltas[i].Id));

                        //有种子
                        hasSeed = true;
                        break; ;
                    }

                }
                //没有种子则结束
                if (!hasSeed)
                {
                    return false;
                }
            }
           
            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "进入", "test1"));

            Dictionary<int ,Seed> seeds = Farm_Game_StoreInfoModel.storage.Seeds;
          
            AddGrid<Seed>(SeedGrid, seeds);
           

            return false;
        }

        //根据不同类型添加商品item，显示名字、价格等
        private void AddGrid<T>(Transform gridTr, Dictionary<int, T> list) where T : BaseObject
        {
            for (int i = 0; i < gridTr.childCount; i++)
            {
                GameObject.Destroy(gridTr.GetChild(i).gameObject);

//                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", gridTr.childCount, "test1"));

            }
          
            foreach (T t in list.Values)
            {
                GameObject go = GameObject.Instantiate(ItemPrefab.UnityObj) as GameObject;
                SeedItem item = go.AddComponent<SeedItem>();
                item.SetData(t);
                ToggleX tx=go.GetComponent<ToggleX>();
                tx.group = gridTr.GetComponent<ToggleGroup>();
                tx.onValueChanged.AddListener((arg0 =>SeedActionModel.currentId_Seed=item.ID ));
                go.transform.SetParent(gridTr);
                
                go.transform.localScale = new Vector3(1, 1, 1);

                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", t.Name, t.ObjectNum));

            }

            if (!SeedGrid.transform.GetComponent<Director>())
            {
                SeedGrid.transform.gameObject.AddComponent<Director>();
            }
            Director d=Director.Instance;
            d.ResetAllbtns();
            
            Director.Instance.StartAnima(0.08f);
         //   isLoading = false;
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
            else
            {
                Debug.LogError("加载商店物品item的prefab失败！");
            }
        }

        //点击关闭商店按钮
        private void OnClickCloseBtn()
        {
            MusicManager.Instance.Playsfx(AudioNames.CloseBtn);
            ViewMgr.Instance.Close(ViewNames.ShopView);
        }

     
    }
}
