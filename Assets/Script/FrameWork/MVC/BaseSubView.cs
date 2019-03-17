using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public abstract class BaseSubView
    {
        public GameObject TargetGo;
        protected BaseViewController ViewController;
        private static BaseSubView _Instance;
        protected BaseSubView(GameObject targetGo, BaseViewController viewController)
        {
            this.TargetGo = targetGo;
            this.ViewController = viewController;
        }
        /// <summary>
        /// 此View类可嵌套
        /// </summary>
        public List<BaseSubView> subViews;

    

        /// <summary>
        /// 填充Subviews（如果存在），注意base.Build()在最后调用
        /// </summary>
        public virtual void BuildSubViews()
        {
            if (subViews != null)
            {
                for (int i = 0; i < subViews.Count; i++)
                {
                    subViews[i].BuildSubViews();
                }
            }
        }
        /// <summary>
        /// 在这里创建此View的MonoBehavior
        /// </summary>
        public virtual void BuildUIContent()
        {
            if (subViews != null)
            {
                for (int i = 0; i < subViews.Count; i++)
                {
                    subViews[i].BuildUIContent();
                }
            }
        }

        /// <summary>
        /// 每次打开此View调用
        /// </summary>
        public virtual void OnOpen()
        {
            if (subViews != null)
            {
                for (int i = 0; i < subViews.Count; i++)
                {
                    subViews[i].OnOpen();
                }
            }
        }

        /// <summary>
        /// 每次关闭此View调用
        /// </summary>
        public virtual void OnClose()
        {
            if (subViews != null)
            {
                for (int i = 0; i < subViews.Count; i++)
                {
                    subViews[i].OnClose();
                }
            }
        }

        /// <summary>
        /// 销毁此View调用
        /// </summary>
        public virtual void OnDestroy()
        {
            if (subViews != null)
            {
                for (int i = 0; i < subViews.Count; i++)
                {
                    subViews[i].OnDestroy();
                }
            }
        }
    }
}