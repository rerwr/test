using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;

namespace Game
{
    public class SeedActionModel : BaseModel<SeedActionModel>
    {
        public static int currentId_Seed;       //当前选择的种子id
        public static int currentId_Field;      //当前想要播种土地id

        private int _actionResult;               //操作是否成功，1为成功，2无法种植在该块田
        private int EXP;                            //播种后增加经验值3
        public Plant plant = new Plant();                 //返回播种后返回该田的植株类
  
        

        public int ActionResult
        {
            get { return _actionResult; }
            set { _actionResult = value; }
        }

        public int Exp
        {
            get { return EXP; }
            set { EXP = value; }
        }

   

        public override void InitModel()
        {

        }

        public  void SetData(Farm_Game_Seed_Anw GenerateAnw)
        {
           
            FieldsModel.Instance.SetData(GenerateAnw.Info);

          
        }
    }
}
