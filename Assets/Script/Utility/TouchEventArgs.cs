using Game;

namespace Game
{
    using System;
    using UnityEngine;

    public class TouchEventArgs
    {
        public bool Cancelled;
        private TouchPhase _phase;
        public float ScreenX;
        public float ScreenY;
        private WorldObject _target;

        public bool IsEnded
        {
            get
            {
                return ((this._phase == TouchPhase.Ended) || (this._phase == TouchPhase.Canceled));
            }
        }

        public WorldObject Target
        {
            get { return _target; }
            set
            {
                _target = value;
               
            }
        }

        public TouchPhase Phase
        {
            get { return _phase; }
            set
            {
                _phase = value;

//                Debug.Log(_phase.ToString()+"-----"+Target??Target.Renderer.gameObject.name);
            }
        }
    }
}

