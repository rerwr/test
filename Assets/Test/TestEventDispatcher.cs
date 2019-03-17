using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using UnityEngine.UI;

public class TestEventDispatcher : MonoBehaviour
{
    public static EventDispatcher disp;
    public int num;
    public Button btn;
	// Use this for initialization
	void Start ()
    {
	    if (disp == null)
	    {
	        disp = new EventDispatcher(typeof(TestEvent));
	    }
	    btn = GetComponent<Button>();
		btn.onClick.AddListener(OnClick);
        disp.AddListener(TestEvent.OnUserClick1,Listener1);
        disp.AddListener(TestEvent.OnUserClick2,Listener2);
        Debug.Log(TestEvent.OnUserClick1+"#"+TestEvent.OnUserClick2);
    }

    private void OnClick()
    {
        if (num == 2)
        {
            disp.Dispatch(TestEvent.OnUserClick2,null);
        }
        else if(num == 1)
        {
            disp.Dispatch(TestEvent.OnUserClick1,null);
            
        }
    }

    private bool Listener1(int id, object o)
    {
        Debug.Log("Listener:1,id:" + id + "@" + this.GetInstanceID());
        //disp.Dispatch(id);
        //throw  new UnityException("salfjaiosd");
        return false;
    }
    private bool Listener2(int id, object o)
    {
        Debug.Log("Listener:2,id:" + id + "@" + this.GetInstanceID());
        return true;
    }

    class TestEvent :EventDispatcher.BaseEvent
    {
        public static int OnUserClick1 = ++id;
        public static int OnUserClick2 = ++id;
    }
}
