using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DG.Tweening;
using Framework;
using Game;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{

    public class FarmUnit : WorldObject
    {
        private bool dirty;
        private int xposi;			//该田建立在地图上的位置点 x�?以地图分格为单位
        private int yposi;            //该田建立在地图上的位置点 y	�?,2）表示当前地块第一行，�?�?可知范围�?,1�?>�?,6�?
        private int farmID;             //农田id

        private int _enablePlant = 100;//农田是否能种植，表示是否开�?
        private int _reclaimCoin;//如果未开垦，则开垦时需要的金币数量
        private int _startPlant;//植株种植开始时�?
        //植物
        private Plant plant;
        //种植牌子
        private Brand brand;



        private List<int> plantIDs = new List<int>();

        private static bool isStart;
        /// <summary>
        /// 已经发送要进行操作的土地ID
        /// </summary>
        public static int SeletedFarmID;

        /// <summary>
        /// 已经发送要选择的动�?
        /// </summary>

        #region MyRegion
        public int Xposi
        {
            get { return xposi; }
            set { xposi = value; }
        }

        public int Yposi
        {
            get { return yposi; }
            set { yposi = value; }
        }

        public int EnablePlant
        {
            get { return _enablePlant; }
            set
            {
                if (value != _enablePlant)
                {
                    RelashLand(value);
                }
                _enablePlant = value;
            }
        }

        public int ReclaimCoin
        {
            get { return _reclaimCoin; }
            set { _reclaimCoin = value; }
        }

        public int StartPlant
        {
            get { return _startPlant; }
            set { _startPlant = value; }
        }

        public Plant Plant
        {
            get { return plant; }
            set { plant = value; }
        }

        public bool Dirty
        {
            get { return dirty; }
            set { dirty = value; }
        }

        public int FarmID
        {
            get { return farmID; }
            set { farmID = value; }
        }

        public Brand Brand
        {
            get { return brand; }
            set { brand = value; }
        }

        #endregion


        ~FarmUnit()
        {

            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "销毁农田", "test1"));

        }
        /// <summary>
        /// 移除植物
        /// </summary>
        public void RemovePlant()
        {
            if (Plant!=null&&Plant.Renderer)
            {
                 GameObject.Destroy(Plant.Go);
           

            }
            this.Plant = null;

        }
        /// <summary>
        /// 收获植物    
        /// </summary>
        /// <param name="num">收获数量</param>
        public void PluckPlant(int num)
        {
            if (plant != null && plant.Go && plant.CurrentType == 4)
            {
                MusicManager.Instance.Playsfx(AudioNames.HarvestPlant);
                ResourceMgr.Instance.LoadResource("Prefab/Count", ((resource, b) =>
                 {
                     GameObject txt = GameObject.Instantiate(resource.UnityObj as GameObject);
                     txt.GetComponent<Text>().text = "×" + num;
                     txt.transform.SetParent(ViewMgr.Instance.FullScreenRoot);
                     if (FriendFarmManager.Instance.isVisiting)
                     {
                         plant.destroyPlant();

                         GameObject go = GameObject.Instantiate(plant.Go);
                         go.transform.position = plant.Go.transform.position;
                         go.transform.GetChild(0).transform.gameObject.SetActive(false);
                         go.transform.GetChild(1).transform.gameObject.SetActive(false);

                         Tweener t = go.transform.DOMoveY(1.5f, 1.2f).SetEase(Ease.Flash).SetRelative();
                         go.GetComponent<SpriteRenderer>().sortingOrder++;
                         go.GetComponent<SpriteRenderer>().DOFade(0.6f, 1.2f).PlayForward();
                         MTRunner.Instance.StartRunner(Update(txt, go,t));
//                         MTRunner.Instance.StartRunner(Wait(txt, go, t));
                         t.onComplete = (() =>
                         {
                             UnityEngine.Object.Destroy(go);
                             UnityEngine.Object.Destroy(txt);

                         });

                     }
                     else
                     {
                       
                             plant.destroyPlant();
                             plant.DestoryShadow();
                             Tweener t = this.plant.Go.transform.DOMoveY(1.5f, 1.2f).SetEase(Ease.Flash).SetRelative();

                             this.plant.Renderer.DOFade(0.6f, 1.2f).PlayForward();
                             plant.Renderer.sortingOrder++;

                             MTRunner.Instance.StartRunner(Update(txt, plant.Go, t));
//                                                      MTRunner.Instance.StartRunner(Wait(txt, plant.Go, t));
                             t.onComplete = (() =>
                             {
                                 RemovePlant();
                                 UnityEngine.Object.Destroy(txt);

                             });
                         
                      

                     }
                 }));


            }
//            GC.Collect();
        }

        IEnumerator Update(GameObject txt, GameObject go, Tweener t)
        {
            while (true)
            {
                yield return MTRunner.TYPE_Upate;
                if (txt&&go)
                {
                    Vector3 pos = go.transform.position;
                    txt.transform.position = Camera.main.WorldToScreenPoint(new Vector3(pos.x + 0.3f, pos.y + 0.1f));
                }
                else
                {
                    break;
                }
             
//                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", t.IsComplete(), t.IsBackwards()));

            }
            
        }
        private int i = 0;
   

        public override void OnClicked(WorldObject worldObject)
        {
            base.OnClicked(worldObject);


            //开始进入播种模�?&& this.Brand == null
            if (CommonActionBarView.Action1 == GameAction.None  && this.Plant == null )
            {
                if (!FriendFarmManager.Instance.isVisiting)
                {
                    //打开播种栏
                    ViewMgr.Instance.Open(ViewNames.SeedBarView);
                    if (ViewMgr.Instance.isOpen(ViewNames.FriendsListView))
                    {
                        ViewMgr.Instance.Close(ViewNames.FriendsListView);

                    }
                }

            }
            //判断翻地清除植物
            if (FieldsController.ProtocalAction == ProtocalAction.None && CommonActionBarView.Action1 == GameAction.Plow && this.Plant != null && this.EnablePlant == 1)
            {
                SystemMsgView.SystemFunction(Function.OpenDialog, Info.Reclaim, null, (delegate
                   {
                       FieldsController.Instance.ReqSenPlowdAction(this.FarmID);
                       loadAni("Animation/fandi");
                   }));

            }

            if (this.Plant == null)
            {
                //当前地块没有植物,进入无模�?
                CommonActionBarView.Action1 = GameAction.None;
                if (ViewMgr.Instance.isOpen(ViewNames.TimeBarView))
                {
                    ViewMgr.Instance.Close(ViewNames.TimeBarView);
                }
            }

            //施肥浇水除虫除草
            if (FieldsController.ProtocalAction == ProtocalAction.None && (CommonActionBarView.Action1 == GameAction.weed || CommonActionBarView.Action1 == GameAction.Debug || CommonActionBarView.Action1 == GameAction.Fertitlize || CommonActionBarView.Action1 == GameAction.Water))
            {
                SeletedFarmID = FarmID;

                switch (CommonActionBarView.Action1)
                {
                    case GameAction.Debug:
                        if (this.plant.IsWorm == 1)
                        {
                            loadAni("Animation/chuchong");
//                            MusicManager.Instance.Playsfx(AudioNames.Debug);
                            FieldsController.Instance.SendWFActionReq(this.FarmID, CommonActionBarView.Action1, 701);
                            FieldsController.ProtocalAction = (ProtocalAction)((int)CommonActionBarView.Action1);

                        }
                        else
                        {
                            SystemMsgView.SystemFunction(Function.Tip, Info.PlantNoBug);
                        }
                        break;
                    case GameAction.Fertitlize:
                        //                        loadAni("Animation/chuchong");
                        //    0                    FieldsController.ProtocalAction =ProtocalAction.Fertitlize;
                        //
                        //                        MusicManager.Instance.Playsfx(AudioNames.Fertilizer);
                        break;
                    case GameAction.Water:
                        if (this.plant.IsWater == 1)
                        {
                            loadAni("Animation/jiaoshui");

//                            MusicManager.Instance.Playsfx(AudioNames.Water);

                            FieldsController.Instance.SendWFActionReq(this.FarmID, CommonActionBarView.Action1, 701);
                            FieldsController.ProtocalAction = (ProtocalAction)((int)CommonActionBarView.Action1);

                        }
                        else
                        {
                            SystemMsgView.SystemFunction(Function.Tip, Info.PlantNoNeedWater);

                        }
                        break;
                    case GameAction.weed:

                        if (this.plant.IsGrass == 1)
                        {
                            loadAni("Animation/chucao");

//                            MusicManager.Instance.Playsfx(AudioNames.Weed);
                            FieldsController.Instance.SendWFActionReq(this.FarmID, CommonActionBarView.Action1, 701);
                            FieldsController.ProtocalAction = (ProtocalAction)((int)CommonActionBarView.Action1);
                        }
                        else
                        {
                            SystemMsgView.SystemFunction(Function.Tip, Info.PlantNoNeedWeed);

                        }
                        break;
                }
            }
            //如果当前已经打开播种栏，则可以播�?&& this.Brand == null
            if (FieldsController.ProtocalAction == ProtocalAction.None && this.EnablePlant == 1 &&
                this.Plant == null  & ViewMgr.Instance.isOpen(ViewNames.SeedBarView) && SeedActionModel.currentId_Seed != 0)
            {
                //
                if (Farm_Game_StoreInfoModel.storage.Seeds.ContainsKey(SeedActionModel.currentId_Seed))
                {
                    SeletedFarmID = this.farmID;
                    FieldsController.Instance.ReqSeedAction(this.farmID, SeedActionModel.currentId_Seed);
                }
                
            }
            else if ((this.Plant != null || this.Brand != null)&&CommonActionBarView.Action1==GameAction.None)
            {
                //复位
                SeedBarView.PlayBack();

                SeedActionModel.currentId_Seed = 0;
                //当前时间条不等于4
                if (this.Plant != null)
                {
                    GlobalDispatcher.Instance.AddListener(GlobalEvent.OnViewLoadFinished, Flash);
                    GlobalDispatcher.Instance.DispathDelay(GlobalEvent.OnFarmUnitClick, plant);


                    ViewMgr.Instance.Open(ViewNames.TimeBarView);

                }
            }
            //如果单个植物可以收获
            if (FieldsController.ProtocalAction == ProtocalAction.None && CommonActionBarView.Action1 == GameAction.None && plant != null && plant.CurrentType == 4)
            {

                if (FriendFarmManager.Instance.isVisiting == false)
                {
                    Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "FieldId", FarmID));

                    FieldsController.Instance.SendPluckReq(this.FarmID, LoginModel.Instance.Uid);

                }
                else
                {
                    if (plant.IsSteal == 0)
                    {
                        Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "FieldId", FarmID));

                        FieldsController.Instance.SendPluckReq(this.FarmID, FriendFarmManager.Instance.FriendUid);
                    }

                }


            }
        }

        private void loadAni(string path)
        {
            ResourceMgr.Instance.LoadResource(path, (delegate (Resource resource, bool b)
            {
                if (resource != null)
                {
                    GameObject go1 = GameObject.Instantiate(resource.UnityObj) as GameObject;
                    go1.AddComponent<AnimationTimer>();
                    if (MusicManager.Instance.IsMute&& go1.GetComponent<AudioSource>())
                    {
                        go1.GetComponent<AudioSource>().mute = true;


                    }
                    Vector3 pos = Go.transform.position;
                    if (path.Contains("chuchong") || path.Contains("chucao"))
                    {
                        go1.transform.position = new Vector3(pos.x + 0.6f, pos.y + 0.1f, pos.z);

                    }
                    else
                    {
                        go1.transform.position = new Vector3(pos.x + 0.6f, pos.y + 0.5f, pos.z);

                    }
                }

            }));
        }
        public override void OnTouchEnd(WorldObject worldObject)
        {

            base.OnTouchEnd(worldObject);


        }

        bool Flash(int id, object arg)
        {
            if (arg == null)
            {
                return false;
            }
            GameObject go = (GameObject)arg;

            if (go && go.name.Contains(ViewNames.TimeBarView))
            {

                GlobalDispatcher.Instance.DispathDelay(GlobalEvent.OnFarmUnitClick, plant);

            }
            return true;
        }

        //开始点击到农田
        public override void OnTouchBegin(WorldObject worldObject)
        {
            base.OnTouchBegin(worldObject);


        }

        //刷新土地
        private void RelashLand(int pattern)
        {
            if (pattern == 1)
            {
                ResourceMgr.Instance.LoadResource("UI/f1", (
                    delegate (Resource resource, bool b)
                    {
                        Texture2D o = resource.UnityObj as Texture2D;
                        ;
                        Sprite sprite = Sprite.Create(o, new Rect(0, 0, o.width, o.height), new Vector2(0.5f, 0.5f));
                        Renderer.sprite = sprite;

                    }));
            }
            else if (pattern == 0)
            {
                ResourceMgr.Instance.LoadResource("UI/f0", ((resource, b) =>
                {
                    Texture2D o = resource.UnityObj as Texture2D;
                    
                    Sprite sprite = Sprite.Create(o, new Rect(0, 0, o.width, o.height), new Vector2(0.5f, 0.5f));
                    Renderer.sprite = sprite;

                }));
            }
        }


    }
}
