using System.Collections.Generic;
using Framework;

namespace Game
{
    public class AwardUnit
    {
        private int _days;
        private int id;
        private int num;

        public int days
        {
            get { return _days; }
            set { _days = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int Num
        {
            get { return num; }
            set { num = value; }
        }
    }
    public class SignModel:BaseModel<SignModel>
    {
      public   List<AwardUnit> awardUnits=new List<AwardUnit>();
        public override void InitModel()
        {
            
        }

        public void SetData(IList<SignRewardUnit> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                AwardUnit awardUnit=new AwardUnit();
                awardUnit.days = list[i].Coid;
                awardUnit.Id = list[i].Award.Id;
                awardUnit.Num = list[i].Award.Count;
                awardUnits.Add(awardUnit);
            }
        }

    }
}