using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using Framework;

namespace Game { 
public class GameTextDataMgr : Singleton<GameTextDataMgr> {
        
    public Dictionary<int, _Data> TextDatas=new Dictionary<int, _Data>();
    private string url = @"http://119.23.48.181:8080/api/gameTextData";
        //119.23.48.181
        //    private string url = @"http://119.23.48.181:8080/api/gameTextData";
        //http请求,得到json数据
        private string GetPage(string requestUrl)
        {
        Stream instream = null;
        StreamReader sr = null;
        HttpWebResponse response = null;
        HttpWebRequest request = null;
        // 准备请求...
            try
            {
                // 设置参数
                request = WebRequest.Create(requestUrl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "GET"; //请求方式GET或POST
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add("Authorization", "Basic YWRtaW46YWRtaW4=");

                //发送请求并获取相应回应数据
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, Encoding.UTF8);
                //返回结果网页（html）代码
                string content = sr.ReadToEnd();
                string err = string.Empty;
                return content;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                SystemMsgView.SystemFunction(Function.Tip, Info.ConnectFailed);
                return "";
            }
        }

    public void GetDatas()
    {
        if (string.IsNullOrEmpty(url))
        {
            return;
        }
        string _json = GetPage(url);
        if (string.IsNullOrEmpty(_json))
        {
            return;
        }
        TextData textData = JsonUtility.FromJson<TextData>(_json);
        if (textData.result != null)
        {
            for (int i = 0; i < textData.result.Length; i++)
            {
                TextDatas.Add(textData.result[i].id, textData.result[i]);
            }
        }
    }
}

[Serializable]
public class TextData
{
    public _Data[] result;
}

[Serializable]
public class _Data
{
    public int id;
    public string key;
    public string title;
    public string content;
}
}
