using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace Game
{
    public class HelpViewController : BaseViewController
    {
        public override void Build()
        {
            Viewlist = new List<BaseSubView>();
            Viewlist.Add(new HelpView(MainGO, this));

        }
    }
}