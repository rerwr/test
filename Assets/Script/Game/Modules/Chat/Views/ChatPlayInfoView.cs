using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework;


namespace Game
{
    public class ChatPlayInfoView : BaseSubView
    {
        private Image Head;
        private Button ChatBtn;
        private Text Level_Text;
        private Slider Level_Slider;
        private Text Property;
        private Image HasMsg;

        private bool isChating=true;

        public ChatPlayInfoView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
        }

        public override void BuildUIContent()
        {
            base.BuildUIContent();

            Head = TargetGo.transform.Find("Head").GetComponent<Image>();
            Level_Text = TargetGo.transform.Find("Level").GetComponent<Text>();
            Level_Slider = TargetGo.transform.Find("Level/Slider").GetComponent<Slider>();
            Level_Slider.minValue = 0;
            Property = TargetGo.transform.Find("Property/Text").GetComponent<Text>();
            HasMsg = TargetGo.transform.Find("HasMsg").GetComponent<Image>();
            ChatBtn= TargetGo.transform.Find("Head").GetComponent<Button>();
            ChatBtn.onClick.AddListener(OnClickChatBtn);
        }

        public override void OnOpen()
        {
            base.OnOpen();
            FriendsInfoController.Instance.GetDispatcher().AddListener(FriendsInfoEvent.OnVisitedFriend, RefreshPlayerInfo);
        }

        private void OnClickChatBtn()
        {
            isChating = !isChating;
            ChantController.Instance.GetDispatcher().Dispatch(ChantControllerEvent.OnChat,isChating);
        }

        private bool RefreshPlayerInfo(int eventId,object arg)
        {
            PlayerInfo player = ChatModel.Instance.ChatTarget;

            Level_Text.text = "LV"+player.UserLevel.ToString();
            Level_Slider.maxValue = player.LevelMaxExp;
            Level_Slider.value = player.UserLevel;
            Property.text = player.GameMoney.ToString();
            Head.rectTransform.sizeDelta = new Vector2(90, 90);
            AsyncImageDownload.Instance.SetAsyncImage(player.HeaderIcon, Head,true);
            Head.color = Color.white;

            isChating = false;
            ChantController.Instance.GetDispatcher().Dispatch(ChantControllerEvent.OnChat, isChating);

            return false;
        }

        public override void OnClose()
        {
            base.OnClose();
            FriendsInfoController.Instance.GetDispatcher().RemoveListener(FriendsInfoEvent.OnVisitedFriend, RefreshPlayerInfo);

        }
    }
}
