using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using System.IO;
using System.Text;
using System;

namespace Game
{
    public class OrderController : Singleton<OrderController>
    {
        public List<MsgUnit> orders=new List<MsgUnit>();

        string folderName = Application.persistentDataPath + "//" + "Orders_" + LoginModel.Instance.Uid;

        private void WriteFile(string c)
        {
            string folderNameWithID = folderName;
            if (!Directory.Exists(folderNameWithID))
            {
                Directory.CreateDirectory(folderNameWithID);
            }

            //储存文本  
            string fileName = "Order";
            FileStream fs = new FileStream(folderNameWithID + "/" + fileName, FileMode.Append);
            BinaryWriter bw = new BinaryWriter(fs);

            //bw.Write(data.timeStamp);
            //bw.Write(data.senderID);
            //bw.Write(data.receiverID);
            //bw.Write(Encoding.UTF8.GetByteCount(data.content));
            //bw.Write(Encoding.UTF8.GetBytes(data.content));
            bw.Write(Encoding.UTF8.GetByteCount(c));
            bw.Write(Encoding.UTF8.GetBytes(c));
            
            bw.Close();
            fs.Close();
            fs.Dispose();
        }

        private List<string> ReadFile()
        {
            List<string> LogList = new List<string>();
            string folderNameWithID = folderName;
            string fileName = folderNameWithID + "/Order";
            //Debug.LogError(fileName);
            if (!File.Exists(fileName))
            {
                return LogList;
            }
            FileStream fs = new FileStream(fileName, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            try
            {
                while (true)
                { 
                    //data.timeStamp = br.ReadInt64();
                    //data.senderID = br.ReadInt32();
                    //data.receiverID = br.ReadInt32();
                    int contentLength = br.ReadInt32();
                    string cLog = Encoding.UTF8.GetString(br.ReadBytes(contentLength));
                    //Debug.LogError(cLog);
                    LogList.Add(cLog);
                    //dataList.Add(data);
                }
            }
            catch (Exception)
            {
                Debug.Log("ReadFile:" + fileName + "----done!");
            }
            br.Close();
            fs.Close();
            fs.Dispose();
            return LogList;
        }

        public void SaveData(MsgUnit m)
        {
            string log = JsonUtility.ToJson(m);
            //Debug.LogError("savedata:"+log);
            WriteFile(log);
        }

        public void ClearOrder()
        {
            orders.Clear();
        }
        public void GetData()
        {
            //Debug.LogError("getdata");
            List<string> LogList = ReadFile();
            List<MsgUnit> _orders = new List<MsgUnit>();
            for (int i = 0; i < LogList.Count; i++)
            {
                //Debug.LogError(LogList[i]);
                MsgUnit c = new MsgUnit();
                c = JsonUtility.FromJson<MsgUnit>(LogList[i]);
                _orders.Add(c);
            }
            orders = _orders;
            if (orders.Count == 0)
            {
                SystemMsgView.SystemFunction(Function.Tip, "当前没有订单");
                return;
            }
            CommitController.Instance.GetDispatcher().Dispatch(CommitController.CommitControllerEvent.OnOrderCallback);
        }

       void  SetServerData(Farm_Game_PaySucc_Anw p)
        {
            
        }
        public void SetData(Farm_Game_PaySucc_Anw p)
        {
            MsgUnit msg = new MsgUnit();
            msg.type = 4;
            msg.id = Convert.ToInt32(p.Ordernum);
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            DateTime dt = startTime.AddSeconds(Convert.ToInt32(p.Time));
            msg.sendTime = dt.ToString("HH:mm");

            string c = "";
            if (p.Type == 1)
            {
                c += "登录支付成功\n";


            }
            else if (p.Type == 2)
            {
                c += "兑换成功\n";


            }

            c += ("订单号:" + p.Ordernum + "\n");
            c += ("支付的邮费:" + p.Money + "\n");
            c += ("时间:" + msg.sendTime + "\n");
            c += ("物品信息:");
            for (int i = 0; i < p.ObjsCount; i++)
            {
                BaseAtrribute ba = LoadObjctDateConfig.Instance.GetAtrribute(p.ObjsList[i].Id);
                c += (ba.Name + "x" + p.ObjsList[i].Count + "\n");
            }
            msg.content = c;
            orders.Add(msg);
            //SaveData(msg);
        }

        public void SetData(Farm_Game_LogisticsOrder p)
        {
            MsgUnit msg = new MsgUnit();
            msg.type = 4;
            msg.id = Convert.ToInt32(p.Ordernum);
//            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
//            DateTime dt = startTime.AddSeconds((double)Convert.ToInt32(p.Sendtime));
//            msg.sendTime = dt.ToString("HH:mm");
            msg.sendTime = p.Sendtime;
            string c = "";
//            if (p.Type== 1)
//            {
//                c += "登录支付成功\n";
//
//              
//            }
//            else if (p.Type == 2)
            {
                c +=( "快递单号："+(p.LogisticsOrder==""?"请等待发货": p.LogisticsOrder) +"\n");

            }
            c += ("快递公司:" + p.Company + "\n");

            c += ("订单号:" + p.Ordernum+"\n");
            c += ("支付的邮费:" + p.Money + "\n");
            c += ("时间:" + msg.sendTime + "\n");
            c += ("物品信息:\n");
            for (int i=0;i< p.ObjsCount;i++)
            {
                BaseAtrribute ba=LoadObjctDateConfig.Instance.GetAtrribute(p.ObjsList[i].Id);
                c += ("["+ba.Name + "x"+p.ObjsList[i].Count+"]"+"\n");
            }
            msg.content = c;
            orders.Add(msg);
            //SaveData(msg);
        }

        public void DeleteData(MsgUnit msg)
        {
            if (File.Exists(folderName+"/Order"))
            {
                File.Delete(folderName + "/Order");
            }
            for (int i=0;i<orders.Count;i++)
            {
                if (orders[i].id == msg.id)
                {
                    orders.Remove(orders[i]);
                    continue;
                }
                SaveData(orders[i]);
            }
        }

        public void SetDatas(Farm_Game_CheckOrder_Anw msgs)
        {
            for (int i = 0; i < msgs.ListInfoCount; i++)
            {
                SetData(msgs.ListInfoList[i]);
            }
        }
    }
}
