using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class DeviceCameraController : MonoBehaviour {

	public enum CameraMode
	{
		FACE_C,
		DEFAULT_C,
		NONE
	}
	[HideInInspector]
	public WebCamTexture cameraTexture;
    private CameraPlaneController camCtrl;

	private bool isPlay = false;
	//public CameraMode e_CameraMode;
	private GameObject e_CameraPlaneObj;
	int matIndex = 0;

	ScreenOrientation orientation;
	public bool isPlaying
	{
		get{
			return isPlay;
		}
	}
	// Use this for initialization  
	void Awake()  
	{  
		StartCoroutine(CamCon());  
		e_CameraPlaneObj = transform.Find ("CameraPlane").gameObject;
        if (camCtrl == null)
            camCtrl = GetComponent<CameraPlaneController>();
	}
	
	// Update is called once per frame  
	void Update()  
	{  
		if (isPlay) {  
			if(e_CameraPlaneObj.activeSelf)
			{
				e_CameraPlaneObj.GetComponent<Renderer>().material.mainTexture = cameraTexture;
			}

		}
	    if(cameraTexture != null && camCtrl!= null)
        {
            Vector2 size = new Vector2(cameraTexture.width, cameraTexture.height);
            if(size != camCtrl.WebCamSize && size != Vector2.zero && size.x != 16)
            {
                camCtrl.WebCamSize = size;
            }
        }
	}

	IEnumerator CamCon()  
	{  
		yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);  
		if (Application.HasUserAuthorization(UserAuthorization.WebCam))  
		{  
			#if UNITY_EDITOR 
			cameraTexture = new WebCamTexture();  	
			#elif UNITY_IOS
			if(Mathf.Min(Screen.width,Screen.height)>1000)
			{
				cameraTexture = new WebCamTexture(Screen.width/2,Screen.height/2);  
			}
			else
			{
				cameraTexture = new WebCamTexture();  
			}
			#elif UNITY_ANDROID
			cameraTexture = new WebCamTexture();  
			#else
			cameraTexture = new WebCamTexture();  
			#endif
			cameraTexture.Play();
			isPlay = true;  
		}  
	}


	public void StopWork()
	{
		this.cameraTexture.Stop();
	}

}

