using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using System;
using System.Security.Policy;
using System.IO;
using System.Net;
using System.Text;

namespace Game {
    public class VersionUpdateManager : Singleton<VersionUpdateManager> {

        public static int version=18;

        private AndroidJavaClass UnityPlayer;
        private AndroidJavaClass Intent;
        private AndroidJavaClass _Uri;

        private AndroidJavaObject currentActivity;

        public string ApkPath;
        public string ApkName;
        public string UpdateUrl;

        public void initProxy()
        {
#if UNITY_ANDROID
            UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            Intent = new AndroidJavaClass("android.content.Intent");
            _Uri = new AndroidJavaClass("android.net.Uri");
#endif

            ApkName = "ShengFei";
        }

        //path为.apk文件的完整路径
        public void installAPP(string path)
        {
            initProxy();
#if UNITY_ANDROID
            currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", Intent.GetStatic<AndroidJavaObject>("ACTION_VIEW"));
            intent.Call<AndroidJavaObject>("setDataAndType", _Uri.CallStatic<AndroidJavaObject>("fromFile", new AndroidJavaObject("java.io.File", new AndroidJavaObject("java.lang.String", path))), new AndroidJavaObject("java.lang.String", "application/vnd.android.package-archive"));// "application/vnd.android.package-archive"
            currentActivity.Call("startActivity", intent);
#endif
        }

        public void StartUpdate()
        {
            VersionUpdateManager.Instance.ApkName = "ShengFei";
            string _url = VersionUpdateManager.Instance.UpdateUrl;
            string _name = VersionUpdateManager.Instance.ApkName;
            if (_url.Contains("apk"))
            {
                MTRunner.Instance.StartRunner(DownloadAndSave(_url, _name, (over, load) =>
                {
                    GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnLoadingApk, load);
                    if (over)
                    {
                        Debug.Log(ApkPath);
                        NativeHandle handle = new NativeHandle();
                        handle.InstallApk(ApkPath);
                    }
                }));
            }
            else
            {
                Application.OpenURL(VersionUpdateManager.Instance.UpdateUrl);

            }

        }

        // 下载并保存资源到本地   
        public static IEnumerator DownloadAndSave(string url, string name, Action<bool, int> Finish = null)
        {
            VersionUpdateManager.Instance.ApkPath = Application.persistentDataPath + "//" + name+".apk";
            //url = Uri.EscapeDataString(url);
            int Loading = 0;
            bool b = false;
            WWW www = new WWW(url);
            if (www.error != null)
            {
                Debug.LogError("error:" + www.error);
            }
            while (!www.isDone)
            {
                Loading = (((int)(www.progress * 100)) % 100);
                Debug.LogError("！isDone:" + www.error);

                if (Finish != null)
                {
                    Finish(b, Loading);
                }

                yield return 1.0f;
            }
            if (www.error != null)
            {
                Debug.LogError("error:" + www.error);
                yield break;
            }
            if (www.isDone)
            {
                Loading = 100;
                byte[] bytes = www.bytes;

                Debug.LogError("isDone:" + www.error);

                b = SaveAssets(Application.persistentDataPath, name, bytes);
                if (Finish != null)
                {
                    Finish(b, Loading);
                }

            }
        }
        
        //保存资源到本地  
        public static bool SaveAssets(string path, string name, byte[] bytes)
        {
            if (File.Exists(path + "//" + name + ".apk"))
            {
                File.Delete(path + "//" + name + ".apk");
            }
            Stream sw;
            FileInfo t = new FileInfo(path + "//" + name+".apk");
            //FileInfo t = new FileInfo(@"C:\Users\pc\Desktop\test123.apk");
            if (!t.Exists)
            {
                try
                {
                    sw = t.Create();
                    sw.Write(bytes, 0, bytes.Length);
                    sw.Close();
                    sw.Dispose();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        public string GetPage(string requestUrl)
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

        public string GetUrl(string _json)
        {
            JR jr =JsonUtility.FromJson<JR>(_json);
            if (jr==null||jr.result==null||jr.result.Length == 0) return null;
            int maxIndex = 0;
            for (int i = 1; i < jr.result.Length; i++)
            {
                if (jr.result[i].version > jr.result[maxIndex].version)
                {
                    maxIndex = i;
                }
            }
#if UNITY_ANDROID
            UpdateUrl = jr.result[maxIndex].url;
            return jr.result[maxIndex].url;
#endif
#if UNITY_IOS
            UpdateUrl = jr.result[maxIndex].urlIos;
            return jr.result[maxIndex].urlIos;
#endif
            return "";
        }

    }

    [Serializable]
    public class JR
    {
        public JsonRespon[] result;
    }

    [Serializable]
    public class JsonRespon
    {
        public int id;
        public int version;
        public string url;
        public string urlIos;
    }

}
