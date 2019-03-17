using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using cn.sharesdk.unity3d;

public class Demo : MonoBehaviour {

	public GUISkin demoSkin;
	public ShareSDK ssdk;
	// Use this for initialization
	void Start ()
	{	
		ssdk = gameObject.GetComponent<ShareSDK>();
		ssdk.authHandler = OnAuthResultHandler;
		ssdk.shareHandler = OnShareResultHandler;
		ssdk.showUserHandler = OnGetUserInfoResultHandler;
		ssdk.getFriendsHandler = OnGetFriendsResultHandler;
		ssdk.followFriendHandler = OnFollowFriendResultHandler;
	}

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
	}
	
	void OnGUI ()
	{

		GUI.skin = demoSkin;
		
		float scale = 1.0f;

		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			scale = Screen.width / 320;
		}
		
		//float btnWidth = 165 * scale;
		float btnWidth= Screen.width / 5 * 2;
		float btnHeight = Screen.height / 25;
		float btnTop = 30 * scale;
		float btnGap = 20 * scale;
		GUI.skin.button.fontSize = Convert.ToInt32(13 * scale);

		if (GUI.Button(new Rect((Screen.width - btnGap) / 2 - btnWidth, btnTop, btnWidth, btnHeight), "Authorize"))
		{
			print(ssdk == null);
			ssdk.Authorize(PlatformType.QQ);
		}
			
		if (GUI.Button(new Rect((Screen.width - btnGap) / 2 + btnGap, btnTop, btnWidth, btnHeight), "Get User Info"))
		{
			ssdk.GetUserInfo(PlatformType.QQ);
		}

		btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnGap) / 2 - btnWidth, btnTop, btnWidth, btnHeight), "Show Share Menu"))
		{
			ShareContent content = new ShareContent();

			//(Android only) 隐藏九宫格里面不需要用到的平台（仅仅是不显示平台）
			//(Android only) 也可以把jar包删除或者把Enabl属性e改成false（对应平台的全部功能将用不了�?			String[] platfsList = { ((int)PlatformType.QQ).ToString(), ((int)PlatformType.Facebook).ToString(), ((int)PlatformType.TencentWeibo).ToString() };
//			content.SetHidePlatforms (platfsList);

			content.SetText("this is a test string.");
			content.SetImageUrl("http://ww3.sinaimg.cn/mw690/be159dedgw1evgxdt9h3fj218g0xctod.jpg");
			content.SetTitle("test title");

			//(Android only) 针对Android绕过审核的多图分享，传图片String数组 
			String[] imageArray =  {"/sdcard/test.jpg", "http://f1.webshare.mob.com/dimgs/1c950a7b02087bf41bc56f07f7d3572c11dfcf36.jpg", "/sdcard/test.jpg"};
			content.SetImageArray (imageArray);

			content.SetTitleUrl("http://www.mob.com");
			content.SetSite("Mob-ShareSDK");
			content.SetSiteUrl("http://www.mob.com");
			content.SetUrl("http://www.mob.com");
			content.SetComment("test description");
			content.SetMusicUrl("http://mp3.mwap8.com/destdir/Music/2009/20090601/ZuiXuanMinZuFeng20090601119.mp3");
			content.SetShareType(ContentType.Image);

			//不同平台分享不同内容
			ShareContent customizeShareParams = new ShareContent();
			customizeShareParams.SetText("Sina share content");
			customizeShareParams.SetImageUrl("http://git.oschina.net/alexyu.yxj/MyTmpFiles/raw/master/kmk_pic_fld/small/107.JPG");
			customizeShareParams.SetShareType(ContentType.Text);
			customizeShareParams.SetObjectID("SinaID");
			content.SetShareContentCustomize(PlatformType.SinaWeibo, customizeShareParams);
			//优先客户端分�?			// content.SetEnableClientShare(true);

			//使用微博API接口应用内分�?iOS only
			 // content.SetEnableSinaWeiboAPIShare(true);

			//通过分享菜单分享
			ssdk.ShowPlatformList (null, content, 100, 100);
		}
			
		if (GUI.Button(new Rect((Screen.width - btnGap) / 2 + btnGap, btnTop, btnWidth, btnHeight), "Show Share View"))
		{
			ShareContent content = new ShareContent();
			content.SetText("this is a test string.");
			content.SetImageUrl("http://ww3.sinaimg.cn/mw690/be159dedgw1evgxdt9h3fj218g0xctod.jpg");
			content.SetTitle("test title");
			content.SetTitleUrl("http://www.mob.com");
			content.SetSite("Mob-ShareSDK");
			content.SetSiteUrl("http://www.mob.com");
			content.SetUrl("http://www.mob.com");
			content.SetComment("test description");
			content.SetMusicUrl("http://mp3.mwap8.com/destdir/Music/2009/20090601/ZuiXuanMinZuFeng20090601119.mp3");
			content.SetShareType(ContentType.Image);

			ssdk.ShowShareContentEditor (PlatformType.SinaWeibo, content);
		}

		btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnGap) / 2 - btnWidth, btnTop, btnWidth, btnHeight), "Share Content"))
		{
			ShareContent content = new ShareContent();
			content.SetText("this is a test string.");
			content.SetImageUrl("http://ww3.sinaimg.cn/mw690/be159dedgw1evgxdt9h3fj218g0xctod.jpg");
			content.SetTitle("test title");
//			content.SetTitleUrl("http://www.mob.com");
//			content.SetSite("Mob-ShareSDK");
			// content.SetSiteUrl("http://www.mob.com");
			content.SetUrl("http://qjsj.youzu.com/jycs/");
//			content.SetComment("test description");
//			content.SetMusicUrl("http://mp3.mwap8.com/destdir/Music/2009/20090601/ZuiXuanMinZuFeng20090601119.mp3");
			content.SetShareType(ContentType.Webpage);
			ssdk.ShareContent (PlatformType.WeChat, content);
		}
			
		if (GUI.Button(new Rect((Screen.width - btnGap) / 2 + btnGap, btnTop, btnWidth, btnHeight), "Get Friends SinaWeibo "))
		{
			//获取新浪微博好友，第一页，每页15条数�?			print ("Click Btn Of Get Friends SinaWeibo");
			ssdk.GetFriendList (PlatformType.SinaWeibo, 15, 0);
		}

		btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnGap) / 2 - btnWidth, btnTop, btnWidth, btnHeight), "Get Token SinaWeibo "))
		{
			Hashtable authInfo = ssdk.GetAuthInfo (PlatformType.SinaWeibo);			
			print ("share result :");
			print (MiniJSON.jsonEncode(authInfo));
		}
			
		if (GUI.Button(new Rect((Screen.width - btnGap) / 2 + btnGap , btnTop, btnWidth, btnHeight), "Close SSO Auth"))
		{
			ssdk.DisableSSO (true);			
		}

		btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnGap) / 2 - btnWidth, btnTop, btnWidth, btnHeight), "Remove Authorize "))
		{
			ssdk.CancelAuthorize (PlatformType.SinaWeibo);			
		}
			
		if (GUI.Button(new Rect((Screen.width - btnGap) / 2 + btnGap, btnTop, btnWidth, btnHeight), "Add Friend "))
		{
			//关注新浪微博
			ssdk.AddFriend (PlatformType.SinaWeibo, "3189087725");			
		}

		btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnWidth) / 2 , btnTop, btnWidth, btnHeight), "ShareWithContentName"))
		{
			Hashtable customFields = new Hashtable ();
			customFields["imgUrl"] = "http://ww1.sinaimg.cn/mw690/006dJESWgw1f6iyb8bzraj31kw0v67a2.jpg";
			//根据配置文件分享【本接口功能仅暂时支持iOS�?			ssdk.ShareWithContentName(PlatformType.SinaWeibo, "ShareSDK", customFields);		
		}

		btnWidth += 80 * scale;
		btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "ShowShareMenuWithContentName"))
		{
			Hashtable customFields = new Hashtable ();
			customFields["imgUrl"] = "http://ww1.sinaimg.cn/mw690/006dJESWgw1f6iyb8bzraj31kw0v67a2.jpg";
			//根据配置文件展示分享菜单分享【本接口功能仅暂时支持iOS�?			ssdk.ShowPlatformListWithContentName ("ShareSDK", customFields, null, 100, 100);
		}

		btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "ShowShareViewWithContentName"))
		{
			Hashtable customFields = new Hashtable ();
			//根据配置文件展示编辑界面分享【本接口功能仅暂时支持iOS�?			customFields["imgUrl"] = "http://ww1.sinaimg.cn/mw690/006dJESWgw1f6iyb8bzraj31kw0v67a2.jpg";
			ssdk.ShowShareContentEditorWithContentName(PlatformType.SinaWeibo, "ShareSDK", customFields);		
		}

		btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "SMS Authorize"))
		{
			ssdk.Authorize(PlatformType.SMS);		
		}

	}
	
	void OnAuthResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{
			if (result.Count > 0) {
				print ("authorize success !" + "Platform :" + type + "result:" + MiniJSON.jsonEncode(result));
			} else {
				print ("authorize success !" + "Platform :" + type);
			}
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel !");
		}
	}
	
	void OnGetUserInfoResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{
			print ("get user info result :");
			print (MiniJSON.jsonEncode(result));
			print ("AuthInfo:" + MiniJSON.jsonEncode (ssdk.GetAuthInfo (PlatformType.QQ)));
			print ("Get userInfo success !Platform :" + type );
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel !");
		}
	}
	
	void OnShareResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{
			print ("share successfully - share result :");
			print (MiniJSON.jsonEncode(result));
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel !");
		}
	}

	void OnGetFriendsResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{			
			print ("get friend list result :");
			print (MiniJSON.jsonEncode(result));
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel !");
		}
	}

	void OnFollowFriendResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{
			print ("Follow friend successfully !");
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel !");
		}
	}
}
