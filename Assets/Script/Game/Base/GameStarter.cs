using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Aliyun.OSS.Samples;
using Assets.Script.Utility;
using Framework;
using UnityEngine;

namespace Game
{


    public class GameStarter : SingletonMonoBehaviour<GameStarter>
    {

        public ViewNames _showView;
        public bool isfirstLogin = false;
        //是否开启开发者模�?
        public bool isDebug = false;
        [Tooltip("是否开启心跳包")] public bool isOpenBeating = true;
        void Start()
        {
            //设置自动锁屏
            Screen.sleepTimeout = 60;
            //            PlayerPrefs.DeleteAll();
            //            GetObjectSample.GetObject("AsyncGetObject");
            //事件通知
            InitBaseClasses();
            //加载基础代码
            InitSigletonMonos();
            //为下面的配置类中事件，完成添加方法执�?
            InitListener();

            //加载配置文件,配置类中有事�?
            InitConfigs();
            //初始model
            InitModels();
            //注册网络组件方法
            InitControllers();
            //加载配置文件后才能打开
            //            ViewMgr.Instance.Open(ViewNames.PayChooseView);
            //没有key则为第一次登录
            if (isDebug)
            {
                PlayerPrefs.DeleteAll();
            }
       
            if (!PlayerSave.HasKey("isFirstLogin"))
            {
                isfirstLogin = true;
                //是第一
                PlayerSave.SetInt("isFirstLogin", 1);
            }
//            string pAlipayOrder = AlipayOrderMgr.Instance.GetAlipayOrder();

//            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", pAlipayOrder, "test1"));

            //            //开发者模式
            //            if (isDebug)
            //            {
            //                isfirstLogin = true;
            //
            //            }
            //                        string  encrycode=  EncryptManager.EncryptDES("isFirstLogin", "98765432");
            //                        string  decrycode=  EncryptManager.DecryptDES(encrycode, "98765432");
            //            
            //                        Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", encrycode, decrycode));
            MusicSetting();
        }

        void MusicSetting()
        {
            if (PlayerSave.HasKey("ismute"))
            {
                if (PlayerSave.GetInt("ismute") == 1)
                {
                    MusicManager.Instance.IsMute = true;
                }
                else
                {
                    MusicManager.Instance.IsMute = false;
                }
            }
        }

        private void InitListener()
        {
            GlobalDispatcher.Instance.AddListener(GlobalEvent.OnUpdateEnd, (e, a) =>
            {
                
                ViewMgr.Instance.Open(ViewNames.OnLineView);
                return true;
            });
            GlobalDispatcher.Instance.AddListener(GlobalEvent.OnLine, (e, a) =>
            {
                //请求公告信息
//                AnnouncementController.Instance.ReqInfo();
                MTRunner.Instance.StartRunner(Wait(0.3f));
                return true;
            });
            GlobalDispatcher.Instance.AddListener(GlobalEvent.OnDataConfigLoadDone, InitLoad);
            GlobalDispatcher.Instance.AddListener(GlobalEvent.OnLoadURLConfig, InitURL);
            GlobalDispatcher.Instance.AddListener(GlobalEvent.OnLoginConfigLoadDone, Connect2Server);
            GlobalDispatcher.Instance.AddListener(LoginEvent.OnLoginSucc, (eventId,arg)=> 
            {
                
                MusicManager.Instance.Playsfx(AudioNames.OpenAudio);
//                if (!isDebug)
//                {
//如果没有支付则打开支付
//                    || LoginModel.Instance.IsPayForApp == false
                    if (LoginModel.Instance.IsPayForApp == false)
                    {
                        //当前为登录支付
                        PayOrderInterfaceMgr.Instance.payfor=PayFor.Login;
                        ViewMgr.Instance.Open(ViewNames.PayChooseView);
                    }
                    else
                    {
                        StoreController.Instance.ReqStoreInfo(LoginModel.Instance.Uid);
                    }
//                    else{
////                        if (GameStarter.Instance.isfirstLogin)
//                        {
//
//                            ViewMgr.Instance.Open(ViewNames.PlayerInfoView);
//
//                        }

//                    }
//                }
                
                return true;
            });
        }

        IEnumerator Wait(float time)
        {
            yield return time;
            ViewMgr.Instance.Open(ViewNames.LoginView);

        }
        #region ListenerMethod
        private bool InitLoad(int eventId, object arg)
        {
            //此处显示初始view
            ViewMgr.Instance.Init();
            StartUpdate();
            MusicManager.Instance.PlayBGM(AudioNames.BGM2);//播放背景音乐
            return true;
        }

        private bool Connect2Server(int eventID, object arg)
        {

            LoadObjctDateConfig.Instance.InitConfig();
            GameTextDataMgr.Instance.GetDatas();
//            PayOrderInterfaceMgr.Instance.GetDatas(1,Urltype.init);
            LoginController.Instance.Connect2Sever();
            return true;
        }

        private bool InitURL(int eventID, object arg)
        {
            SpritesManager.Instance.Init();
            return true;
        }

        #endregion

        private void InitBaseClasses()
        {
            GlobalDispatcher.Create();
        }

        private void InitSigletonMonos()
        {
            gameObject.AddComponent<MTRunner>();
            gameObject.AddComponent<Clock>();
            gameObject.AddComponent<SocketMgr>();
            gameObject.AddComponent<MusicManager>();
            gameObject.AddComponent<SpritesManager>();
        }

        private void InitConfigs()
        {
            ViewURLConfig.Instance.InitConfig();
            ViewConfig.Instance.InitConfig();
            LoginConfig.Instance.InitConfig();
        }

        private void InitModels()
        {
            LoginModel.Instance.InitModel();
            Farm_Game_StoreInfoModel.Instance.InitModel();
            ShopModel.Instance.InitModel();
            FieldsModel.Instance.InitModel();
            CommitViewModel.Instance.InitModel();
            FriendsInfoModel.Instance.InitModel();
            SingeFriendInfoModel.Instance.InitModel();
            RankInfoModel.Instance.InitModel();
            MessageModel.Instance.InitModel();
            SearchInfoModel.Instance.InitModel();
            ChatModel.Instance.InitModel();
        }
        private void InitControllers()
        {
            LoginController.Instance.InitController();
            HeartBeatController.Instance.InitController();
            CommonController.Instance.InitController();
            FieldsController.Instance.InitController();
            AnnouncementController.Instance.InitController();

            FriendsInfoController.Instance.InitController();
            ShopController.Instance.InitController();
            StoreController.Instance.InitController();
            FactoryController.Instance.InitController();
            MessageController.Instance.InitController();
            DogInfoController.Instance.InitController();
            SignController.Instance.InitController();
            NewPlayerCreateController.Instance.InitController();
            ChantController.Instance.InitController();
            CommitController.Instance.InitController();
            FriendFarmManager.Instance.Init();

        }

        public void StartUpdate()
        {
            if (PlayerPrefs.GetInt("version") == 0)
            {
                PlayerPrefs.SetInt("version", VersionUpdateManager.version);
            }
            string versionStr="";
#if UNITY_ANDROID
            versionStr = @"http://119.23.48.181:8080/api/gameCheckVersion?version=" + VersionUpdateManager.version + "&os=android";
#endif

#if UNITY_IOS
            versionStr = @"http://119.23.48.181:8080/api/gameCheckVersion?version="+ VersionUpdateManager.version + "&os=ios";
#endif
            string _json = VersionUpdateManager.Instance.GetPage(versionStr);
            VersionUpdateManager.Instance.UpdateUrl = VersionUpdateManager.Instance.GetUrl(_json);
            if (!string.IsNullOrEmpty(VersionUpdateManager.Instance.UpdateUrl))
            {
                ViewMgr.Instance.Open(ViewNames.UpdateView);
            }
            else
            {
                ViewMgr.Instance.Open(ViewNames.OnLineView);
            }
        }

        void OnDisable()
        {
            SocketMgr.Instance.Disconnect();
            HeartBeatController.Instance.Stop();
        }

     

        void OnApplicationFocus(bool hasFocus)
        {

            if (hasFocus&& GlobalDispatcher.Instance!=null)
            {
                GlobalDispatcher.Instance.Dispatch(GlobalEvent.Onfocus);
            }

        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (!pauseStatus&& GlobalDispatcher.Instance!=null)
            {
                GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnPause);

            }

        }


     


        void Update()
        {
//            Debug.Log("<color=#0000ffff><-----Update-----></color>"+GC.GetTotalMemory(false));
        }


    }
}
