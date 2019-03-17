using Game;


namespace Game
{
    using System;


    using UnityEngine;

    public sealed class TouchHandler : SceneNodeAdapter<WorldObject>
    {
        private bool _dragMode;
        private bool _hovered;
        private bool _isBackTouched;
        private bool _pressed;

        public event Action Clicked;

        public event Action TouchBegan;

        public event Action TouchEnded;

        public TouchHandler(WorldObject target = null)
        {
            base.Target = target;
        }

        protected override void OnTrackingBegan(WorldObject target)
        {
            target.Touched += this.Target_Touched;
        }

        protected override void OnTrackingEnded(WorldObject target)
        {
            target.Touched -=this.Target_Touched;
            this.ResetState();
        }

        public void ResetState()
        {
          
            this._pressed = false;
            this._hovered = false;
            this._isBackTouched = false;
        }

        private void Target_Touched(TouchEventArgs args)
        {
            bool flag = this._pressed;
            bool flag2 = false;
            WorldObject target = base.Target;
           
            //此处判定是否可以点击
            this._hovered = (args.Target != null) && ((target == args.Target));
            this._isBackTouched = target == args.Target;
            if ((args.Phase == TouchPhase.Began) && !this._pressed)
            {
                //刚开始点击的时候
                this._pressed = true;
            }

            else if (((args.Phase == TouchPhase.Moved) || (args.Phase == TouchPhase.Stationary)) && this._pressed)
            {
                if (!this._hovered && !this._dragMode)
                {
                    this._pressed = false;
                }
            }

            else if (args.Phase == TouchPhase.Ended)
            {
                //结束点击的时候
                if (this._pressed)
                {
                    if (!this._dragMode)
                    {
                        flag2 = true;
                    }
                    else
                    {
                        this._pressed = false;
                    }
                }
                else
                {
                    this._hovered = false;
                }
            }

            if (flag2)
            {
                if ((this.Clicked != null) && !args.Cancelled)
                {
                    this.Clicked();
                }
                this._pressed = false;
                this._hovered = false;
            }
            if (((this.TouchBegan != null) && this._pressed) && !flag)
            {
                this.TouchBegan();
            }
            else if (((this.TouchEnded != null) && !this._pressed) && flag)
            {
                this.TouchEnded();
            }
        }

        public bool DragMode
        {
            get
            {
                return this._dragMode;
            }
            set
            {
                this._dragMode = value;
            }
        }

        public bool IsBackTouched
        {
            get
            {
                return this._isBackTouched;
            }
        }

        public bool IsHovered
        {
            get
            {
                return this._hovered;
            }
        }

        public bool IsTouched
        {
            get
            {
                return this._pressed;
            }
        }
    }
}

