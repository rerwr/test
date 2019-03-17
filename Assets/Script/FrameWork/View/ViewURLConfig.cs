using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using System.Xml;
using System.Xml.Serialization;
using Assets.Scripts.Logic.Utility;
using System;

namespace Framework
{
    public class ViewURLConfig : BaseConfig<ViewURLConfig>
    {
        public Dictionary<int, string> URLs = new Dictionary<int, string>();

        public override void InitConfig()
        {
            ResourceMgr.Instance.LoadResource("Config/URL",OnLoad);
        }

        private void OnLoad(Resource res,bool succ)
        {

          

            if (succ)
            {
                TextAsset asset = res.UnityObj as TextAsset;
                if (asset != null)
                {
                    string t = asset.text;

                    Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", t, "test1"));

                    ViewPathList vl1 = GenericXmlSerializer.ReadFromXmlString<ViewPathList>(t);

                    for (int i = 0; i < vl1.viewURLs.Count; i++)
                    {
                        URLs.Add(vl1.viewURLs[i].ID, vl1.viewURLs[i].URL);
                        //Debug.Log(vl1.viewURLs[i].ID+":" +vl1.viewURLs[i].URL);
                    }
                }
                GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnLoadURLConfig);
            }
        }

        public string GetURL(int id)
        {
            if (URLs.ContainsKey(id))
            {
                return URLs[id];
            }
            else
            {
                Debug.Log("物品id为："+id+" 的物品图片在配置表中不存在！");
                return null;
            }
        }

        [XmlRoot("ViewPathList")]
        public class ViewPathList
        {
            [XmlElement("ViewURL")]
            public List<ViewURL> viewURLs = new List<ViewURL>();
        }

        [Serializable, XmlRoot("ViewURL")]
        public class ViewURL
        {
            [XmlAttribute("name")]
            public string viewId;
            [XmlElement("URL")]
            public string url;

            public int ID
            {
                get { return int.Parse(viewId); }
            }

            public string URL
            {
                get { return url; }
            }
        }
    }
}
