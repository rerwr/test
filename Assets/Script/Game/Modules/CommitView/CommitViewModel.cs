using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ToggleAndObj
    {
        private Toggle t;
        private int id;
        private int num;

        public Toggle T
        {
            get { return t; }
            set { t = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int Num
        {
            get { return num; }
            set { num = value; }
        }

        public ToggleAndObj(int id, Toggle t, int num)
        {
            this.id = id;
            this.t = t;
            this.num = num;
        }
    }
    public class CommitViewModel:BaseModel<CommitViewModel>
    {
      

        private int _postage;//邮费
        private string OrderInfo;
        public List<ToggleAndObj> taos = new List<ToggleAndObj>();
        private int payType;
        private string name;
        private string address;
        private string beaty;
        private string phone;
        private string city;
        private string count;
        private string country;
        private string province;
        private int selectPinpai;
        public Dictionary<string, Brand> brands=new Dictionary<string, Brand>();
        public string OrderInfo1
        {
            get { return OrderInfo; }
            set { OrderInfo = value; }
        }

        public int PayType1
        {
            get { return payType; }
            set { payType = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string Beaty
        {
            get { return beaty; }
            set { beaty = value; }
        }

        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        public string City
        {
            get { return city; }
            set { city = value; }
        }

        public string Count
        {
            get { return count; }
            set { count = value; }
        }

        public string Country
        {
            get { return country; }
            set { country = value; }
        }

        public int Postage
        {
            get { return _postage; }
            set { _postage = value; }
        }

        public string  ordernum { get; set; }

        public int PayType
        {
            get { return payType; }
            set { payType = value; }
        }

        public string Province
        {
            get { return province; }
            set { province = value; }
        }

        public int SelectPinpai
        {
            get { return selectPinpai; }
            set { selectPinpai = value; }
        }

        public override void InitModel()
        {
            SetData();
        }
        private void SetData()
        {
           //string url = @"http://39.108.134.200:8080/api/gameExchangeBrandData";
            string url = @"http://119.23.48.181:8080/api/gameExchangeBrandData";
            string json = VersionUpdateManager.Instance.GetPage(url);
            GameDataResult r = JsonUtility.FromJson<GameDataResult>(json);
            if (r == null || r.result == null || r.result.Length == 0) return;
            for (int i = 0; i < r.result.Length; i++)
            {
                brands.Add(r.result[i].name,r.result[i]);

                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", r.result[i].ToString(), "test1"));

            }
        
        }


        [Serializable]
        public class GameDataResult
        {
            public Brand[] result;
        }

        [Serializable]
        public class Brand
        {
            public int id;
            public bool enabled;
            public string name;

            public override string ToString()
            {
                return string.Format("Id: {0}, Enable: {1}, Name: {2}", id, enabled, name);
            }
        }
    }
}