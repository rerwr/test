using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using System;

namespace Game
{
    public class SearchInfoModel : BaseModel<SearchInfoModel>
    {
        public Dictionary<int, PlayerInfo> SearchList = new Dictionary<int, PlayerInfo>();

        public override void InitModel()
        {

        }

        public void SetData(Farm_Game_SearchFriend_Anw anw)
        {
            SearchList = DataSettingManager.SetAnwData(anw.SearchListList);
        }
    }
}