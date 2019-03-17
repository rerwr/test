using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework;

namespace Game
{
    public class FcItem : MonoBehaviour
    {
        public void SetData(BaseObject bo)
        {
            transform.Find("Name").GetComponent<Text>().text = bo.Name;
            transform.Find("Count").GetComponent<Text>().text ="x"+bo.ObjectNum.ToString();
            Image img = transform.Find("Image").GetComponent<Image>();
            Sprite sp = SpritesManager.Instance.GetSprite(bo.ID);
            float height = 45;
            float width = sp.rect.width / sp.rect.height * height;
            img.rectTransform.sizeDelta = new Vector2(width , height);
            img.sprite = sp;
            img.color = Color.white;
        }
    }
}
