using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{

    public class CloseViewInfo
    {
        public CloseViewInfo(int pattern)
        {
            this.pattern = pattern;

        }
        public int pattern;

    }
    public class RegisterView : BaseSubView
    {

        public InputField Input;
        public InputField invite;
        public InputField code;
        public Button codeBtn;
        public Button CloseBtn;
        public Text codeBtnText;
        public InputField Password;
        public InputField ConfirmPw;
        public Button registerBtn;
        public LoginView lv;

        private string pattern = @"1\d{10}";
        public Regex regex = new Regex(@"
(?=.*[0-9])                     #必须包含数字
(?=.*[a-zA-Z])                  #必须包含小写或大写字母
.{8,30}                         #至少8个字符，最多30个字符
", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
        public RegisterView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {

        }

        public override void BuildUIContent()
        {
            base.BuildUIContent();
            Input = TargetGo.transform.Find("Account").GetComponent<InputField>();
            code = TargetGo.transform.Find("Code").GetComponent<InputField>();
            Password = TargetGo.transform.Find("Password").GetComponent<InputField>();
            ConfirmPw = TargetGo.transform.Find("ConfirmPw").GetComponent<InputField>();
            registerBtn = TargetGo.transform.Find("RegisterBtn").GetComponent<Button>();
            codeBtn = TargetGo.transform.Find("Code/Button").GetComponent<Button>();
            codeBtnText = TargetGo.transform.Find("Code/Button/Text").GetComponent<Text>();
            CloseBtn = TargetGo.transform.Find("CloseBtn").GetComponent<Button>();
            invite = TargetGo.transform.Find("invite").GetComponent<InputField>();

            registerBtn.onClick.AddListener(OnregisterBtnClick);
            codeBtn.onClick.AddListener(OncodeBtnClick);
            CloseBtn.onClick.AddListener(ClosePanel);
            lv = ViewMgr.Instance.views[ViewNames.LoginView].Viewlist[0] as LoginView;
            Input.onEndEdit.AddListener(Changac);
            Password.onEndEdit.AddListener(Changpw);
            
         
        }
        private void Changpw(string a)
        {
            if (Input.text != "")
            {
                PlayerSave.SetString("Password", Password.text);
            }

        }
        private void Changac(string a)
        {
            if (Input.text!="")
            {
                PlayerSave.SetString("Account", Input.text);
            }

        }
        private void ClosePanel()
        {
            if (typeof(RegisterView) == this.GetType())
            {

                CloseViewInfo info = new CloseViewInfo(3);

                lv.egv.HideView(1, info);

            }
            else
            {
                CloseViewInfo info = new CloseViewInfo(4);

                lv.egv.HideView(0, info);

            }
            //选取模式2关闭
            ClearInput();
        }
        public void CodeBtnListener(bool isListener)
        {
            if (isListener)
            {
                codeBtn.onClick.AddListener(OncodeBtnClick);

            }
            else
            {

                Debug.Log("------falseRemove------");
                codeBtn.onClick.RemoveAllListeners();

            }
        }
        public override void OnOpen()
        {
          
            
            base.OnOpen();
            LoginController.Instance.GetDispatcher().AddListener(LoginEvent.VeriCodeSucc, RefrashUI);
            LoginController.Instance.GetDispatcher().AddListener(LoginEvent.RegisterSucc, OnRegisterSucc);

        }


        public override void OnClose()
        {
            base.OnClose();
            LoginController.Instance.GetDispatcher().RemoveListener(LoginEvent.VeriCodeSucc, RefrashUI);
            GlobalDispatcher.Instance.RemoveListener(LoginEvent.RegisterSucc, OnRegisterSucc);
        }

        private void ClearInput()
        {
            Input.text = "";
            code.text = "";
            Password.text = "";
            ConfirmPw.text = "";
            invite.text = "";
        }

        public bool RefrashUI(int id, object arg)
        {
            MTRunner.Instance.StartRunner(Reflash());

            return false;
        }

        IEnumerator Reflash()
        {

            int i = 60;
            while (i >= 0)
            {
                yield return 1f;

                Debug.Log("times:" + i);

                codeBtnText.text = i + "s";

                if (i == 0)
                {
                    codeBtnText.text = "重新发送";
                    CodeBtnListener(true);
                }
                i--;

            }
        }
        /// <summary>
        /// 注册成功调用事件
        /// </summary>
        /// <param name="eid"></param>
        /// <param name="args"></param>
        public virtual bool OnRegisterSucc(int eid, object args)
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
            ViewMgr.Instance.Open(ViewNames.SystemMsgView);
            SystemMsgView.SystemFunction(Function.Tip, Info.RegisterSucc, 2);
            //                LoginController.Instance.SendloginSDKReq(Input.text, Password.text, (int)Application.platform, "111114442");
            lv.egv.Set(Password.text, Input.text);
            CloseViewInfo info = new CloseViewInfo(3);
//            lv.egv.HideView(1, info);
            return false;
        }

        public virtual void OnregisterBtnClick()
        {
            ConfirmRegisterInfo(1);
        }
        //pattern=1表示注册，2为忘记密码
        public void ConfirmRegisterInfo(int pattern)
        {
            if (Input.text != "" && code.text != "" && Password.text != "" && ConfirmPw.text != "")
            {
                if (regex.IsMatch(Password.text))
                {
                    if (Password.text == ConfirmPw.text)
                    {
                        if (pattern==1)
                        {

                            LoginController.Instance.SendRegisterReq(Input.text, code.text, Password.text, pattern,invite.text);

                        }
                        else
                        {

                            LoginController.Instance.SendRegisterReq(Input.text, code.text, Password.text, pattern);

                        }
                    }
                    else
                    {
                        
                        SystemMsgView.SystemFunction(Function.Tip, Info.PWNotSame);
                    }
                }
                else
                {
                    
                    SystemMsgView.SystemFunction(Function.Tip, Info.PWPattern);
                }
            }
            else
            {
               
                SystemMsgView.SystemFunction(Function.Tip, Info.SelectionNull);
            }
        }
        /// <summary>
        /// 发送验证码按钮
        /// 
        /// </summary>
        public virtual void OncodeBtnClick()
        {

            if (Input.text != "")
            {
                if (Regex.IsMatch(Input.text, pattern))
                {
                    LoginController.Instance.VeriCodeReq(Input.text);
                    CodeBtnListener(false);
                }
                else
                {

                    
                    SystemMsgView.SystemFunction(Function.Tip, Info.PhoneNum);
                }
            }
            else
            {
             
                SystemMsgView.SystemFunction(Function.Tip, Info.InputFieldNotNull);
            }
        }


    }
}
