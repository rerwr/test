using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using System;

namespace Game
{
    public class ChatModel : BaseModel<ChatModel>
    {
        public PlayerInfo ChatTarget;
        public Sprite HeadIcon;
        public List<ChatLog> chatLog;
        public int currentPage = 1;

        public override void InitModel()
        {
            ChatTarget = new PlayerInfo();
            chatLog = new List<ChatLog>();
        }

        public void SetData(Farm_Game_ChatLog_Anw p)
        {
            chatLog = DataSettingManager.SetAnwData(p.ChatListList);
        }
    }

    //聊天记录
    [Serializable]
    public class ChatLog
    {
        public int SendPlayer;  //该信息的发送者：1.自己；2.好友；3.时间；
        public string Content;  //该条信息的内容；
        public string sendTime; //发送的时间；
    }
}
