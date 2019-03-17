using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;
using UnityEngine;

namespace Game
{
    //********************************************************************************************************************//
    //  名称: 点击进入某个好友信息请求回应
    //  描述: 服务器接收点击进入某个好友信息请求操作,根据好友ID找到地图全部数据，以及基本信息,18块田
    //  标识: module = 3,sub = 9
    //  方向: Server To App
    public class SingeFriendInfoModel:FieldsModel
    {
        private  static SingeFriendInfoModel _Instance1;
        public PlayerInfo info=new PlayerInfo();//被点击的玩家信息
        public new  static SingeFriendInfoModel Instance
        {
            get
            {
               return _Instance1??new SingeFriendInfoModel();
            }
           
        }

        public override void InitModel()
        {
            
        }

        public  void SetData(Farm_Game_SingleFriendInfo_Anw GenerateAnw)
        {
            FieldsModel.Instance.SetData(GenerateAnw.MapArrayList);
            ChatModel.Instance.ChatTarget = DataSettingManager.SetAnwData(GenerateAnw.OneFriendInfo);
            ChatModel.Instance.currentPage = 1;
            //ChantController.Instance.ReqChatLog(1);
            ChatLogManager.Instance.GetData(ChatModel.Instance.ChatTarget.UserGameId, 1);
        }
   
    }
}
