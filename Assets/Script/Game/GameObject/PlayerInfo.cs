
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game
{
    public class PlayerInfo
    {
        private int UserGameID ;    //请求的客户ID,如果存在此用户的游戏角色则为玩家的角色号，如果没有则返回0
        private string _gameName ;    //请求的客户游戏名称
        private int _sex ;  //人物主角的性别，1表示男，2表示女			
        private string _headerIcon ;   //头像ICON的编号	
        private int _gameMoney ;    //游戏币
     //   private int _gameDiamond ;  //晶钻
     //   private int _gameTicket ;   //点券
        private int _userLevel ;    //玩家等级
        private string _userNobility ;   //玩家称号
        private int _userExp ; //玩家经验
        private int _levelMaxExp ; //等级最大经验
     //   private string _farmMotto ; //农场个性签名
        
        private int _aciton ;	//表示当前用户是否存在可执行操作,此为用户朋友的操作，在朋友列表中表示，1为收获果实，2为浇水，3为杀虫,4为除草，5为施肥

        private int dogLevel;
        private string url;
        private int rank;
        private string phoneNum;
        private int dogUpgradMaxExp;
        private int DogCurrentEXP;
        
        #region 属性

        public int UserGameId
        {
            get { return UserGameID; }
            set { UserGameID = value; }
        }

        public string GameName
        {
            get { return _gameName; }
            set { _gameName = value; }
        }

        public int Sex
        {
            get { return _sex; }
            set { _sex = value; }
        }

        public string HeaderIcon
        {
            get { return _headerIcon; }
            set { _headerIcon = value; }
        }

        public int GameMoney
        {
            get { return _gameMoney; }
            set { _gameMoney = value; }
        }

 

        public int UserLevel
        {
            get { return _userLevel; }
            set { _userLevel = value; }
        }

        public string UserNobility
        {
            get { return _userNobility; }
            set { _userNobility = value; }
        }

        public int UserExp
        {
            get { return _userExp; }
            set { _userExp = value; }
        }

        public int LevelMaxExp
        {
            get { return _levelMaxExp; }
            set { _levelMaxExp = value; }
        }



        public int DogLevel
        {
            get { return dogLevel; }
            set { dogLevel = value; }
        }

        public string Url
        {
            get { return url; }
            set{url = value;}
        }

        public int Rank
        {
            get { return rank; }
            set { rank = value; }
        }

        public string PhoneNum
        {
            get { return phoneNum; }
            set { phoneNum = value; }
        }

        public int DogUpgradMaxExp
        {
            get { return dogUpgradMaxExp; }
            set { dogUpgradMaxExp = value; }
        }

        public int DogCurrentExp
        {
            get { return DogCurrentEXP; }
            set { DogCurrentEXP = value; }
        }

        public int Aciton
        {
            get { return _aciton; }
            set { _aciton = value; }
        }

        #endregion
    }
}
