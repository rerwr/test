using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;
using UnityEngine;

namespace Game
{
    public class PayChooseView:BaseSubView
    {
        public  GameObject PaySuccessView;
        public PayChooseView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
        }
        public override void BuildSubViews()
        {
            subViews = new List<BaseSubView>();
            PayAtionModel.Instance.Pv=new PayView(TargetGo.transform.Find("PayView").gameObject, ViewController);
            PayAtionModel.Instance.Pcv= new PaySuccessView(TargetGo.transform.Find("PaySuccessView").gameObject, ViewController);
            subViews.Add(PayAtionModel.Instance.Pv);
            
            subViews.Add(PayAtionModel.Instance.Pcv);
            base.BuildSubViews();
        }

    }
}
