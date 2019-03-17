using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Framework;
using Game;
using UnityEngine;

namespace Game
{
    class ChangePWView : RegisterView
    {
        public ChangePWView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {

        }

        public void BuildUIContent()
        {
            base.BuildUIContent();
        }

        public override void OnregisterBtnClick()
        {
            base.ConfirmRegisterInfo(2);

        }

        public override void OnOpen()
        {

            LoginController.Instance.GetDispatcher().AddListener(LoginEvent.VeriCodeSucc, RefrashUI);
            LoginController.Instance.GetDispatcher().AddListener(LoginEvent.ChangePWSucc, OnRegisterSucc);
            if (PlayerSave.HasKey("Account"))
            {
                Input.text = PlayerSave.GetString("Account");

//                Password.text = PlayerSave.GetString("Password");
            }
        }

        public override void OnClose()
        {
            LoginController.Instance.GetDispatcher().RemoveListener(LoginEvent.VeriCodeSucc, RefrashUI);
            GlobalDispatcher.Instance.RemoveListener(LoginEvent.ChangePWSucc, OnRegisterSucc);
        }

        public override bool OnRegisterSucc(int eid, object args)
        {
            LoginModel.Instance.Account = Input.text;
            LoginModel.Instance.Password = Password.text;
            if (!string.IsNullOrEmpty(LoginModel.Instance.Account))
            {
                PlayerSave.SetString("Account", LoginModel.Instance.Account);
            }
            if (!string.IsNullOrEmpty(LoginModel.Instance.Password))
            {
                PlayerSave.SetString("Password", LoginModel.Instance.Password);

            }

            SystemMsgView.SystemFunction(Function.Tip, Info.RegisterSucc,2);
            lv.egv.Set(Password.text, Input.text);
            CloseViewInfo info = new CloseViewInfo(4);
            lv.egv.HideView(1, info);
            
            return false;
        }
    }
}
