using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using Aliyun.OSS.Samples;
using Game;
using Framework;

public class AsyncImageDownload : MonoBehaviour
{

    private Sprite defaultIcon;

    private static AsyncImageDownload _instance = null;
    public static AsyncImageDownload GetInstance() { return Instance; }
    public static AsyncImageDownload Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("AsyncImageDownload");
                _instance = obj.AddComponent<AsyncImageDownload>();
                DontDestroyOnLoad(obj);
                _instance.Init();
            }
            return _instance;
        }
    }

    public bool Init()
    {


        return true;

    }

    private void ClearImgTexture(Image img)
    {
        img.material.SetTexture(0, null);
        Resources.UnloadUnusedAssets();

        System.GC.Collect();
    }

    public void Delete(string url)
    {


        if (Directory.Exists(path) && File.Exists(path + url.GetHashCode()))
        {
            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "删除图片", url));

            File.Delete(path + url.GetHashCode());
        }

    }
    /// <summary>
    /// 加载图片        
    /// </summary>
    /// <param name="url">链接</param>
    /// <param name="img">需要放入的图片</param>
    /// <param name="isneedReDownload">如果已经有这个图片名在Image,是否需要重新加载</param>
    public void SetAsyncImage(string url, Image img, bool isneedReDownload = false)
    {

        if (string.IsNullOrEmpty(url))
        {
            if (defaultIcon != null)
            {
                img.sprite = defaultIcon;
            }
            else
            {
                ResourceMgr.Instance.LoadResource("Sprites/HeadIcon/headIcon_0", (res, succ) =>
                {
                    if (succ)
                    {
                        ClearImgTexture(img);
                        Texture2D t2d = res.UnityObj as Texture2D;
                        Sprite sp = Sprite.Create(t2d, new Rect(0, 0, t2d.width, t2d.height),
                            new Vector2(0.5f, 0.5f));
                        if (img != null)
                        {
                            img.sprite = sp;
                            defaultIcon = sp;
                        }

                    }
                });
            }
        }

        else
        {

            if (img.sprite)
            {
//                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", img.sprite.name, url.GetHashCode().ToString()));
                if (!isneedReDownload)
                {
                    if (img.sprite.name == url.GetHashCode().ToString())
                    {
                        Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "图片一样不需要加载", "test1"));
                        return;
                    }
                }

            }
            //需要重新加载,则删除本地的图片让他自动重新下载
            if (isneedReDownload)
            {
               Delete(url);
            }
                if (File.Exists(path + url.GetHashCode()))
                {
                    StartCoroutine(LoadLocalImage(url, img));

                }
                else
                if (url.Contains("http"))
                {
                    //如果之前不存在缓存文件  
                    StartCoroutine(DownloadImage(url, img, isneedReDownload));
                }
//                else if (url.Length > 4)
//                {
//                    //从云服务器取
//                    GetObjectSample.GetObject(url, img);
//                }
                else
                {
                    Debug.LogError(string.Format("<color=#ff0000ff><---{0}-{1}----></color>", "加载失败",url));
                }
            
        

        }
    }

    IEnumerator DownloadImage(string url, Image img,bool isNeed2RedownLown=false)
    {

        Debug.Log("downloading new image:" +url);//url转换HD5作为名字  
        using (WWW www = new WWW(url))
        {
            yield return www;
            if (www.error != null)
            {
                if (defaultIcon != null)
                {
                    img.sprite = defaultIcon;
                }
                else
                {
                    ResourceMgr.Instance.LoadResource("Sprites/HeadIcon/headIcon_0", (res, succ) =>
                    {
                        if (succ)
                        {
                            ClearImgTexture(img);
                            Texture2D t2d = res.UnityObj as Texture2D;
                            Sprite sp = Sprite.Create(t2d, new Rect(0, 0, t2d.width, t2d.height),
                                new Vector2(0.5f, 0.5f));
                            img.sprite = sp;
                            defaultIcon = sp;
                        }
                    });
                }
            }
            else
            {
                ClearImgTexture(img);
                Texture2D tex2d = www.texture;
                //将图片保存至缓存路径  
                byte[] pngData = tex2d.EncodeToPNG();
                //当需要重新加载时
                //如果文件已经改变，则删除
//                if (isNeed2RedownLown&&img.sprite!=null)
//                {
//                    string newMD5=EncryptManager.GetMD5Hash(pngData);
//
//                    Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "file:///" + path + url.GetHashCode(), "test1"));
//                    string url1 = path + url.GetHashCode();
//                    if (File.Exists(url1))
//                    {
//
//                        string md5=EncryptManager.GetMD5Hash(url1);
//
////                        Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", newMD5, md5));
//
//                        if (md5!=newMD5)
//                        {
//                            File.Delete(url1);
//                            File.WriteAllBytes(url1, pngData);
//                            Sprite m_sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height),
//                                new Vector2(0.5f, 0.5f));
//                            img.sprite = m_sprite;
//                            img.sprite.name = url.GetHashCode().ToString();
//                        }
//                        else
//                        {
//
//                            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "md5相同不删除", "test1"));
//
//                        }
//                    }
//                    else
//                    {
//
//                        File.Delete(url1);
//                        File.WriteAllBytes(url1, pngData);
//                        Sprite m_sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height),
//                            new Vector2(0.5f, 0.5f));
//                        img.sprite = m_sprite;
//                        img.sprite.name = url.GetHashCode().ToString();
//
//                    }
//                }
//                else
                {
                    File.WriteAllBytes(path + url.GetHashCode(), pngData);
                    Sprite m_sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height),
                        new Vector2(0.5f, 0.5f));
                    img.sprite = m_sprite;
                    img.sprite.name = url.GetHashCode().ToString();
                }

             
            }
        }
    }

    IEnumerator LoadLocalImage(string url, Image img)
    {
        string filePath = "file:///" + path + url.GetHashCode();
        //Debug.Log("getting local image:" + filePath);
    
        using (WWW www = new WWW(filePath))
        {
            yield return www;
            ClearImgTexture(img);
            Texture2D texture = www.texture;
        
            Sprite m_sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
            if (!img.IsDestroyed())
            {
                img.sprite = m_sprite;
                img.sprite.name = url.GetHashCode().ToString();

            }
        }

    }

    public string path
    {
        get
        {
            //pc,ios //android :jar:file//  

            //            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", Application.persistentDataPath + "/", "test1"));

            return Application.persistentDataPath + "/";

        }
    }
}