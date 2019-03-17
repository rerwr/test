using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Framework;
using Game;

public class UI_Control_ScrollFlow_Item : MonoBehaviour
{
    private UI_Control_ScrollFlow parent;
    [HideInInspector]
    public RectTransform rect;
    public Image img;

    public int ID;
    public List<NeedClass> NeedIds;
    
    public int Count;
    public float v=0;
    private Vector3 p, s;
    /// <summary>
    /// 缩放值
    /// </summary>
    public float sv;
   // public float index = 0,index_value;
    private Color color;

    public void Init(UI_Control_ScrollFlow _parent)
    {
        NeedIds = new List<NeedClass>();
        rect =this. GetComponent<RectTransform>();
        img = this.transform.Find("Image").GetComponent<Image>();
        LoadImage();
        SetNeedItems();
        parent = _parent;
        color = img.color;
    }

    public void Drag(float value)
    {
        v += value;
        p=rect.localPosition;
        p.x=parent.GetPosition(v);
        rect.localPosition = p;

        color.a = parent.GetApa(v);
        img.color = color;
        sv = parent.GetScale(v);
        s.x = sv;
        s.y = sv;
        s.z=1;
        rect.localScale = s;
    }

    private void LoadImage()
    {
        Sprite sp = SpritesManager.Instance.GetSprite(ID);
        img.rectTransform.sizeDelta = new Vector2(80, 120);
        img.sprite = sp;
        img.color = Color.white;

        BaseAtrribute ba2 = LoadObjctDateConfig.Instance.GetAtrribute(ID);
        transform.Find("Name").GetComponent<Text>().text = ba2.Name;

    }

    //合成此物品需要的东西
    private void SetNeedItems()
    {
        if (LoadObjctDateConfig.Instance.needs.ContainsKey(ID))
        {
            NeedIds = LoadObjctDateConfig.Instance.needs[ID];
        }
    }
}
