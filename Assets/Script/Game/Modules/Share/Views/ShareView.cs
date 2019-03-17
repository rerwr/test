using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ShareView:BaseSubView
    {
        private Button CloseBtn;
        private Button WeChatBtn;
        private Button QQBtn;
        private Button MomentsBtn;
        private ShareDemo sharedemo;
        public ShareView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {

        }

        public override void BuildUIContent()
        {
            base.BuildUIContent();
            CloseBtn=TargetGo.transform.Find("BG/CloseBtn").GetComponent<Button>();
            CloseBtn.onClick.AddListener(CloseView);
            WeChatBtn = TargetGo.transform.Find("BG/WeChantBtn").GetComponent<Button>();
            WeChatBtn.onClick.AddListener(OnClickWeChat);
            QQBtn = TargetGo.transform.Find("BG/QQBtn").GetComponent<Button>();
            QQBtn.onClick.AddListener(OnClickQQ);
            MomentsBtn = TargetGo.transform.Find("BG/MomentsBtn").GetComponent<Button>();
            MomentsBtn.onClick.AddListener(OnClickMoments);
            sharedemo = Camera.main.gameObject.GetComponent<ShareDemo>();

        }

        public void CloseView()
        {
            ViewMgr.Instance.Close(ViewNames.ShareView);
        }

        private void OnClickWeChat()
        {
            sharedemo.OnShareWechant();
        }

        private void OnClickQQ()
        {
            sharedemo.OnShareQQ();

        }

        private void OnClickMoments()
        {
            sharedemo.OnShareMomentsClick();
        }
    }
}
