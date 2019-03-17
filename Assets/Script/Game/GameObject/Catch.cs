using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game
{
    public class Catch:WorldObject
    {
        private int fieldID;

        public int FieldId
        {
            get { return fieldID; }
            set { fieldID = value; }
        }

        ~ Catch()
        {

            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "销毁一个收获对象 ", "test1"));

        }

        
        public override void OnClicked(WorldObject worldObject)
        {
            base.OnClicked(worldObject);
            Debug.Log("------test------");
          
            if (FriendFarmManager.Instance.isVisiting == false)
            {

                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "FieldId", FieldId));

                FieldsController.Instance.SendPluckReq(this.FieldId, LoginModel.Instance.VisitingId);
            }
            else
            {
                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "FieldId", FieldId));

                FieldsController.Instance.SendPluckReq(FieldId, FriendFarmManager.Instance.FriendUid);
            }
        }

        public override void OnTouchEnd(WorldObject worldObject)
        {
            base.OnTouchEnd(worldObject);
        }

        public override void OnTouchBegin(WorldObject worldObject)
        {
            base.OnTouchBegin(worldObject);

        }
    }
}
