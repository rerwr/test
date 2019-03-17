using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Framework;
using Game;
using Google.ProtocolBuffers;
using UnityEngine;
using System.IO;
using System.Text;


namespace Framework
{
    public class SocketMgr : SingletonMonoBehaviour<SocketMgr>
    {
        private object lockobj = new object();
        private TcpClient client;
        private SocketReceiver _receiver;
        private SocketSender _sender;
        private Socket clientSocket;
        public bool _isneed2loginview = true;
        private status _status = status.Disconnect;

        private bool isfirstopen = true;
        private bool isconnecting = false;
        public status Status
        {
            get { return _status; }
            private set
            {
                var old = _status;
                _status = value;
                if (old != value)
                {
                    switch (value)
                    {
                        case status.Disconnect:
                            Debug.Log("Disconnect");
                            MTRunner.Instance.RunOnMainThread(() => GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnDisconnect));

                            if (!isconnecting)
                            {

                                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "开始重连", "test1"));

                                MTRunner.Instance.StartRunner(Wait());

                                isconnecting = true;
                                isfirstopen = false;
                            }
                            break;
                        case status.Connecting:

                            Debug.Log("Connecting");

                            break;
                        case status.Connected:
                            Debug.Log("Connected");
                            MTRunner.Instance.RunOnMainThread(() => GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnConnect));

                            break;
                    }

                }
            }
        }

        public enum status
        {
            Disconnect,
            Connecting,
            Connected,
        }

        void Start()
        {
            GlobalDispatcher.Instance.AddListener(GlobalEvent.Onfocus, ReConnect);
            GlobalDispatcher.Instance.AddListener(GlobalEvent.OnPause, ReConnect);
            //3s检测一次是否失联
            GlobalDispatcher.Instance.AddListener(GlobalEvent.OnThreeSecond, ReConnect);

        }

        bool ReConnect(int id, object arg)
        {
            //            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "3s", "test1"));

            if (Status == status.Disconnect)
            {
                //确保正在连接不会启动其他线程
                if (!isconnecting)
                {

                    Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "开始重连", "test1"));

                    MTRunner.Instance.StartRunner(Wait());

                    isconnecting = true;
                    isfirstopen = false;
                }
            }

            return false;
        }

        public void ConnectIPv4(string ip, int port)
        {
            Disconnect();
            client = new TcpClient(AddressFamily.InterNetwork);
            IAsyncResult a = client.BeginConnect(IPAddress.Parse(ip), port, ConnectCallback, client);

        }
        /// <summary>
        /// 注意这个回调不在主线�?
        /// </summary>
        /// <param name="ar"></param>
        private void ConnectCallback(IAsyncResult ar)
        {
            lock (ar)
            {
                TcpClient c = (TcpClient)ar.AsyncState;

                if (c != null && c == client)
                {
                    try
                    {
                        c.EndConnect(ar);
                        if (c == client)
                        {
                            lock (lockobj)
                            {
                                if (c == client)
                                {
                                    if (c.Connected)
                                    {
                                        _receiver = new SocketReceiver(this, client);
                                        _sender = new SocketSender(this, client);
                                        Status = status.Connected;
                                    }
                                    else
                                    {
                                        Disconnect();
                                    }
                                }
                                else
                                {
                                    c.Close();
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.ToString());
                        Disconnect();
                    }

                }
            }

        }

        public void Disconnect()
        {
            if (client != null)
            {
                lock (lockobj)
                {
                    if (client != null)
                    {
                        client.Close();
                        client = null;
                        if (_receiver != null)
                            _receiver.Close();
                        _receiver = null;
                        if (_sender != null)
                            _sender.Close();
                        _sender = null;
                    }
                    Status = status.Disconnect;



                }
            }
        }


        private IEnumerator Wait()
        {

            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "waitfordisconnect", "test1"));

            //            if ( HeartBeatController.Instance.errorInfo == ErrorInfo.isLogined)
            //            {
            //                yield return "error";
            //
            //            }

            float time = 0;
            while (true)
            {
                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "检测是否断线", "test1"));

                yield return 0.5f;
                time++;
                //返回登录界面的时候就不显示重新登录界面
                if (_isneed2loginview)
                {
                    SystemMsgView.SystemFunction(Function.Tip, Info.DisconectedInfo);

                }
                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "connecting", "test1"));

                if (SocketMgr.Instance.Status == status.Disconnect)
                {
                    LoginController.Instance.Connect2Sever();


                    Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "send", "test1"));
                }

                //连接完成后就发送连接
                if (SocketMgr.Instance.Status == status.Connected)
                {
                    if (_isneed2loginview)
                    {
                        LoginController.Instance.ReqLogin(LoginModel.Instance.Uid, LoginModel.Instance.Token, (int)Application.platform);
                        SystemMsgView.SystemFunction(Function.Tip, Info.ConnectSuccess, 3);

                    }
                    isconnecting = false;

                    yield break;
                }

            }
        }


        public void SendMsg(byte module, byte subid, IBuilder builder = null)
        {
            if (_sender != null)
            {
                MsgToSend msg = MsgToSend.Create(module, subid, builder);

                _sender.Enqueue(msg);
            }
        }
        private Queue<MsgRec> msgCache = new Queue<MsgRec>();
        void Update()
        {
            if (_receiver != null)
            {
                _receiver.Update();
                _receiver.PeakQueue(msgCache);
                while (msgCache.Count > 0)
                {
                    var msg = msgCache.Dequeue();

                    //                    Debug.Log(msg.moduleId+":"+msg.subId);
                    NetMsgListener d = NetMsgListenerMgr.Instance.GetListener(msg.moduleId, msg.subId);
                    if (d != null)
                    {
                        //                        try
                        {
                            d(msg);
                        }
                        //                        catch (Exception e)
                        //                        {
                        //                            Debug.LogError(e.Message+e.StackTrace);
                        //                        }
                    }
                }
            }
            if (_sender != null)
            {
                _sender.Update();
            }
        }

        private void ReportError(TcpClient receivingclient, string log)
        {

            if (client == receivingclient)
            {
                Disconnect();
                Debug.LogError(log);
            }
        }

        public class SocketReceiver
        {
            private object lockobj = new object();
            private TcpClient client;
            private Stream stream;
            private SocketMgr socketMgr;
            private volatile bool running;
            private Queue<MsgRec> recQueue;
            private volatile FileStream fs;
            private Thread t;
            public SocketReceiver(SocketMgr socketMgr, TcpClient client)
            {
                this.socketMgr = socketMgr;
                this.client = client;
                this.stream = client.GetStream();
                recQueue = new Queue<MsgRec>();
                running = true;
                if (t==null)
                {
                    t = new Thread(Run);
                    t.Start();
                }
            }

            private void Run()
            {
                byte[] head = new byte[MsgRec.HEAD_SIZE];
                while (running)
                {
                    MsgRec rec = null;
                    int received = 0;
                    try
                    {

                        received = stream.Read(head, 0, head.Length);
                        //                        Debug.Log(received);
                    }
                    catch (Exception e)
                    {
                        socketMgr.ReportError(client, "receive head error: " + e.ToString());
                        running = false;
                        break;
                    }
                    if (received != head.Length)
                    {
                        socketMgr.ReportError(client, "receive head size error(maybe stream end):" + received + "-" + head.Length);
                        running = false;
                        break;
                    }
                    rec = MsgRec.Create(head);
                    if (rec.bodyLength > 0)
                    {
                        byte[] buff = new byte[rec.bodyLength];

                        try
                        {
                            Debug.Log("socket begin receive body length: " + buff.Length);
                            received = stream.Read(buff, 0, buff.Length);

                            //                            Debug.Log("socket receive body length: " + buff.Length);
                        }
                        catch (Exception e)
                        {
                            socketMgr.ReportError(client, "receive content error: " + e.Message + e.StackTrace);
                            running = false;
                            break;
                        }
                        if (received != buff.Length)
                        {
                            socketMgr.ReportError(client, "content receive size error(maybe stream end):" + received + "/" + buff.Length);
                            running = false;
                            break;
                        }

                        rec.content = buff;
                        rec.Deserialize();
                        lock (lockobj)
                        {
                            recQueue.Enqueue(rec);
                        }
                    }
                    else
                    {
                        lock (lockobj)
                        {
                            recQueue.Enqueue(rec);
                        }
                    }
                }
            }

            public void PeakQueue(Queue<MsgRec> msgCache)
            {
                if (recQueue.Count > 0)
                {
                    lock (lockobj)
                    {
                        while (recQueue.Count > 0)
                        {
                            msgCache.Enqueue(recQueue.Dequeue());
                        }
                    }
                }
            }

            public void Update()
            {

            }

            public void Close()
            {
                running = false;
            }
        }

        public class SocketSender
        {
            private object lockobj = new object();
            private TcpClient client;
            private Stream stream;
            private SocketMgr socketMgr;
            private volatile bool running;
            Queue<MsgToSend> sendQueue;
            private volatile FileStream fs;
            private Thread t;
            public SocketSender(SocketMgr socketMgr, TcpClient client)
            {

                this.socketMgr = socketMgr;
                this.client = client;
                stream = client.GetStream();
                sendQueue = new Queue<MsgToSend>();
                running = true;
                if (t == null)
                {
                    t = new Thread(Run);
                    t.Start();
                }
            
            }

            private void Run()
            {
                Queue<MsgToSend> sending = new Queue<MsgToSend>();
                while (running)
                {
                    while (sendQueue.Count == 0)
                    {
                        Thread.Sleep(1);
                    }
                    while (sendQueue.Count > 0)
                    {
                        lock (lockobj)
                        {
                            sending.Enqueue(sendQueue.Dequeue());
                        }
                    }
                    while (sending.Count > 0)
                    {
                        MsgToSend msg = sending.Dequeue();
                        msg.Serialize();
                        byte[] buff = msg.raw;
                        try
                        {
                            stream.Write(buff, 0, buff.Length);

                            Debug.Log("socket write length: " + buff.Length);
                        }
                        catch (Exception e)
                        {
                            socketMgr.ReportError(client, "send error: " + e.Message + e.StackTrace);
                            running = false;
                            break;
                        }
                    }
                }
            }

            public void Update()
            {

            }

            public void Enqueue(MsgToSend msg)
            {
                lock (lockobj)
                {
                    sendQueue.Enqueue(msg);
                }
            }

            public void Close()
            {
                if (fs != null)
                {
                    fs.Close();
                    fs = null;
                }
                ;
                running = false;
            }
        }
    }
}
