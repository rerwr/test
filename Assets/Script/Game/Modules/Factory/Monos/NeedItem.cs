using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Game;
using UnityEngine.UI;

public class NeedItem : MonoBehaviour,IPointerDownHandler,IPointerUpHandler{
    public int id;
    public Transform imageTr;

    public void OnPointerDown(PointerEventData eventData)
    {
        Sprite s = SpritesManager.Instance.GetSprite(id);
        Image i=imageTr.Find("Image").GetComponent<Image>();
        i.sprite = s;
        i.rectTransform.sizeDelta = new Vector2(s.bounds.size.x/ s.bounds.size.y*100, 100);

        BaseAtrribute ba = LoadObjctDateConfig.Instance.GetAtrribute(id);
        imageTr.Find("Text").GetComponent<Text>().text = ba.Name;
        imageTr.Find("Price/Text").GetComponent<Text>().text = ba.Price.ToString();
        imageTr.gameObject.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        imageTr.gameObject.SetActive(false);
    }
}
