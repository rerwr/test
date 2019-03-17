using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace Game
{
    public class ChatView : BaseSubView
    {
        public ChatView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
        }

        public override void BuildSubViews()
        {
            subViews = new List<BaseSubView>();
            subViews.Add(new ChatMsgView(TargetGo.transform.Find("MessageWindow").gameObject,ViewController));
            subViews.Add(new ChatPlayInfoView(TargetGo.transform.Find("PlayerInfo").gameObject, ViewController));

            base.BuildSubViews();
        }
    }
}
