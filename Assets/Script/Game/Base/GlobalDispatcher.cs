using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;

namespace Game
{ 
    public class GlobalDispatcher:EventDispatcher
    {
        private static GlobalDispatcher _Instance;
        public static GlobalDispatcher Instance
        {
            get { return _Instance; }
        }

        public static void Create()
        {
            _Instance = new GlobalDispatcher(typeof(GlobalEvent));
        }
        public GlobalDispatcher(Type events) : base(events)
        {
        }

        internal void Dispatch(int onStoreUnitsChange, Func<int, object, bool> initShopGrid)
        {
            throw new NotImplementedException();
        }
    }
}
