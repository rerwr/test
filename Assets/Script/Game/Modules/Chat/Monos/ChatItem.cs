using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework;
using Game;

public class ChatItem : MonoBehaviour {

    public void Init(ChatLog c)
    {
        PlayerInfo friend = ChatModel.Instance.ChatTarget;
        Text content = transform.Find("Content").Find("Text").GetComponent<Text>();
        content .text= c.Content;

        Transform headTr = transform.Find("Head");
        Image Head = headTr.GetComponent<Image>();
        if (c.SendPlayer == 2)
        {
            headTr.localPosition = new Vector3(-147, 0, 0);
            Head.rectTransform.sizeDelta = new Vector2(48, 48);
            AsyncImageDownload.Instance.SetAsyncImage(friend.HeaderIcon, Head);
            Head.color = Color.white;
        }
        else if (c.SendPlayer == 1)
        {
            Head.rectTransform.sizeDelta = new Vector2(48, 48);
            AsyncImageDownload.Instance.SetAsyncImage(LoginModel.Instance.Head, Head);
            Head.color = Color.white;
        }
        //else if (c.SendPlayer == 3)
        //{
        //    transform.Find("Content").GetComponent<Image>().color = new Color(1,1,1,0);
        //   content.fontSize = 20;
        //    content.color = Color.gray;
        //    content.alignment = TextAnchor.MiddleCenter;
        //}
        transform.Find("Time").GetComponent<Text>().text =c.sendTime;
        transform.Find("Content").Find("Text").GetComponent<ContentSizeFitter>().SetLayoutVertical();
        RectTransform r = transform.Find("Content").GetComponent<RectTransform>();
        ContentSizeFitter con = transform.Find("Content").GetComponent<ContentSizeFitter>();
        con.SetLayoutVertical();
        LayoutRebuilder.ForceRebuildLayoutImmediate(r);
        float _h= LayoutUtility.GetPreferredSize(r, 1);

        float h = transform.Find("Content").GetComponent<RectTransform>().sizeDelta.y;
        float w = transform.GetComponent<RectTransform>().sizeDelta.x;

        //Debug.Log(transform.GetComponent<RectTransform>().sizeDelta+"..."+w+"..."+_h);
        Vector2 v = transform.GetComponent<RectTransform>().sizeDelta;
        if (h > v.y)
        {
            transform.GetComponent<RectTransform>().sizeDelta = new Vector2(w, h);
        }
    }

}
