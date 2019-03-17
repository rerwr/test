using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DG.Tweening;
using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class CommonActionBarView : BaseSubView
    {
        private Toggle btnPlow;
        private Toggle btnWater;
        private Toggle btnFeed;
        private Toggle btnDebug;
        private Toggle btnWeed;
        private Toggle btnHarvest;

        private static Tweener btnPlowTweener;
        private static Tweener btnWaterTweener;
        private static Tweener btnDebugTweener;
        private static Tweener btnWeedTweener;
        private static Tweener btnHarvestTweener;
        private bool isfirst = true;
        private static GameAction action = GameAction.None;
        private static GameObject o=null;

        public CommonActionBarView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
            ResourceMgr.Instance.LoadResource("Prefab/Shield03",((resource, b) =>
            {
                o=resource.UnityObj as GameObject;
                o = GameObject.Instantiate(o);
                o.SetActive(false);
            }));
        }

        public static GameAction Action1
        {
            get { return action; }
            set
            {
                action = value;
                if (value == GameAction.None)
                {
                    if (o)
                    {
                        o.SetActive(false);
                    }
                    btnPlowTweener.PlayBackwards();
                    btnWaterTweener.PlayBackwards();
                    btnDebugTweener.PlayBackwards();
                    btnWeedTweener.PlayBackwards();
                    btnHarvestTweener.PlayBackwards();
                }
            }
        }

        public override void BuildUIContent()
        {
            base.BuildUIContent();

            btnPlow = TargetGo.transform.Find("Plow").GetComponent<Toggle>();
            btnWater = TargetGo.transform.Find("Water").GetComponent<Toggle>();
            btnDebug = TargetGo.transform.Find("DeBug").GetComponent<Toggle>();
            btnWeed = TargetGo.transform.Find("Weed").GetComponent<Toggle>();
            btnHarvest = TargetGo.transform.Find("Harvest").GetComponent<Toggle>();

            btnPlow.onValueChanged.AddListener(OnClickPlow);
            btnWater.onValueChanged.AddListener(OnClickWater);
            btnDebug.onValueChanged.AddListener(OnClickDebug);
            btnWeed.onValueChanged.AddListener(OnClickWeed);
            btnHarvest.onValueChanged.AddListener(OnClickHarvest);

        }
    
        void OnClickPlow(bool isClick)
        {
            if (isfirst && isClick)
            {
                btnPlowTweener = DotweenManager.BeginTouchActionUI(btnPlow.gameObject);
                isfirst = false;
                action = GameAction.Plow;
                o.SetActive(true);
                Vector3 pos = btnPlow.transform.position;
                o.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y-33,200));
            }
            else
            {
                btnPlowTweener.PlayBackwards();

                action = GameAction.None;
                isfirst = true;
            }

        }

        void OnClickWater(bool isClick)
        {
            if (isfirst && isClick)
            {
                btnWaterTweener = DotweenManager.BeginTouchActionUI(btnWater.gameObject);
                action = GameAction.Water;
                isfirst = false;
                o.SetActive(true);

                Vector3 pos = btnWater.transform.position;
                o.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y-33,200));

            }
            else
            {
                isfirst = true;
                action = GameAction.None;
                btnWaterTweener.PlayBackwards();

            }
        }

        void OnClickDebug(bool isClick)
        {
            if (isfirst && isClick)
            {
                btnDebugTweener = DotweenManager.BeginTouchActionUI(btnDebug.gameObject);
                isfirst = false;
                action = GameAction.Debug;
                o.SetActive(true);

                Vector3 pos = btnDebug.transform.position;
                o.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y-33,200));

            }
            else
            {
                isfirst = true;
                action = GameAction.None;
                btnDebugTweener.PlayBackwards();
            }

        }

        void OnClickWeed(bool isClick)
        {
            if (isfirst && isClick)
            {
                btnWeedTweener = DotweenManager.BeginTouchActionUI(btnWeed.gameObject);
                isfirst = false;
                action = GameAction.weed;
                o.SetActive(true);

                Vector3 pos = btnWeed.transform.position;
                o.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y-33,200));

            }
            else
            {
                isfirst = true;
                action = GameAction.None;
                btnWeedTweener.PlayBackwards();

            }
        }
        IEnumerator GetAll()
        {
            for (int i = 0; i < FieldsModel.Instance.farms.Count; i++)
            {
                if (FieldsModel.Instance.farms[i + 1].Plant != null&& FieldsModel.Instance.farms[i + 1].Plant.RemainTime==0f)
                {
                    Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "FieldId", FieldsModel.Instance.farms[i + 1].FarmID));

                    FieldsController.Instance.SendPluckReq(FieldsModel.Instance.farms[i + 1].FarmID,LoginModel.Instance.Uid);
                    yield return 0.4f;
                    
                }
            }
            MTRunner.Instance.StartRunner(Wait(1));

        }


        IEnumerator Wait(float time)
        {
            yield return time;
//            if (FieldsController.Instance.ids.Count<=0)
            {
                FieldsController.Instance.SendFieldsReflashAction();

            }

        }
        void OnClickHarvest(bool isClick)
        {
            if (isfirst && isClick)
            {
                btnHarvestTweener = DotweenManager.BeginTouchActionUI(btnHarvest.gameObject);
                isfirst = false;
                o.SetActive(true);
                Vector3 pos=btnHarvest.transform.position;
                o.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y-33,200));
         
                MTRunner.Instance.StartRunner(GetAll());

                MusicManager.Instance.Playsfx(AudioNames.HarvestPlant);
                
            }
            else
            {
                isfirst = true;
                action = GameAction.None;
                btnHarvestTweener.PlayBackwards();

            }
        }

    }
}
