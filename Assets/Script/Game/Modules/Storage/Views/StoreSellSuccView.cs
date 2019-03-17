using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework;

namespace Game
{
    public class StoreSellSuccView : BaseSubView
    {
        private Button CloseBtn;
        private Text GetMoney;
        private Text SellContent;

        public StoreSellSuccView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
        }

        public override void BuildUIContent()
        {
            base.BuildUIContent();

            CloseBtn = TargetGo.transform.Find("CloseBtn").GetComponent<Button>();
            CloseBtn.onClick.AddListener(OnClickCloseBtn);
            GetMoney = TargetGo.transform.Find("Money").GetComponent<Text>();
            SellContent = TargetGo.transform.Find("SellContent").GetComponent<Text>();

            TargetGo.SetActive(false);
        }

        public override void OnOpen()
        {
            base.OnOpen();

            StoreController.Instance.GetDispatcher().AddListener(StoreEvent.OnSellSucc,OnSellSucc);
        }

        //点击关闭按钮
        private void OnClickCloseBtn()
        {
            MusicManager.Instance.Playsfx(AudioNames.CloseBtn);
            StoreController.Instance.currentSellID=0;
            StoreController.Instance.currentSellNumber=0;
            TargetGo.SetActive(false);
        }

        private bool OnSellSucc(int eventId,object arg)
        {
            int itemId = StoreController.Instance.currentSellID;
            int count = StoreController.Instance.currentSellNumber;
            string type = "";
            BaseAtrribute ba = LoadObjctDateConfig.Instance.GetAtrribute(itemId);
            switch (ba.Type) {
                case ObjectType.Seed:
                    type = "种子";
                    break;
                case ObjectType.Result:
                    type = "果实";
                    break;
                case ObjectType.PrimaryOil:
                case ObjectType.SeniorOil:
                    type = "精油";
                    break;
                case ObjectType.Fertilizer:
                    type = "肥料";
                    break;
                default:
                    Debug.Log("卖出物体类型错误");
                    break;

            }
            GetMoney.text = (count * ba.Price).ToString();
            SellContent.text = "[出售" + count+"份"+type + "]";
            TargetGo.SetActive(true);

            MusicManager.Instance.Playsfx(AudioNames.GetMoney);
            return false;
        }

        public override void OnClose()
        {
            base.OnClose();

            StoreController.Instance.GetDispatcher().RemoveListener(StoreEvent.OnSellSucc, OnSellSucc);
        }

    }
}
