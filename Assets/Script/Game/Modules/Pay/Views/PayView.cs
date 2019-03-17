using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PayView : BaseSubView
    {
        private Button CloseBtn;
        private Button EnsurePay;
        private Button ZhiFuBaoBtn;
        private Button WeChatBtn;
        private Text money;
        private Text txt;
        private payType type = payType.none;
        private string content = "您需支付       才能激活游戏";
        private string num = "10元";
        private enum payType
        {
            none,
            wechat,
            alipay,
        }
        public PayView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {

        }
        public override void BuildUIContent()
        {
            base.BuildUIContent();

            EnsurePay = TargetGo.transform.Find("PayBtn").GetComponent<Button>();
            CloseBtn = TargetGo.transform.Find("CloseBtn").GetComponent<Button>();
            EnsurePay.onClick.AddListener(OnEnsurePayClik);
            CloseBtn.onClick.AddListener(ClosePayPanel);
            money = TargetGo.transform.Find("money").GetComponent<Text>();
            txt = TargetGo.transform.Find("Text").GetComponent<Text>();

            ZhiFuBaoBtn = TargetGo.transform.Find("ZhiFuBaoBtn").GetComponent<Button>();
            ZhiFuBaoBtn.onClick.AddListener(OnClickZhiFuBao);
            WeChatBtn = TargetGo.transform.Find("WeChantPayBtn").GetComponent<Button>();
            WeChatBtn.onClick.AddListener(OnClickWeChat);
        }

        public override void OnClose()
        {
            base.OnClose();
            CommitController.Instance.GetDispatcher().RemoveListener(CommitController.CommitControllerEvent.OnExchange, UpPayClick);
            GlobalDispatcher.Instance.RemoveListener(GlobalEvent.OnPayEnded, PayEnd);
        }

        public override void OnOpen()
        {
            base.OnOpen();
            //服务器端已经配置好
            CommitController.Instance.GetDispatcher().AddListener(CommitController.CommitControllerEvent.OnExchange, UpPayClick);
            GlobalDispatcher.Instance.AddListener(GlobalEvent.OnPayEnded, PayEnd);
            if (PayOrderInterfaceMgr.Instance.payfor == PayFor.Login)
            {
                //第一次支付
                CloseBtn.gameObject.SetActive(false);
            }
            else
            {
                num = CommitViewModel.Instance.Postage + "元";
                content = "您需支付       运费才能兑换";
                CloseBtn.gameObject.SetActive(true);
            }
            money.text = num;
            txt.text = content;
        }

        
        private bool  PayEnd(int id,object arg)
        {
            EnsurePay.onClick.AddListener(OnEnsurePayClik);

            return false;
        }

        private void OnEnsurePayClik()
        {
            
            //直接登录模式直接调起
            if (PayOrderInterfaceMgr.Instance.payfor == PayFor.Login && !PlayerSave.HasKey("Login") )
            {

                NativeHandle handle = new NativeHandle();
                if (type == payType.alipay)
                {
                    object payData = PayOrderInterfaceMgr.Instance.GetDatas(LoginModel.Instance.Uid, Urltype.alipay, PayOrderInterfaceMgr.Instance.payfor, GetPayCount());
                    if (payData != null)
                    {
                        handle.Alipay(payData.ToString());

                        Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", payData.ToString(), "test1"));

                    }
                }
                else if (type == payType.wechat)
                {
                    object o = PayOrderInterfaceMgr.Instance.GetDatas(LoginModel.Instance.Uid, Urltype.wx, PayOrderInterfaceMgr.Instance.payfor, GetPayCount());
                    if (o != null)
                    {
                        PayData payData = o as PayData;
                        handle.WechatPay(payData.appid, payData.partnerid, payData.prepayid, payData.noncestr, payData.timestamp, payData.package, payData.sign);

                    }
                }
                else
                {
                    SystemMsgView.SystemFunction(Function.Tip, Info.Chooseone);
                }
            }
            else if(PayOrderInterfaceMgr.Instance.payfor == PayFor.Exchange)
            {
                
                CommitViewModel cvm = CommitViewModel.Instance;
                CommitController.Instance.OidExchangeReq(LoginModel.Instance.Uid, 1, cvm.Name, cvm.Phone,cvm.Province,cvm.City,cvm.Country,cvm.SelectPinpai, cvm.Address, cvm.Beaty, (int)type);
            }
            EnsurePay.onClick.RemoveListener(OnEnsurePayClik);
        }
        private void ClosePayPanel()
        {
            ViewMgr.Instance.Close(ViewNames.PayChooseView);

        }

        int GetPayCount()
        {
            int num = 0;
            for (int i = 0; i < CommitViewModel.Instance.taos.Count; i++)
            {
                if (CommitViewModel.Instance.taos[i].T.isOn)
                {
                    num += CommitViewModel.Instance.taos[i].Num;

                }
            }
            return num;
        }
        //调起支付
        private bool UpPayClick(int id, object obj)
        {
            NativeHandle handle = new NativeHandle();
            if (type == payType.alipay)
            {
                object payData = PayOrderInterfaceMgr.Instance.GetDatas(LoginModel.Instance.Uid, Urltype.alipay, PayOrderInterfaceMgr.Instance.payfor,GetPayCount());
                if (payData != null)
                {
                    handle.Alipay(payData.ToString());

                    type=payType.none;
                }
            }
            else if (type == payType.wechat)
            {
                object o = PayOrderInterfaceMgr.Instance.GetDatas(LoginModel.Instance.Uid, Urltype.wx, PayOrderInterfaceMgr.Instance.payfor, GetPayCount());
                if (o != null)
                {
                    PayData payData = o as PayData;
                    handle.WechatPay(payData.appid, payData.partnerid, payData.prepayid, payData.noncestr, payData.timestamp, payData.package, payData.sign);
                    type = payType.none;

                }
            }
            else
            {
                SystemMsgView.SystemFunction(Function.Tip, Info.Chooseone);
            }
            return false;
        }

        //点击支付宝支付
        private void OnClickZhiFuBao()
        {
            WeChatBtn.gameObject.GetComponent<Image>().color = new Color(1, 1, 1);

            ZhiFuBaoBtn.gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);

            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "alipay", "test1"));

            type = payType.alipay;
        }

        //点击微信支付
        private void OnClickWeChat()
        {
            ZhiFuBaoBtn.gameObject.GetComponent<Image>().color = new Color(1, 1, 1);

            WeChatBtn.gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
            type = payType.wechat;
            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "wechat", "test1"));

        }
    }
}
