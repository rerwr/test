using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework;

namespace Game
{
    public class OnLineView : BaseSubView
    {
        private Button CloseBtn;
        private Text Text;
        public OnLineView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
        }

        public override void BuildUIContent()
        {
            base.BuildUIContent();
            Text=TargetGo.transform.Find("Content/Text").GetComponent<Text>();
            CloseBtn = TargetGo.transform.Find("CloseBtn").GetComponent<Button>();
            CloseBtn.onClick.AddListener(OnClickCloseBtn);
        }

        public override void OnOpen()
        {
            base.OnOpen();
            if (GameTextDataMgr.Instance.TextDatas.ContainsKey(1)&& !string.IsNullOrEmpty(GameTextDataMgr.Instance.TextDatas[1].content))
            {
                Text.text = GameTextDataMgr.Instance.TextDatas[1].content;
            }
            GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnUpdateEnd);
        }

        private void OnClickCloseBtn()
        {
            GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnLine);
            ViewMgr.Instance.Close(ViewNames.OnLineView);
        }
    }
}
