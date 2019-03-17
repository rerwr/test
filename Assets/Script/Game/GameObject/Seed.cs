using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game;

namespace Game
{
    public class Seed : BaseObject
    {
        private float _grothTime;


        public float GrothTime
        {
            get { return _grothTime; }
            set { _grothTime = value; }
        }
    }
}
