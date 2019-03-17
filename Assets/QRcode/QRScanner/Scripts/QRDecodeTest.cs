using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Framework;
using Game;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class QRDecodeTest : MonoBehaviour {

	public QRCodeDecodeController e_qrController;

	public Text UiText;
	public GameObject resetBtn;
	public GameObject scanLineObj;
    public GameObject Camera;
    public Button Back;

    private List<DeltaStoreUnit> gets;
    // Use this for initialization
    void Start () {

		if (e_qrController != null) {
			e_qrController.e_QRScanFinished += qrScanFinished;
		}
        Back.onClick.AddListener((() =>
        {
            BackMainScene();
        }));
        GlobalDispatcher.Instance.AddListener(GlobalEvent.OnStoreUnitsChange, LoadData);
        GlobalDispatcher.Instance.AddListener(GlobalEvent.onPlayerPanelReflash, LoadData);

	}
    bool LoadData(int id, object arg)
    {

        if (FieldsController.ProtocalAction == ProtocalAction.QRcode)
        {
            if (arg != null)
            {
                 gets= arg as List<DeltaStoreUnit>;
                
                MTRunner.Instance.StartRunner(Wait(1.5f));
            }

        }
        return false;
    }

    IEnumerator Wait(float time)
    {
        yield return time;
        SystemMsgView.SystemFunction(Function.GetDialog, gets);

    }

    private void BackMainScene()
    {
        Scene s = SceneManager.GetSceneAt(1);
        SceneManager.UnloadSceneAsync(s);
        Scene scene = SceneManager.GetSceneAt(0);

        for (int i = 0; i < scene.GetRootGameObjects().Length; i++)
        {
            if (scene.GetRootGameObjects()[i].name == "Panel2D" || scene.GetRootGameObjects()[i].name == "Canvas")
            {
                Transform t = scene.GetRootGameObjects()[i].transform;
                t.gameObject.SetActive(true);
            }
        }
    }

    public void SetChildActive(bool isAcive)
    {
        if (isAcive)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

	void qrScanFinished(string dataText)
	{
        LoadingImageManager.Instance.AddLoadingItem();
//		UiText.text = dataText;
		if (resetBtn != null) {
			resetBtn.SetActive(true);
		}

		if(scanLineObj != null)
		{
			scanLineObj.SetActive(false);
		}

        Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "扫描到的信息", dataText));
        //扫描成功后立马返回主场景
	    BackMainScene();

        CommonController.Instance.SendScanQRcodeOrRecommandReq(dataText,1);
        FieldsController.ProtocalAction=ProtocalAction.QRcode;
	   

	}

 


    /// <summary>
    /// reset the QRScanner Controller 
    /// </summary>
    public void Reset()
	{
		if (e_qrController != null) {
			e_qrController.Reset();
		}

		if (UiText != null) {
			UiText.text = "";	
		}
		
		if (resetBtn != null) {
			resetBtn.SetActive(false);
		}

		if(scanLineObj != null)
		{
			scanLineObj.SetActive(true);
		}
	}
	

}
