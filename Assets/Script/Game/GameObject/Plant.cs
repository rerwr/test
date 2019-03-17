using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using Framework;
using Game;
using UnityEngine;

namespace Game
{
    public class Plant : WorldObject
    {
 

        private int _currentType = -1;//当前的种植状态，0未种植1发芽 2成株3开花4结果5成熟6枯萎，客户端根据当前当前状态显示不同的植物,10表示没有状态
        private int _grothTime;                 //农作物生长的周期，单位秒

        private double _remainTime;                //植物成熟剩余的时间

        private long startTime;	// 种下种子时的时间戳
        private int isSteal;		// 植物是否被偷取过，每个植物只能被偷取一次0表示没有，1表示已经偷取过
        private int isWorm = -1;		// 是否长虫了
        private int isGrass = -1;   // 是否长草
        private int isWater = -1;   // 是否缺水
        private Tweener t;
        private Catch cCatch;
        private int _farmId;

        private SpriteRenderer ResultSpriteRenderer;


        private GameObject WormSpriteRenderer;


        private GameObject GrassSpriteRenderer;

        private GameObject LessWaterSpriteRenderer;

        private SpriteRenderer shadowRenderer;
        private string _typeName;
        public double HasGrouthTime = 0;
        public double percentage = 0;

        public Plant()
        {
            GlobalDispatcher.Instance.AddListener(GlobalEvent.OnLoadingGetIcon, OnLoadEndPlayICON);

        }

        ~ Plant()
        {
//            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "销毁一个植物", Name));
            
        }

        public override string ToString()
        {
            return string.Format("{0}, CurrentType: {1}, GrothTime: {2}, RemainTime: {3}, StartTime: {4}, IsSteal: {5}, IsWorm: {6}, IsGrass: {7}, TypeName: {8}, IsWater: {9}, FarmID: {10}", base.ToString(), CurrentType, GrothTime, RemainTime, StartTime, IsSteal, IsWorm, IsGrass, TypeName, IsWater, FarmID);
        }

        /// <summary>
        /// 加载完后以上一一下
        /// </summary>
        private bool OnLoadEndPlayICON(int id, object arg)
        {

            if (t != null && t.IsPlaying())
            {
                t.Restart();

            }

            return false;
        }

        public void destroyPlant()
        {
            GlobalDispatcher.Instance.RemoveListener(GlobalEvent.OnLoadingGetIcon, OnLoadEndPlayICON);
            GlobalDispatcher.Instance.RemoveListener(GlobalEvent.OnOneSecond, ReflashTime);


            DisposeBadIcon();
         
        }
        /// <summary>
        /// 清除内存物体
        /// </summary>
        public void DisposeBadIcon()
        {
            if (ResultSpriteRenderer)
            {
                RemoveCatch();

            }

            if (WormSpriteRenderer)
            {
                GameObject.Destroy(WormSpriteRenderer.gameObject);
            }
            if (ResultSpriteRenderer)
            {
                GameObject.Destroy(ResultSpriteRenderer.gameObject);

            }

            if (GrassSpriteRenderer)
            {
                GameObject.Destroy(GrassSpriteRenderer.gameObject);

            }
            if (LessWaterSpriteRenderer)
            {

                GameObject.Destroy(LessWaterSpriteRenderer.gameObject);

            }
           
        }
        
        public void HideResultIcon()
        {
            if (ResultSpriteRenderer)
            {
                ResultSpriteRenderer.enabled = false;
            }
        }
        public void DestoryShadow()
        {
            if (shadowRenderer)
            {
                GameObject.Destroy(shadowRenderer.gameObject);
            }
        }
        public void ShowBadIcon()
        {
            if (WormSpriteRenderer)
            {
                WormSpriteRenderer.SetActive(true);
            }
            if (ResultSpriteRenderer)
            {
                ResultSpriteRenderer.enabled = true;
            }

            if (GrassSpriteRenderer)
            {
                GrassSpriteRenderer.SetActive(true);
            }
            if (LessWaterSpriteRenderer)
            {
                LessWaterSpriteRenderer.SetActive(true);
            }
            if (shadowRenderer)
            {
                shadowRenderer.enabled = true;
            }
        }
        public override void OnClicked(WorldObject worldObject)
        {
            base.OnClicked(worldObject);
        }

        public override void OnTouchBegin(WorldObject worldObject)
        {

        }

        public override void OnTouchEnd(WorldObject worldObject)
        {
            base.OnTouchEnd(worldObject);
            Debug.Log("------点击植物结束------");
        }

        /// <summary>
        /// 产生植株图标,以及计算好影子的大小
        /// </summary>
        private void RelashMainIcon()
        {
            if (Url == string.Format("SeedSprite/{0}_{1}", this.CurrentType != 0 ? ID.ToString() : "200", this.CurrentType))
            {
                return;
            }
            if (!Renderer)
            {
                return;
            }
            ResourceMgr.Instance.LoadResource(string.Format("SeedSprite/{0}_{1}", this.CurrentType != 0 ? ID.ToString() : "200", this.CurrentType), (

                delegate (Resource resource, bool b)
                {
                    Url = string.Format("SeedSprite/{0}_{1}", this.CurrentType != 0 ? ID.ToString() : "200", this.CurrentType);

                    //                    Debug.Log(string.Format("<color=#ffffffff><-当前加载植物--{0}-{1}----></color>", this.CurrentType != 0 ? ID.ToString() : "200", this.CurrentType));

                    Texture2D o = resource.UnityObj as Texture2D;
                    ;
                    Sprite sprite = Sprite.Create(o, new Rect(0, 0, o.width, o.height), new Vector2(0.5f, 0.1f));
                    if (Renderer)
                    {
                        Renderer.sprite = sprite;
                        Renderer.color = HexToColor("DAF6D5FF");
                        Renderer.transform.localScale = new Vector3(0.4f, 0.4f, 1);

                        if (!shadowRenderer)
                        {
                            ShowShadow();
                        }
                        else
                        {
                            float cell = (Renderer.sprite.textureRect.size.x) / (shadowRenderer.sprite.textureRect.size.x + 25);


                            shadowRenderer.gameObject.transform.localScale = Vector3.one * cell;

                            //                        Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", Vector3.one * cell, "test1"));

                        }
                        GC.Collect();
                        Resources.UnloadUnusedAssets();
                    }
          

                }));
        }

        private void ShowShadow()
        {
            GameObject Shadow = new GameObject("Shadow" + FarmID);

            Shadow.transform.SetParent(Go.transform, false);
            Shadow.transform.position = Go.transform.position;
            shadowRenderer = Shadow.AddComponent<SpriteRenderer>();
            shadowRenderer.sprite = SpritesManager.Instance.GetSprite(901);
            shadowRenderer.sortingLayerName = "plant";
            shadowRenderer.sortingOrder = 100;
            float cell = (Renderer.sprite.textureRect.size.x) / (shadowRenderer.sprite.textureRect.size.x + 25);

            shadowRenderer.gameObject.transform.localScale = Vector3.one * cell;
//            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", Vector3.one * cell, "test1"));

        }


        private void ReFlashIcon(NegativeAction go1, int id, string str,Vector3 v3)
        {
            GameObject go =new GameObject(str);
            go.transform.SetParent(Go.transform, false);
            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sortingLayerName = "tools";
            sr.sprite = SpritesManager.Instance.GetSprite(id);
            sr.color = HexToColor("DAF6D5FF");
            sr.transform.localScale = new Vector3(2, 2, 1.4f);
            sr.transform.localPosition = v3;
            switch (go1)
            {
                    case NegativeAction.bug:
                         WormSpriteRenderer=go;
                        sr.sortingOrder = 90;

                    break;
                    case NegativeAction.drying:
                        LessWaterSpriteRenderer = go;
                        sr.sortingOrder = 100;

                    break;
                    case NegativeAction.weed:
                        GrassSpriteRenderer = go;
                      
                        sr.sortingOrder = 90;
                    break;
            }
        }
        /// <summary>
        /// 产生虫子
        /// </summary>
        private void RelashWormIcon()
        {
            if (!WormSpriteRenderer)
            {
                ReFlashIcon(NegativeAction.bug, 905, "bug",new Vector3(0.5f,1,0));

            }

        }
        /// <summary>
        /// 产生草
        /// </summary>
        private void RelashGrassIcon()
        {
            if (!GrassSpriteRenderer)
            {
                ReFlashIcon(NegativeAction.weed, 903, "grass", new Vector3(-1.2f, 0, 0));

            }
        }

        /// <summary>
        /// 缺水
        /// </summary>
        private void RelashWaterIcon()
        {
            if (!LessWaterSpriteRenderer)
            {
                ReFlashIcon(NegativeAction.drying, 904, "water", new Vector3(0.5f, 1, 0));

            }
        }


        /// <summary>
        /// hexcolor转color
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        private Color HexToColor(string hex)
        {
            byte br = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte bg = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte bb = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            byte cc = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            float r = br / 255f;
            float g = bg / 255f;
            float b = bb / 255f;
            float a = cc / 255f;
            return new Color(r, g, b, a);
        }
        //刷新收获图标
        private void RelashReslutIcon()
        {
            if (Go && Renderer && !ResultSpriteRenderer)
            {
                Go.SetActive(false);
                GameObject resultICON = new GameObject("result" + this.FarmID);

                resultICON.transform.SetParent(Go.transform, false);
                resultICON.transform.localScale = Vector3.one * 2f;

                Vector3 pos = Renderer.transform.localPosition;
                resultICON.transform.localPosition = new Vector3(pos.x, pos.y + 1.9f, pos.z);
                t = resultICON.transform.DOLocalMoveY(1.012f, 1).SetAutoKill(false).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Flash);

                BoxCollider2D bc=resultICON.AddComponent<BoxCollider2D>();
                bc.size=new Vector2(0.85f,1.1f);
                bc.offset=new Vector2(0, -0.16f);
                ResultSpriteRenderer = resultICON.AddComponent<SpriteRenderer>();
                ResultSpriteRenderer.sprite = SpritesManager.Instance.GetSprite(902);
                ResultSpriteRenderer.sortingLayerName = "tools";
                ResultSpriteRenderer.sortingOrder = this.Renderer.sortingOrder + 10;
                //世界物体信息
                Dictionary<string, WorldObject> objs = FieldsModel.Instance.otherObjs;
                cCatch = new Catch();
                cCatch.Renderer = ResultSpriteRenderer;
                cCatch.FieldId = this.FarmID;

                //                    Debug.Log("result" + this.FarmID);
                if (!objs.ContainsKey("result" + this.FarmID))
                {
                    objs.Add("result" + this.FarmID, cCatch);
                }
                else
                {
                    objs["result" + this.FarmID] = cCatch;
                }
                Go.SetActive(true);

                //让所有获取图标同步上下
                GlobalDispatcher.Instance.DispathDelay(GlobalEvent.OnLoadingGetIcon, null);
            }

        }


        //读取配置文件信息
        //        BaseAtrribute ba = LoadObjctDateConfig.Instance.GetAtrribute();
        #region MyRegion
        public int CurrentType
        {
            get { return _currentType; }
            set
            {
                _currentType = value;

                if (_currentType >= 4)
                {
                    if (isWorm==1)
                    {
                         IsWorm = 0;

                    }
                    if (IsGrass==1)
                    {
                        IsGrass = 0;

                    }
                    if (IsWater==1)
                    {
                        IsWater = 0;
                    }

                }
                //收获状态
                if (_currentType == 4)
                {
                    //收获状态时没有负面状态
                    
                    if (FriendFarmManager.Instance.isVisiting)
                    {
                        //已经被偷就不显示收获
                        if (this.isSteal == 1)
                        {
                            if (ResultSpriteRenderer)
                            {
                                RemoveCatch();

                            }
                        }
                        else
                        {
                            //没偷的可以刷新
                            RelashReslutIcon();

                        }
                    }
                    else
                    {
                        //自己的4一定会有收获图片
                        RelashReslutIcon();

                    }
                    if (!FriendFarmManager.Instance.isVisiting)
                    {
                        LocalNotification.SendNotification(1, 1000, "果实成熟啦", "点击进入收获吧！",
                            new Color32(0xff, 0x44, 0x44, 255));

//                        Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "发送通知栏通知", "test1"));

                    }
                   

                }
                else
                {
                    //其他状态删除收获
                    if (ResultSpriteRenderer)
                    {

                        RemoveCatch();
                    }
                }
              
                //刷新主界面
                RelashMainIcon();
                
            }
        }
        /// <summary>
        /// GC回收收获对象
        /// </summary>
        public void RemoveCatch()
        {
            GameObject.Destroy(ResultSpriteRenderer.gameObject);
            if (cCatch!=null)
            {
                cCatch.FieldId = 0;
                GameObject.Destroy(cCatch.Go);
                cCatch = null;
            }
          
            Dictionary<string, WorldObject> objs = FieldsModel.Instance.otherObjs;
   
            objs["result" + this.FarmID] = null;
            
        }

        public int GrothTime
        {
            get { return _grothTime; }
            set
            {
                _grothTime = value;

                if (value<=0)
                {
                    _grothTime = 0;
                }
                
                //                Debug.Log(ConvertIntDateTime(value).ToString());
            }
        }



        public double RemainTime
        {
            get { return _remainTime; }
            set { _remainTime = value; }
        }



        public long StartTime
        {
            get { return startTime; }
            set
            {
                if (startTime != value)
                {
                    startTime = value;
                    GlobalDispatcher.Instance.AddListener(GlobalEvent.OnOneSecond, ReflashTime);


                    ReflashTime(1, value);
                    //                    Debug.Log(i);
                }

            }
        }



        bool ReflashTime(int id, object time)
        {
            DateTime starTime = Clock.ConvertIntDateTime(StartTime);
            //结束时的系统时间
            DateTime endTime = Clock.ConvertIntDateTime(StartTime + GrothTime);
            if (endTime >= ServerTime.Now)
            {
                RemainTime = (endTime - ServerTime.Now).TotalSeconds;

            }
            else
            {
                RemainTime = 0;
              
            }
            //成熟所剩余的时间,结果的时间减去现在的时间
            //                    Debug.Log(starTime);
            //根据服务器时间与当前时间计算出已经生长百分比
            HasGrouthTime = (ServerTime.Now - starTime).TotalSeconds;


            //剩余时间bizh
            percentage = HasGrouthTime / GrothTime;

            //            DateTime t = ConvertIntDateTime(remainTime);
            //            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", t.ToString(" HH:mm:ss"), "test1"));


            //植物有五种状态,从0开始数
            double i = percentage * 4;


            if (i > 4)
            {
                i = 4;
            }
            if (i<=0)
            {
                i = 0;
            }
            //对类型赋值
            CurrentType = (int)i;

            //            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "当前种植状态", CurrentType));

//            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", Name, i));

            return false;
        }

        public int IsSteal
        {
            get { return isSteal; }
            set
            {
                isSteal = value;
              
            }
        }
        //虫子
        public int IsWorm
        {
            get { return isWorm; }
            set
            {
                isWorm = value;
                if (!FriendFarmManager.Instance.isVisiting && CurrentType < 4)
                {
                    if (value == 0)
                    {

                        if (WormSpriteRenderer )
                        {
                            GameObject.Destroy(WormSpriteRenderer.gameObject);
                        }
                    }
                    else if (value == 1)
                    {
                        if (isWorm<4)
                        {
                            RelashWormIcon();

                        }
                        else
                        {
                            if (WormSpriteRenderer)
                            {
                                GameObject.Destroy(WormSpriteRenderer.gameObject);
                            }
                        }

                    }

                }
                else
                {
                    if (WormSpriteRenderer)
                    {
                        GameObject.Destroy(WormSpriteRenderer.gameObject);
                    }
                }
            }
        }

        public int IsGrass
        {
            get { return isGrass; }
            set
            {
                isGrass = value;
                if (!FriendFarmManager.Instance.isVisiting && CurrentType < 4)
                {
                    if (value == 0)
                    {

                        if (GrassSpriteRenderer)
                        {
                            GameObject.Destroy(GrassSpriteRenderer.gameObject);
                        }
                    }
                    else if (value == 1)
                    {
                        if (CurrentType<4)
                        {
                            RelashGrassIcon();

                        }
                        else
                        {
                            if (GrassSpriteRenderer)
                            {
                                GameObject.Destroy(GrassSpriteRenderer.gameObject);
                            }
                        }

                    }
                }
                else
                {
                    if (GrassSpriteRenderer )
                    {
                        GameObject.Destroy(GrassSpriteRenderer.gameObject);
                    }
                }
            }
        }


        public string TypeName
        {
            get
            {
                switch (CurrentType)
                {
                    case 0:
                        return "发芽期";
                    case 1:
                        return "幼苗期";
                    case 2:
                        return "生长期";
                    case 3:
                        return "成熟期";
                    case 4:
                        return "结果期";
                    default:
                        return "生长ing...";
                }
            }

        }

        public int IsWater
        {
            get { return isWater; }
            set
            {
                isWater=value;
//                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "删除水", "test1"));
                
                if (!FriendFarmManager.Instance.isVisiting&&CurrentType<4)
                {
//                    Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "删除水", "test1"));

                    if (value == 0)
                    {

//                        Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "删除水", "test1"));

                        if (LessWaterSpriteRenderer )
                        {
//                            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "删除水物体", "test1"));

                            GameObject.Destroy(LessWaterSpriteRenderer.gameObject);
                        }
                    }
                    else if (value == 1)
                    {
//                        if (CurrentType==0)
                        {
                            RelashWaterIcon();

                        }
//                        else
//                        {
//                            if (LessWaterSpriteRenderer)
//                            {
//                                GameObject.Destroy(LessWaterSpriteRenderer.gameObject);
//                            }
//                        }

                    }
                }
                else
                {
                    if (LessWaterSpriteRenderer )
                    {
                        GameObject.Destroy(LessWaterSpriteRenderer.gameObject);
                    }
                }


            }
        }

        public int FarmID
        {
            get { return _farmId; }
            set
            {
                _farmId = value;

//                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "植物的土地ID", value));

            }
        }

        #endregion

    }

}
