using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;

namespace Game
{
    public class NewPlyerCreateModel:BaseModel<NewPlyerCreateModel>
    {
        private int _createResult;
        private int UserGameID;


        public int CreateResult
        {
            get { return _createResult; }
            set { _createResult = value; }
        }

        public int UserGameId
        {
            get { return UserGameID; }
            set { UserGameID = value; }
        }

        public override void InitModel()
        {
            
        }

        public  void SetData(Farm_Game_NewUser_Anw GenerateAnw)
        {
            CreateResult=GenerateAnw.CreateResult;
            UserGameID=GenerateAnw.UserGameID;
        }
    }
}
