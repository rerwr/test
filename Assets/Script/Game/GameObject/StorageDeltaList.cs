using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public class StorageDeltaList
    {
        private Dictionary<int,DogFood> dogFoods = new Dictionary<int,DogFood>();
        private Dictionary<int,Fertilizer> fertilizers = new Dictionary<int,Fertilizer>();
        private Dictionary<int,Oil> oils = new Dictionary<int,Oil>();
        private Dictionary<int,Seed> seeds = new Dictionary<int,Seed>();
        private Dictionary<int,Result> results = new Dictionary<int,Result>();
        private Dictionary<int, Elixir> elixirs = new Dictionary<int, Elixir>();
        private Dictionary<int, Formula> formulas = new Dictionary<int, Formula>();

        public Dictionary<int, DogFood> DogFoods
        {
            get { return dogFoods; }
            set { dogFoods = value; }
        }

        public Dictionary<int, Fertilizer> Fertilizers
        {
            get { return fertilizers; }
            set { fertilizers = value; }
        }

        public Dictionary<int, Oil> Oils
        {
            get { return oils; }
            set { oils = value; }
        }

        public Dictionary<int, Seed> Seeds
        {
            get { return seeds; }
            set
            {
                seeds = value;
            }
        }

        public Dictionary<int, Result> Results
        {
            get { return results; }
            set { results = value; }
        }

        public Dictionary<int, Elixir> Elixirs
        {
            get { return elixirs; }
            set { elixirs = value; }
        }

        public Dictionary<int, Formula> Formulas
        {
            get { return formulas; }
            set { formulas = value; }
        }
    }
}
