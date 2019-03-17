using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Framework;
using UnityEngine;

public class UpdateManager :Singleton<UpdateManager>
{
    
   
 
    public string ServerPath = "";




   public string GetBasePath(AssetSource sourceType)
    {
        string str = string.Empty;
        switch (sourceType)
        {
            case AssetSource.Online:
                if (Application.platform != RuntimePlatform.Android)
                {
                    if (Application.platform == RuntimePlatform.IPhonePlayer)
                    {
                        return (this.ServerPath + "/IOS_Lua/");
                    }
                    if (Application.platform == null)
                    {
                        return (this.ServerPath + "/IOS_Lua/");
                    }
                    if (Application.platform == RuntimePlatform.WindowsPlayer)
                    {
                        return (this.ServerPath + "/PC_Lua/");
                    }
                    return (this.ServerPath + "/Android_Lua/");
                }
                return (this.ServerPath + "/Android_Lua/");

            case AssetSource.StreamingAssets:
                if (Application.platform !=RuntimePlatform.Android )
                {
                    if (Application.platform == RuntimePlatform.IPhonePlayer)
                    {
                        return ("file:" + Application.dataPath + "/Raw/");
                    }
                    if (Application.platform ==RuntimePlatform.WindowsPlayer)
                    {
                        return ("file://" + Application.dataPath + "/StreamingAssets/");
                    }
                    return ("file://" + Application.dataPath + "/StreamingAssets/");
                }
                return ("jar:file://" + Application.dataPath + "!/assets/");

            case AssetSource.Cache:
                if (Application.platform != RuntimePlatform.Android)
                {
                    if (Application.platform == RuntimePlatform.IPhonePlayer)
                    {
                        return (Application.temporaryCachePath + "/");
                    }
                    if (Application.platform == null)
                    {
                        return (Application.dataPath + "/StreamingAssets/");
                    }
                    if (Application.platform == RuntimePlatform.WindowsPlayer)
                    {
                        return (Application.dataPath + "/StreamingAssets/");
                    }
                    return (Application.persistentDataPath + "/");
                }
                return (Application.persistentDataPath + "/");
        }
        return str;
    }



    
    public enum AssetSource
    {
        None,
        Online,
        StreamingAssets,
        Cache
    }

   
}

