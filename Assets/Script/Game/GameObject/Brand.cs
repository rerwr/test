using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DG.Tweening;
using Framework;
using UnityEngine;

namespace Game
{
    public  class Brand : WorldObject
    {
        GameObject brand;
        private  int selectID;
        private static Brand instance;
        public static Brand Instance
        {
            get
            {
                if (instance==null)
                {
                    instance = new Brand();
                }
                return instance;
            }
        }
        ~ Brand()
        {

            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", "销毁一个牌子", "test1"));

        }
        public Brand()
        {

//            Debug.LogError(string.Format("<color=#ff0000ff><---{0}-{1}----></color>", "创建一个版", "test1"));

        }
        public  int SelectId
        {
            get { return selectID; }
            set
            {
                if (value!=selectID)
                {

                    LoadBrand(value);
                }
                selectID = value;
            }
        }

        //加载牌子
        private void LoadBrand(int Current)
        {
            
            if (!brand&&FieldsModel.Instance.farms.ContainsKey(Current))
            {
                
                ResourceMgr.Instance.LoadResource("Prefab/brand", (
                    delegate (Resource resource, bool b)
                    {
                        if (FieldsModel.Instance.farms.ContainsKey(Current))
                        {
                            //变化的选择ID,田从1开始数
                            FarmUnit unit = FieldsModel.Instance.farms[Current] as FarmUnit;
                            GameObject go = GameObject.Instantiate(resource.UnityObj as GameObject);
                            
                            go.transform.SetParent(unit.Renderer.transform, false);

                            go.transform.position = new Vector3(unit.Renderer.transform.position.x, unit.Renderer.transform.position.y, 1);
                            //放入下一个地皮ID的brand
                            unit.Brand = this;
                            unit.Brand.Go = go;
                            this.brand = go;

                        }


                    }));
            }
            else
            {
                //上一个值的农田牌子设置为空
                FarmUnit unit = FieldsModel.Instance.farms[Current-1];

                unit.Brand = null;

                if (FieldsModel.Instance.farms.ContainsKey(Current))
                {
                    FarmUnit unit1 = FieldsModel.Instance.farms[Current];
                    unit1.Brand = this;
                    Go.transform.position = unit1.Renderer.gameObject.transform.position;
                    Go.transform.SetParent(unit1.Renderer.transform, true);

                    Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", Current, SelectId));

                }
                else
                {
                    if(Go!=null)GameObject.Destroy(Go);
//                    Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", Current, "test1"));

                }

            }

        }
        public override void OnTouchEnd(WorldObject worldObject)
        {
            
            //            tColor.PlayBackwards();
        }

        public override void OnTouchBegin(WorldObject worldObject)
        {
            if (!FriendFarmManager.Instance.isVisiting)
            {
                DotweenManager.BeginTouchBranAnim(Go);
                ViewMgr.Instance.Open(ViewNames.PlowNeedView);
            }
        }
        public override void OnClicked(WorldObject worldObject)
        {
            
        }

       
    }
}
