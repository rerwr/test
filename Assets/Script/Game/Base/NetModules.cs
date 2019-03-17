using System.Collections;
using System.Collections.Generic;
//using NUnit.Compatibility;
using UnityEngine;

public static class NetModules
{
    public static class NetBase
    {
        public const byte ModuleId = 0;

        public const byte Beat = 1;//游戏心跳包响应, 服务器回应,服务器时间
        public const byte ErrorInfo = 2;//服务器发送到APP的错误提示协议，对一些不需要特别结果的请求的标准相应
    }

    public static class Account
    {
        public const byte ModuleId = 1;

        public const byte ReqLoginSDK = 0;//登陆SDK
        public const byte LogoutSDK = 1;//登出SDK
        public const byte ReqLogin = 2;//登陆
      
        public const byte ReqReLogin = 4;//重登
//        public const byte ReqCreateName = 5;//创建名字
        public const byte Farm_Game_Register_Req = 6;//注册或忘记密码
        public const byte Farm_Game_VeriCode_Req = 7;//注册/忘记密码请求验证码请求
    }
    public static class FarmGameProtocal
    {
        public const byte ModuleId = 0x02;

  
        public const byte Farm_Game_MapInfo_Req = 2;             //App向服务器发起地图信息请求，附带角色ID，以请求该角色的农场主界面地图。
        public const byte Farm_Game_NewUser_Req = 3;             //当用户注册完，首次登陆游戏创建新角色时，发送此消息到服务器申请建立角色信息
        public const byte Farm_Game_RankInfo_Req = 4;           //App向服务器发起仓库信息请求，附带角色ID，以请求该角色的仓库存储信息。
        public const byte Farm_Game_StoreInfo_Req = 5;            //App向服务器请求农田数据信息，附带角色ID。
        public const byte Farm_Game_FriendsInfo_Req = 6;         //App向服务器请求朋友数据信息，附带角色ID。
        public const byte Farm_Game_ShopInfo_Req = 7;            //App向服务器请求信息,
        public const byte Farm_Game_AnnouncementInfo_Req = 8;    //App向服务器请求公告信息
        public const byte Farm_Game_Store_Update = 9;            //返回仓库变化的信息
    }

   
    public static class GameAction
    {
        public const byte ModuleId = 0x03;

        public const byte Farm_Game_Action_Fandi_Req = 1;        //翻地操作回应
        public const byte Farm_Game_Seed_Req = 2;                //播种请求
        public const byte Farm_Game_Action_WF_Req = 3;           //  名称: 浇水，施肥，除虫，除草操作
        public const byte Farm_Game_Action_Pluck_Req = 4;        ///  名称: 采摘操作
        public const byte Farm_Game_Action_Anw = 5;//  描述: 服务器接收App采摘操作，根据附带角色ID和农田编号，执行相关操作，并返回本地块的当前属性
        public const byte Farm_Game_buyOrSell_Req = 6;                 //  名称: 商店购买请求
        public const byte Farm_Game_sell_Req = 7;                //  名称: 道具卖出请求
        public const byte Farm_Game_pay_Req = 8;                 //  名称: 充值请求
        public const byte Farm_Game_SingleFriendInfo_Req = 9;        //  名称: 点击进入某个好友信息请求
        public const byte Farm_Game_OilUpgrade_Req = 10;        //  名称: 仓库升级请求
        public const byte Farm_Game_AddFriend_Req = 11;        //  名称: App向服务器请求点击好友头像添加操作
        public const byte Farm_Game_Sign_Req = 12;        //  名称:签到请求,让服务器记录签到信息
        public const byte Farm_Game_DogUpgrad_Req = 13;        //  名称: 服务器根据用户在线时间计算狗的升级经验，推送给客户端，客户端给狗一个亮光升级
        public const byte Farm_Game_ReClaim_Req = 14;        //  名称: 农田开垦，向服务器端提交农田ID
        public const byte Farm_Game_ScanQRcodeOrRecommand_Req = 15;        //  名称: A农田开垦，向服务器端提交农田ID，如果用户刚开始填写了邀请码就发送这个
       
        public const byte Farm_Game_Chant_Req = 17;        //  名称:App向服务器发起仓库升级请求，附带角色ID,发起兑换，服务器根据实际仓库情况判断
        public const byte Farm_Game_CoinOrExpChange_Anw = 18;        //  名称:App向服务器发起仓库升级请求，附带角色ID,发起兑换，服务器根据实际仓库情况判断
        public const byte Farm_Game_MessageSend_Req = 19;     //名称:App向服务器发起消息列表请求
        public const byte Farm_Game_AgreeAdd_Req = 20;     //名称:App向服务器同意好友请求
        public const byte Farm_Game_SearchFriend_Req = 21;     //名称:App向服务器请求搜索玩家
        public const byte Farm_Game_ChatLog_Req = 22;     //名称:App向服务器请求聊天记录
        public const byte Farm_Game_DeleteMsg_Req = 23;     //名称:App向服务器请求删除已读消息
        public const byte Farm_Game_FeedDog_Req = 24;     //名称:App向服务器喂狗请求
        public const byte Farm_Game_TotalSell_Req = 25;     //名称:App向服务器全部卖出请求
        public const byte Farm_Game_InfoLog_Req = 26;     //名称:App向服务器简介
        public const byte Farm_Game_OidExchange_Req = 27;        //  根据物品数量获得邮费
        public const byte Farm_Game_Postage_Req = 28;        //  根据物品数量获得邮费
        public const byte Farm_Game_PayUI_Req = 29;        // pp向服务器发起支付升级请求，服务器返回订单信息调起支付界面
        public const byte Farm_Game_PaySucc_Req = 30;        // App向服务器发起支付升级请求，服务器校验该订单号是否已经支付
        public const byte Farm_Game_CheckOrder_Req = 31;        // 查询订单


    }
}

