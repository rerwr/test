    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework;

namespace Game
{
    public class DogInfoView : BaseSubView
    {
        private Button CloseBtn;
        private Button FeedBtn;
        private Text FeedBtn_Text;
        
        private Text Level;
        private Text Chance;
        private Slider Grow_Slider;
        private Text Grow_Text;

        private Text DogFoodCount;

        public DogInfoView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
        }

        public override void BuildUIContent()
        {
            base.BuildUIContent();
            CloseBtn = TargetGo.transform.Find("CloseBtn").GetComponent<Button>();
            CloseBtn.onClick.AddListener(OnClickCloseBtn);
            FeedBtn = TargetGo.transform.Find("FeedBtn").GetComponent<Button>();
            FeedBtn.onClick.AddListener(OnClickFeedBtn);
            Level = TargetGo.transform.Find("Level").GetComponent<Text>();
            Chance = TargetGo.transform.Find("Chance").GetComponent<Text>();
            Grow_Slider = TargetGo.transform.Find("Grow").GetComponent<Slider>();
            Grow_Text = TargetGo.transform.Find("Grow/Exp").GetComponent<Text>();
            DogFoodCount= TargetGo.transform.Find("DogFood/Count").GetComponent<Text>();
            FeedBtn_Text = TargetGo.transform.Find("FeedBtn/Text").GetComponent<Text>();
        }

        public override void OnOpen()
        {
            base.OnOpen();
            InitDogInfo(0,null);
            DogInfoController.Instance.GetDispatcher().AddListener(DogInfoEvent.OnDogChange,InitDogInfo);
        }

        private bool InitDogInfo(int eventId,object arg)
        {
            LoginModel player = LoginModel.Instance;
            StorageDeltaList deltas=Farm_Game_StoreInfoModel.storage;
            int dogFoodCount=0;
            if (deltas.DogFoods.Count>0)
            {
                dogFoodCount = deltas.DogFoods[601].ObjectNum;

            }
            Level.text = "等级：LV" + player.DogLv;
            Grow_Slider.maxValue = player.DogUpgradeMaxExp;
            Grow_Slider.minValue = 0;
            Grow_Slider.value = player.DogCurrentExp;
            Chance.text = "防盗概率：" + player.Chance + "%";

            if (player.DogLv >= 3)
            {
                Grow_Text.text = "已到最高等级";
                Grow_Slider.maxValue = 1;
                Grow_Slider.minValue = 0;
                Grow_Slider.value =1;

                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "test", "test1"));

            }
            else
            {
                Grow_Text.text = player.DogCurrentExp + "/" + player.DogUpgradeMaxExp;
            }

            
            if (dogFoodCount <= 0)
            {
                DogFoodCount.text = "没有狗粮了\n请前往商店购买！";
                FeedBtn_Text.text = "前往商店";
            }
            else
            {
                DogFoodCount.text = "x"+dogFoodCount.ToString();
                FeedBtn_Text.text = "喂养";
            }

            return false;
        }

        //点击喂狗粮
        private void OnClickFeedBtn()
        {
            if (LoginModel.Instance.DogLv>=3)
            {
                SystemMsgView.SystemFunction(Function.Tip, Info.DogMax);
                return;
            }
            if (Farm_Game_StoreInfoModel.storage.DogFoods.Count==0)
            {
                //SystemMsgView.SystemFunction(Function.OpenDialog, Info.DogFoodNumNotEngouth,ViewNames.ShopView,(() => ViewMgr.Instance.Close(ViewNames.DogInfoView)));
                ShopController.Instance.Model = 3;
                ViewMgr.Instance.Open(ViewNames.ShopView);
                ViewMgr.Instance.Close(ViewNames.DogInfoView);
            }
            else
            {
                DogInfoController.Instance.FeedDog();

            }
        }

        private void OnClickCloseBtn()
        {
            ViewMgr.Instance.Close(ViewNames.DogInfoView);
        }

        public override void OnClose()
        {
            base.OnClose();
            DogInfoController.Instance.GetDispatcher().RemoveListener(DogInfoEvent.OnDogChange, InitDogInfo);
        }
    }
}
