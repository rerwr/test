using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace Game
{
    public class ShopView : BaseSubView
    {

        public ShopView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
        }

        public override void BuildSubViews()
        {
            subViews = new List<BaseSubView>();
            subViews.Add(new ShopItemsView(TargetGo.transform.Find("ShopWindow").gameObject, ViewController));
            subViews.Add(new ShopBuyView(TargetGo.transform.Find("BuyWindow").gameObject, ViewController));
            subViews.Add(new ShopSuccView(TargetGo.transform.Find("BuySuccView").gameObject, ViewController));
            base.BuildSubViews();
        }
    }
}