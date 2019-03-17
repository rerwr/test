using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework;
using System;

namespace Game
{
    public class StoreSellView : BaseSubView
    {
        private Button BtnClose;
        private Button BtnSell;
        private Text BtnText;

        private Image Info_Image;
        private Text Info_Name;
        private Text Info_UnitPrice;
        private Text Introduce_Text;
        private ScrollRect Introduce_Scroll;

        private Transform Count;
        private Button Count_AddBtn;
        private Button Count_ReduceBtn;
        private InputField Count_Number;

        private Transform SellOilView;
        private Button SellOilBtn;
        private Button ExChangeBtn;

        private int ID=0;//当前售卖的物品的id

        public StoreSellView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
            
        }
        
        public override void BuildUIContent()
        {
            base.BuildUIContent();

            BtnClose = TargetGo.transform.Find("CloseBtn").GetComponent<Button>();
            BtnClose.onClick.AddListener(OnClickCloseBtn);
            BtnSell = TargetGo.transform.Find("SellBtn").GetComponent<Button>();
            BtnSell.onClick.AddListener(OnClickSellBtn);
            BtnText= TargetGo.transform.Find("SellBtn/text").GetComponent<Text>();

            Info_Image = TargetGo.transform.Find("SellInfo/Image").GetComponent<Image>();
            Info_Name = TargetGo.transform.Find("SellInfo/Name").GetComponent<Text>();
            Info_UnitPrice = TargetGo.transform.Find("SellInfo/Coin/UnitPrice").GetComponent<Text>();
            Introduce_Text= TargetGo.transform.Find("Introduce/Image/Text").GetComponent<Text>();
            Introduce_Scroll= TargetGo.transform.Find("Introduce/Image").GetComponent<ScrollRect>();

            Count = TargetGo.transform.Find("SellInfo/Count");
            Count_AddBtn = TargetGo.transform.Find("SellInfo/Count/Add").GetComponent<Button>();
            Count_AddBtn.onClick.AddListener(delegate () { this.OnClickCountBtn(1); });
            Count_ReduceBtn = TargetGo.transform.Find("SellInfo/Count/Reduce").GetComponent<Button>();
            Count_ReduceBtn.onClick.AddListener(delegate () { this.OnClickCountBtn(-1); });
            Count_Number = TargetGo.transform.Find("SellInfo/Count/InputField").GetComponent<InputField>();
            Count_Number.onValueChanged.AddListener(delegate (string str){this.OnClickCountBtn(0);});
            Count_Number.text = "1";

            SellOilView= TargetGo.transform.Find("SellOilBtn");
            SellOilView.gameObject.SetActive(false);
            SellOilBtn = TargetGo.transform.Find("SellOilBtn/SellOil").GetComponent<Button>();
            SellOilBtn.onClick.AddListener(OnClickSellBtn);
            ExChangeBtn= TargetGo.transform.Find("SellOilBtn/ExchangeOil").GetComponent<Button>();
            ExChangeBtn.onClick.AddListener(OnClickExChange);

            WindowShowOrHide(false);
        }

        public override void OnOpen()
        {
            base.OnOpen();
           
            StoreController.Instance.GetDispatcher().AddListener(StoreEvent.OnSelectItem,ReFreshView);
            
        }

        //点击关闭窗口按钮
        public void OnClickCloseBtn() {
            Count_Number.text = "1";
            WindowShowOrHide(false);

            MusicManager.Instance.Playsfx(AudioNames.CloseBtn);
        }

        //点击Sell按钮
        private void OnClickSellBtn()
        {
            MusicManager.Instance.Playsfx(AudioNames.OnClick4);

            int userId = LoginModel.Instance.Uid;
            int itemId = ID;
            int count =int.Parse(Count_Number.text);
            StoreController.Instance.SellItem(userId,itemId,count,1);
            StoreController.Instance.currentSellID = itemId;
            StoreController.Instance.currentSellNumber = count;
            WindowShowOrHide(false);
        }

        //点击兑换按钮
        private void OnClickExChange()
        {
            MusicManager.Instance.Playsfx(AudioNames.OnClick4);
            
            int itemId = ID;
            int count = int.Parse(Count_Number.text);
            StoreController.Instance.currentSellID = itemId;
            StoreController.Instance.currentSellNumber = count;
            ViewMgr.Instance.Open(ViewNames.CommitView);
            WindowShowOrHide(false);
        }

        //点击调节售卖数量按钮，调节数量
        private void OnClickCountBtn(int x)
        {
            int count;
            if (!int.TryParse(Count_Number.text,out count))
            {
                Count_Number.text = "";
                return;
            }
            count += x;
            if (count < 1) count = 1;
            BaseObject itemInfo = Farm_Game_StoreInfoModel.Instance.GetData(ID);
            if (itemInfo != null)
            {
                if (count > itemInfo.ObjectNum) count = itemInfo.ObjectNum;
            }
            Count_Number.text = count.ToString();

            MusicManager.Instance.Playsfx(AudioNames.OnClick3);
        }

        //在背包中选中物体后，更新sellview中的内容
        private bool ReFreshView(int eventId, object arg)
        {
            ID = (int)arg;

            //根据ID在model中获取info
            BaseObject itemInfo=Farm_Game_StoreInfoModel.Instance.GetData(ID);
            string Item_path = itemInfo.Url;

            Sprite sp = SpritesManager.Instance.GetSprite(ID);
            Info_Image.sprite = sp;
            Info_Image.rectTransform.sizeDelta = new Vector2(sp.rect.width, sp.rect.height);
            Info_Image.color = Color.white;

            Info_Name.text =itemInfo.Name;
            Info_UnitPrice.text = itemInfo.Price.ToString();
            Introduce_Text.text = itemInfo.Des;

            Count_Number.text = "1";

            BaseAtrribute ba = LoadObjctDateConfig.Instance.GetAtrribute(ID);
            if ( ba.Type == ObjectType.elixir)
            {
               BtnSell.gameObject.SetActive(false);
                SellOilView.gameObject.SetActive(true);
            }
            else
            {
                BtnSell.gameObject.SetActive(true);
                SellOilView.gameObject.SetActive(false);
            }

            WindowShowOrHide(true);
            MusicManager.Instance.Playsfx(AudioNames.OnClick5);
            return false;
        }

        //该窗口的隐藏于显示
        private void WindowShowOrHide(bool isOpen)
        {
            Introduce_Scroll.verticalNormalizedPosition = 1;
            TargetGo.SetActive(isOpen);
        }
        public override void OnClose()
        {
            base.OnClose();
            WindowShowOrHide(false);
            StoreController.Instance.GetDispatcher().RemoveListener(StoreEvent.OnSelectItem,ReFreshView);
        }
    }
}
