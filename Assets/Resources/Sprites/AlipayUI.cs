using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PayInfo
{
    public string subject;  // 显示在按钮上的内容,跟支付无关系  
    public float money;     // 商品价钱  
    public string title;    // 商品描述  
}

public class AlipayUI : MonoBehaviour
{
    public List<Button> buttons = null;
    public List<PayInfo> payInfos = null;
    private AndroidJavaObject currentActivity = null;

    void Start()
    {
        // Init UI  
        for (int i = 0; i < buttons.Count; i++)
        {
            var payInfo = payInfos[i];
            buttons[i].GetComponentInChildren<Text>().text = payInfos[i].subject;
#if UNITY_ANDROID && !UNITY_EDITOR
            buttons[i].onClick.AddListener(() =>   
            {  
                Alipay(payInfo);  
            });  
#endif
        }
#if UNITY_ANDROID && !UNITY_EDITOR
// 固定写法  
        AndroidJavaClass javaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");  
        currentActivity = javaClass.GetStatic<AndroidJavaObject>("currentActivity");  
#endif
    }

    public void Alipay(PayInfo payInfo)
    {
        // AlipayClient是Android里的方法名字，写死.  
        // payInfo.money是要付的钱，只能精确分.  
        // payInfo.title是商品描述信息，注意不能有空格.  
        currentActivity.Call("AlipayClient", payInfo.money, payInfo.title, "");
    }
}