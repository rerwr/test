using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace Game
{
    class RankInfoModel : BaseModel<RankInfoModel>
    {
        public Dictionary<int, PlayerInfo> RankList = new Dictionary<int, PlayerInfo>();

        public override void InitModel()
        {

        }

        public void SetData(Farm_Game_RankInfo_Anw GenerateAnw)
        {
            if (GenerateAnw != null)
            {
                RankList = DataSettingManager.SetAnwData(GenerateAnw.UserInfosList);
            }
        }
    }
}
