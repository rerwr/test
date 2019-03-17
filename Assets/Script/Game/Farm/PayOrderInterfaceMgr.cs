using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using Framework;

namespace Game
{
    public enum Urltype
    {
        wx,
        alipay,

    }
    //支付的目的
    public enum PayFor
    {
        Null,
        Login,
        Exchange
    }
    public class PayOrderInterfaceMgr : Singleton<PayOrderInterfaceMgr>
    {
        private PayData textData;
        public PayFor payfor = PayFor.Null;
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

        public object GetDatas(int uid, Urltype type, PayFor payfor, int count = 0)
        {
            string url = "";
            switch (type)
            {
                case Urltype.wx:
                    if (payfor == PayFor.Login)
                    {
                        url = @"http://"+LoginConfig.Instance.serverIP+":8080/api/unifiedorder?uid=" + uid + "&type=init";
                        if (string.IsNullOrEmpty(url))
                        {
                            SystemMsgView.SystemFunction(Function.Tip, Info.ConnectFailed2);
                            return null;
                        }
                        string _json = GetPage(url);
                        if (string.IsNullOrEmpty(_json))
                        {
                            SystemMsgView.SystemFunction(Function.Tip, Info.ConnectFailed2);

                            return null;
                        }
                        textData = JsonUtility.FromJson<PayData>(_json);

                        Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", textData.ToString(), "test1"));

                        return textData;

                    }
                    else if (payfor == PayFor.Exchange)
                    {
                        url = @"http://"+LoginConfig.Instance.serverIP+":8080/api/unifiedorder?uid=" + uid + "&type=exchange" + "&count=" +
                              count;



                        if (string.IsNullOrEmpty(url))
                        {
                            SystemMsgView.SystemFunction(Function.Tip, Info.ConnectFailed2);
                            return null;
                        }
                        string _json = GetPage(url);
                        if (string.IsNullOrEmpty(_json))
                        {
                            SystemMsgView.SystemFunction(Function.Tip, Info.ConnectFailed2);

                            return null;
                        }
                        textData = JsonUtility.FromJson<PayData>(_json);
                        Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", textData.ToString(), "test1"));

                        return textData;

                    }

                    break;
                case Urltype.alipay:
                    if (payfor == PayFor.Login)
                    {

                        url = @"http://"+LoginConfig.Instance.serverIP+":8080/api/aliPay?uid=" + uid + "&type=init";
                        if (string.IsNullOrEmpty(url))
                        {
                            SystemMsgView.SystemFunction(Function.Tip, Info.ConnectFailed2);
                            return null;
                        }
                        string payorder = GetPage(url);
                        Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", payorder.ToString(), "test1"));

                        return payorder;
                    }
                    else if (payfor == PayFor.Exchange)
                    {
                        url = @"http://"+LoginConfig.Instance.serverIP+":8080/api/aliPay?uid=" + uid + "&type=exchange" + "&count=" +
                              count;
                        if (string.IsNullOrEmpty(url))
                        {
                            SystemMsgView.SystemFunction(Function.Tip, Info.ConnectFailed2);
                            return null;
                        }
                        string _json = GetPage(url);
                        if (string.IsNullOrEmpty(_json))
                        {
                            SystemMsgView.SystemFunction(Function.Tip, Info.ConnectFailed2);

                            return null;
                        }
                        Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", _json.ToString(), "test1"));

                        return _json;

                    }
                    return textData;

                    break;
            }
            return textData;


        }
    }

    [Serializable]
    public class PayData
    {

        public string package;
        public string appid;
        public string sign;
        public string partnerid;
        public string prepayid;

      

        public string noncestr;
        public string timestamp;

        public override string ToString()
        {
            return string.Format("Package: {0}, Appid: {1}, Sign: {2}, Partnerid: {3}, Prepayid: {4}, Noncestr: {5}, Timestamp: {6}", package, appid, sign, partnerid, prepayid, noncestr, timestamp);
        }
    }


}
