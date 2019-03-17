using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Framework;

namespace Game
{
    class PlowNeedViewController:BaseViewController
    {
        public override void Build()
        {
            
            base.Build();
            Viewlist=new List<BaseSubView>();
            Viewlist.Add(new PlowView(MainGO,this));
            Viewlist.Add(new PlowSuccessView(MainGO,this));
        }
    }
}
