using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;

namespace Game
{
    public enum NegativeAction
    {
        drying,//干燥
        bug,//长虫
        weed//长草
    }
    //********************************************************************************************************************//
    //  名称: 长虫干燥长草缺肥操作生成
    //  描述: 服务器根据规则主动推送当前状态，根据附带角色ID和农田编号，执行相关操作，并返回本地块的当前属性
    //  标识: module = 3,sub = 5
    //  方向: Server To App
    public class ConditionActionModel:BaseModel<ConditionActionModel>
    {
        private int _FieldsID;
        private int ActionID;
        public NegativeAction negativeAction;
        public int FieldsID
        {
            get { return _FieldsID; }
            set { _FieldsID = value; }
        }

        public int ActionId
        {
            get { return ActionID; }
            set
            {
                switch (value)
                {
                    case 1:
                        negativeAction = NegativeAction.drying;
                        break;
                    case 2:
                        negativeAction = NegativeAction.bug;
                        break;
                    case 3:
                        negativeAction = NegativeAction.weed;
                        break;


                }

            }
        }

        public override void InitModel()
        {
           
        }

        public  void SetData(Farm_Game_Action_Anw GenerateAnw)
        {
            FieldsID = GenerateAnw.FieldsID;
            ActionID =GenerateAnw.ActionID;
        }
    }
}
