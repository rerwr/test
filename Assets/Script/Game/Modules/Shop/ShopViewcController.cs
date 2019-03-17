using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace Game
{
    public class ShopViewController : BaseViewController
    {
        public override void Build()
        {
            Viewlist = new List<BaseSubView>();
            Viewlist.Add(new ShopView(MainGO, this));
            base.Build();
        }
    }
}
