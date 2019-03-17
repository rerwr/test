using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public class Result:BaseObject
    {
        private int _upGradeToOilNum;//升级到初级精油需要的数量

        public int UpGradeToOilNum
        {
            get { return _upGradeToOilNum; }
            set { _upGradeToOilNum = value; }
        }
    }
}
