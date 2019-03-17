using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;

namespace Game
{
    //********************************************************************************************************************//
    //  名称: 商店购买/卖出回应
    //  描述: 服务器接收App购买操作，服务器根据角色ID和商品Id中获得商品的价格，并且计算游戏币是否能够，返回购买/卖出后的游戏币
    //  标识: module = 3,sub = 6
    //  方向: Server To  App
    public class BuyOrSellAtionModel:BaseModel<BuyOrSellAtionModel>
    {
        private int _actionResult;
    
     
        public PlayerInfo info=new PlayerInfo();
      

        public int ActionResult
        {
            get { return _actionResult; }
            set { _actionResult = value; }
        }

  

        public override void InitModel()
        {
            
        }

        public  void SetData(Farm_Game_buyOrSell_Anw GenerateAnw)
        {
       
        }
    }
}
