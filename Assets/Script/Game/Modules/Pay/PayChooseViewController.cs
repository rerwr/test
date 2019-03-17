using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;
using UnityEngine;

namespace Game
{
    public class PayChooseViewController: BaseViewController
    {
        
        public override void Build()
        {
            Viewlist = new List<BaseSubView>();
            Viewlist.Add(new PayChooseView(MainGO, this));
            base.Build();
        }


    }
}
