using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework;

namespace Game
{
    public class IntroduceView : BaseSubView
    {
        protected Button CloseBtn;
        protected Text Text;

        public IntroduceView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
            Text = TargetGo.transform.Find("Content/Text").GetComponent<Text>();

            CloseBtn = TargetGo.transform.Find("CloseBtn").GetComponent<Button>();
            CloseBtn.onClick.AddListener(OnClickCloseBtn);
        }

        public override void BuildUIContent()
        {
          
            base.BuildUIContent();

        }

        public override void OnOpen()
        {
            base.OnOpen();
            if (GameTextDataMgr.Instance.TextDatas.ContainsKey(2) && !string.IsNullOrEmpty(GameTextDataMgr.Instance.TextDatas[2].content))
            {
                Text.text = GameTextDataMgr.Instance.TextDatas[2].content;
            }
        }

        public virtual void OnClickCloseBtn()
        {
            ViewMgr.Instance.Close(ViewNames.IntroduceView);
        }
    }
}