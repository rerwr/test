using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Game;
using Google.ProtocolBuffers;
using UnityEngine;

namespace Framework
{
    //  【protobuf】
    //  使用版本 2.4.1
    //  使用 ToString方式序列化。
    
    //  【协议组成格式】
    //   协议头 + 经过protobuf序列化得到的协议数据内容
    
    //  【协议头格式】
    //  BYTE m_Header[2];       //长度2 * 协议头标识0xAF; 0xFE;
    //  BYTE m_Direction;       //长度1 * 1标识从App来，2标识同服务器来
    //  int m_Gamecode;         //长度4 * 游戏登录返回的游戏码
    
    //  BYTE m_Module;          //长度1 * 消息模块 module
    //  BYTE m_Sub;             //长度1 * 消息子类 sub
    
    //  BYTE m_Succ;            //长度1 * errorcode:0.不处理 1.成功 2.失败，其他.由消息定义
    
    //  int m_DataLen;          //长度4 * 消息体的长度
    
    //  body                  	//消息体 (ProtoBuf编码） 

    public abstract class BaseNetMessage
    {
        //public const int HEAD_SIZE = 14;//2+1+4+1+1+1+4
        public const int HEAD_SIZE = 8;//2+1+4+1+1+1+4

        public static int GameCode = 0;

        public byte moduleId;
        public byte subId;
        public byte succ;
        public int gamecode;
    }
    public class MsgToSend : BaseNetMessage
    {
        private static readonly byte[] BuffEmpty = new byte[0];

        public static MsgToSend Create(byte module, byte subid, IBuilder builder = null)
        {
            MsgToSend msg = new MsgToSend();
            msg.moduleId = module;
            msg.subId = subid;
            msg._protoBuilder = builder;
            
            return msg;
        }
        public IBuilder _protoBuilder;
        public byte[] raw;
        public void Serialize()
        {
            IMessage _proto = null;
            if (_protoBuilder != null)
            {
                _proto = _protoBuilder.WeakBuild();
                if (moduleId==0&&subId==1)
                {

                }
                else
                {
                    Debug.Log(" <color=#00ff00ff>" + "Send Msg:" + moduleId + "-" + subId + "proto:" + "</color>" + Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(_proto.ToString())));

                }
                _protoBuilder = null;
            }
            byte[] buf = (_proto == null) ? BuffEmpty : _proto.ToByteArray();
//            for (int i = 0; i < buf.Length; i++)
//            {
//                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", buf[i], "test1"));
//            }
            ByteArray ba = new ByteArray(HEAD_SIZE + buf.Length);
            //ba.WriteByte(0xAF);
            //ba.WriteByte(0xFE);
            ba.WriteByte(0x01);
            //ba.WriteInt(GameCode);//gamecode
            ba.WriteByte(moduleId);
            ba.WriteByte(subId);

            ba.WriteByte(1);

            ba.WriteInt(buf.Length);

            ba.WriteBytes(buf, buf.Length, false);

            raw = ba.Buff;
        }
    }

    public class MsgRec : BaseNetMessage
    {
        public int bodyLength;
        public byte[] content;
        public IMessage _proto;
        public static MsgRec Create(byte[] headbuff)
        {
            MsgRec msg = new MsgRec();
            ByteArray ba = new ByteArray(headbuff);
            //ba.ReadByte();
            //ba.ReadByte();
            ba.ReadByte();

            
            //int gamecode = ba.ReadInt();
            msg.moduleId = ba.ReadByte();
            msg.subId = ba.ReadByte();
            msg.succ = ba.ReadByte();
            msg.bodyLength = ba.ReadInt();
//            Debug.LogError(msg.bodyLength);
            Debug.Log("<color=#ffff00ff>socket receive head: " + msg.moduleId+"-" + msg.subId+"-----"+msg.succ+ "</color>");
    
            return msg;
        }
        public void Deserialize()
        {
//            Debug.LogError("------test------");
            ParserFun parser = SocketParser.Instance.GetParser(moduleId, subId);
            if (parser != null)
            {
                try
                {
                    _proto = parser(content);
                    if (moduleId == 0 && subId == 1)
                    {
                       

                    }
                    else
                    {
                        string s = "<color=#00ffffff><== receive Message,moduleId=" + moduleId + ",subId=" + subId + "</color>" + "#\n" + Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(_proto.ToString()));
                        Debug.Log(s);
                    }
                    
                }
                catch (Exception e)
                {
                    Debug.LogError("<== receive Message error,moduleId=" + moduleId + ",subId=" + subId + "#" + e.Message + e.StackTrace);
                }
            }
            else
            {
                Debug.LogError("parser null:" + subId);
            }
        }
    }

}
