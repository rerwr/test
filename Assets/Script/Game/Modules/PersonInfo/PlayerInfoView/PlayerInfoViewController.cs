using System.Collections.Generic;
using Framework;

namespace Game
{
    public class PlayerInfoViewController:BaseViewController
    {
        public override void Build()
        {
            base.Build();
            Viewlist=new List<BaseSubView>();
            Viewlist.Add(new PlayerInfoView(MainGO,this));
        }
    }
}