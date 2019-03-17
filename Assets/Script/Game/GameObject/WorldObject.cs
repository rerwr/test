using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


namespace Game
{

    public class WorldObject : BaseObject
    {

       
        Tweener tScale;
        Tweener tColor;

        public event Action<WorldObject> Clicked;

        public event Action<WorldObject> TouchBegan;

        public event Action<WorldObject> TouchEnded;

        public bool Enable
        {
            get { return _enable; }
            set { _enable = value; }
        }

        private bool _enable = true;
        public WorldObject()
        {
            TouchHandler _touchHandler = new TouchHandler(this);
            _touchHandler.TouchBegan += new Action(this.OnTouchBegan);
            _touchHandler.TouchEnded += new Action(this.OnTouchEnded);
            _touchHandler.Clicked += new Action(this.OnContentClicked);
            this.Clicked += OnClicked;
            this.TouchBegan += OnTouchBegin;
            this.TouchEnded += OnTouchEnd;

//            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "创建一个世界物体", "test1"));

        }
        public virtual void OnTouchEnd(WorldObject worldObject)
        {
//            tScale.PlayBackwards();
//            tColor.PlayBackwards();

        }

        
        public virtual void OnTouchBegin(WorldObject worldObject)
        {
            if (tScale==null)
            {
                BeginTouchAnim(Go, 0.06f);

            }
            else
            {
                if (!tScale.IsPlaying())
                {
                   BeginTouchAnim(Go, 0.06f);

                }
            }
            
        }
        public virtual void OnClicked(WorldObject worldObject)
        {
            MusicManager.Instance.Playsfx(AudioNames.OnClick1);

        }

        private void OnContentClicked()
        {

            if (this.Enable && (this.Clicked != null))
            {
                this.Clicked(this);
            }

        }

        private void OnTouchEnded()
        {
            if (this.TouchEnded != null)
            {
                this.TouchEnded(this);
            }
        }

        private void OnTouchBegan()
        {
            if (this.Enable && (this.TouchBegan != null))
            {
                this.TouchBegan(this);
            }
        }

        public Action<TouchEventArgs> Touched;
        /// <summary>
        /// 每块地被触摸调用此事件
        /// </summary>
        /// <param name="args"></param>
        public void DispatchTouch(TouchEventArgs args)
        {
            if (args.Target.GetType() == typeof(FarmUnit))
            {
                FarmUnit target = args.Target as FarmUnit;
                //如果当前土地已经开垦，可以点击
                if (target.EnablePlant == 1)
                {
                    //如果有植物，则点击植物
                    if (target.Plant != null)
                    {
                        target.Plant.Touched(args);
                    }
                    //点击农田
                    target.Touched(args);
                }
                else
                {
                    if (target.Brand != null)
                    {
                        target.Brand.Touched(args);
                    }
                }
            }
            else
            {
                WorldObject obj = args.Target as WorldObject;
                obj.Touched(args);
            }
         
        }
        /// <summary>
        /// 播放点击动画
        /// </summary>
        /// <param name="go"></param>
        /// <param name="strength"></param>
        void BeginTouchAnim(GameObject go, float strength)
        {
            Vector3 v3 = go.transform.localScale;
            
            //此处不同
            tScale = go.transform.DOScale(new Vector3(v3.x + strength, v3.y + strength, v3.z), 0.3f).SetEase(Ease.Flash).SetAutoKill(false);
            if (go.transform.GetComponent<SpriteRenderer>())
            {
                tColor = go.transform.GetComponent<SpriteRenderer>().DOBlendableColor(new Color(0.8f, 0.8f, 0.8f), 0.3f).SetEase(Ease.InBounce).SetAutoKill(false);
                if (!tColor.IsPlaying() && tColor.IsComplete())
                {
                    tColor.PlayForward();

                }
            }
            tScale.onComplete += () =>
            {
                tScale.PlayBackwards();
                tColor.PlayBackwards();
            };
            if (!tScale.IsPlaying() && tScale.IsComplete())
            {
                tScale.PlayForward();
            }
        
       



        }
    }
}
