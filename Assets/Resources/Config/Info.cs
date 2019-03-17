using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;

namespace Game
{
    public class Info
    {
        public Dictionary<int, string> infos = new Dictionary<int, string>();

        private static Info instance;

        public static Info Instance()
        {
            if (instance == null)
            {
                instance = new Info();
            }
            return instance;
        }
        //服务器的自定
        public Info()
        {

            infos.Add(100, "账号为空."); //22为账号为空
            infos.Add(101, "密码错误."); //22为密码错误
            infos.Add(102, "登陆超时."); //22为登陆超时
            infos.Add(103, "未知错误."); //22为未知错误
            infos.Add(104, "账号已经存在."); //22账号重复
            infos.Add(105, "参数错误."); //22账号重复
            infos.Add(106, "土地未开垦."); //22账号重复
            infos.Add(107, "请先翻地."); //22账号重复
            infos.Add(108, "种子不存在."); //22账号重复
            infos.Add(109, "请先翻地."); //22账号重复
            infos.Add(110, "当前状态不可收获."); //22账号重复

            infos.Add(111, "土地已经开垦."); //土地已经开垦
            infos.Add(112, "物品数量不足."); //物品数量不足

            infos.Add(113, "金币数量不足."); //金币数量不足
            infos.Add(114, "物品不存在."); //物品不存在
            infos.Add(115, "合成精油所需果实的数量不足."); //物品不存在
            infos.Add(116, "今日已经签到.");
            infos.Add(117, "已经是好友.");
            infos.Add(118, "已经发出申请，等待对方同意.");
            infos.Add(119, "你的账号在别处登录.");
            infos.Add(120, "你们不是好友.");
            infos.Add(121, "该土地已经摘取.");
            infos.Add(122, "冷却中.");
            infos.Add(123, "验证码不正确.");
            infos.Add(124, "二维码已经使用.");
            infos.Add(125, "当前阶段无法进行该操作.");
            infos.Add(126, "施肥只能在3阶段.");
            infos.Add(127, "你已经施过肥了.");
            infos.Add(128, "没有虫子.");
            infos.Add(129, "没有长草.");
            infos.Add(130, "你被狗狗抓住.");
            infos.Add(131, "不需要浇水.");
            infos.Add(132, "购买游戏才能使用哦!");

        }

        public const string DisconectedInfo = "网络正在连接...";
        public const string ConnectSuccess = "连接成功";
        public const string ConnectFailed = "网络连接失败，请联网重新登录";
        public const string ConnectFailed2 = "支付失败参数错误";
        public const string Chooseone = "请选择一种支付方式";
        public const string NoFriendCanSteal = "没有好友可以被偷花!";
        public const string NoOrder = "没有可查询的订单";


        public const string PhoneNum = "请输入11位手机号";
        public const string InputFieldNotNull = "输入栏不能为空";
        public const string ChooseFieldNotRight = "请选择合适的选项";

        public const string Null = "没有可以兑换的高级精油";
        public const string AddFriendSucc = "添加好友成功";
        public const string BUYGAME = "购买游戏才能使用哦!";
        public const string NoneDispose = "无处理";//无处理
        public const string RegisterSucc = "注册成功";           //1表示成功
        public const string Lose = "失败";           //2表示失败
        public const string LessCoin = "缺少金币";           //3表示缺少金币
        public const string PhoneNumError = "号码不正确";           //4表示号码不正确
        public const string NonePhoneNum = "没有该号码";           //5没有该号码
        public const string CantPluck = "不可翻地操作";           //6该地块不可翻地操作
        public const string CantSeed = "地块无法播种";           //7该地块无法播种
        public const string InviteCodeErrot = "邀请码错误";           //8为邀请码错误
        public const string InviteCodeTakeEffect = "用户邀请推广的生效";           //9当前用户邀请推广的生效
        public const string crudity = "表示未成熟不可采摘";           //10表示未成熟不可采摘
        public const string DogCatch = "采摘失败，被狗抓住";           //11采摘失败，被狗抓住
        public const string ResultNumError = "果实数量不符合合成条件";           //12果实数量不符合合成条件
        public const string PrimaryOilLess = "初级精油不足一瓶，无法合成半成品精油";           //13初级精油不足一瓶，无法合成半成品精油
        public const string WaterError = "浇水操作失败";           //14浇水操作失败，
        public const string WeedError = "除草操作失败";           //15除草操作失败
        public const string DebugError = "除虫操作失败";           //16除虫操作失败
        public const string FertilizerError = "施肥操作失败";           //17施肥操作失败
        public const string ElixirLess = "仙丹数量不满足兑换条件";           //18仙丹数量不满足兑换条件
        public const string SeniorOilError = "高级精油不满足兑换条件";           //19高级精油不满足兑换条件
        public const string VeriCodeError = "验证码失效";           ////20验证码失效
        public const string ChatInfoSendError = "聊天信息发送失败";           //21聊天信息发送失败
        public const string QRCodeError = "二维码错误";         //22为二维码错误
        public const string SelectionNull = "所有选项不能为空";         //22为二维码错误
        public const string PWPattern = "1必须包含数字,2必须包含小写或大写字母,3必须包含特殊符号,4至少8个字符，最多30个字符";         //22为二维码错误
        public const string WXPaySucc = "恭喜您支付成功！开始圣菲的种植之旅。";         //22为二维码错误
        public const string WXPayFail = "支付失败";         //22为二维码错误
        public const string WXPayCancel = "支付取消";         //22为二维码错误
        public const string PWNotSame = "前后输入密码不一致";         //22为二维码错误

        public const string Reclaim = "是否对该土地进行翻地";
        public const string SeedNumNotEngouth = "种子数量不足，请到商店中购买";
        public const string DogFoodNumNotEngouth = "狗粮数量不足，请到商店中购买";
        public const string PlantNoBug = "该植物没有虫子，无法除虫";
        public const string PlantNoNeedWater = "该植物不缺水，不需要浇水";
        public const string PlantNoNeedWeed = "该植物没有杂草，不需要除草";
        public const string Fertilizer = "是否对该植物进行施肥操作";
        public const string Headhas = "请选择一个头像自定义";
        public const string namehas = "名字不能为空";
        public const string Uploading = "正在上传中...";
        public const string SeniorOilNotEnough = "当前高级精油数量不足";
        public const string Exchange = "恭喜您兑换成功!";
        public const string Logined = "恭喜您成功购买圣菲花园，祝您游戏愉快!";
        public const string CodeHasUsed = "二维码已经被使用";
        public const string DogMax = "宠物狗已到达最高级不需要喂养";
        public const string Sendcash = "<size=22>1.运费用于支付快递和保险\n2.可以在消息面板中查询运费订单\n3.快递的状态用户根据快递单号自行查询\n4.圣菲国际保留解释权\n</size>";






        public const string ERROR_INFO = "操作延迟，重新刷新土地";



    }
}
