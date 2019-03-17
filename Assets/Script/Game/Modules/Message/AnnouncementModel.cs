using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;

namespace Game
{
    class AnnouncementModel:BaseModel<AnnouncementModel>
    {
        private string anouncement;
        private string versionNum;
        private string LoginInfo ;//登录首面板简介
        private string expressInfo ;//快递说明
        private string CompanyInfo ;//公司简介
  
        private string strategy ;//策略
        private string info ;//任务面板公告横条
        private string other2 ;//策略
        private string other3 ;//策略

        public string LoginInfo1
        {
            get { return LoginInfo; }
            set { LoginInfo = value; }
        }

        public string ExpressInfo
        {
            get { return expressInfo; }
            set { expressInfo = value; }
        }

        public string CompanyInfo1
        {
            get
            {
                return CompanyInfo;
            }
            set { CompanyInfo = value; }
        }

        public string Strategy
        {
            get { return strategy; }
            set { strategy = value; }
        }

        public string Info
        {
            get { return info; }
            set { info = value; }
        }

        public string Other2
        {
            get { return other2; }
            set { other2 = value; }
        }

        public string Other3
        {
            get { return other3; }
            set { other3 = value; }
        }

        public string Anouncement
        {
            get { return anouncement; }
            set { anouncement = value; }
        }

        public string VersionNum
        {
            get { return versionNum; }
            set { versionNum = value; }
        }

     

        public override void InitModel()
        {
            

        }

        public  void SetData(Farm_Game_AnnouncementInfo_Anw GenerateAnw)
        {
            Anouncement = GenerateAnw.Announcement;
            VersionNum = GenerateAnw.VersionNum;
        }
    }
}
