using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class CommonPlayerInfoView:BaseSubView
    {
        private Image PlayerIcon;
//        private Text Name;
        private Text PlayerLevel;
        private Text ECoinCountText;
        private Text EXPtext;
        private Text Name;
        private Text Oiltext;
        private Slider EXPtextBAR;
        private Slider OiltextBAR;

        //public  bool isClicked = false;

        private Button AddCrystals;
        private Button AddCoin;
        private Button AddDiammonds;
        private Button playerInfo;
        private Button btnSign;
        private Button btnTip;
        private Button btnStrategy;

        private Button HasMsg_Btn;
        private Image HasMsg_Image;
        private Text HasMsg_Text;

        private ANNController ann;
        private Button btnFrends;

        public CommonPlayerInfoView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
            
        }

        public override void BuildUIContent()
        {
            base.BuildUIContent();

            PlayerIcon = TargetGo.transform.Find("HeadPortrait/BG/PlayerInfoIcon").GetComponent<Image>();
            playerInfo = TargetGo.transform.Find("HeadPortrait/BG/PlayerInfoIcon").GetComponent<Button>();
//            Name = TargetGo.transform.Find("HeadPortrait/PlayerName/Text").GetComponent<Text>();
            PlayerLevel = TargetGo.transform.Find("HeadPortrait/PlayerLevel").GetComponent<Text>();
            Name = TargetGo.transform.Find("HeadPortrait/Name").GetComponent<Text>();
            btnSign = TargetGo.transform.Find("sign").GetComponent<Button>();
            btnTip = TargetGo.transform.Find("tip").GetComponent<Button>();
            btnStrategy = TargetGo.transform.Find("strategy").GetComponent<Button>();
            btnStrategy.onClick.AddListener((() => ViewMgr.Instance.Open(ViewNames.StrategyView)));
            ECoinCountText = TargetGo.transform.Find("HeadPortrait/ECoin/CountText").GetComponent<Text>();
            EXPtext = TargetGo.transform.Find("HeadPortrait/EXPbar/Text").GetComponent<Text>();
            EXPtextBAR = TargetGo.transform.Find("HeadPortrait/EXPbar").GetComponent<Slider>();
            Oiltext = TargetGo.transform.Find("HeadPortrait/JingYou/fillback (1)/Text").GetComponent<Text>();
            OiltextBAR = TargetGo.transform.Find("HeadPortrait/JingYou/fillback (1)").GetComponent<Slider>();
            btnFrends = TargetGo.transform.Find("Friends").GetComponent<Button>();
            btnFrends.onClick.AddListener(OnClickFriends);

            ann = TargetGo.transform.Find("Announcement").gameObject.AddComponent<ANNController>();
            ann.Init();

            playerInfo.onClick.AddListener(ChangePlayerIcon);

            btnSign.onClick.AddListener(OnClickSign);
            btnTip.onClick.AddListener(OnClickTipBtn);


            HasMsg_Btn = TargetGo.transform.Find("HasMsg").GetComponent<Button>();
            HasMsg_Btn.onClick.AddListener(OnClickHasMsg);
            HasMsg_Image = TargetGo.transform.Find("HasMsg/MsgCount").GetComponent<Image>();
            HasMsg_Text = TargetGo.transform.Find("HasMsg/MsgCount/Text").GetComponent<Text>();
        }
        public override void OnClose()
        {
            base.OnClose();
            AnnouncementController.Instance.GetDispatcher().RemoveListener(AnnouncementEvent.OnAnn, OnRefreshAnn);
            StoreController.Instance.GetDispatcher().RemoveListener(StoreEvent.OnStoreUnits, OnOilInfoChange);
                        GlobalDispatcher.Instance.RemoveListener(GlobalEvent.onPlayerPanelReflash, RefreshInfo);
            GlobalDispatcher.Instance.RemoveListener(GlobalEvent.onPlayerPanelReflash, ReflashIcon);

            GlobalDispatcher.Instance.RemoveListener(GlobalEvent.OnMsgChange, OnOnRefreshMsg);
        }
        void OnClickFriends()
        {

            if (!ViewMgr.Instance.isOpen(ViewNames.FriendsListView))
            {
                ViewMgr.Instance.Open(ViewNames.FriendsListView);
                //isClicked = true;
            }
            else
            {
                FriendsListView.RankingListView.SetActive(false);
                ViewMgr.Instance.Close(ViewNames.FriendsListView);
                //isClicked = false;

            }
        }
        public override void OnOpen()
        {
            base.OnOpen();
            OnOnRefreshMsg(0,MessageController.Instance.MsgCount);
         

            GlobalDispatcher.Instance.AddListener(GlobalEvent.onPlayerPanelReflash, ReflashIcon);
            GlobalDispatcher.Instance.AddListener(GlobalEvent.onPlayerPanelReflash, RefreshInfo);
            CommonController.Instance.GetDispatcher().AddListener(CommonController.CommonEvent.OnPersonReflash, RefreshInfo);
            ReflashIcon(0,null);

            GlobalDispatcher.Instance.Dispatch(GlobalEvent.onPlayerPanelReflash);
  
            StoreController.Instance.ReqStoreInfo(LoginModel.Instance.Uid);
            StoreController.Instance.GetDispatcher().AddListener(StoreEvent.OnStoreUnits,OnOilInfoChange);

            AnnouncementController.Instance.GetDispatcher().AddListener(AnnouncementEvent.OnAnn,OnRefreshAnn);

            MessageController.Instance.GetDispatcher().AddListener(MessageEvent.OnGetMsgList, (a,b)=> 
            {
                MessageController.Instance.MsgCount = MessageModel.Instance.MsgList.Count;
                GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnMsgChange, MessageController.Instance.MsgCount);
                return true;
            });

            GlobalDispatcher.Instance.AddListener(GlobalEvent.OnMsgChange, OnOnRefreshMsg);
            MessageController.Instance.MsgListReq();
        }


     

        private bool OnOnRefreshMsg(int eventId, object arg)
        {
            int msgCount = (int)arg;
            if (msgCount <= 0)
            {
                HasMsg_Image.gameObject.SetActive(false);
            }
            else
            {
                HasMsg_Text.text = msgCount.ToString();
                HasMsg_Image.gameObject.SetActive(true);
            }
            return false;
        }

        private bool OnRefreshAnn(int eventId,object arg)
        {
            ann.SetAnn(AnnouncementModel.Instance.Anouncement);
            return false;
        }

        void OnClickHasMsg()
        {
            ViewMgr.Instance.Open(ViewNames.MsgListView);
        }

        void OnClickSign()
        {
            
            ViewMgr.Instance.Open(ViewNames.CalendarView);
        }

        public void ChangePlayerIcon()
        {
            if (FriendFarmManager.Instance.isVisiting)
            {
                FriendFarmManager.Instance.GoHome();
            }
            else
            {
                ViewMgr.Instance.Open(ViewNames.PlayerInfoView);

            }
        }

        private void OnClickTipBtn()
        {
            ViewMgr.Instance.Open(ViewNames.JYSenseView);
        }

   

        private bool ReflashIcon(int id, object arg)
        {

//            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", LoginModel.Instance.Head, "test1"));

            AsyncImageDownload.Instance.SetAsyncImage(LoginModel.Instance.Head, PlayerIcon,true);
            Name.text = LoginModel.Instance.Nickname;
        
            return false;

        }
        //TODO
        //更新Player信息
        private bool RefreshInfo(int id,object arg)
        {
            
            LoginModel info=  LoginModel.Instance;
            int delta = int.Parse(ECoinCountText.text) - info.Gold;

//            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "delta", delta));

            if (delta>-9999999&&delta < 9999999 && delta!=0)
                {
                   
                    ResourceMgr.Instance.LoadResource("Prefab/GetTip", ((resource, b) =>
                    {
                        GameObject go = GameObject.Instantiate(resource.UnityObj) as GameObject;
                        go.transform.SetParent(ViewMgr.Instance.FullScreenRoot, false);
                        if (delta>=0)
                        {
                            go.transform.Find("Text").GetComponent<Text>().text = "-";
                        }
                        go.GetComponent<JumpingNumberTextComponent>().Change(0, delta > 0 ? delta : -delta);
                        MTRunner.Instance.StartRunner(Wait(2.2f, go));
                    }));
            }else
                    ECoinCountText.text = LoginModel.Instance.Gold.ToString();
                
            
            PlayerLevel.text = "V"+info.Lv.ToString();
            
            EXPtext.text = ((float)info.Exp/3600).ToString("0.0") +"/"+ ((float)info.LevelMaxExp/3600).ToString("0.0") + "h";
            EXPtextBAR.value = (float) info.Exp / info.LevelMaxExp;
           
            return false;
        }

        IEnumerator Wait(float time,GameObject go)
        {

            yield return time;
            ECoinCountText.text = LoginModel.Instance.Gold.ToString();

            GameObject.Destroy(go);

        }

        //刷新玩家精油数量显示
        private bool OnOilInfoChange(int eventId,object arg)
        {
            Dictionary<int,Oil> oils=Farm_Game_StoreInfoModel.storage.Oils;
            if (oils == null) return false;

            int count = 0;
            foreach (var oil in oils.Values)
            {
                if (oil.OilType == 1)
                {
                    count += oil.ObjectNum;
                }
            }

            Oiltext.text = count.ToString();
            OiltextBAR.minValue = 0;
            OiltextBAR.maxValue = 10000;
            OiltextBAR.value = count;
            return false;
        }

     
    }
}
