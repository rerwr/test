using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace Game
{
    public class LoginViewController : BaseViewController
    {
        public override void Build()
        {
            Viewlist = new List<BaseSubView>();
            Viewlist.Add(new LoginView(MainGO, this));
            base.Build();
            
        }
   
    }
}
