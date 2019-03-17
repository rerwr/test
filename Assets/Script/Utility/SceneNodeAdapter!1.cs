namespace Game
{
    using System;

    public abstract class SceneNodeAdapter<TSceneNode> where TSceneNode: WorldObject
    {
        private TSceneNode _target;

        protected SceneNodeAdapter()
        {
        }

        protected abstract void OnTrackingBegan(TSceneNode target);
        protected abstract void OnTrackingEnded(TSceneNode target);

        public TSceneNode Target
        {
            get
            {
                return this._target;
            }
            set
            {
                if (this._target != value)
                {
                    if (this._target != null)
                    {
                        this.OnTrackingEnded(this._target);
                    }
                    this._target = value;
                    if (value != null)
                    {
                        this.OnTrackingBegan(value);
                    }
                }
            }
        }
    }
}

