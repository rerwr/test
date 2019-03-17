using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework;
using UnityEngine.EventSystems;
using System;

namespace Game
{
    public class storeItem : MonoBehaviour,IPointerClickHandler
    {
        public int UserGameID;  //背包物品id
        //输入显示的数据
        public void SetData(BaseObject bo)
        {
            UserGameID = bo.ID;

            BaseAtrribute ba = LoadObjctDateConfig.Instance.GetAtrribute(bo.ID);
            string _count = "";
            if (ba.Type == ObjectType.PrimaryOil)
            {
                //_count = (10 * bo.ObjectNum).ToString() + "ml";
                _count = "x" + bo.ObjectNum.ToString();
            }
            else if (ba.Type == ObjectType.SemiOil)
            {
                _count = (0.5 * bo.ObjectNum).ToString() + "ml";
            }
            else
            {
                _count = "x" + bo.ObjectNum.ToString();
            }
            transform.Find("Count").GetComponent<Text>().text =_count;
            transform.Find("Price").GetComponent<Text>().text = ba.Price.ToString();
            transform.Find("Name").GetComponent<Text>().text = bo.Name;

            Image img = transform.Find("Image").GetComponent<Image>();
            Sprite sp= SpritesManager.Instance.GetSprite(bo.ID);
            img.sprite = sp;
            float height = 69;
            float width = sp.rect.width / sp.rect.height * height;
            img.rectTransform.sizeDelta = new Vector2(width, height);
            //img.rectTransform.sizeDelta = new Vector2(sp.rect.width,sp.rect.height);
            img.color = Color.white; 
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            StoreController.Instance.SelectItem(this.GetComponent<storeItem>().UserGameID);
        }
    }
}