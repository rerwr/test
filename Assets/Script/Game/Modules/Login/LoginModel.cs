using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;
namespace Game
{
    public class LoginModel : BaseModel<LoginModel>
    {

        private int uid ;            // 角色uid
        private string username ;       // 返回名字为空时，则弹出创建名字选择性别的提示
        private string head ;           // 头像，如果http开头的则是网络图片，否则是本地默认头像
        private int lv ;             // 当前等级
        private int exp ;                // 当前经验值
        private int _levelMaxExp ;    //等级最大经验
        private int gold;          // 金币
        private string code;          // 验证码

        private int oil ;                // 初级精油总量
        private int dog_lv ;         // 宠物狗等级

        private int _continueDays ;      //连续登陆天数

        private int DogUpgradeMaxEXP ;//狗狗升级需要的最大经验值
        private int DogCurrentEXP ;//狗狗当前的经验
        private int chance;         //  防盗概率

        private string token;          //token

        private string _account;   //账号

        private string _password; //密码
        private int VisitingID;//正在
        private string openid ;//微信或者qq登录的回调ID
        private string headimgurl ;//头像URL
        private string province ;
        private string nickname ;
        private string city ;//
        private int isbinding=100;
        private string sex;//
        private string language;
        private bool isPayForAPP=false;
        private long ordernum;

        #region MyRegion
        public string Openid
        {
            get
            {
                return openid??"";
            }
            set { openid = value; }
        }

        public string WechatHeadimgurl
        {
            get { return headimgurl??""; }
            set { headimgurl = value; }
        }

        public string Province
        {
            get { return province??""; }
            set { province = value; }
        }

        public string Nickname
        {
            get
            {
                return nickname??"";


            }
            set { nickname = value; }
        }

        public string City
        {
            get { return city??""; }
            set { city = value; }
        }

        public string Sex
        {
            get { return sex ?? "0"; }
            set { sex = value; }
        }

        public string Language
        {
            get { return language ?? ""; }
            set { language = value; }
        }

        public int DogCurrentExp
        {
            get { return DogCurrentEXP; }
            set { DogCurrentEXP = value; }
        }

        public int DogUpgradeMaxExp
        {
            get { return DogUpgradeMaxEXP; }
            set { DogUpgradeMaxEXP = value; }
        }

        public int Chance
        {
            get { return chance; }
            set { chance = value; }
        }

        public int Gold
        {
            get { return gold; }
            set { gold = value; }
        }

        public int ContinueDays
        {
            get { return _continueDays; }
            set { _continueDays = value; }
        }

        public int DogLv
        {
            get { return dog_lv; }
            set { dog_lv = value; }
        }

        public int Oil1
        {
            get { return oil; }
            set { oil = value; }
        }

        public int LevelMaxExp
        {
            get { return _levelMaxExp; }
            set { _levelMaxExp = value; }
        }

        public int Exp
        {
            get { return exp; }
            set { exp = value; }
        }

        public int Lv
        {
            get { return lv; }
            set { lv = value; }
        }

        public string Head
        {
            get
            {
                if (head=="")
                {
                    return WechatHeadimgurl;

                }
                else
                {
                    return head;
                }
            }
            set { head = value; }
        }


        
        public int Uid
        {
            get { return uid; }
            set
            {
                uid = value;
                VisitingID = value;
            }
        }

        public string Token
        {
            get { return token; }
            set { token = value; }
        }

        public string Account
        {
            get
            {
                return _account??"";
            }
            set { _account = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public int VisitingId
        {
            get { return VisitingID; }
            set { VisitingID = value; }
        }

        public int Isbinding
        {
            get { return isbinding; }
            set { isbinding = value; }
        }

        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        public bool IsPayForApp
        {
            get { return isPayForAPP; }
            set { isPayForAPP = value; }
        }

        public long Ordernum
        {
            get { return ordernum; }
            set { ordernum = value; }
        }

        #endregion

        public override void InitModel()
        {

        }

        public void SetData(Farm_Game_UserInfo_Anw GenerateAnw)
        {

        }
    }


}
