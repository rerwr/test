using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace Game
{
    public class DogInfoViewController : BaseViewController
    {
        public override void Build()
        {
            Viewlist = new List<BaseSubView>();
            Viewlist.Add(new DogInfoView(MainGO, this));
            base.Build();
        }
    }
}
