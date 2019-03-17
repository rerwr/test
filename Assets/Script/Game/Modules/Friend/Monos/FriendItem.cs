using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework;
using Game;
using UnityEngine.EventSystems;

//,IPointerClickHandler ,IPointerEnterHandler,IBeginDragHandler,IEndDragHandler,IDragHandler, IPointerUpHandler,IPointerExitHandler,IEndDragHandler
public class FriendItem : MonoBehaviour
{

    public int playerID;

    public void SetData(PlayerInfo player)
    {
        playerID = player.UserGameId;
  
//        transform.Find("name").GetComponent<Text>().text = player.GameName.Length<6? player.GameName: player.GameName.Insert(player.GameName.Length / 2, "\n");
        transform.Find("name").GetComponent<Text>().text = player.GameName;
        transform.Find("Level/Text").GetComponent<Text>().text = "LV"+player.UserLevel.ToString();

        Transform gainBtn = transform.Find("GainBtn");
        Button iconBtn = transform.GetComponent<Button>();
        iconBtn.onClick.AddListener(OnClickGainBtn);
        Image img = transform.GetComponent<Image>();
        AsyncImageDownload.Instance.SetAsyncImage(player.HeaderIcon, img);
        img.rectTransform.sizeDelta = new Vector2(100, 100);
        img.color = Color.white;
        
        if (player.Aciton == 1)
        {
            gainBtn.gameObject.SetActive(true);
        }
        else
        {
            gainBtn.gameObject.SetActive(false);
        }
    }

    //点击进入玩家农场偷菜
    public void OnClickGainBtn()
    {
        if (FriendFarmManager.Instance.FriendUid!= playerID)
        {
            FriendFarmManager.Instance.GoFriendFarm(playerID);

        }
        //FriendsInfoController.Instance.SendSingleFriendInfoReq (playerID);
        //FriendsInfoController.Instance.CurrentID = playerID;
        //        SliderControl.Instance.OnPointerDown();
    }

//    public void OnPointerClick(PointerEventData eventData)
//    {
//        FriendFarmManager.Instance.GoFriendFarm(playerID);
////        SliderControl.Instance.OnPointerDown();
////        Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "pclick", "test1"));
//
//    }

//    public void OnPointerEnter(PointerEventData eventData)
//    {
////        SliderControl.Instance.OnPointerDown();
//
//        Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "OnPointerEnter", "test1"));
//
//    }
//
//    public void OnBeginDrag(PointerEventData eventData)
//    {
//        Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "enter", "test1"));
//
//    }
//
//    public void OnEndDrag(PointerEventData eventData)
//    {
//        Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "OnEndDrag", "test1"));
//
//    }
//
//    public void OnDrag(PointerEventData eventData)
//    {
//        Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "OnDrag", "test1"));
//
//    }

//    public void OnPointerUp(PointerEventData eventData)
//    {
//        //        SliderControl.Instance.OnPointerUp();
//
//        Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "OnPointerUp", "test1"));
//
//    }
//
//    public void OnPointerExit(PointerEventData eventData)
//    {
//        SliderControl.Instance.OnPointerUp();
//    }
//
//    public void OnEndDrag(PointerEventData eventData)
//    {
//        SliderControl.Instance.OnPointerUp();
//
//    }
}
