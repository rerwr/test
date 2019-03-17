using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class SignSucessView : BaseSubView
    {
        private Button CloseBtn;
        private Image seedIcon;
        private Text Text;
        private Text Num;
        public SignSucessView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
          

        }

        public override void BuildUIContent()
        {
            base.BuildUIContent();
            CloseBtn = TargetGo.transform.Find("closeBtn").GetComponent<Button>();
            seedIcon = TargetGo.transform.Find("seedBG/seedIcon").GetComponent<Image>();
            Text = TargetGo.transform.Find("seedBG/Text").GetComponent<Text>();
            Num = TargetGo.transform.Find("seedBG/Num").GetComponent<Text>();

            CloseBtn.onClick.AddListener(CloseSignSuccessBtn);
           GlobalDispatcher.Instance.AddListener(GlobalEvent.OnStoreUnitsChange, LoadData);

            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "add", "test1"));
        }

        public override void OnOpen()
        {
            GlobalDispatcher.Instance.AddListener(GlobalEvent.OnStoreUnitsChange, LoadData);
            base.OnOpen();
        }

        public override void OnClose()
        {
            base.OnClose();
            GlobalDispatcher.Instance.RemoveListener(GlobalEvent.OnStoreUnitsChange, LoadData);


        }

        private bool LoadData(int id, object arg)
        {
            if (arg==null)
            {
                return false;
            }
            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "test", "test1"));
            
            List<DeltaStoreUnit> DeltaStoreUnits = arg as List<DeltaStoreUnit>;
            int ID = DeltaStoreUnits[0].Id;
            int deltaNum = DeltaStoreUnits[0].Deltacount1;
            Sprite sp = SpritesManager.Instance.GetSprite(ID);
            seedIcon.sprite = sp;
            seedIcon.rectTransform.sizeDelta = new Vector2(75, 75);
            Num.text ="×"+ deltaNum.ToString();
            Text.text = LoadObjctDateConfig.Instance.GetAtrribute(ID).Name;
            
            Show();
            return false;
        }

        public void Show()
        {
            TargetGo.transform.gameObject.SetActive(true);

        }


        private void CloseSignSuccessBtn()
        {
            this.TargetGo.SetActive(false);
        }

    }
}
