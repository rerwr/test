using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public class Oil:BaseObject
    {
        private int _oilType;//精油的种类，1为初级精油，2为半成品精油，3为高级精油
        private int GainEXP;//合成精油收获的经验值1-100
        private int _onceLackResult;
        private int combinCount; //合成数量

        public int OilType
        {
            get { return _oilType; }
            set { _oilType = value; }
        }

        public int GainExp
        {
            get { return GainEXP; }
            set { GainEXP = value; }
        }

        public int OnceLackResult
        {
            get { return _onceLackResult; }
            set { _onceLackResult = value; }
        }

        public int CombinCount
        {
            get { return combinCount; }
            set { combinCount = value; }
        }
    }
}
