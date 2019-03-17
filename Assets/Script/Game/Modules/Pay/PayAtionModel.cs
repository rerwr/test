using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;

namespace Game
{
    //********************************************************************************************************************//
    //  名称: 充值回应
    //  描述: 服务器接收App购买操作，从角色的游戏币数扣，返回购买后的游戏币
    //  标识: module = 3,sub = 8
    //  方向: Server To App
    public class PayAtionModel:BaseModel<PayAtionModel>
    {
        private int _actionResult;
        public PlayerInfo Infos=new PlayerInfo();
        private PaySuccessView pcv;
        private PayView pv;
        public int ActionResult
        {
            get { return _actionResult; }
            set { _actionResult = value; }
        }

        public PayView Pv
        {
            get { return pv; }
            set { pv = value; }
        }

        public PaySuccessView Pcv
        {
            get { return pcv; }
            set { pcv = value; }
        }

        public override void InitModel()
        {
            
        }
        
        public  void SetData(Farm_Game_pay_Anw GenerateAnw)
        {
           
//          StorageDeltaList delta = DataSettingManager.SetAnwData(GenerateAnw.DeltaArray);
//          Farm_Game_StoreInfoModel.TakeOrPutAction(GenerateAnw.TakeAction, delta);
        }

    }
}
