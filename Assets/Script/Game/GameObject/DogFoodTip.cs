using DG.Tweening;
using Framework;
using UnityEngine;

namespace Game
{
    public class DogFoodTip : WorldObject
    {
        private Tweener t;
        public DogFoodTip(GameObject go)
        {
            this.Go = go;
            t = Go.transform.DOLocalMoveY(0.9f, 1).SetAutoKill(false).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Flash);
            GlobalDispatcher.Instance.AddListener(GlobalEvent.OnLoadingGetIcon, OnLoadEndPlayICON);
            
        }
        ~ DogFoodTip()
        {

            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "销毁狗粮提示", "test1"));

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
        public override void OnClicked(WorldObject worldObject)
        {
            if (!FriendFarmManager.Instance.isVisiting)
            {
                ViewMgr.Instance.Open(ViewNames.DogInfoView);
            }
            base.OnClicked(worldObject);

        }
    }
}