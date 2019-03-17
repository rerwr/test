using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework;
using System;

namespace Game
{
    public class ChatMsgView : BaseSubView
    {
        private InputField SendMsg_Content;
        private Button SendMsg_Button;

        private Transform MsgWindows;
        private GameObject chatItem;
        private bool isFrist = true;
        private bool isReqingLog = false;
        private ScrollRect SR;
        private Transform loadingImg;

        public ChatMsgView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
        }

        public override void BuildUIContent()
        {
            base.BuildUIContent();

            SendMsg_Content = TargetGo.transform.Find("SendMessage/InputField").GetComponent<InputField>();
            SendMsg_Button = TargetGo.transform.Find("SendMessage/Button").GetComponent<Button>();
            SendMsg_Button.onClick.AddListener(OnClickSendMsg);
            MsgWindows = TargetGo.transform.Find("Message/MessageWindow");
            ResourceMgr.Instance.LoadResource("Prefab/ChatItem", OnLoadPre);
            SR = TargetGo.transform.Find("Message").GetComponent<ScrollRect>();
            SR.onValueChanged.AddListener(OnGetLog);
            loadingImg= TargetGo.transform.Find("Image");
            loadingImg.gameObject.SetActive(false);
        }

        public override void OnOpen()
        {
            base.OnOpen();
            FriendsInfoController.Instance.GetDispatcher().AddListener(FriendsInfoEvent.OnVisitedFriend, OnVisit);
            ChantController.Instance.GetDispatcher().AddListener(ChantControllerEvent.OnReciveLog,OnReviceLog);
            ChantController.Instance.GetDispatcher().AddListener(ChantControllerEvent.OnChat, OnOpenChatView);
            MessageController.Instance.GetDispatcher().AddListener(MessageEvent.OnGetMsgList, OnReviceContent);
            if (ChatModel.Instance.currentPage == 1)
            {
                //ChantController.Instance.ReqChatLog(1);
                ChatLogManager.Instance.GetData(ChatModel.Instance.ChatTarget.UserGameId,1);
            }
            //TargetGo.SetActive(false);
            FriendFarmManager.Instance.MsgWindows = this.MsgWindows;
        }

        private bool OnVisit(int eventId, object arg)
        {
            
            return false;
        }

        private void OnGetLog(Vector2 v)
        {
            if (!isReqingLog&&v.y>1+1.5f/SR.content.childCount)
            {
                //Debug.LogError(v.y + "  " + 1.5f / SR.content.childCount);
                isReqingLog = true;
                loadingImg.gameObject.SetActive(true);
                //ChantController.Instance.ReqChatLog(ChatModel.Instance.currentPage);
                ChatLogManager.Instance.GetData(ChatModel.Instance.ChatTarget.UserGameId, ChatModel.Instance.currentPage);
                //Debug.LogFormat("<color=yellow><---{0}----></color>","请求聊天记录");
                //Debug.LogError("请求聊天记录"+ ChatModel.Instance.currentPage);
            }
        }

        //加载chatitem预制回调
        private void OnLoadPre(Resource res,bool succ)
        {
            if (succ)
            {
                chatItem = res.UnityObj as GameObject;
                if (!isFrist)
                {
                    OnReviceLog(0, null);
                }
            }
        }

        //点击发送消息按钮
        private void OnClickSendMsg()
        {
            if (SendMsg_Content.text != "")
            {
                ChantController.Instance.SendContent(SendMsg_Content.text);
                ChatLog c = new ChatLog();
                c.SendPlayer = 1;
                c.Content = SendMsg_Content.text;
                c.sendTime = System.DateTime.Now.Year+"/"+ System.DateTime.Now.Month + "/" + 
                    System.DateTime.Now.Day + " " + System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute;
                AddContent(c);
                SendMsg_Content.text = "";
                ChatLogManager.Instance.SaveData(ChatModel.Instance.ChatTarget.UserGameId, c);
            }
        }

        //接收聊天信息
        private bool OnReviceContent(int eventId, object arg)
        {
            Dictionary<int, MsgUnit> msgList = MessageModel.Instance.MsgList;
            List<MsgUnit> _msgList = new List<MsgUnit>(msgList.Values);
            foreach (MsgUnit msg in _msgList)
            {
                if (msg.type == 2&& msg.PlayerUid==ChatModel.Instance.ChatTarget.UserGameId)
                {
                    ChatLog c = new ChatLog();
                    c.SendPlayer = 2;
                    c.Content = msg.content;

                    System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
                    DateTime dt = startTime.AddSeconds(msg.SendTime);
                    //System.Debug.Log(dt.ToString("yyyy/MM/dd HH:mm:ss:ffff"));
                    c.sendTime = dt.ToString("yyyy/MM/dd HH:mm");
                    AddContent(c);
                    ChatLogManager.Instance.SaveData(ChatModel.Instance.ChatTarget.UserGameId, c);
                    MessageController.Instance.DelMsg(msg.id);
                }
            }
            return false;
        }

        //将聊天记录回调放入聊天窗口
        private bool OnReviceLog(int eventId,object arg)
        {
            if (TargetGo.activeInHierarchy == false)
            {
                return false;
            }

            isFrist = false;
            if (chatItem == null)
            {
                return false;
            }

            //for (int i = 0; i < MsgWindows.childCount; i++)
            //{
            //    if (MsgWindows.GetChild(i) != null)
            //    {
            //        GameObject.Destroy(MsgWindows.GetChild(i).gameObject);
            //    }
            //}

            List<ChatLog> log=ChatModel.Instance.chatLog;
            for (int i = 0; i < log.Count; i++)
            {
                GameObject go = GameObject.Instantiate(chatItem) as GameObject;
                go.transform.SetParent(MsgWindows);
                go.transform.localScale = new Vector3(1, 1, 1);
                go.transform.SetSiblingIndex(i);
                ChatItem _chatLog = go.AddComponent<ChatItem>();
                _chatLog.Init(log[i]);
            }

            //Debug.LogError("OnReviceLog:"+ log.Count);
            loadingImg.gameObject.SetActive(false);
            MTRunner.Instance.StartRunner(DelayRun(()=> 
            {
                //Debug.LogError("isReqingLog = false;");
                isReqingLog = false;
            }));
            return false;
        }

        private IEnumerator DelayRun(Action a)
        {
            yield return 3.0f;
            a();
        }

        //将一条聊天内容放入聊天窗口
        private void AddContent(ChatLog c)
        {
            GameObject go = GameObject.Instantiate(chatItem) as GameObject;
            go.transform.SetParent(MsgWindows);
            go.transform.localScale = new Vector3(1, 1, 1);

            ChatItem _chatLog = go.AddComponent<ChatItem>();
            _chatLog.Init(c);
        }

        //打开chatview
        private bool OnOpenChatView(int eventId,object arg)
        {
            bool isChating = (bool)arg;
            TargetGo.SetActive(isChating);
            if (TargetGo.activeInHierarchy)
            {
                OnReviceLog(0,null);
            }
            return false;
        }

        public override void OnClose()
        {
            base.OnClose();
            FriendsInfoController.Instance.GetDispatcher().RemoveListener(FriendsInfoEvent.OnVisitedFriend, OnVisit);
            ChantController.Instance.GetDispatcher().RemoveListener(ChantControllerEvent.OnReciveLog, OnReviceLog);
            MessageController.Instance.GetDispatcher().RemoveListener(MessageEvent.OnGetMsgList, OnReviceContent);
            ChantController.Instance.GetDispatcher().RemoveListener(ChantControllerEvent.OnChat, OnOpenChatView);
        }
    }
}