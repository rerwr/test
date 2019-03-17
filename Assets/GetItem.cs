

using Game;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GetItem : MonoBehaviour, IPointerClickHandler
{
    public int ID;
    private GridLayoutGroup grid;

    public virtual void SetData(DeltaStoreUnit bo)
    {
        ID = bo.Id;
        BaseAtrribute ba = LoadObjctDateConfig.Instance.GetAtrribute(ID);

        transform.Find("Name").GetComponent<Text>().text = "×"+bo.Deltacount1.ToString();
    
        Image img = transform.Find("Image").GetComponent<Image>();
        
        Sprite sp = SpritesManager.Instance.GetSprite(ID);
        img.sprite = sp;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        ShopController.Instance.OnSelectItem(ID);
    }

}