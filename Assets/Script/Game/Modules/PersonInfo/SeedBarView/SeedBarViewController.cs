using System.Collections.Generic;
using Framework;

namespace Game
{
    public class SeedBarViewController:BaseViewController
    {
        public override void Build()
        {
            Viewlist = new List<BaseSubView>();
            Viewlist.Add(new SeedBarView(MainGO, this));
            base.Build();
        }
    }
}