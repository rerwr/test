using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    class ChangeSuccView:BaseSubView
    {
        private Button btn;
        private Text text;
        public ChangeSuccView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
        }

        public override void BuildUIContent()
        {
            base.BuildUIContent();
            btn=TargetGo.transform.Find("ChangeSuccView").GetComponent<Button>();
            text=TargetGo.transform.Find("ChangeSuccView/Text").GetComponent<Text>();
        }
    }
}
