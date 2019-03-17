using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;

namespace Game
{

    //********************************************************************************************************************//
    //  名称: 一键操作回应
    //  描述: 服务器接收App采摘操作，根据附带角色ID和农田编号，执行相关操作，并返回本地块的当前属性
    //  标识: module = 3,sub = 4
    //  方向: Server To App
    public class PluckModel :BaseModel<PluckModel>
    {
        private int _actionResult;
        private int takeAction;
       
        public int ActionResult
        {
            get { return _actionResult; }
            set { _actionResult = value; }
        }

        public int TakeAction
        {
            get { return takeAction; }
            set { takeAction = value; }
        }

        public override void InitModel()
        {
            
        }

        public  void SetData(Farm_Game_Action_Pluck_Anw GenerateAnw)
        {
//          ActionResult = GenerateAnw.ActionResult;
//          Farm_Game_StoreInfoModel.Instance.SetData(GenerateAnw.DeltaArrayList);
//          Farm_Game_StoreInfoModel.TakeOrPutAction(GenerateAnw.TakeAction,delta);

        }
    }
}
