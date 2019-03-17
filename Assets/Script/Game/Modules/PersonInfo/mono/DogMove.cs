using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DragonBones;
using Framework;
using Game;
using UnityEngine;
public enum DogState
{
    Sleep,
    Move,
    Catch
}
public class DogMove : SingletonMonoBehaviour<DogMove>
{
    private DogState dogState;

    private UnityArmatureComponent uac;
    Tweener t;
    GameObject dogSleep;
    GameObject dogCatch;
    private bool isCatch=false;
    public DogState State
    {
        get { return dogState; }
        set
        {
            if (value==dogState)
            {
                return;
            }
            switch (value)
            {
                case DogState.Sleep:
                    GetComponent<MeshRenderer>().enabled = false;
                    dogCatch.SetActive(false);
                    dogSleep.SetActive(true);
                    break;
                case DogState.Move:
                    GetComponent<MeshRenderer>().enabled = true;
                    dogCatch.SetActive(false);
                    dogSleep.SetActive(false);
                    break;
                case DogState.Catch:
                
                    GetComponent<MeshRenderer>().enabled = false;
                    dogCatch.SetActive(true);
                    dogSleep.SetActive(false);
                    break;
            }
            dogState = value;
            
     
        }
    }


   

  
    // Use this for initialization
    void Start()
    {
        uac = GetComponent<UnityArmatureComponent>();
        dogSleep = transform.Find("dog_sleep").gameObject;
        dogCatch = transform.Find("dog_catch").gameObject;
        //        uac.flipX = true;
        t = transform.DOMove(new Vector3(2, 0.12f, 1), 6).SetAutoKill(false).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        Tweener rotate = transform.DORotate(new Vector3(0, 0, 0), 0.01f).SetAutoKill(false);
        State = DogState.Move;

        t.onStepComplete += () =>
        {
            if (rotate.IsBackwards())
            {
                rotate.PlayForward();
            }
            else
            {
                rotate.PlayBackwards();
            }
        };
        t.PlayForward();
        GlobalDispatcher.Instance.AddListener(GlobalEvent.OnDogCatch, OnEventDispach);
//        MTRunner.Instance.StartRunner(Wait(5));
    }
    private bool OnEventDispach(int eventid, object arg)
    {
        isCatch = true;

        Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "触发狗抓", "test1"));

        return false;
    }

    IEnumerator Wait(float time)
    {
        yield return time;
        GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnDogCatch);

    }
    float time = 0;
    // Update is called once per frame
    void Update()
    {

        time += Time.deltaTime;
        if (!isCatch)
        {
            if (time >= Random.Range(1, 3) * 5)
            {
                State = DogState.Sleep;
                t.Pause();

                if (time > Random.Range(1, 3) * 15)
                {
                    t.PlayForward();
                    State = DogState.Move;
                    time = 0;
                }
            }
        }
        else
        {
            State = DogState.Catch;
            t.Pause();
            if (time>Random.Range(3,4))
            {
                isCatch = false;
                time = 0;
            }
        }
        

    }

    
}
