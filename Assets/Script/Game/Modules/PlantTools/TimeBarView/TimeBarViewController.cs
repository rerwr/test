using System.Collections.Generic;
using Framework;

namespace Game
{
    public class TimeBarViewController:BaseViewController
    {
        public override void Build()
        {
            base.Build();
            Viewlist=new List<BaseSubView>();
            Viewlist.Add(new TimeBarView(MainGO,this));

        }

    }
}