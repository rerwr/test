using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using System;

namespace Game
{
    public class MessageModel : BaseModel<MessageModel>
    {
        public Dictionary<int,MsgUnit> MsgList;

        public override void InitModel()
        {
            MsgList = new Dictionary<int, MsgUnit>();
        }

        public void SetData(Farm_Game_MessageSend_Anw p)
        {
            MsgList = DataSettingManager.SetAnwData(p.MsgListList);
        }
    }

    //消息类
    [Serializable]
    public class MsgUnit
    {
        public int id;
        public int type;           //消息的类型：1.系统通知;2.普通用户消息; 3.添加好友请求;4.订单消息
        public string content;     //消息内容
        public string sendTime;    //消息的时间
        public int SendTime;       //消息的时间(int)

        public  int PlayerUid;	//如果是玩家信息，则返回玩家的信息
        public string PlayerHead;
        public string PlayerName;
    }
}