using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using Game;
using UnityEngine;

namespace Framework
{
    public class ViewMgr : Singleton<ViewMgr>
    {
        private RectTransform ViewRoot;
        private RectTransform Canvas;
        private Transform Panel2D;
        public RectTransform DialogRoot;
        public RectTransform WindowRoot;
        public RectTransform FullScreenRoot;

        public Dictionary<string, BaseViewController> views = new Dictionary<string, BaseViewController>();
        public void Init()
        {
            ViewRoot = GameObject.Find("Canvas/ViewRoot").transform as RectTransform;
           
            Canvas = GameObject.Find("Canvas").transform as RectTransform;
            Panel2D = GameObject.Find("Panel2D").transform;
            DialogRoot = ViewRoot.Find("DialogRoot") as RectTransform;
            WindowRoot = ViewRoot.Find("WindowRoot") as RectTransform;
            FullScreenRoot = ViewRoot.Find("FullScreenRoot") as RectTransform;
        }

        public void HideAllView()
        {
            Canvas.gameObject.SetActive(false);
            Panel2D.gameObject.SetActive(false);
        }
        public void ShowAllView()
        {
            Canvas.gameObject.SetActive(true);
            Panel2D.gameObject.SetActive(true);
        }
      
        private void OnLoadViewRes(Resource res, bool succ)
        {
            if (succ)
            {
                string viewname = ViewConfig.GetViewName(res.path);
                BaseViewController vc;
                //此处为修改框架，防止同时存在两个view，直接覆盖
                if (views.TryGetValue(viewname, out vc))
                {
                    views.Remove(viewname);
                    vc.Close();
                }
                GameObject go = GameObject.Instantiate(res.UnityObj) as GameObject;

                switch (ViewConfig.Instance.GetViewCo(viewname).viewtype)
                {
                    case ViewType.Dialog:
                        go.transform.SetParent(DialogRoot,false);
                        break;
                    case ViewType.Window:
                        go.transform.SetParent(WindowRoot,false);
                        break;
                    case ViewType.Full:
                        go.transform.SetParent(FullScreenRoot,false);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                vc = BaseViewController.Create(viewname,go);
                
                
                views.Add(viewname,vc);
                //从此处跳转到View覆盖方法
                vc.Build();
                vc.OnBuild();
                vc.Open();
                vc.OnOpen();

//                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", go.name, "test1"));

                GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnViewLoadFinished,go);
            }
            else
            {
                Debug.LogError("load view "+res.path+" fail");
            }
        }

        public bool isOpen(string viewName)
        {
            BaseViewController vc;
            if (views.TryGetValue(viewName, out vc))
            {
                return vc.IsOpen;
            }
            return false;
        }
        public void Open(string viewName, object p = null)
        {
            BaseViewController vc;
            if (views.TryGetValue(viewName, out vc))
            {
                if (!vc.IsOpen)
                {
                    vc.Open();
                    vc.OnOpen();
                }
            }
            else
            {
                string viewpath = ViewConfig.GetViewPath(viewName);
                ResourceMgr.Instance.LoadResource(viewpath, OnLoadViewRes);
            }


        }
//
        public void CloseAllview()
        {
            // ReSharper disable once GenericEnumeratorNotDisposed
           Dictionary<string ,BaseViewController>.Enumerator baseViewController = views.GetEnumerator();
            while (baseViewController.MoveNext())
            {
                if (views.ContainsKey(baseViewController.Current.Key))
                {
                    if (baseViewController.Current.Key!=ViewNames.CommonView)
                    {
                        Close(baseViewController.Current.Key);

                    }
                }
                
            }
             
              
            
        }
        public void Close(string viewName)
        {
//            MusicManager.Instance.Playsfx(AudioNames.CloseBtn);
            BaseViewController vc;
            if (views.TryGetValue(viewName, out vc))
            {
                vc.Close();
                vc.OnClose();
                ViewCo co = ViewConfig.Instance.GetViewCo(viewName);
                if (co.closeType == ViewCo.CloseType.Destroy)
                {
                    vc.OnDestroy();
                    vc.Destroy();
                    views.Remove(viewName);

                }
            }
        }
    }

    public enum ViewType
    {
        Dialog,
        Window,
        Full,
    }
   
    public class ViewNames
    {
        public const string LoginView = "LoginView";
        public const string CalendarView = "CalendarView";
        public const string PayChooseView = "PayChooseView";
        public const string ShareView = "ShareView";
        public const string SettingView = "SettingView";

        public const string PlowNeedView = "PlowNeedView";
        public const string RankingListView = "RankingListView";
        public const string ShopView = "ShopView";
        public const string FriendsListView = "FriendsListView";

        public const string StoreView = "StoreView";
        public const string FactoryView = "FactoryView";
        public const string CommonView = "CommonView";
        public const string SystemMsgView = "SystemMsgView";
        public const string CommitView = "CommitView";
        public const string SeedBarView = "SeedBarView";

        public const string MsgListView = "MsgListView";
        public const string CreateNewPlayerView = "CreateNewPlayerView";
        public const string PlayerInfoView = "PlayerInfoView";
        public const string DogInfoView = "DogInfoView";
        public const string ChatView = "ChatView";

        public const string OnLineView = "OnLineView";
        public const string IntroduceView = "IntroduceView";
        public const string TimeBarView = "TimeBarView";
        public const string UpdateView = "UpdateView";
        public const string JYSenseView = "JYSenseView";
        public const string StrategyView = "StrategyView";
        public const string HelpView = "HelpView";
    }

}
