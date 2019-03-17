using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;

public class LoadingImageManager : SingletonMonoBehaviour<LoadingImageManager> {
    
    private int LoadingCount=0;
    private  Transform tr;
    private Transform loadingTr; //正在加载的模块，加载中隐藏，加载完成后显示
    private float Speed = 270.0f;//旋转速度
    private float MaxLoadTime=1.5f;//最大加载时间
    private Vector3 centerpos;
    
    void Start()
    {
       
        tr = GetComponent<Transform>();
       centerpos= tr.position;
        ShowOrHide(false);
    }

    private void ShowOrHide(bool isShow)
    {
        if (tr.gameObject)
        {
            tr.gameObject.SetActive(isShow);
        

        }
    }
    private void End()
    {
        //loadingTr.gameObject.SetActive(true);
        ShowOrHide(false);
    }

    //开始加载
    public void StartLoading(Transform t, Vector2 pos)
    {
        LoadingCount = 1;
        loadingTr = t;
        //loadingTr.gameObject.SetActive(false);
        tr.position = pos;
        if (LoadingCount > 0)
        {
            ShowOrHide(true);
        }
        StartCoroutine(LoadingTime());

    }
    //结束加载
    public void StopLoading()
    {
        LoadingCount --;

        if (LoadingCount == 0)
        {
            End();
        }

    }
    public void Ending()
    {

        LoadingCount = 0;
       
            End();
        

    }
    //注册加载物体
    public void AddLoadingItem()
    {
        LoadingCount++;
        if (LoadingCount > 0)
        {
            ShowOrHide(true);
            transform.position = centerpos;
        }
        MTRunner.Instance.StartRunner(LoadingTime());

    }

    //移除完成加载的物体
    public void ReduceLoadingItem()
    {
        LoadingCount--;

        if (LoadingCount == 0)
        {
            End();
        }
    }

  
    void FixedUpdate()
    {
        if (LoadingCount > 0)
        {
            tr.Rotate(new Vector3(0, 0, -10), Speed*Time.deltaTime);
        }
    }

    IEnumerator LoadingTime()
    {
        yield return 1.7f;
        if (LoadingCount > 0)   //到达最大加载时间还没加载完
        {
//            Debug.LogError("加载物体超时");
            ReduceLoadingItem();
        }
    }
}
