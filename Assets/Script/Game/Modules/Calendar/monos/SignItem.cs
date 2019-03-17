using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.monos
{
    public class SignItem: MonoBehaviour, IPointerClickHandler
    {
        public int ID;
        private AwardUnit a;
        public  void SetData(AwardUnit bo)
        {
            ID = bo.Id;
            

            BaseAtrribute ba = LoadObjctDateConfig.Instance.GetAtrribute(bo.Id);
          
            transform.Find("Text").GetComponent<Text>().text = "第"+bo.days+"天";

            transform.Find("Name").GetComponent<Text>().text ="×"+ bo.Num.ToString();
          
            Image img = transform.Find("Image").GetComponent<Image>();
            Sprite sp = SpritesManager.Instance.GetSprite(bo.Id);
            img.rectTransform.sizeDelta =new Vector2(89, 88);
            img.sprite = sp;
            img.color = Color.white;
        }

    
        public virtual void OnPointerClick(PointerEventData eventData)
        {
//            ShopController.Instance.OnSelectItem(ID);
        }

        public void ShowGetMask()
        {
            transform.Find("mask").GetComponent<Image>().enabled =true;

        }
        public void HideMask()
        {
            transform.Find("mask").GetComponent<Image>().enabled = false;

        }
    }
}