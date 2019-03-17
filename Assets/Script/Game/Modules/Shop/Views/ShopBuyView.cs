using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework;
using System;

namespace Game
{
    public class ShopBuyView : BaseSubView
    {
        private Image Info_image;
        private Text Info_name;
        private Text Info_price;
        private Text Info_grothTime;

        private Button Btnclose;
        private Button Btnbuy;

        private InputField Count_text;
        private Button Btnadd;
        private Button Btnreduce;

        private int ID;

        public ShopBuyView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
        }

        public override void BuildUIContent()
        {
            base.BuildUIContent();

            Info_image = TargetGo.transform.Find("Info/ShopItem/Image").GetComponent<Image>();
            Info_name = TargetGo.transform.Find("Info/ShopItem/Name").GetComponent<Text>();
            Info_price = TargetGo.transform.Find("Info/ShopItem/Price").GetComponent<Text>();
            Info_grothTime= TargetGo.transform.Find("Info/ShopItem/GrothtTime").GetComponent<Text>();

            Count_text = TargetGo.transform.Find("Count/InputField").GetComponent<InputField>();
            Count_text.onValueChanged.AddListener(delegate (string str) { this.OnClickCountBtn(0); });
            Count_text.text = "1";
            Btnadd = TargetGo.transform.Find("Count/Add").GetComponent<Button>();
            Btnadd.onClick.AddListener(delegate () { this.OnClickCountBtn(1); });
            Btnreduce = TargetGo.transform.Find("Count/Reduce").GetComponent<Button>();
            Btnreduce.onClick.AddListener(delegate () { this.OnClickCountBtn(-1); });

            Btnclose = TargetGo.transform.Find("CloseBtn").GetComponent<Button>();
            Btnclose.onClick.AddListener(OnClickCloseBtn);
            Btnbuy = TargetGo.transform.Find("BuyBtn").GetComponent<Button>();
            Btnbuy.onClick.AddListener(OnClickBuyBtn);

            WindowHideOrShow(false);
        }

        public override void OnOpen()
        {
            base.OnOpen();

            ShopController.Instance.GetDispatcher().AddListener(ShopEvent.OnSelectShopItem,OnSelectItem);
            StoreController.Instance.GetDispatcher().AddListener(StoreEvent.OnStoreUnits, OnBuySucc);
        }

        //选择商品后刷新信息
        private bool OnSelectItem(int eventId,object arg)
        {
            int id = (int)arg;
            ID = id;
            BaseAtrribute ba = LoadObjctDateConfig.Instance.GetAtrribute(id);
            if (ba.Type == ObjectType.Seed)
            {
                Info_grothTime.text = "生长周期：" + ba.GrothTime/3600 + "h";
            }
            else
            {
                Info_grothTime.text = "";
            }
            Info_name.text = ba.Name;
            Info_price.text = ba.Price.ToString();

            Sprite sp = SpritesManager.Instance.GetSprite(id);
            Info_image.rectTransform.sizeDelta = new Vector2(sp.rect.width, sp.rect.height);
            Info_image.sprite = sp;
            Info_image.color = Color.white;

            WindowHideOrShow(true);
            MusicManager.Instance.Playsfx(AudioNames.OnClick5);
            return false;
        }

        //点击关闭购买窗口按钮
        private void OnClickCloseBtn()
        {
            Count_text.text = "1";
            Info_grothTime.text = " ";
            WindowHideOrShow(false);
            MusicManager.Instance.Playsfx(AudioNames.CloseBtn);
        }

        //点击确认购买
        private void OnClickBuyBtn()
        {
            int count;
            if (int.TryParse(Count_text.text, out count))
            {
                ShopController.Instance.SendBuyOrSellReq(ID,count,0);
            }
            else
            {
                Debug.Log("请输入购买数量");
            }
            MusicManager.Instance.Playsfx(AudioNames.OnClick4);
        }

        //点击调节购买数量按钮
        private void OnClickCountBtn(int x) {
            int count;
            if (!int.TryParse(Count_text.text, out count))
            {
                Count_text.text = "";
                return;
            }
            count += x;
            if (count < 1) count = 1;
            Count_text.text = count.ToString();

            MusicManager.Instance.Playsfx(AudioNames.OnClick3);
        }

        //购买窗口的隐藏与显示
        private void WindowHideOrShow(bool isShow)
        {
            TargetGo.SetActive(isShow);
        }

        //购买成功回调
        private bool OnBuySucc(int eventId,object arg)
        {
            WindowHideOrShow(false);
            return false;
        }

        public override void OnClose()
        {
            base.OnClose();
            WindowHideOrShow(false);
            ShopController.Instance.GetDispatcher().RemoveListener(ShopEvent.OnSelectShopItem,OnSelectItem);
            ShopController.Instance.GetDispatcher().RemoveListener(ShopEvent.OnBuySucc, OnBuySucc);
        }
    }
}