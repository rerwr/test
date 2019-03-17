using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;
using UnityEngine;

namespace Game
{
    //********************************************************************************************************************//
    //  名称: 好友信息回应
    //  描述: 服务器接收好友信息操作
    //  标识: module = 2,sub = 6
    //  方向: Server To App
    class FriendsInfoModel :BaseModel<FriendsInfoModel>
    {
        private int _actionResult;
        public Dictionary<int,PlayerInfo> playerInfos=new Dictionary<int, PlayerInfo>();
        //        public List<FriendItem> frienditems = new List<FriendItem>();
        /// <summary>
        /// 刚加载的每页对应的好友消息
        /// </summary>
        public List<Dictionary<int, PlayerInfo>> pages = new List<Dictionary<int, PlayerInfo>>();
        /// <summary>
        /// 每页对应的朋友页脚本
        /// </summary>
        public Dictionary<int,List<FriendItem> > hasLoadPage=new Dictionary<int, List<FriendItem>>();
        public int currentLoadpage = 0;
        public int ActionResult
        {
            get { return _actionResult; }
            set { _actionResult = value; }
        }

        public void DestroyAllFriends()
        {
            foreach (KeyValuePair<int, List<FriendItem>> pair in hasLoadPage)
            {
                foreach (FriendItem friendItem in pair.Value)
                {
                    if (friendItem)
                    {
                         GameObject.Destroy(friendItem.gameObject);

                    }
                }
            }
        }
        public override void InitModel()
        {
            
        }

        public  void SetData(Farm_Game_FriendsInfo_Anw GenerateAnw)
        {
             Dictionary<int, PlayerInfo> temp = new Dictionary<int, PlayerInfo>();

            //ActionResult = GenerateAnw.ActionResult;
            temp = DataSettingManager.SetAnwData(GenerateAnw.InfoList);
            foreach (var data in temp)
            {
                if (!playerInfos.ContainsKey(data.Key))
                {
                    playerInfos.Add(data.Key,data.Value);

                }
                else
                {
                    playerInfos[data.Key]= data.Value;

                }
            }
        }
    }
}
