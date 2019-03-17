using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace Game
{
    public class JYSenseViewController : BaseViewController
    {
        public override void Build()
        {
            Viewlist = new List<BaseSubView>();
            Viewlist.Add(new JYSenseView(MainGO, this));
            base.Build();
        }
    }
}
