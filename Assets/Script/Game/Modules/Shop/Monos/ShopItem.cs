using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework;
using UnityEngine.EventSystems;
using System;

namespace Game
{
    public class ShopItem : MonoBehaviour,IPointerClickHandler
    {
        public int ID;
        private GridLayoutGroup grid;
        public virtual void SetData(BaseObject bo)
        {
            ID = bo.ID;
            transform.Find("Name").GetComponent<Text>().text = bo.Name;
            transform.Find("Price").GetComponent<Text>().text = bo.Price.ToString();

            BaseAtrribute ba = LoadObjctDateConfig.Instance.GetAtrribute(bo.ID);
            if (ba.Type == ObjectType.Seed)
            {
                transform.Find("GrothTime").GetComponent<Text>().text = "生长周期：" + ba.GrothTime /3600+ "h";
            }
            else
            {
                transform.Find("GrothTime").GetComponent<Text>().text = " ";
            }

            Image img = transform.Find("Image").GetComponent<Image>();
            Sprite sp = SpritesManager.Instance.GetSprite(bo.ID);
            img.rectTransform.sizeDelta = new Vector2(sp.rect.width, sp.rect.height);
            img.sprite = sp;
            img.color = Color.white;
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            ShopController.Instance.OnSelectItem(ID);
        }
    }
}
