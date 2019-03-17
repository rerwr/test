using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Assets.Scripts.Logic.Utility;
using Framework;
using UnityEngine;

namespace Game
{
    [XmlRoot("LoginConfig")]
    public class LoginConfig : BaseConfig<LoginConfig>
    {
        public string serverName;
        [XmlAttribute("serverIP")]
        public string serverIP;
        [XmlAttribute("serverPort")]
        public int serverPort;
        public override void InitConfig()
        {
            ResourceMgr.Instance.LoadResource("Config/Main", ((resource, b) =>
            {
                TextAsset s =resource.UnityObj as TextAsset;

//                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", s.text, "test1"));

                LoginConfig loginConfig = GenericXmlSerializer.ReadFromXmlString<LoginConfig>(s.text);
                serverIP = loginConfig.serverIP;
                serverPort = loginConfig.serverPort;
                //读取Login配置
             
                GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnLoginConfigLoadDone);
//                Debug.Log(serverIP + ":" + serverPort);
            }));
           
        }

        
    }

}
