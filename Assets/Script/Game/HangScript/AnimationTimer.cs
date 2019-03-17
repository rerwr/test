using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using Game;

public class AnimationTimer : MonoBehaviour {

	private float time;
	public string farmeNo;
	public float Max_time = 4;

    private UnityEngine.Animator animator;
    // Use this for initialization
    void Start ()
	{
	     animator= GetComponent<Animator>();
        
	}

	void Update()
	{
		time += Time.deltaTime * 1;
//		print ("时间:"+time)
		if (time > Max_time)
		{
		    Destroy(this.gameObject);

           
        }
	    if (animator&&animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
	    {
	        Destroy(this.gameObject);

	    }
    }
}
