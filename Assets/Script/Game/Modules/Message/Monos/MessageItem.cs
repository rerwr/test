using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using Framework;
using Game;
using System;

public class MessageItem : MonoBehaviour
{
    MsgUnit msgUnit;

    //初始化msgitem
    public void Init(MsgUnit msgUnit)
    {
        this.msgUnit = msgUnit;
        transform.Find("Content/Message").GetComponent<Text>().text= msgUnit.content;
        transform.Find("Content/Time").GetComponent<Text>().text = msgUnit.sendTime;
        transform.Find("DeleteBtn").GetComponent<Button>().onClick.AddListener(OnClickDeleteBtn);

        ContentAnimController anim=transform.Find("Content/ContentBtn").gameObject.AddComponent<ContentAnimController>();
        anim.Init();

        SetMode();
    }

    //根据不同类型的消息，item呈现不同的结构
    private void SetMode() 
    {
        Text Name = transform.Find("Content/Name").GetComponent<Text>();
        Image Head = transform.Find("Content/Head").GetComponent<Image>();
        Button ContentBtn= transform.Find("Content/ContentBtn").GetComponent<Button>();
        ContentBtn.interactable = false;
        Button AgreeBtn= transform.Find("Content/AgreeBtn").GetComponent<Button>();
        AgreeBtn.gameObject.SetActive(false);

        switch (msgUnit.type)
        {
            case 1:
                Name.text = "系统通知";
                Head.color = Color.white;
                ContentBtn.onClick.AddListener(OnClickToSystemMsg);
                break;
            case 2:
                Name.text = msgUnit.PlayerName;
                AsyncImageDownload.Instance.SetAsyncImage(msgUnit.PlayerHead, Head);
                Head.color = Color.white;
                ContentBtn.onClick.AddListener(OnClickToChat);
                break;
            case 3:
                Name.text = msgUnit.PlayerName;
                AsyncImageDownload.Instance.SetAsyncImage(msgUnit.PlayerHead, Head);
                Head.color = Color.white;
                AgreeBtn.gameObject.SetActive(true);
                AgreeBtn.onClick.AddListener(OnClickAgreeBtn);
                break;
            case 4:
                Name.text = "订单";
                Head.color = Color.white;
                ContentBtn.onClick.AddListener(OnClickToSystemMsg);
                break;
        }
    }

    //点击打开系统通知面板
    private void OnClickToSystemMsg()
    {
        MessageController.Instance.GetDispatcher().Dispatch(MessageEvent.OnClickSystemMsg,msgUnit.content);
        if (msgUnit.type != 4)
        {
            MessageController.Instance.DelMsg(msgUnit.id);
            GameObject.Destroy(this.gameObject);
        }
    }
    //点击进入聊天界面
    private void OnClickToChat()
    {
        ChatLog c = new ChatLog();
        c.SendPlayer = 2;
        c.Content = msgUnit.content;
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
        DateTime dt = startTime.AddSeconds(msgUnit.SendTime);
        //System.Debug.Log(dt.ToString("yyyy/MM/dd HH:mm:ss:ffff"));
        c.sendTime = dt.ToString("yyyy/MM/dd HH:mm");
        ChatLogManager.Instance.SaveData(msgUnit.PlayerUid,c);

        FriendFarmManager.Instance.GoFriendFarm(msgUnit.PlayerUid);
        MessageController.Instance.DelMsg(msgUnit.id);
        //GameObject.Destroy(this.gameObject);
        ViewMgr.Instance.Close(ViewNames.MsgListView);
        GameObject.Destroy(this.gameObject);
    }
    //点击同意好友请求按钮
    private void OnClickAgreeBtn()
    {
        FriendsInfoController.Instance.AgreeAddFriend(LoginModel.Instance.Uid,msgUnit.PlayerUid);

        MessageController.Instance.DelMsg(msgUnit.id);
        GameObject.Destroy(this.gameObject);
    }
    //点击删除按钮
    private void OnClickDeleteBtn()
    {
        if (msgUnit.type == 2)
        {
            ChatLog c = new ChatLog();
            c.SendPlayer = 2;
            c.Content = msgUnit.content;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            DateTime dt = startTime.AddSeconds(msgUnit.SendTime);
            //System.Debug.Log(dt.ToString("yyyy/MM/dd HH:mm:ss:ffff"));
            c.sendTime = dt.ToString("yyyy/MM/dd HH:mm");
            ChatLogManager.Instance.SaveData(msgUnit.PlayerUid, c);
        }
        if (msgUnit.type != 4)
        {
            MessageController.Instance.DelMsg(msgUnit.id);
            GameObject.Destroy(this.gameObject);
        }
        if (msgUnit.type == 4)
        {
            OrderController.Instance.DeleteData(msgUnit);
            GameObject.Destroy(this.gameObject);
        }
    }
}
