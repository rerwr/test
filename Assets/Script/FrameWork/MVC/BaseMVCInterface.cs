using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;
using System;
using System.Reflection;
using Google.ProtocolBuffers;

namespace Framework
{
    public abstract class BaseConfig<T> where T : new()
    {
        private static readonly object _lock = new object();
        private static T _Instance;
        public static T Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (_lock)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new T();
                        }
                    }
                }
                return _Instance;
            }
        }
        /// <summary>
        /// 在这里读取配置的XML
        /// </summary>
        public abstract void InitConfig();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>

    public abstract class BaseModel<T> where T : new() 
    {
        private static readonly object _lock = new object();
        private static T _Instance;
        public static T Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (_lock)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new T();
                        }
                    }
                }
                return _Instance;
            }
        }
        public abstract void InitModel();
        
      
    }

    public class NetBaseSet
    {
        public NetBaseSet()
        {
            
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>

    public abstract class BaseController<T> where T : new()
    {
       
        private static readonly object _lock = new object();
        private static T _Instance;
        
        public static T Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (_lock)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new T();
                        }
                    }
                }
                return _Instance;
            }
        }
        private EventDispatcher _dispatcher;
        protected NetProxy _Proxy;

        protected BaseController()
        {
            _dispatcher = new EventDispatcher(GetEventType());
        }
        protected abstract Type GetEventType();

        public abstract void InitController();
   
        public EventDispatcher GetDispatcher()
        {
            return _dispatcher;
        }



    }


    public class NetProxy
    {
        private byte _module;
        public NetProxy(byte netModule)
        {
            _module = netModule;
        }
        
        public void AddNetListenner(byte sub, NetMsgListener responeListener)
        {
            NetMsgListenerMgr.Instance.RegisterMsgListener(_module,sub, responeListener);
        }

        public void SendMsg(byte module, byte sub, IBuilder builder=null)
        {
            SocketMgr.Instance.SendMsg(module,sub,builder);
        }
    }

    
}
