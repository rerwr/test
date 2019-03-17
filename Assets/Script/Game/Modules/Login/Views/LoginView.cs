 using System;
using System.Collections;
using System.Collections.Generic;


using DG.Tweening;
using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class LoginView : BaseSubView
    {
      
        public  GameObject EnterGameView;
        public  GameObject RegisterView;
        public  GameObject ChangeSuccView;
        public  GameObject ChangePWView;


        public static GameObject BG;
        public GameObject LeftUperLogo;
        public  RegisterView resRegisterView;
        public   EnterGameView egv;

        public override void BuildSubViews()
        {
            subViews = new List<BaseSubView>();
            BG= TargetGo.transform.Find("BG").gameObject;
            LeftUperLogo = TargetGo.transform.Find("LeftUperLogo").gameObject;

            EnterGameView = TargetGo.transform.Find("EnterGameView").gameObject;
            egv= new EnterGameView(EnterGameView, ViewController);
            subViews.Add(egv);

            RegisterView = TargetGo.transform.Find("RegisterView").gameObject;
            resRegisterView=new RegisterView(RegisterView, ViewController);
            subViews.Add(resRegisterView);

            ChangePWView = TargetGo.transform.Find("ChangePWView").gameObject;
            subViews.Add(new ChangePWView(ChangePWView, ViewController));
            base.BuildSubViews();
        }

      
       

        public LoginView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
        }
    }
}
