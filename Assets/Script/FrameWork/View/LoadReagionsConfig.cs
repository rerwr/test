


using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Framework;
using Game;
using UnityEngine;

/// <summary>
/// 加载地名配置文件
/// </summary>
public class LoadReagionsConfig : BaseConfig<LoadReagionsConfig>
{
    public TextAsset TextAsset;
    private string url;
    /// <summary>
    /// 最终得到某省某市某县
    /// </summary>
   public Dictionary<string,Dictionary<string,List<string>>> regionsInfo=new Dictionary<string, Dictionary<string, List<string>>>();
    
    public override void InitConfig()
    {
       
        ResourceMgr.Instance.LoadResource("Config/regionInfo",((resource, b) =>
        {
            TextAsset text =resource.UnityObj as TextAsset;


            InitData(text.text);

        } ));
 

    }

    private void InitData(string content)
    {
      
        string[] ContentLines = content.Split(new string[] { "\r\n" }, StringSplitOptions.None); 
        bool isCity = false;
        string cityName = "";//地级市
        string strReadline;
   
        for (int j = 0; j < ContentLines.Length; j++)
        {
            strReadline = ContentLines[j];
            
            if (strReadline != "")
            {
                string[] str = strReadline.Split(':');

//                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", str[0], str[1]));

                //地级市
                if (isCity)
                {
                    //没有该省名，添加一个省名
                    if (!regionsInfo.ContainsKey(str[1]))
                    {
                        Dictionary<string, List<string>> strList = new Dictionary<string, List<string>>();
                        regionsInfo.Add(str[1], strList);
                    }
                    Dictionary<string, List<string>> citys = regionsInfo[str[1]];
                    //如果该省没有地级市名
                    if (!citys.ContainsKey(str[0]))
                    {
                        List<string> list = new List<string>();
                        citys.Add(str[0], list);

                        regionsInfo[str[1]] = citys;

                    }

                    cityName = str[0];
                    isCity = false;
                }
                else
                {
                    
                    
                    //添加县级市
                    Dictionary<string, List<string>> str1s = regionsInfo[str[1]];

                        List<string> list = str1s[cityName]; //该地级市的县级城市列表
                        if (!list.Contains(str[0]))
                        {
                            list.Add(str[0]);
                        }

                  
                }
            }
            else
            {
                //读到空行则表示下一行为地级城市
                isCity = true;
                cityName = "";
            }
           
        }
        GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnRegionsConfigLoadDone);
       
    }
}
