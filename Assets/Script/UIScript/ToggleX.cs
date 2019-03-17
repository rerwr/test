using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class ToggleX : Toggle
{
    private Animator _animator;
    private Toggle _tog;
    public int ToggleId;
    public Image IconImage;
    public  ToggleXEvent OnTogXSelected=new ToggleXEvent();

    void Awake()
    {
        ToggleId = transform.GetSiblingIndex();
        _animator = GetComponent<Animator>();
        _tog = GetComponent<Toggle>();
        _tog.onValueChanged.AddListener(OnTogChanged);
    }

    public void Init(int Id)
    {
        ToggleId = Id;
    }

    private void OnTogChanged(bool arg0)
    {
        _animator.Play(_tog.isOn?"Highlighted":"Normal");
        if(_tog.isOn) OnTogXSelected.Invoke(ToggleId);
    }

    private bool _pointIn = false;
    public override void OnPointerDown(PointerEventData eventData)
    {
        _animator.Play("Pressed");
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (_pointIn)
        {
            _animator.Play("Highlighted");
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        _pointIn = true; _animator.Play("Highlighted");
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        _pointIn = false;
        if (!_tog.isOn)
        {
            _animator.Play("Normal");
        }
    }

    public void FadeIn()
    {
        _animator.Play("button_fadeIn");
    }
   
    public class ToggleXEvent : UnityEvent<int>
    {
        
    }

    
}
