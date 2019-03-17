using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;
using UnityEngine;

namespace Game
{
    public class Dog:WorldObject
    {

        
        ~ Dog()
        {

            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "销毁狗", "test1"));

        }
        public override void OnTouchBegin(WorldObject worldObject)
        {
            if (!FriendFarmManager.Instance.isVisiting)
            {
                ViewMgr.Instance.Open(ViewNames.DogInfoView);
                MusicManager.Instance.Playsfx(AudioNames.dog);
            }
        }

    }
}
