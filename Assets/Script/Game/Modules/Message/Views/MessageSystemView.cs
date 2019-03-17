using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework;

namespace Game
{
    public class MessageSystemView : BaseSubView
    {
        private Text Content;
        private Button CloseBtn;

        public MessageSystemView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
        }

        public override void BuildUIContent()
        {
            base.BuildUIContent();

            Content = TargetGo.transform.Find("Content/Content").GetComponent<Text>();
            CloseBtn=TargetGo.transform.Find("CloseBtn").GetComponent<Button>();
            CloseBtn.onClick.AddListener(OnClickCloseBtn);
        }

        public override void OnOpen()
        {
            base.OnOpen();
            TargetGo.SetActive(false);
            MessageController.Instance.GetDispatcher().AddListener(MessageEvent.OnClickSystemMsg,ShowSystemMsg);
        }

        private bool ShowSystemMsg(int eventId,object arg)
        {
            string content = (string)arg;
            Content.text = content;
            TargetGo.SetActive(true);
            return false;
        }

        private void OnClickCloseBtn()
        {
            TargetGo.SetActive(false);
        }

        public override void OnClose()
        {
            base.OnClose();
            MessageController.Instance.GetDispatcher().AddListener(MessageEvent.OnClickSystemMsg, ShowSystemMsg);
        }
    }
}
