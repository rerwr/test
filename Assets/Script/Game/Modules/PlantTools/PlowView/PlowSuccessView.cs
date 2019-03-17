using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PlowSuccessView:BaseSubView
    {
        private Button btn;
        public PlowSuccessView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
            
        }

        public override void BuildUIContent()
        {
            base.BuildUIContent();
            btn=TargetGo.transform.Find("PlowSuccessView/CloseBtn").GetComponent<Button>();
            btn.onClick.AddListener((() => ViewMgr.Instance.Close(ViewNames.PlowNeedView)));
        }

        public void ShowView()
        {
            TargetGo.transform.Find("PlowSuccessView").gameObject.SetActive(true);
        }
    }
}
