using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework;

namespace Game
{
    public class FactorySuccView : BaseSubView
    {
        private Image ProduceImage;
        private Image ShiningImage;

        private Transform PrimaryContent;
        private Transform SeniorContent;
        private Transform GoodsContent;
        private Button SeniorContent_ExchangeBtn;
        private Button CloseBtn;

        private int produceType=1;//合成物品type，1为初级精油，2为高级精油，3为实物；

        public FactorySuccView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
        }

        public override void BuildUIContent()
        {
            base.BuildUIContent();

            ProduceImage = TargetGo.transform.Find("ProduceImage/Image").GetComponent<Image>();
            ShiningImage = TargetGo.transform.Find("ShiningImage").GetComponent<Image>();
            ShiningImage.gameObject.AddComponent<ShiningImage>();
            PrimaryContent = TargetGo.transform.Find("PrimaryContent");
            SeniorContent = TargetGo.transform.Find("SeniorContent");
            GoodsContent = TargetGo.transform.Find("GoodsContent");

            SeniorContent_ExchangeBtn = TargetGo.transform.Find("SeniorContent/ExchangeNow").GetComponent<Button>();
            SeniorContent_ExchangeBtn.onClick.AddListener(OnClickBtn);
            CloseBtn = TargetGo.transform.Find("CloseBtn").GetComponent<Button>();
            CloseBtn.onClick.AddListener(OnClickCloseBtn);
            WindowShowOrHide(false);
        }

        public override void OnOpen()
        {
            base.OnOpen();
            FactoryController.Instance.GetDispatcher().AddListener(FactoryControllerEvent.OnUpgrade,OnSucc);
            FactoryController.Instance.GetDispatcher().AddListener(FactoryControllerEvent.OnUpgrading, OnUpgrading);
        }

        //点击立即兑换按钮
        private void OnClickBtn()
        {

        }
        
        private bool OnUpgrading(int eventId, object arg)
        {
            int produceId = (int)arg;
            BaseAtrribute ba = LoadObjctDateConfig.Instance.GetAtrribute(produceId);
            if (ba.Type == ObjectType.PrimaryOil)
            {
                produceType = 1;
            }
            else if (ba.Type == ObjectType.SemiOil)
            {
                produceType = 2;
            }
            else if (ba.Type == ObjectType.elixir)
            {
                produceType = 3;
            }

            Sprite sp = SpritesManager.Instance.GetSprite(produceId);
            ProduceImage.rectTransform.sizeDelta = new Vector2(56, 80);
            ProduceImage.sprite = sp;
            ProduceImage.color = Color.white;
            return false;
        }

        //当合成成功时调用
        private bool OnSucc(int eventId, object arg)
        {
            WindowShowOrHide(true);
            if (produceType == 1)
            {
                PrimaryContent.gameObject.SetActive(true);
                SeniorContent.gameObject.SetActive(false);
                GoodsContent.gameObject.SetActive(false);
            }
            else if(produceType==2)
            {
                PrimaryContent.gameObject.SetActive(false);
                SeniorContent.gameObject.SetActive(true);
                GoodsContent.gameObject.SetActive(false);
            }
            else if (produceType == 3)
            {
                PrimaryContent.gameObject.SetActive(false);
                SeniorContent.gameObject.SetActive(false);
                GoodsContent.gameObject.SetActive(true);
            }

            MusicManager.Instance.Playsfx(AudioNames.ProduceSucc);
            //MTRunner.Instance.StartRunner(CloseWindow());
            return false;
        }

        //2秒后关闭该窗口
        //private IEnumerator CloseWindow()
        //{
        //    yield return 2f;
        //    WindowShowOrHide(false);
        //}

        //合成成功窗口的显示与隐藏
        private void WindowShowOrHide(bool isShow)
        {
            TargetGo.SetActive(isShow);
        }

        private void OnClickCloseBtn()
        {
            MusicManager.Instance.Playsfx(AudioNames.CloseBtn);
            WindowShowOrHide(false);
        }

        public override void OnClose()
        {
            base.OnClose();
            FactoryController.Instance.GetDispatcher().RemoveListener(FactoryControllerEvent.OnUpgrade, OnSucc);
            FactoryController.Instance.GetDispatcher().RemoveListener(FactoryControllerEvent.OnUpgrade, OnUpgrading);
        }
    }
}
