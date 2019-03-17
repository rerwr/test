using System;
using UnityEngine;
using System.Collections;
using System.Text;

using UnityEngine.UI;
using cn.sharesdk.unity3d;
using Framework;
using Game;

//导入ShareSdk




public class ShareDemo : SingletonMonoBehaviour<ShareDemo>
{


    private ShareSDK shareSdk;
    public Text message;


    void Start()
    {

        shareSdk = GetComponent<ShareSDK>();

        //分享回调事件
        shareSdk.shareHandler += ShareResultHandler;
        shareSdk.shareHandler += OnShareResultHandler;

        //授权回调事件
        shareSdk.authHandler += AuthResultHandler;
        shareSdk.authHandler += OnShareResultHandler;
        //用户信息事件
        shareSdk.showUserHandler += GetUserInfoResultHandler;

    }


    //分享
    public void OnShareMomentsClick()
    {
        ShareContent content = new ShareContent();

        //这个地方要参考不同平台需要的参数    可以看ShareSDK提供的   分享内容参数表.docx
        content.SetText("我在圣菲花园开启种植之旅啦,一起来加入吧。");                            //分享文字
        content.SetImageUrl("http://119.23.48.181:8080/resource/themes/default/images/flower_icon.png");   //分享图片
        content.SetTitle("圣菲花园");                                            //分享标题
        content.SetUrl("http://gameserv.sanfeime.com:8080/invite.html?invite_code=" + LoginModel.Instance.Code);                                    //分享网址
        content.SetShareType(ContentType.Webpage);


        //shareSdk.ShowPlatformList(null, content, 100, 100);                      //弹出分享菜单选择列表
        shareSdk.ShowShareContentEditor(PlatformType.WeChatMoments, content);                 //指定平台直接分享
    }

    public void OnShareWechant()
    {
        ShareContent content = new ShareContent();

        //这个地方要参考不同平台需要的参数    可以看ShareSDK提供的   分享内容参数表.docx
        content.SetText("我在圣菲花园开启种植之旅啦,一起来加入吧。");                            //分享文字
        content.SetImageUrl("http://119.23.48.181:8080/resource/themes/default/images/flower_icon.png");   //分享图片
        content.SetTitle("圣菲花园");                                            //分享标题
        content.SetUrl("http://gameserv.sanfeime.com:8080/invite.html?invite_code="+LoginModel.Instance.Code);                                    //分享网址

        Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "url", "http://gameserv.sanfeime.com:8080/invite.html?invite_code=" + LoginModel.Instance.Code));

        //        content.SetMusicUrl("http://up.mcyt.net/md5/53/OTg1NjA5OQ_Qq4329912.mp3");//分享类型为音乐时用
        content.SetShareType(ContentType.Webpage);
        //shareSdk.ShowPlatformList(null, content, 100, 100);                      //弹出分享菜单选择列表
        shareSdk.ShowShareContentEditor(PlatformType.WeChat, content);                 //指定平台直接分享
    }

    public void OnShareQQ()
    {
        ShareContent content = new ShareContent();

        //这个地方要参考不同平台需要的参数    可以看ShareSDK提供的   分享内容参数表.docx
        content.SetText("我在圣菲花园开启种植之旅啦,一起来加入吧。");                            //分享文字
        content.SetImageUrl("http://119.23.48.181:8080/resource/themes/default/images/flower_icon.png");   //分享图片
        content.SetTitle("圣菲花园");                                            //分享标题
        content.SetTitleUrl("http://gameserv.sanfeime.com:8080/invite.html?invite_code="+LoginModel.Instance.Code);
        Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "url", "http://gameserv.sanfeime.com:8080/invite.html?invite_code=" + LoginModel.Instance.Code));

        shareSdk.ShowShareContentEditor(PlatformType.QQ, content);                 //指定平台直接分享
    }

    void OnShareResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {

        Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "回调成功", "test1"));

        if (state == ResponseState.Success)
        {

            print("share successfully - share result :");
            print(MiniJSON.jsonEncode(result));
        }
        else if (state == ResponseState.Fail)
        {
#if UNITY_ANDROID
            print("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
#endif
        }
        else if (state == ResponseState.Cancel)
        {
            print("cancel !");
        }
    }


    // 分享结果回调
    void ShareResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {   //成功
        if (state == ResponseState.Success)
        {
            //            message.text =("share result :");
            //            message.text = (MiniJSON.jsonEncode(result)); 
            SystemMsgView.SystemFunction(Function.Tip, "分享成功", 1);
       
        }
        //失败
        else if (state == ResponseState.Fail)
        {

            SystemMsgView.SystemFunction(Function.Tip, ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]), 1.5f);
           
        }
        //关闭
        else if (state == ResponseState.Cancel)
        {
            SystemMsgView.SystemFunction(Function.Tip, "取消分享", 1.5f);
     
        }
    }


    //授权
    public void OnAuthQQClick()
    {
        //请求QQ授权//请求这个授权是为了获取用户信息来第三方登录
        shareSdk.Authorize(PlatformType.QQ);
    }

    public void OnAuthWeChantClick()
    {
        LoadingImageManager.Instance.AddLoadingItem();

        //请求QQ授权//请求这个授权是为了获取用户信息来第三方登录
        shareSdk.Authorize(PlatformType.WeChat);
    }

    //授权结果回调
    void AuthResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {
//        LoadingImageManager.Instance.StopLoading();


        if (state == ResponseState.Success)
        {
            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "回调成功", "test1"));
//            LoadingImageManager.Instance.AddLoadingItem();

            shareSdk.GetUserInfo(type);
            
        }
        else if (state == ResponseState.Fail)
        {

//            SystemMsgView.SystemFunction(Function.Tip, "登录失败" + result["error_code"] + "; error msg = " + result["error_msg"], 1.5f);
       
            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "登录失败" + result["error_code"] + "; error msg = " + result["error_msg"], "test1"));
        }
        else if (state == ResponseState.Cancel)
        {

//            SystemMsgView.SystemFunction(Function.Tip, "取消登录", 1.5f);
            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "取消登录", "test1"));

        }
    }
    //private void LoadIcon()
    //{
    //
    //   if (LoginModel.Instance.WechatHeadimgurl != "")
    //    {
    //
    //        AsyncImageDownload.Instance.SetAsyncImage(LoginModel.Instance.WechatHeadimgurl);
    //    }
    //}

    //获取用户信息
    void GetUserInfoResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {

        Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "获取信息成功", ""));
       
        if (state == ResponseState.Success)
        {
            //获取成功的话 可以写一个类放不同平台的结构体，用PlatformType来判断，用户的Json转化成结构体，来做第三方登录。
            switch (type)
            {
                case PlatformType.QQ:

                    Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "用户信息回调成功", (MiniJSON.jsonEncode(result))));

                   
                    LoginView lv1 = ViewMgr.Instance.views[ViewNames.LoginView].Viewlist[0] as LoginView;
                    lv1.egv.SendLoginSDKReq("3");
                    break;
                case PlatformType.WeChat:
                    Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "", (MiniJSON.jsonEncode(result))));

                    IDictionaryEnumerator ide = result.GetEnumerator();
                    
                    while (ide.MoveNext())
                    {
                        switch (ide.Key.ToString())
                        {
                            case "province":
                                LoginModel.Instance.Province = ide.Value.ToString();
                                break;
                            case "headimgurl":
                                LoginModel.Instance.WechatHeadimgurl = ide.Value.ToString();
                             
                                break;
                            case "city":
                                LoginModel.Instance.City = ide.Value.ToString();
                                break;
                            case "sex":
                                LoginModel.Instance.Sex = ide.Value.ToString();
                                break;
                            case "language":
                                LoginModel.Instance.Language = ide.Value.ToString();
                                break;
                            case "unionid":
                                LoginModel.Instance.Openid = ide.Value.ToString();
                             
                                break;
                            case "nickname":
                                LoginModel.Instance.Nickname = ide.Value.ToString();
                               
                                break;
                            
                        }
                    }

                    //LoadIcon();
                    SystemMsgView.SystemFunction(Function.Tip, ("授权成功"));
                   
                    LoginView lv = ViewMgr.Instance.views[ViewNames.LoginView].Viewlist[0] as LoginView;
                    lv.egv.SendLoginSDKReq("2");
                    break;
            }

        }

        else if (state == ResponseState.Fail)
        {
#if UNITY_ANDROID
            print("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
#endif
            SystemMsgView.SystemFunction(Function.Tip, ("获取信息失败 :") + ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]), 1.5f);
       
        }
        else if (state == ResponseState.Cancel)
        {
            SystemMsgView.SystemFunction(Function.Tip, "取消获取信息", 1.5f);
  
        }
    }


}


/*

//QQ用户信息结构体
 struct QQUser 
{
     public string yellow_vip_level;
     public string msg;
     public string province;
     public string gender;
     public string is_yellow_year_vip;
     public int is_lost;
     public string nickname;
     public int ret;
     public string level;
     public string city;
     public string figureurl;
     public string figureurl_1;
     public string figureurl_2;
     public string figureurl_qq_1;
     public string figureurl_qq_2;
     public string vip;
     public string is_yellow_vip;
}
*/
