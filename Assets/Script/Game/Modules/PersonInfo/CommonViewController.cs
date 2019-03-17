using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Framework;
using Game;

namespace Game
{

    public class CommonViewController : BaseViewController
    {
        public override void Build()
        {
            Viewlist = new List<BaseSubView>();
            Viewlist.Add(new CommonView(MainGO, this));
            base.Build();
        }
    }
}
