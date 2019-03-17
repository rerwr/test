using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework;

namespace Game
{
    public class JYSenseView : BaseSubView
    {
        private Button CloseBtn;

        private Text Introduce;
        private Text Common;
        private Text Identify;
        private Text SFOil;

        public JYSenseView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
        }

        public override void BuildUIContent()
        {
            base.BuildUIContent();
            CloseBtn = TargetGo.transform.Find("SenseView/CloseBtn").GetComponent<Button>();
            CloseBtn.onClick.AddListener(OnClickCloseBtn);

            Introduce = TargetGo.transform.Find("SenseView/Items/Grid/Introduce/Content/Text").GetComponent<Text>();
            Common = TargetGo.transform.Find("SenseView/Items/Grid/Common/Content/Text").GetComponent<Text>();
            Identify = TargetGo.transform.Find("SenseView/Items/Grid/Identify/Content/Text").GetComponent<Text>();
            SFOil = TargetGo.transform.Find("SenseView/Items/Grid/SFOil/Content/Text").GetComponent<Text>();
        }

        public override void OnOpen()
        {
            base.OnOpen();

            if (GameTextDataMgr.Instance.TextDatas.ContainsKey(3) && !string.IsNullOrEmpty(GameTextDataMgr.Instance.TextDatas[3].content))
            {
                Introduce.text = GameTextDataMgr.Instance.TextDatas[3].content;
            }
            if (GameTextDataMgr.Instance.TextDatas.ContainsKey(4) && !string.IsNullOrEmpty(GameTextDataMgr.Instance.TextDatas[4].content))
            {
                Common.text = GameTextDataMgr.Instance.TextDatas[4].content;
            }
            if (GameTextDataMgr.Instance.TextDatas.ContainsKey(5) && !string.IsNullOrEmpty(GameTextDataMgr.Instance.TextDatas[5].content))
            {
                Identify.text = GameTextDataMgr.Instance.TextDatas[5].content;
            }
            if (GameTextDataMgr.Instance.TextDatas.ContainsKey(6) && !string.IsNullOrEmpty(GameTextDataMgr.Instance.TextDatas[6].content))
            {
                SFOil.text = GameTextDataMgr.Instance.TextDatas[6].content;
            }
        }

        private void OnClickCloseBtn()
        {
            ViewMgr.Instance.Close(ViewNames.JYSenseView);
        }
    }
}
