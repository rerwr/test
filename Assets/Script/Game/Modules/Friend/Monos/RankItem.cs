using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework;
using Game;


public class RankItem : MonoBehaviour {

    public int playerID;
    public int rank;
    private Transform parent;
    private Vector3 pos;
    public void SetDate(PlayerInfo player)
    {
        playerID = player.UserGameId;
        rank = player.Rank;

        transform.Find("name").GetComponent<Text>().text= player.GameName;
        transform.Find("level").GetComponent<Text>().text = "LV"+player.UserLevel.ToString();
        Text rank_Text = transform.Find("Ranking").GetComponent<Text>();
        rank_Text.text = "";
        Transform No1=transform.Find("Ranking/NO1");
        Transform No2 = transform.Find("Ranking/NO2");
        Transform No3 = transform.Find("Ranking/NO3");
        switch (player.Rank)
        {
            case 1:No1.gameObject.SetActive(true);break;
            case 2:No2.gameObject.SetActive(true);break;
            case 3:No3.gameObject.SetActive(true);break;
            default:rank_Text.text = player.Rank.ToString();break;
        }

        Button gainBtn = transform.Find("GainBtn").GetComponent<Button>();

        Button addFriend= transform.Find("AddFriendBtn").GetComponent<Button>();
        if (FriendsInfoModel.Instance.playerInfos.ContainsKey(playerID))
        {
            addFriend.gameObject.SetActive(false);
        }
        else
        {
            if (player.Aciton == 1)
            {
                gainBtn.gameObject.SetActive(true);
            }
            else
            {
                gainBtn.gameObject.SetActive(false);
            }
            addFriend.onClick.AddListener(OnClickAddFriend);
        }
        if (GameStarter.Instance.isDebug)
        {
                gainBtn.gameObject.SetActive(true);

        }
        Image img = transform.Find("Icon").GetComponent<Image>();
        Toggle imgbtn = transform.Find("Icon").GetComponent<Toggle>();
        parent = img.transform.parent;
        pos = img.transform.localPosition;

        imgbtn.onValueChanged.AddListener((arg0 =>
        {
            if (img.sprite.name=="avatar")
            {
                return;
            }
            if (arg0)
            {
                img.rectTransform.localScale=new Vector3(6,6,6);
                img.rectTransform.parent = ViewMgr.Instance.FullScreenRoot;
                img.transform.localPosition=new Vector3(12,pos.y);
            }
            else
            {
                img.rectTransform.localScale = new Vector3(1, 1, 1);

                img.rectTransform.parent = parent;
                img.rectTransform.localPosition = pos;

            }
        } ));
        img.rectTransform.sizeDelta = new Vector2(76, 76);
        img.color = Color.white;
        AsyncImageDownload.Instance.SetAsyncImage(player.HeaderIcon, img);
        gainBtn.onClick.AddListener(OnClickGainBtn);
    }

    //点击进入玩家农场偷菜
    private void OnClickGainBtn()
    {
        FriendFarmManager.Instance.GoFriendFarm(playerID);
    }

    //点击添加好友
    private void OnClickAddFriend()
    {
        FriendsInfoController.Instance.AddFriend(LoginModel.Instance.Uid,playerID);
        ViewMgr.Instance.Open(ViewNames.SystemMsgView);
        SystemMsgView.SystemFunction(Function.Tip, "添加成功，等待对方接受", 2);
        //FriendsInfoController.Instance.currentPage = 1;
        //FriendsInfoController.Instance.RankListReq(LoginModel.Instance.Uid, 1);
    }
}
