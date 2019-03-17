using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;



using DG.Tweening;
using Framework;

using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class EnterGameView : BaseSubView
    {
        private InputField inputAccount;

        private InputField password;
        private Button BtnLogin;

        private Button register;
        private Button forgetpw;

        private Button QQLogin;
        private Button WechantLogin;
        string pattern = @"1\d{10}";

        private Button BtnConnect;

        private LoginView lv;

        private Tweener EnterGameTweener;

        private Tweener ChangePWViewTweener;

        private Tweener RegisterViewTweener;

        public EnterGameView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {

        }


        public override void BuildUIContent()
        {
            base.BuildUIContent();
            inputAccount = TargetGo.transform.Find("InputBG/InputAccount").GetComponent<InputField>();

            password = TargetGo.transform.Find("InputBG/Password").GetComponent<InputField>();
            BtnLogin = TargetGo.transform.Find("InputBG/BtnLogin").GetComponent<Button>();
            BtnLogin.onClick.AddListener(SendLoginSDKReq);

            register = TargetGo.transform.Find("InputBG/registerBtn").GetComponent<Button>();
            register.onClick.AddListener(OnregisterClick);
            forgetpw = TargetGo.transform.Find("InputBG/forgetPasswordBtn").GetComponent<Button>();
            forgetpw.onClick.AddListener(OnforgetpwClick);

            QQLogin = TargetGo.transform.Find("QQLogin").GetComponent<Button>();
            QQLogin.onClick.AddListener(LoginQQ);

            WechantLogin = TargetGo.transform.Find("WechantLogin").GetComponent<Button>();
            WechantLogin.onClick.AddListener(LoginWechant);
            lv = ViewMgr.Instance.views[ViewNames.LoginView].Viewlist[0] as LoginView;
            //            var mover =BtnLogin.gameObject.AddComponent<LoginViewMover>();
            //            mover.speed = 10;
            //            mover.max = 10;
            //            mover.min = -10;
           
            inputAccount.onEndEdit.AddListener(Chang);
            password.onEndEdit.AddListener(Chang);
        }

        private void Chang(string a)
        {
            if (password.text!="")
            {
                PlayerSave.SetString("Account", inputAccount.text);
                PlayerSave.SetString("Password", password.text);
            }
           
        }
        public override void OnOpen()
        {
            base.OnOpen();
//            if (GameStarter.isfirstLogin)
            {
                EnterGameTweener = DotweenManager.DOLocalMoveY(TargetGo).SetAutoKill(false);
                GlobalDispatcher.Instance.AddListener(LoginEvent.OnLoginSucc, HideView);
                if (PlayerSave.HasKey("Account") || PlayerSave.HasKey("Password"))
                {

                    Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", PlayerSave.GetString("Account"), "test1"));

                    password.text = PlayerSave.GetString("Password");
                    inputAccount.text = PlayerSave.GetString("Account");
                }
            }
        }


        public override void OnClose()
        {
            base.OnClose();
            GlobalDispatcher.Instance.RemoveListener(LoginEvent.OnLoginSucc, HideView);
        }

        private void Clear()
        {
            inputAccount.text = "";
            password.text = "";
        }

        public void LoginQQ()
        {
//            NativeHandle hadHandle=new NativeHandle();
//            hadHandle.WechatPay();

        }
        public void LoginWechant()
        {
            ShareDemo.Instance.OnAuthWeChantClick();
            LoadingImageManager.Instance.AddLoadingItem();

        }
        public void Set(string pw, string a)
        {
            inputAccount.text = a;
            password.text = pw;
        }
       

        /// <summary>
        /// 点击忘记密码按钮
        /// </summary>
        private void OnforgetpwClick()
        {
//            Debug.Log("------test------");

            EnterGameTweener.PlayBackwards();
            EnterGameTweener.onPause += Pause;
            Clear();

        }
        private void Pause()
        {

            TargetGo.SetActive(false);
            ChangePWViewTweener = DotweenManager.DOLocalMoveY(lv.ChangePWView).SetAutoKill(false);
            //确保此方法只运行一次
            EnterGameTweener.onPause -= Pause;
        }
        /// <summary>
        /// 点击注册按钮
        /// </summary>
        public void OnregisterClick()
        {
//            Debug.Log("------test------");
            EnterGameTweener.PlayBackwards();
            EnterGameTweener.onPause += PauseRegister;
            Clear();
        }

        private void PauseRegister()
        {

            TargetGo.SetActive(false);
            RegisterViewTweener = DotweenManager.DOLocalMoveY(lv.RegisterView).SetAutoKill(false);
            //确保此方法只运行一次
            EnterGameTweener.onPause -= PauseRegister;
        }
        /// <summary>
        /// 隐藏面板动画
        /// 
        /// </summary>
        public bool HideView(int id, object arg)
        {
            CloseViewInfo closeView = arg as CloseViewInfo;
            int pattern = closeView.pattern;
            //返回的时候保存密码
            if (PlayerSave.HasKey("Account") || PlayerSave.HasKey("Password"))
            {
                password.text = PlayerSave.GetString("Password");
                inputAccount.text = PlayerSave.GetString("Account");
            }
            if (pattern == 0)
            {
                //直接登录动画
                EnterGameTweener.PlayBackwards();

                EnterGameTweener.onPause += () =>
                {
                    LoginEnterToMain();
                };

            }
            else if (pattern == 1)
            {
                //注册账号界面直接登录
                RegisterViewTweener.PlayBackwards();

                RegisterViewTweener.onPause += () =>
                {
                    LoginEnterToMain();

                };
            }
            else if (pattern == 2)
            {
                ChangePWViewTweener.PlayBackwards();

                ChangePWViewTweener.onPause += () =>
                {

                    LoginEnterToMain();
                };


            }
            else if (pattern == 3)
            {
                //注册账号界面直接登录
                RegisterViewTweener.PlayBackwards();

                RegisterViewTweener.onPause += () =>
                {
                    lv.EnterGameView.SetActive(true);
                    EnterGameTweener.PlayForward();
                    if (PlayerSave.HasKey("Account") && PlayerSave.HasKey("Password"))
                    {
                        password.text = PlayerSave.GetString("Password");
                        inputAccount.text = PlayerSave.GetString("Account");
                    }
                };
            }
            else if (pattern == 4)
            {
                ChangePWViewTweener.PlayBackwards();

                ChangePWViewTweener.onPause += () =>
                {
                    lv.EnterGameView.SetActive(true);

                    EnterGameTweener.PlayForward();
                    if (PlayerSave.HasKey("Account") && PlayerSave.HasKey("Password"))
                    {
                        password.text = PlayerSave.GetString("Password");
                        inputAccount.text = PlayerSave.GetString("Account");
                    }
                };
            }
           
            return false;
        }

        private void LoginEnterToMain()
        {
            MTRunner.Instance.StartRunner(wait(0.3f));
            //设置重连需要登录
            SocketMgr.Instance._isneed2loginview = true;

            //让登陆背景消失
            DotweenManager.DoFade(LoginView.BG);
            ViewMgr.Instance.Close(ViewNames.LoginView);
            ViewMgr.Instance.Open(ViewNames.CommonView);
           
            //            FieldsController.Instance.SendFieldsReflashAction();
            //变换主场景背景音乐

            MusicManager.Instance.PlayBGM(AudioNames.BGM1);

        }
        
        IEnumerator wait(float time)
        {
            yield return time;
            LoadingImageManager.Instance.StopLoading();
            
        }
        //1-0 普通手机号登录方法
        private void SendLoginSDKReq()
        {
            if (inputAccount.text != "" && password.text != "")
            {
                if (Regex.IsMatch(inputAccount.text, pattern))
                {
                    LoadingImageManager.Instance.AddLoadingItem();
                    LoginModel.Instance.Account = inputAccount.text;
                    LoginModel.Instance.Password = password.text;
                    
#if UNITY_ANDROID&&!UNITY_EDITOR

                    LoginController.Instance.SendloginSDKReq(inputAccount.text, password.text,
                        (int)Application.platform, "11111111", 1.ToString(),"");
                    //TODO
                    //                    LoginController.Instance.SendloginSDKReq(inputAccount.text, password.text,
                    //                        (int)Application.platform, GetSignatureMD5Hash(), 1.ToString());

                    Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", EncryptManager.GetSignatureMD5Hash(), "test1"));
#else
                     LoginController.Instance.SendloginSDKReq(inputAccount.text, password.text,
                        (int)Application.platform,"000000000",1.ToString(),"code");
#endif

                }
                else
                {
                   
                    SystemMsgView.SystemFunction(Function.Tip, Info.PhoneNum, 1.5f);
                }

            }
            else
            {
            
                SystemMsgView.SystemFunction(Function.Tip, Info.InputFieldNotNull, 1.5f);
            }
        }
        //微信登录方法
        public void SendLoginSDKReq(string type)
        {
         
#if UNITY_ANDROID
                    //前面账号密码要为空
                    LoginController.Instance.SendloginSDKReq("", "",
                        (int)Application.platform, "11111", type,"");
                    
                    //TODO
                    //                    LoginController.Instance.SendloginSDKReq(inputAccount.text, password.text,
                    //                        (int)Application.platform, GetSignatureMD5Hash(), type);

//                    Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", GetSignatureMD5Hash(), "test1"));
#else
                     LoginController.Instance.SendloginSDKReq(inputAccount.text, password.text,
                        (int)Application.platform,"000000000",type,"");
#endif

                
         
        }



      


    }


}
