using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Aliyun.OSS.Samples;
using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PlayerInfoView:BaseSubView
    {
        private Button closebtn;
        private Button saveBtn;
        //private Button head;
        private Image headIcon;
        private Button headIconbtn;
        private InputField name;
        private Text acount;
        private Text code;
        private NativeHandle m_Native;
        private  int i = 0;
        public PlayerInfoView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
            
        }

        public override void BuildUIContent()
        {
            base.BuildUIContent();
            m_Native = new NativeHandle();

            closebtn = TargetGo.transform.Find("CloseBtn").GetComponent<Button>();
            closebtn.onClick.AddListener(OnClickCloseBtn);
            saveBtn = TargetGo.transform.Find("SaveBtn").GetComponent<Button>();
            saveBtn.onClick.AddListener(OnClickSaveBtn);

            headIcon = TargetGo.transform.Find("HeadBG/BG/Head").GetComponent<Image>();
            headIconbtn = TargetGo.transform.Find("HeadBG/BG/Head").GetComponent<Button>();
            name = TargetGo.transform.Find("Info/NameF").GetComponent<InputField>();
            acount= TargetGo.transform.Find("Info/AcountF/Text").GetComponent<Text>();
            code = TargetGo.transform.Find("Info/CodeF/Text").GetComponent<Text>();
            headIconbtn.onClick.AddListener(OnHeadClcik);
        }
        //�������գ���ѡ��ͼƬ
        private void OnHeadClcik()
        {
            //���뱾��
            m_Native.SettingAvaterFormMobile("Main Camera", "OnAvaterCallBack", GetUIDPath(LoginModel.Instance.Uid).GetHashCode().ToString());

        }
        public override void OnOpen()
        {
            base.OnOpen();
            NewPlayerCreateController.Instance.GetDispatcher().AddListener(CreateEvent.OnCreateSucc, Callback);

            //�������գ���ѡ��ͼƬ��ص�
            GlobalDispatcher.Instance.AddListener(GlobalEvent.OnAvatarEnd, Avatrcallback);

            Callback(1,0);
            if (GameStarter.Instance.isfirstLogin)
            {
                closebtn.gameObject.SetActive(false);
                GameStarter.Instance.isfirstLogin = false;
            }

        }

        public override void OnClose()
        {
            base.OnClose();
            NewPlayerCreateController.Instance.GetDispatcher().RemoveListener(CreateEvent.OnCreateSucc, Callback);

        }

        bool Avatrcallback(int id, object arg)
        {
            AsyncImageDownload.Instance.SetAsyncImage(GetUIDPath(LoginModel.Instance.Uid), headIcon);
            LoginModel.Instance.Head = EncryptManager.GetMD5HashFromString(LoginModel.Instance.Uid.ToString()) + ".png";
            GlobalDispatcher.Instance.DispathDelay(GlobalEvent.onPlayerPanelReflash);
            
            return false;
        }
        //��Ƭ�ص�
        bool Callback(int id,object arg)
        {
            name.text = LoginModel.Instance.Nickname;
            
            acount.text = LoginModel.Instance.Account;
            code.text = LoginModel.Instance.Code;
            AsyncImageDownload.Instance.SetAsyncImage(LoginModel.Instance.Head, headIcon,true);
            
            return false;
        }
     
        private void OnClickCloseBtn()
        {
            ViewMgr.Instance.Close(ViewNames.PlayerInfoView);

        }
        /// <summary>
        /// ��ͼƬ�ϴ���oos������
        /// </summary>
        private void OnClickSaveBtn()
        {
            try
            {
                if (headIcon.sprite.name == "headIcon_0")
                {
                    SystemMsgView.SystemFunction(Function.Tip, Info.Headhas);
                    return;
                }
                if (name.text == "")
                {
                    SystemMsgView.SystemFunction(Function.Tip, Info.namehas);
                    return;
                }
                //ɾ������
                AsyncImageDownload.Instance.Delete(LoginModel.Instance.Head);

                SystemMsgView.SystemFunction(Function.Tip, Info.Uploading);

                MTRunner.Instance.StartRunner(wait2LoadIcon());
            }
            catch (Exception e)
            {

                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", e.Message, "test1"));

            }
        }
        /// <summary>
        /// ���Ϸ���������ɾ������
        /// </summary>
        /// <returns></returns>
        IEnumerator wait2LoadIcon()
        {
            yield return 0.3f;
            var a = NewPlayerCreateController.Instance;
            byte[] t2d = headIcon.sprite.texture.EncodeToPNG();
            //�ϴ�
            PutObjectSample.PutObjectFromString(GetUIDPath(LoginModel.Instance.Uid), t2d);
            //����ı�ͷ��
            a.CreateNewUserReq(name.text, int.Parse(LoginModel.Instance.Sex), GetUIDPath(LoginModel.Instance.Uid));
            //ɾ������ͷ�񣬷�ֹasyncimage��Ϊ���ػ���
            GlobalDispatcher.Instance.Dispatch(GlobalEvent.onPlayerPanelReflash);

            ViewMgr.Instance.Close(ViewNames.PlayerInfoView);
        }
        /// <summary>
        /// ���md5ͼƬ��
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        private string GetUIDPath(int uid)
        {
            return EncryptManager.GetMD5HashFromString(uid.ToString())+".png";
        }
    }
}