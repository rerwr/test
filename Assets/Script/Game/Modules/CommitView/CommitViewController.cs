using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Framework;

namespace Game
{
    public class CommitViewController:BaseViewController
    {
        public override void Build()
        {
            Viewlist=new List<BaseSubView>();
            Viewlist.Add(new CommitView(MainGO,this));
            base.Build();

        }
    }
}
