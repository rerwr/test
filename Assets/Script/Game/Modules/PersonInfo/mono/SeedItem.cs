using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using Framework;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

namespace Game
{
    public class SeedItem:ShopItem
    {
      

        public override void SetData(BaseObject bo)
        {
//            LoadingImageManager.Instance.AddLoadingItem();
            ID = bo.ID;
            transform.Find("Name").GetComponent<Text>().text = bo.Name;
            transform.Find("Price").GetComponent<Text>().text = "×"+bo.ObjectNum.ToString();

            BaseAtrribute ba = LoadObjctDateConfig.Instance.GetAtrribute(bo.ID);
            if (ba.Type == ObjectType.Seed)
            {
                transform.Find("GrothTime").GetComponent<Text>().text = ba.GrothTime/3600 + "h";
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

        public override void OnPointerClick(PointerEventData eventData)
        {
            GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnSelectSeed, ID);
          
        }
    }
}
