using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework;

namespace Game
{
    public class UpdateView : BaseSubView
    {
        private Slider slider;
        private Text loading_Text;
        private Text speed_Text;

        private Transform Info;
        private Button UpdateBtn;
        private Button CancelBtn;

        public UpdateView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
        }

        public override void BuildUIContent()
        {
            base.BuildUIContent();

            slider = TargetGo.transform.Find("Loading/Slider").GetComponent<Slider>();
            slider.maxValue = 100;
            slider.minValue = 0;
            loading_Text= TargetGo.transform.Find("Loading/Text").GetComponent<Text>();
            Info = TargetGo.transform.Find("Info");
            UpdateBtn= TargetGo.transform.Find("Info/UpdateBtn").GetComponent<Button>();
            UpdateBtn.onClick.AddListener(OnClickUpdateBtn);
            CancelBtn = TargetGo.transform.Find("Info/CancelBtn").GetComponent<Button>();
            CancelBtn.onClick.AddListener(OnClickCancelBtn);
            speed_Text=TargetGo.transform.Find("Loading/Speed").GetComponent<Text>();
        }

        public override void OnOpen()
        {
            base.OnOpen();
            GlobalDispatcher.Instance.AddListener(GlobalEvent.OnLoadingApk, OnLoading);
        }

        private void OnClickUpdateBtn()
        {
#if UNITY_ANDROID
            VersionUpdateManager.Instance.StartUpdate();
#endif
#if UNITY_IOS
            Application.OpenURL(VersionUpdateManager.Instance.UpdateUrl);
#endif
            Info.gameObject.SetActive(false);
            //Application.OpenURL(VersionUpdateManager.Instance.UpdateUrl);
        }

        private void OnClickCancelBtn()
        {
            GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnUpdateEnd);
            ViewMgr.Instance.Close(ViewNames.UpdateView);
        }
        

        private bool OnLoading(int eventId,object arg)
        {
            int loading = (int)arg;
            speed_Text.text = loading+"%";
            slider.value = loading;
            if(loading>=100)
            {
                loading_Text.text = "下载完成，等待更新！";
            }
            return false;
        }

        public override void OnClose()
        {
            base.OnClose();
            GlobalDispatcher.Instance.RemoveListener(GlobalEvent.OnLoadingApk, OnLoading);
        }
    }
}