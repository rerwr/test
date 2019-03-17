using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Framework;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class LoginController : BaseController<LoginController>
    {
        private enum hide   
        {
            directEnter,//直接进入
            register,
            changepw,
            regis2,
            bind,
        }

        private bool isfirstStart = true;
        private hide hide1;
        public static int pattern = 0;  //0表示有密码直接登录，1表示注册成功后顺手登录,2表示登陆成功
        public string tempA;
        public string tempPW;
        private LoginView lv;
        private RegisterView rv;
        protected override Type GetEventType()
        {
            return typeof(LoginEvent);
        }

        public override void InitController()
        {
            _Proxy = new NetProxy(NetModules.Account.ModuleId); //这里是初始化本模块的网络组件
            //1-0手机帐号密码登录协议
            SocketParser.Instance.RegisterParser(NetModules.Account.ModuleId, NetModules.Account.ReqLoginSDK,
                ResponeLoginSDK.ParseFrom);
            _Proxy.AddNetListenner(NetModules.Account.ReqLoginSDK, LoginSDKRespone);
            //1-1
            SocketParser.Instance.RegisterParser(NetModules.Account.ModuleId, NetModules.Account.LogoutSDK,
                ReqLogoutSDK.ParseFrom); //注册对应消息（在服务器发过来时）的Proto解析器
            _Proxy.AddNetListenner(NetModules.Account.LogoutSDK, LogoutCallback);

            //1-2
            SocketParser.Instance.RegisterParser(NetModules.Account.ModuleId, NetModules.Account.ReqLogin,
                ResponeLogin.ParseFrom); //注册对应消息（在服务器发过来时）的Proto解析器
            _Proxy.AddNetListenner(NetModules.Account.ReqLogin, LoginCallback); //注册对应消息（在服务器发过来时）的回调

            //1-4
            SocketParser.Instance.RegisterParser(NetModules.Account.ModuleId, NetModules.Account.ReqReLogin,
                ResponeLogin.ParseFrom);
            _Proxy.AddNetListenner(NetModules.Account.ReqReLogin, LoginCallback);

            //1-6
            _Proxy.AddNetListenner(NetModules.Account.Farm_Game_Register_Req, RegisterCallBack);
            //1-7
            _Proxy.AddNetListenner(NetModules.Account.Farm_Game_VeriCode_Req, VeriCodeCallBack);
            GetDispatcher().AddListener(LoginEvent.LoginSDKRespone, ReqLoginSend);

        }
        //此时为微信点击，绑定面板
        void onclick()
        {
            
            if (rv.Input.text != "" && rv.code.text != "" && rv.Password.text != "" && rv.ConfirmPw.text != "")
            {

                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "test", "test1"));

                if (rv.regex.IsMatch(rv.Password.text))
                {
                    if (rv.Password.text == rv.ConfirmPw.text)
                    {
                         SendloginSDKReq(rv.Input.text, rv.Password.text, (int)Application.platform, "md5", "2", rv.code.text);
                      
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

        private bool iswechant = false;
        private bool ReqLoginSend(int eventId, object arg)
        {
            if (LoginModel.Instance.Isbinding==0)
            {
                iswechant = true;
                lv = ViewMgr.Instance.views[ViewNames.LoginView].Viewlist[0] as LoginView;

                lv.egv.OnregisterClick();
                 rv=lv.resRegisterView;
                rv.registerBtn.onClick.RemoveAllListeners();
                rv.registerBtn.onClick.AddListener(onclick);
            }
            else
            {
                if (iswechant)
                {
                    //微信登录采用面板2隐藏方式隐藏
                    hide1 = hide.register;
                }
                else
                {
                    hide1 = hide.directEnter;
                }
                ReqLogin(LoginModel.Instance.Uid, LoginModel.Instance.Token, (int)Application.platform);
               
                
            }
            return false;
        }

        //1-6
        //名称: 账号注册/忘记密码请求
        public void SendRegisterReq(string PhoneNum, string VeriCode,
            string password, int pattern, string BeRecommandedCode = "")
        {
            LoginController.pattern = pattern;
            var builder = Farm_Game_Register_Req.CreateBuilder();
            tempA = PhoneNum;
            tempPW = password;
            builder.PhoneNum = PhoneNum;
            builder.VeriCode = VeriCode;
            builder.Password = password;
            builder.Pattern = pattern;
            builder.BeRecommandedCode = BeRecommandedCode;
            _Proxy.SendMsg(NetModules.Account.ModuleId, NetModules.Account.Farm_Game_Register_Req, builder);

        }

        private void RegisterCallBack(MsgRec msg)
        {
            if (msg.succ == 1)
            {
                lv = ViewMgr.Instance.views[ViewNames.LoginView].Viewlist[0] as LoginView;

                EnterGameView gv = lv.subViews[0] as EnterGameView;
                SocketMgr.Instance.ConnectIPv4(LoginConfig.Instance.serverIP, LoginConfig.Instance.serverPort);
                if (LoginController.pattern == 1)
                {
                    GetDispatcher().Dispatch(LoginEvent.RegisterSucc);
                }
                else if (LoginController.pattern == 2)
                {
                    GetDispatcher().Dispatch(LoginEvent.ChangePWSucc);
                }
                gv.Set(tempPW, tempA);
                //                Debug.Log("------test------");
            }

        }

        //1-7//名称: 注册/忘记密码请求验证码请求响应
        private void VeriCodeCallBack(MsgRec msg)
        {
            if (msg.succ == 1)
            {
                Debug.Log("验证码已经发送");
                GetDispatcher().Dispatch(LoginEvent.VeriCodeSucc);

            }
        }

        public void VeriCodeReq(string phoneNum)
        {
            var builder = Farm_Game_VeriCode_Req.CreateBuilder();
            builder.PhoneNum = phoneNum;
            _Proxy.SendMsg(NetModules.Account.ModuleId, NetModules.Account.Farm_Game_VeriCode_Req, builder);

        }


        public void Connect2Sever()
        {
            if (SocketMgr.Instance.Status != SocketMgr.status.Connected)
            {

                SocketMgr.Instance.ConnectIPv4(LoginConfig.Instance.serverIP, LoginConfig.Instance.serverPort);

                //                Debug.LogError(LoginConfig.Instance.serverIP + ":" + LoginConfig.Instance.serverPort.ToString());
            }
        }

        //1-0
        private void LoginSDKRespone(MsgRec msg)
        {
            ResponeLoginSDK respone = (ResponeLoginSDK)msg._proto;
            
            LoginModel info = LoginModel.Instance;
            info.Token = respone.Token;
            info.Uid = respone.Uid;
            info.Isbinding=respone.IsBinding;
          
           
            GetDispatcher().Dispatch(LoginEvent.LoginSDKRespone);

        }
        //1-0
        public void SendloginSDKReq(string account, string password, int platform, string sign,string type,string Verification)
        {
            var builder = ReqLoginSDK.CreateBuilder();
            builder.Account = account;
            builder.Password = password;
            builder.Platform = platform;
            builder.Sign = sign;
            builder.Sex = LoginModel.Instance.Sex;
            builder.City = LoginModel.Instance.City;
            builder.Openid = LoginModel.Instance.Openid;
            builder.Language = LoginModel.Instance.Language;
            builder.Province = LoginModel.Instance.Province;
            builder.Headimgurl = LoginModel.Instance.WechatHeadimgurl;
            builder.Nickname = LoginModel.Instance.Nickname;
            builder.Type = type;
            builder.Verification = Verification;
            
            _Proxy.SendMsg(NetModules.Account.ModuleId, NetModules.Account.ReqLoginSDK, builder);
        }

        //1-1
        //退出登录下线
        private void LogoutCallback(MsgRec msg)
        {
            if (msg.succ == 1)
            {
                Debug.Log("登出成功");
                GetDispatcher().Dispatch(LoginEvent.LogoutSucc);
            }
        }

        public void Logout()
        {
            var logout = ReqLogoutSDK.CreateBuilder();
            
           _Proxy.SendMsg(NetModules.Account.ModuleId,NetModules.Account.LogoutSDK, logout);
        }

        //1-2 app->server  
        // 登录游戏
        public void ReqLogin(int uid, string token, int platform)
        {
            var builder = global::ReqLogin.CreateBuilder();
            builder.Uid = uid;
            builder.Token = token;
            //          builder.Platform = platform;
            _Proxy.SendMsg(NetModules.Account.ModuleId, NetModules.Account.ReqLogin, builder);
        }

        /// <summary>
        /// 注意此方法已经在_Proxy.AddNetListenner
        /// 于是在服务器发来对应消息后，本方法被调用1-2,1-4
        /// </summary>
        /// <param name="msg"></param>
        private void LoginCallback(MsgRec msg)
        {
            
            ResponeLogin p = (ResponeLogin)msg._proto; //取出Proto对象（在注册了本消息的Proto解析器后，此项才可能存在
            LoginModel loginModel = LoginModel.Instance;
           
            loginModel.Nickname = p.Username; //从Proto对象中取值

//            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "接收", p.Username));

            loginModel.Uid = p.Uid;
            loginModel.Head = p.Head;
            loginModel.Lv = p.Lv;
            loginModel.Exp = p.Exp;
            loginModel.LevelMaxExp=p.LevelMaxExp;
            loginModel.Oil1 = p.Oil;
            loginModel.DogLv = p.DogLv;
            loginModel.ContinueDays = p.ContinueDays;
            loginModel.Gold = p.Gold;
            loginModel.DogUpgradeMaxExp = p.DogUpgradeMaxEXP;
            loginModel.DogCurrentExp = p.DogCurrentEXP;
            loginModel.Chance = p.Chance;
            loginModel.Code = p.InvitationCode;
            loginModel.Account=p.Account;
            FieldsModel.Instance.SetData(p.MapArrayList);
            SignModel.Instance.SetData(p.AwardInfosList);
            loginModel.IsPayForApp=p.IsPayForAPP==1?true:false;
            if (FriendFarmManager.Instance.isVisiting)
            {
                FriendFarmManager.Instance.GoHome();
            }
            if (isfirstStart)
            {
                ViewMgr.Instance.Open(ViewNames.IntroduceView);
                isfirstStart = false;
            }

            CloseViewInfo info = new CloseViewInfo((int)hide1);
            GlobalDispatcher.Instance.Dispatch(LoginEvent.OnLoginSucc, info);
            GlobalDispatcher.Instance.Dispatch(GlobalEvent.onPlayerPanelReflash);
        }


        //1-4 client->server
        public void ReqRelogin(int uid, string token, int platform)
        {
            ReqReLogin.Builder builder = ReqReLogin.CreateBuilder();
            builder.Uid = uid;
            builder.Token = token;

            _Proxy.SendMsg(NetModules.Account.ModuleId, NetModules.Account.ReqReLogin, builder);

        }


    }

    public class LoginEvent : EventDispatcher.BaseEvent
    {
        public static readonly int OnLoginSucc = ++id;
        public static readonly int VeriCodeSucc = ++id;
        public static readonly int LoginSDKRespone = ++id;
        public static readonly int RegisterSucc = ++id;
        public static readonly int ChangePWSucc = ++id;
        public static readonly int LogoutSucc = ++id;
    }


}
