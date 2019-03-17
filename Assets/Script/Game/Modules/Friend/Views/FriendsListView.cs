using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;
using UnityEngine;

namespace Game
{
    class FriendsListView: BaseSubView
    {
        public static GameObject RankingListView;
        public FriendsListView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
        }

        public override void BuildSubViews()
        {
            subViews = new List<BaseSubView>();
            subViews.Add(new FriendsView(TargetGo.transform.Find("FriendsView").gameObject, ViewController));
            RankingListView = TargetGo.transform.Find("RankingListView").gameObject;
            subViews.Add(new RankingListView(RankingListView, ViewController));
            base.BuildSubViews();
        }
    }
}
