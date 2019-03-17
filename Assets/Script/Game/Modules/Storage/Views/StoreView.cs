using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace Game
{
    public class StoreView : BaseSubView
    {

        public StoreView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
        }

        public override void BuildSubViews()
        {
            subViews = new List<BaseSubView>();
            subViews.Add(new StoreItemView(TargetGo.transform.Find("StoreItemView").gameObject, ViewController));
            subViews.Add(new StoreSellView(TargetGo.transform.Find("StoreSellView").gameObject, ViewController));
            subViews.Add(new StoreSellSuccView(TargetGo.transform.Find("SellSuccessView").gameObject, ViewController));
            base.BuildSubViews();
        }
    }
}