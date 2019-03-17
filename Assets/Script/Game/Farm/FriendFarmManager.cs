using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework;

namespace Game
{
    public class FriendFarmManager : Singleton<FriendFarmManager>
    {
        public int FriendUid;
        public bool isVisiting=false;   //是否正在访问好友农场
        
        private Button goHome;
        private Text goHome_Text;
        private Transform actionBar;
        private Transform funtionBar;
        public Transform MsgWindows;
        public List<int> hasSteal=new List<int>();
        public void Init()
        {
            goHome = GameObject.FindWithTag("GoHomeBtn").GetComponent<Button>();
            goHome.onClick.AddListener(OnClickSeedFriend);
            goHome_Text= goHome.transform.Find("Text").GetComponent<Text>();
            goHome_Text.text = "别家看看";
        }

        //访问好友农场
        public void GoFriendFarm(int friendUid)
        {
            ChatModel.Instance.chatLog.Clear();
            if (MsgWindows != null)
            {
                for (int i = 0; i < MsgWindows.childCount; i++)
                {
                    if (MsgWindows.GetChild(i) != null)
                    {
                        GameObject.Destroy(MsgWindows.GetChild(i).gameObject);
                    }
                }
            }
            ViewMgr.Instance.Open(ViewNames.ChatView);
            FriendUid = friendUid;
            if (actionBar == null)
            {
                actionBar = GameObject.Find("ActionBar").transform;
            }
            if (funtionBar==null)
            {
                funtionBar = GameObject.Find("FunctionBar").transform;
            }
            goHome.onClick.RemoveAllListeners();
            goHome.onClick.AddListener(GoHome);
            goHome_Text.text = "返回农场";

            actionBar.gameObject.SetActive(false);
            funtionBar.gameObject.SetActive(false);
            FriendsInfoController.Instance.SendSingleFriendInfoReq(FriendUid);
            CommonActionBarView.Action1 = GameAction.None;
            //GlobalDispatcher.Instance.Dispatch(FriendsInfoEvent.OnVisitedFriend);
            isVisiting = true;
        }

        //别家看看
        public void OnClickSeedFriend()
        {
            CommonActionBarView.Action1=GameAction.None;
            bool isHas = false;
            Dictionary<int, PlayerInfo> friends = FriendsInfoModel.Instance.playerInfos;
            foreach (PlayerInfo friend in friends.Values)
            {
                if (friend.Aciton == 1&&!hasSteal.Contains(friend.UserGameId))
                {
                    FriendFarmManager.Instance.GoFriendFarm(friend.UserGameId);
                    hasSteal.Add(friend.UserGameId);
                    isHas = true;
                    break;
                }
            }
            if (isHas == false)
            {
                SystemMsgView.SystemFunction(Function.Tip,Info.NoFriendCanSteal);
            }
        }

        private int index;
        //返回自己的农场
        public void GoHome()
        {
            if (isVisiting)
            {
                GlobalDispatcher.Instance.Dispatch(FriendsInfoEvent.Gohome);
                goHome.onClick.RemoveAllListeners();
                goHome.onClick.AddListener(OnClickSeedFriend);
                goHome_Text.text = "别家看看";

                actionBar.gameObject.SetActive(true);
                funtionBar.gameObject.SetActive(true);
                FriendsInfoController.Instance.SendFriendsInfoReq(0);
                FieldsController.Instance.SendFieldsReflashAction();
                ViewMgr.Instance.Close(ViewNames.ChatView);
                FriendUid = 0;
                isVisiting = false;
            }
          
        }
    }
}