using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    class PlowView :BaseSubView
    {
        private Button BtnPlow;
        private Button BtnClose;
        private Text t;

        public PlowView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {

        }

        public override void BuildUIContent()
        {
            t=TargetGo.transform.Find("PlowView/Plow/CoinNum").GetComponent<Text>();
            BtnClose = TargetGo.transform.Find("PlowView/Plow/CloseBtn").GetComponent<Button>();
            BtnPlow = TargetGo.transform.Find("PlowView/Plow/PlowBtn").GetComponent<Button>();
            BtnClose.onClick.AddListener(Close);
            BtnPlow.onClick.AddListener(Onclick);
            base.BuildUIContent();
        }

        private void Close()
        {
            ViewMgr.Instance.Close(ViewNames.PlowNeedView);
        }

        public void CloseView()
        {

            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "close", "plowview"));

            TargetGo.transform.Find("PlowView").gameObject.SetActive(false);
        }
        private void Onclick()
        {
             FieldsController.Instance.SendReclaimReq(Brand.Instance.SelectId);
        }
    }
}
