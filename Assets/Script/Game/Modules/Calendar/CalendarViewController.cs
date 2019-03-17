using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;

namespace Game
{
    public class CalendarViewController:BaseViewController
    {
        public override void Build()
        {
            Viewlist=new List<BaseSubView>();
            Viewlist.Add(new CalendarView(MainGO,this));
            base.Build();
        }
    }
}
