using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;
using UnityEngine;

namespace Game
{
    public class CalendarView:BaseSubView
    {
        public static  GameObject SignSucessView;
        public CalendarView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {

        }

        public override void BuildSubViews()
        {
            subViews=new List<BaseSubView>();
            subViews.Add(new CalendarSignView(TargetGo.transform.Find("CalendarSignView").gameObject,ViewController));
            SignSucessView=TargetGo.transform.Find("SignSucessView").gameObject;
            subViews.Add(new SignSucessView(SignSucessView, ViewController));
//            base.BuildSubViews();
        }
      

    }
}
