using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using System.IO;
using System;
using System.Text;

namespace Game
{
    public class ChatLogManager : Singleton<ChatLogManager>
    {
        int perPage = 10;
        string folderName = Application.persistentDataPath + "//" + "Chat_"+LoginModel.Instance.Uid;

        private void WriteFile(int id,string c)
        {
            int targetID=id;

            string folderNameWithID = folderName + "/" + targetID;
            if (!Directory.Exists(folderNameWithID))
            {
                Directory.CreateDirectory(folderNameWithID);
            }
            
            //储存文本  
            string fileName = "ChatLog_"+targetID;
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

        private List<string> ReadFile(int id,int page)
        {
            List<string> LogList = new List<string>();
            string folderNameWithID = folderName + "/" + id;
            string fileName = folderNameWithID + "/" +"ChatLog_" + id;
            //Debug.LogError(fileName);
            if (!File.Exists(fileName))
            {
                return LogList;
            }
            FileStream fs = new FileStream(fileName, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            int line = 0;
            int startLine=0;
            int needReadLine = perPage;
            try
            {
                while(true)
                {
                    int contentLength = br.ReadInt32();
                    string cLog = Encoding.UTF8.GetString(br.ReadBytes(contentLength));
                    line++;
                }
            }
            catch (Exception)
            {

            }

            br.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开始

            if (needReadLine + line - page * perPage < 0)
            {
                br.Close();
                fs.Close();
                fs.Dispose();
                return LogList;
            }
            if (line - page * perPage < 0)
            {
                needReadLine += (line - page * perPage);
            }
            else
            {
                startLine = line - page * perPage;
            }
            //Debug.LogError("page:" + page);
            //Debug.LogError("line:" + line);
            //Debug.LogError("needReadLine:" + needReadLine);
            //Debug.LogError("startLine:" + startLine);

            try
            {
                for (int i = 1; i <=startLine; i++)
                {
                    br.ReadBytes(br.ReadInt32());
                }
                for(int j = 0; j <needReadLine; j++)
                {
                    //data.timeStamp = br.ReadInt64();
                    //data.senderID = br.ReadInt32();
                    //data.receiverID = br.ReadInt32();
                    int contentLength = br.ReadInt32();
                    string cLog= Encoding.UTF8.GetString(br.ReadBytes(contentLength));
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

        public void SaveData(int id,ChatLog c)
        {
            string log = JsonUtility.ToJson(c);
            //Debug.LogError("savedata:"+log);
            WriteFile(id,log);
        }

        public void GetData(int id,int page)
        {
            //Debug.LogError("getdata");
            List<string> LogList = ReadFile(id,page);
            List<ChatLog> chatLog = new List<ChatLog>();
            for (int i = 0; i < LogList.Count; i++)
            {
                //Debug.LogError(LogList[i]);
                ChatLog c = new ChatLog();
                c = JsonUtility.FromJson<ChatLog>(LogList[i]);
                chatLog.Add(c);
            }
            ChatModel.Instance.chatLog = chatLog;
            if(chatLog.Count!=0)ChatModel.Instance.currentPage++;
            ChantController.Instance.GetDispatcher().Dispatch(ChantControllerEvent.OnReciveLog);
        }
    }
}
