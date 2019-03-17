using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework;

namespace Game
{
    public class AddFriendSuccView : BaseSubView
    {
        private Button CloseBtn;

        public AddFriendSuccView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
        }

        public override void BuildUIContent()
        {
            base.BuildUIContent();
            CloseBtn = TargetGo.transform.Find("CloseBtn").GetComponent<Button>();
            CloseBtn.onClick.AddListener(OnClickCloseBtn);
        }

        public override void OnOpen()
        {
            base.OnOpen();
            TargetGo.SetActive(false);
            FriendsInfoController.Instance.GetDispatcher().AddListener(FriendsInfoEvent.onAgreeFriend,OnAgree);
        }

        private bool OnAgree(int eventId,object arg)
        {
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
            FriendsInfoController.Instance.GetDispatcher().RemoveListener(FriendsInfoEvent.onAgreeFriend, OnAgree);
        }
    }
}