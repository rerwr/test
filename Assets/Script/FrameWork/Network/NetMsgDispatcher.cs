using System.Collections.Generic;
using System;
using Game;
using Google.ProtocolBuffers;


namespace Framework
{
    public delegate void NetMsgListener(MsgRec msg);
    public class NetMsgListenerMgr : Singleton<NetMsgListenerMgr>
	{
		private List<List<NetMsgListener>> _listeners = new List<List<NetMsgListener>>(256);
        public void RegisterMsgListener(byte module, byte sub, NetMsgListener listener)
        {
            var list = GetFunList(module, sub);
            if (list[sub] != null && list[sub] != listener)
            {
                UnityEngine.Debug.LogError(string.Format("NetMessageListener Duplicat error: old:{0},new:{1}", list[sub].Method.Name, listener.Method.Name));
            }
            list[sub] = listener;
        }

        public void UnRegisterMsgListener(byte module, byte sub, NetMsgListener listener)
        {
            var list = GetFunList(module, sub);
            list[sub] = null;
        }

        public NetMsgListener GetListener(byte module, byte subid)
        {
            int ie = module;
            int ic = subid;
            if (_listeners.Count <= ie)
            {
                return null;
            }
            var list = _listeners[ie];
            if (list.Count <= ic)
            {
                return null;
            }
            return list[subid];
        }

        private List<NetMsgListener> GetFunList(byte module, byte sub)
        {
            int ie = module;
            int ic = sub;
            while (_listeners.Count <= ie)
            {
                _listeners.Add(new List<NetMsgListener>());
            }
            var list = _listeners[ie];
            while (list.Count <= ic)
            {
                list.Add(null);
            }
            return list;
        }
    }
}
