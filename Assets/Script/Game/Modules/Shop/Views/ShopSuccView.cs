using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework;

namespace Game
{
    public class ShopSuccView : BaseSubView
    {
        private Button CloseBtn;
        private Text SuccText;

        private Text Name;
        private Text Time;
        private Text Price;
        private Image _Image;

        private int ID;

        public ShopSuccView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
        }

        public override void BuildUIContent()
        {
            base.BuildUIContent();

            CloseBtn = TargetGo.transform.Find("CloseBtn").GetComponent<Button>();
            CloseBtn.onClick.AddListener(OnClickCloseBtn);
            SuccText= TargetGo.transform.Find("SuccText").GetComponent<Text>();
            Name= TargetGo.transform.Find("BuyItem/Name").GetComponent<Text>();
            Time= TargetGo.transform.Find("BuyItem/Time").GetComponent<Text>();
            Price= TargetGo.transform.Find("BuyItem/Price").GetComponent<Text>();
            _Image= TargetGo.transform.Find("BuyItem/Image").GetComponent<Image>();
        }

        public override void OnOpen()
        {
            base.OnOpen();
            TargetGo.SetActive(false);
            ShopController.Instance.GetDispatcher().AddListener(ShopEvent.OnSelectShopItem, OnSelectShopItem);
            ShopController.Instance.GetDispatcher().AddListener(ShopEvent.OnBuying, OnBuying);
        }

        private void OnClickCloseBtn()
        {
            TargetGo.SetActive(false);
            MusicManager.Instance.Playsfx(AudioNames.CloseBtn);
        }

        private bool OnSelectShopItem(int eventId,object arg)
        {
            ID = (int)arg;
            return false;
        }

        private bool OnBuying(int eventId,object arg)
        {
            int count = (int)arg;
            SuccText.text = "成功购买" + count+"个";
            StoreController.Instance.GetDispatcher().AddListener(StoreEvent.OnStoreUnits,OnBuySucc);
            return false;
        }

        //购买成功回调
        private bool OnBuySucc(int eventId,object arg)
        {
            BaseAtrribute ba = LoadObjctDateConfig.Instance.GetAtrribute(ID);
            Name.text = ba.Name;
            Price.text = ba.Price.ToString();

            if (ba.Type == ObjectType.Seed)
            {
                Time.text = "成长周期： " + ba.GrothTime/3600 + "h";
            }
            else
            {
                Time.text = "";
            }
            
            Sprite sp = SpritesManager.Instance.GetSprite(ID);
            _Image.rectTransform.sizeDelta = new Vector2(100, 100);
            //_Image.rectTransform.sizeDelta = new Vector2(sp.rect.width, sp.rect.height);
            _Image.sprite = sp;
            _Image.color = Color.white;

            TargetGo.SetActive(true);
            MusicManager.Instance.Playsfx(AudioNames.GetMoney);
            return true;
        }

        public override void OnClose()
        {
            base.OnClose();

            ShopController.Instance.GetDispatcher().RemoveListener(ShopEvent.OnSelectShopItem, OnSelectShopItem);
            ShopController.Instance.GetDispatcher().RemoveListener(ShopEvent.OnBuying, OnBuying);

        }
    }
}
