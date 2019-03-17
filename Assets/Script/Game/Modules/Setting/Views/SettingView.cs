using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public class SettingView:BaseSubView
    {
        private Button CloseBtn;
        private Button LogoutBtn;
        private Button Audio_On;
        private Button Audio_Off;

        private Slider Volum_Slider;

        public SettingView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {

        }

        public override void BuildUIContent()
        {
            base.BuildUIContent();
            CloseBtn=TargetGo.transform.Find("BG/CloseBtn").GetComponent<Button>();
            CloseBtn.onClick.AddListener(CloseView);
            LogoutBtn = TargetGo.transform.Find("BG/LogoutBtn").GetComponent<Button>();
            LogoutBtn.onClick.AddListener(OnClickLogout);

            Audio_On = TargetGo.transform.Find("BG/Audio/GameObject/Btn").GetComponent<Button>();
            Audio_On.onClick.AddListener(delegate() { this.OnClickAudioToggle(true);});
            Audio_Off = TargetGo.transform.Find("BG/Audio/GameObject/Btn (1)").GetComponent<Button>();
            Audio_Off.onClick.AddListener(delegate () { this.OnClickAudioToggle(false); });

            Volum_Slider = TargetGo.transform.Find("BG/Volum/Slider").GetComponent<Slider>();
            Volum_Slider.minValue = 0;
            Volum_Slider.maxValue = 1.0f;
            Volum_Slider.onValueChanged.AddListener(OnChangeVolumn);
            if (PlayerSave.HasKey("ismute"))
            {
                if (PlayerSave.GetInt("ismute")==0)
                {
                    Audio_On.gameObject.SetActive(false);
                    Audio_Off.gameObject.SetActive(true);
                    MusicManager.Instance.IsMute = false;
                }
                else
                {
                    Audio_On.gameObject.SetActive(true);
                    Audio_Off.gameObject.SetActive(false);
                
                    MusicManager.Instance.IsMute = true;
                }
            }
        }

        //点击登录退出按钮
        private void OnClickLogout()
        {
            if (!string.IsNullOrEmpty(LoginModel.Instance.Account))
                PlayerSave.SetString("Account", LoginModel.Instance.Account);
            if(!string.IsNullOrEmpty(LoginModel.Instance.Password))
                PlayerSave.SetString("Password",LoginModel.Instance.Password);
            
            SocketMgr.Instance._isneed2loginview = false;
            SocketMgr.Instance.Disconnect();
            FriendsInfoModel.Instance.DestroyAllFriends();
            ViewMgr.Instance.CloseAllview();
//            ViewMgr.Instance.Close(ViewNames.SettingView);
//            ViewMgr.Instance.Close(ViewNames.SeedBarView);
            
            ViewMgr.Instance.Open(ViewNames.LoginView);

        }

        //调节音乐大小
        private void OnChangeVolumn(float volumn)
        {
            MusicManager.Instance.Volumn = volumn;
        }

        //点击音效开关
        private void OnClickAudioToggle(bool isOn)
        {
            MusicManager.Instance.IsMute = !isOn;
            PlayerSave.SetInt("ismute", MusicManager.Instance.IsMute?1:0);
         
        }

        public void CloseView()
        {
            ViewMgr.Instance.Close(ViewNames.SettingView);
        }
    }
}
