using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace Game
{
    public class StrategyViewController: BaseViewController
    {
        public override void Build()
        {
            Viewlist = new List<BaseSubView>();
            Viewlist.Add(new StrategyView(MainGO, this));
            
        }
    }
}
