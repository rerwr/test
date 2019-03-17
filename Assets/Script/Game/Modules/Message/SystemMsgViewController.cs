using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;

namespace Game
{
    public class SystemMsgViewController:BaseViewController
    {
        public override void Build()
        {
            Viewlist=new List<BaseSubView>();
            Viewlist.Add(new SystemMsgView(MainGO,this));
            base.Build();

        }
    }
}
