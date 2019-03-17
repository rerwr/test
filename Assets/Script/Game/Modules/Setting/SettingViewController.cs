using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;

namespace Game
{
    class SettingViewController : BaseViewController
    {
        public override void Build()
        {
            Viewlist = new List<BaseSubView>();
            Viewlist.Add(new SettingView(MainGO, this));
            base.Build();
        }
    }
}
