using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PaySuccessView: BaseSubView
    {
        private Button closeBtn;
        private bool isSucc;
        public PaySuccessView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
        }
        public override void BuildUIContent()
        {
            base.BuildUIContent();
           
            closeBtn = TargetGo.transform.Find("CloseBtn").GetComponent<Button>();
            
            closeBtn.onClick.AddListener(ClosePayPanel);

        }

        public override void OnOpen()
        {
           

            GlobalDispatcher.Instance.AddListener(GlobalEvent.OnCommitSucc,Succ);
            GlobalDispatcher.Instance.AddListener(GlobalEvent.OnStoreUnitsChange, SuccRewad);

            base.OnOpen();
        }

        public override void OnClose()
        {
            GlobalDispatcher.Instance.RemoveListener(GlobalEvent.OnCommitSucc, Succ);
            GlobalDispatcher.Instance.RemoveListener(GlobalEvent.OnStoreUnitsChange, SuccRewad);

            base.OnClose();
        }

        private bool SuccRewad(int id, object arg)
        {
            if (arg!=null)
            {
                List<DeltaStoreUnit> a = arg as List<DeltaStoreUnit>;

                if (a.Count<=3)
                {
                    ViewMgr.Instance.Close(ViewNames.PayChooseView);
                    SystemMsgView.SystemFunction(Function.GetDialog, a);
                }
            

            }
            return false;
        }

        private bool Succ(int id,object arg)
        {
            isSucc = true;
            return false;
        }
        private void ClosePayPanel()
        {
            ViewMgr.Instance.Close(ViewNames.PayChooseView);
        }
    }
}
