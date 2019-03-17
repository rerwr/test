using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Assets.Scripts.Logic.Utility;
using Game;
using UnityEngine;
//using UnityEngine.Tizen;
//using Debug = UnityEngine.Debug;

namespace Framework
{
    public class ViewConfig : BaseConfig<ViewConfig>
    {
        public Dictionary<string, ViewCo> viewconfigs = new Dictionary<string, ViewCo>();
        [XmlRoot("ViewList")]
        public class ViewList
        {
            [XmlElement("ViewCo")]
            public List<ViewCo> viewCos = new List<ViewCo>();

        }

        public override void InitConfig()
        {

//           ResourceMgr.Instance.LoadResource(,OnLoad);
           ResourceMgr.Instance.LoadResource("Config/ViewConfig", OnLoad);
         
        }

        private void OnLoad(Resource res, bool succ)
        {
            if (succ)
            {
                TextAsset asset = res.UnityObj as TextAsset;
                if (asset != null)
                {
                    string t = asset.text;

//                    UnityEngine.Debug.Log(t);
                    ViewList co1 = GenericXmlSerializer.ReadFromXmlString<ViewList>(t);

                    for (int i = 0; i < co1.viewCos.Count; i++)
                    {
                        viewconfigs.Add(co1.viewCos[i].viewName, co1.viewCos[i]);
//                        UnityEngine.Debug.LogError(string.Format("viewconfig--name：{0}+type：{1}+close:{2}", co1.viewCos[i].viewName, co1.viewCos[i].viewtype, co1.viewCos[i].closeType));
                    }
                }
                GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnDataConfigLoadDone);
            }
        }

        public ViewCo GetViewCo(string viewName)
        {

//            UnityEngine.Debug.Log(viewName);
            return viewconfigs[viewName];
        }

        public static string GetViewName(string path)
        {
            int index = path.LastIndexOf('/');
            return path.Substring(index + 1);
        }

        public static string GetViewPath(string viewName)
        {
            return "UI/Views/" + viewName;
        }

    }
    [Serializable, XmlRoot("ViewList")]
    public class ViewCo
    {
        [XmlAttribute("name")]
        public string viewName;
        [XmlElement("viewtype")]
        public string viewtype1;
        [XmlElement("closetype")]
        public string closeType1;

        public CloseType closeType
        {
            get
            {
                switch (closeType1)
                {
                    case "Hide":
                        return CloseType.Hide;
                    case "Destroy":
                        return CloseType.Destroy;
                    default:
                        return CloseType.Destroy;
                }

            }
        }

        public ViewType viewtype
        {
            get
            {
                switch (viewtype1)
                {
                    case "Dialog":
                        return ViewType.Dialog;
                    case "Full":
                        return ViewType.Full;
                    case "Window":
                        return ViewType.Window;
                    default:
                        return ViewType.Window;

                }
            }
        }
        public enum CloseType
        {
            Hide,
            Destroy
        }
    }
}
