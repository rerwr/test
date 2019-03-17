using UnityEngine;
using System.Collections;
using Framework;
using UnityEngine.UI;

public class SliderControl : SingletonMonoBehaviour<SliderControl>
{

    public Scrollbar m_Scrollbar;
    public ScrollRect m_ScrollRect;

    private float mTargetValue;

    public bool mNeedMove = false;

    private const float MOVE_SPEED = 1F;

    private const float SMOOTH_TIME = 0.2F;

    private float mMoveSpeed = 0f;

    public void OnPointerDown()
    {
        mNeedMove = false;

        Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "pointdown", "test1"));

    }

    public void OnPointerUp()
    {
        // 判断当前位于哪个区间，设置自动滑动至的位置
//        if (m_Scrollbar.value <= 0.125f)
//        {
//            mTargetValue = 0;
//        }
//        else if (m_Scrollbar.value <= 0.375f)
//        {
//            mTargetValue = 0.25f;
//        }
//        else if (m_Scrollbar.value <= 0.625f)
//        {
//            mTargetValue = 0.5f;
//        }
//        else if (m_Scrollbar.value <= 0.875f)
//        {
//            mTargetValue = 0.75f;
//        }
//        else
//        {
//            mTargetValue = 1f;
//        }



        if (m_Scrollbar.value >= 0.125f&&m_Scrollbar.value<0.5f)
        {
            mTargetValue =1;
        }
        if (m_Scrollbar.value <= 0.875f && m_Scrollbar.value > 0.5f)
         
        {
            mTargetValue = 0;
        }
//        
        Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", m_Scrollbar.value, "test1"));

        mNeedMove = true;
        mMoveSpeed = 0;
    }

 

    void Update()
    {
        if (mNeedMove)
        {
            if (Mathf.Abs(m_Scrollbar.value - mTargetValue) < 0.01f)
            {
                m_Scrollbar.value = mTargetValue;
                mNeedMove = false;
                return;
            }
            m_Scrollbar.value = Mathf.SmoothDamp(m_Scrollbar.value, mTargetValue, ref mMoveSpeed, SMOOTH_TIME);
        }
    }

}