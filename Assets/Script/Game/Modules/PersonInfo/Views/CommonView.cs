using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;
using UnityEngine;

namespace Game
{
    public class CommonView :BaseSubView
    {
    
        public CommonView(GameObject targetGo, BaseViewController viewController)
            : base(targetGo, viewController)
        {
        }

        public override void BuildSubViews()
        {
            subViews = new List<BaseSubView>();
//            Debuger.LogError(TargetGo.name);
//            Debuger.Log(TargetGo.name);
            subViews.Add(new CommonActionBarView(TargetGo.transform.Find("ActionBar").gameObject, ViewController));
            subViews.Add(new CommonFunctionBarView(TargetGo.transform.Find("FunctionBar").gameObject, ViewController));
            subViews.Add(new CommonPlayerInfoView(TargetGo.transform.Find("PlayerInfo").gameObject, ViewController));
           
            base.BuildSubViews();
        }
        
    }
}
