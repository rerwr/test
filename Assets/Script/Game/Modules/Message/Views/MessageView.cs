using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework;

namespace Game
{
    public class MessageView : BaseSubView
    {
        private Button ClearBtn;
        private Button CloseBtn;
        private Button OrderBtn;
        private Transform MsgList;

        private GameObject MessageItemPrefab;

        public MessageView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
        }

        public override void BuildSubViews()
        {
            subViews = new List<BaseSubView>();
            subViews.Add(new MessageSystemView(TargetGo.transform.Find("SystemMsgView").gameObject, ViewController));
            subViews.Add(new AddFriendSuccView(TargetGo.transform.Find("AddFriendSuccView").gameObject, ViewController));
            base.BuildSubViews();
        }

        public override void BuildUIContent()
        {
            base.BuildUIContent();
            CloseBtn = TargetGo.transform.Find("Msg/CloseBtn").GetComponent<Button>();
            CloseBtn.onClick.AddListener(OnClickCloseBtn);
            ClearBtn = TargetGo.transform.Find("Msg/ClearBtn").GetComponent<Button>();
            ClearBtn.onClick.AddListener(OnClearBtn);
            MsgList = TargetGo.transform.Find("Msg/Message/MessageList");
            OrderBtn= TargetGo.transform.Find("Msg/OrderBtn").GetComponent<Button>();
            OrderBtn.onClick.AddListener(OnClickOrderBtn);
            ResourceMgr.Instance.LoadResource("Prefab/MessageItem", OnLoadMsgItem);
        }

        public override void OnOpen()
        {
            base.OnOpen();
            MessageController.Instance.MsgListReq();
            MessageController.Instance.GetDispatcher().AddListener(MessageEvent.OnGetMsgList,LoadMsgList);
            CommitController.Instance.GetDispatcher().AddListener(CommitController.CommitControllerEvent.OnOrderCallback, LoadOrderList);
        }

        private void OnClearBtn()
        {
            for (int i = 0; i < MsgList.childCount; i++)
            {
                GameObject.Destroy(MsgList.GetChild(i).gameObject);
            }
        }
        //点击关闭消息界面按钮
        private void OnClickCloseBtn()
        {
            ViewMgr.Instance.Close(ViewNames.MsgListView);
        }

        //点击查询订单消息
        private void OnClickOrderBtn()
        {
            OnClearBtn();
            OrderController.Instance.ClearOrder();
//                        OrderController.Instance.GetData();
            CommitController.Instance.CheckOrderReq();
        }

        //加载MessageItem回调
        private void OnLoadMsgItem(Resource res,bool succ)
        {
            if (succ)
            {
                MessageItemPrefab = res.UnityObj as GameObject;
                LoadMsgList(0,null);
            }
        }

        //加载消息列表
        private bool LoadMsgList(int eventId,object arg)
        {
            for (int i = 0; i < MsgList.childCount; i++)
            {
                GameObject.Destroy(MsgList.GetChild(i).gameObject);
            }
            
            if (MessageItemPrefab == null)
            {
                return false;
            }

            Dictionary<int, MsgUnit> msgList =MessageModel.Instance.MsgList;

            foreach (MsgUnit msg in msgList.Values)
            {
                if (msg.PlayerUid==LoginModel.Instance.Uid) continue;
                GameObject go = GameObject.Instantiate(MessageItemPrefab) as GameObject;
                go.transform.SetParent(MsgList);
                go.transform.localScale = new Vector3(1, 1, 1);

                MessageItem msgItem = go.AddComponent<MessageItem>();
                msgItem.Init(msg);
            }
            return false;
        }

        private bool LoadOrderList(int eventId, object arg)
        {
            for (int i = 0; i < MsgList.childCount; i++)
            {
                GameObject.Destroy(MsgList.GetChild(i).gameObject);
            }

            if (MessageItemPrefab == null)
            {
                return false;
            }

            List<MsgUnit> msgList = OrderController.Instance.orders;

            for(int i=0;i<msgList.Count;i++)
            {
                GameObject go = GameObject.Instantiate(MessageItemPrefab) as GameObject;
                go.transform.SetParent(MsgList);
                go.transform.localScale = new Vector3(1, 1, 1);

                MessageItem msgItem = go.AddComponent<MessageItem>();
                msgItem.Init(msgList[i]);
            }
            return false;
        }

        public override void OnClose()
        {
            base.OnClose();
            MessageController.Instance.GetDispatcher().RemoveListener(MessageEvent.OnGetMsgList, LoadMsgList);
        }
    }
}
