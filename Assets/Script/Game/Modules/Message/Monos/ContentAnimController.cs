using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using Framework;
using Game;
using System;

public class ContentAnimController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,
    IDragHandler,IBeginDragHandler,IEndDragHandler

{
    private ScrollRect scrollRect;
    private Button contentBtn;
    private float mindis = 20f;

    public float MinDistance = 80f; //判断拖动的最小距离
    private Vector2 startPos;
    private Vector2 endPos;
    //动画
    private Transform contentTr;
    public float MoveSpeed = 0.3f;
    private Tween tweenLeft;
    private Tween tweenRight;

    public void Init()
    {
        contentTr = transform.parent;
        scrollRect=contentTr.parent.parent.parent.GetComponent<ScrollRect>();
        contentBtn = transform.GetComponent<Button>();
    }



    public void OnPointerDown(PointerEventData eventData)
    {
        startPos = eventData.position;
        Debug.Log("点击进入");
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        endPos = eventData.position;
        if (endPos.x - startPos.x < -MinDistance)
        {
            tweenLeft = contentTr.DOLocalMoveX(-64, MoveSpeed);
            //Debug.Log("向左托");
        }
        else if (endPos.x - startPos.x > MinDistance)
        {
            tweenRight = contentTr.DOLocalMoveX(0, MoveSpeed);
            //Debug.Log("向右托");
        }
        else if(Mathf.Abs(endPos.y-startPos.y)< mindis)
        {
            contentBtn.onClick.Invoke();
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        ((IBeginDragHandler)scrollRect).OnBeginDrag(eventData);
        //Debug.Log("OnBeginDrag");
    }
    public void OnDrag(PointerEventData eventData)
    {
        ((IDragHandler)scrollRect).OnDrag(eventData);
        //Debug.Log("OnDrag");
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        ((IEndDragHandler)scrollRect).OnEndDrag(eventData);
        //Debug.Log("OnEndDrag");
    }
}
