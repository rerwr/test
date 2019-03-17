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
        //进入拍照，或选择图片
        private void OnHeadClcik()
        {
            //存入本地
            m_Native.SettingAvaterFormMobile("Main Camera", "OnAvaterCallBack", GetUIDPath(LoginModel.Instance.Uid).GetHashCode().ToString());

        }
        public override void OnOpen()
        {
            base.OnOpen();
            NewPlayerCreateController.Instance.GetDispatcher().AddListener(CreateEvent.OnCreateSucc, Callback);

            //进入拍照，或选择图片后回调
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
        //照片回调
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
        /// 将图片上传到oos服务器
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
                //删除本地
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
        /// 传上服务器并且删除本地
        /// </summary>
        /// <returns></returns>
        IEnumerator wait2LoadIcon()
        {
            yield return 0.3f;
            var a = NewPlayerCreateController.Instance;
            byte[] t2d = headIcon.sprite.texture.EncodeToPNG();
            //上传
            PutObjectSample.PutObjectFromString(GetUIDPath(LoginModel.Instance.Uid), t2d);
            //请求改变头像
            a.CreateNewUserReq(name.text, int.Parse(LoginModel.Instance.Sex), GetUIDPath(LoginModel.Instance.Uid));
            //删除本地头像，防止asyncimage认为本地还有
            GlobalDispatcher.Instance.Dispatch(GlobalEvent.onPlayerPanelReflash);

            ViewMgr.Instance.Close(ViewNames.PlayerInfoView);
        }
        /// <summary>
        /// 获得md5图片名
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        private string GetUIDPath(int uid)
        {
            return EncryptManager.GetMD5HashFromString(uid.ToString())+".png";
        }
    }
}