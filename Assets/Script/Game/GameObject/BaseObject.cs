using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class BaseObject
    {
        private string _name ;               //道具名称
        private int _id ;  		//道具ID
        private int _objectNum ;	//数量
        private int _price ;          //对应类型的价值。


        private string url ;               //道具资源对应的url，平时可为空，当需要更改图片时，设置图片地址
        private int _shopTag ;			//道具商城显示分类标签道具的商场的显示分类标签。1种子，2肥料，3狗粮,
        private int _storeShowTag ;		//道具的仓库显示分类标签。1种子类2果实类，3精油类，4肥料类

        private string des ;               //道具功能描述
        private SpriteRenderer renderer;
        private GameObject go;

      
        #region MyRegion
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public override string ToString()
        {
            return string.Format("Name: {0}, ID: {1}, ObjectNum: {2}, Price: {3}, Url: {4}, ShopTag: {5}, StoreShowTag: {6}, Des: {7}, Renderer: {8}, Go: {9}", Name, ID, ObjectNum, Price, Url, ShopTag, StoreShowTag, Des, Renderer, Go);
        }

    
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public int ObjectNum
        {
            get { return _objectNum; }
            set
            {
                //数量变化TODO
                _objectNum = value;
                
            }
        }

        public int Price
        {
            get { return _price; }
            set { _price = value; }
        }

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public int ShopTag
        {
            get { return _shopTag; }
            set { _shopTag = value; }
        }

        public int StoreShowTag
        {
            get { return _storeShowTag; }
            set { _storeShowTag = value; }
        }

        public string Des
        {
            get { return des; }
            set { des = value; }
        }

      

        public SpriteRenderer Renderer
        {
            get { return renderer; }
            set
            {
                renderer = value;
                if (value)
                {
                     Go = value.gameObject;

                }
            }
        }

        public GameObject Go
        {
            get { return go; }
            set { go = value; }
        }

        #endregion


    }
}
