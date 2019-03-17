

using System;
using System.Collections;
using Game;
using UnityEngine;

using Object = UnityEngine.Object;

namespace Framework
{
    public class ResourceMgr : Singleton<ResourceMgr>
    {
    
        public void LoadResource(string src, Action<Resource, bool> OnLoad)
        {
            string path = GetFullPath(src);
            MTRunner.Instance.StartRunner(DelayRun(path,OnLoad));
        }

        private IEnumerator DelayRun(string path, Action<Resource, bool> onLoad)
        {
            yield return 0.03f;//随机延迟时间0~0.333秒
            try
            {

//                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "isloading", path));

                UnityEngine.Object o = Resources.Load(path);
                if (o != null)
                {
                    Resource res = new Resource();
                    res.path = path;
                    res.UnityObj = o;
                    onLoad(res, true);
//                    Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "success", path));

                }
                else
                {
                    onLoad(null,false);
                    Debug.LogError("null Resource"+path);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("load " + path + " fail\n" + e.Message + e.StackTrace);
            }
        }

      
        private string GetFullPath(string src)
        {
            return src;
        }

       
    }
    
    public class Resource
    {
        public string path;
        public Object UnityObj { get; set; }
    }

}
