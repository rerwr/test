using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;
using Game;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{

    class CommitView : BaseSubView
    {
        private InputField inputName;
        private InputField inputPhone;
        private InputField inputAddress;
        private InputField inputBeaty;
        private Button commitBtn;
        private Button commitBtnCancel;
        private Button ques;

        private Dropdown Provincedropdown;
        private Dropdown CityDropDown;
        private Dropdown PinPaiDropDown;
        private Dropdown CountryDropDown;
        private GameObject ExchangeGrid;
        private Text Carriage;
        GameObject Exchangeitem;



        private Dictionary<string, Dictionary<string, List<string>>> regions;

        Dropdown.OptionData op1 = new Dropdown.OptionData();
        List<Dropdown.OptionData> ProvincelList = new List<Dropdown.OptionData>();
        List<Dropdown.OptionData> CitylList = new List<Dropdown.OptionData>();
        List<Dropdown.OptionData> CountryoplList = new List<Dropdown.OptionData>();
        List<ToggleAndObj> taos;
        public CommitView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {

        }

        public override void BuildSubViews()
        {
            inputName = TargetGo.transform.Find("InputFieldName").GetComponent<InputField>();
            inputPhone = TargetGo.transform.Find("InputFieldPhone").GetComponent<InputField>();
            inputAddress = TargetGo.transform.Find("InputFieldAddress").GetComponent<InputField>();
            inputBeaty = TargetGo.transform.Find("InputFieldBeauty").GetComponent<InputField>();
            commitBtn = TargetGo.transform.Find("Button").GetComponent<Button>();
            commitBtnCancel = TargetGo.transform.Find("ButtonClose").GetComponent<Button>();
            ques = TargetGo.transform.Find("ques").GetComponent<Button>();

            ques.onClick.AddListener((() => SystemMsgView.SystemFunction(Function.Tip, !string.IsNullOrEmpty(AnnouncementModel.Instance.ExpressInfo) ? AnnouncementModel.Instance.ExpressInfo : Info.Sendcash, 10f)));
            commitBtn.onClick.AddListener(OnClickCommit);
            commitBtnCancel.onClick.AddListener(Cancel);
            Provincedropdown = TargetGo.transform.Find("DropdownProvince").GetComponent<Dropdown>();
            Provincedropdown.onValueChanged.AddListener(OnProvinceChange);

            CityDropDown = TargetGo.transform.Find("DropdownCity").GetComponent<Dropdown>();
            CityDropDown.onValueChanged.AddListener(OnCityChange);
            PinPaiDropDown = TargetGo.transform.Find("PinPai").GetComponent<Dropdown>();



            CountryDropDown = TargetGo.transform.Find("DropdownCountry").GetComponent<Dropdown>();

            ExchangeGrid = TargetGo.transform.Find("DropdownBrand/grid/content").gameObject;

            Carriage = TargetGo.transform.Find("DropdownCarriage/Label").GetComponent<Text>();

            if (PlayerSave.HasKey("name")) inputName.text = PlayerSave.GetString("name");
            if (PlayerSave.HasKey("address")) inputAddress.text = PlayerSave.GetString("address");
            if (PlayerSave.HasKey("beaty")) inputBeaty.text = PlayerSave.GetString("beaty");
            if (PlayerSave.HasKey("phone")) inputPhone.text = PlayerSave.GetString("phone");

            base.BuildSubViews();

        }

        public override void OnClose()
        {
            GlobalDispatcher.Instance.RemoveListener(GlobalEvent.OnRegionsConfigLoadDone, DataSet);
            CommitController.Instance.GetDispatcher().RemoveListener(CommitController.CommitControllerEvent.OnPostageCallback, OnReflashPostage);

            PlayerSave.SetString("name", inputName.text);
            PlayerSave.SetString("address", inputAddress.text);
            PlayerSave.SetString("beaty", inputBeaty.text);
            PlayerSave.SetString("phone", inputPhone.text);
            PlayerSave.SetInt("province", Provincedropdown.value);
            PlayerSave.SetInt("city", CityDropDown.value);
            PlayerSave.SetInt("country", CountryDropDown.value);
            PlayerSave.SetInt("pinpai", PinPaiDropDown.value);


            StoreController.Instance.currentSellID = 0;
            StoreController.Instance.currentSellNumber = 0;
            base.OnClose();

        }
        private void Cancel()
        {
            ViewMgr.Instance.Close(ViewNames.CommitView);
        }

        public void OnClickCommit()
        {

            if (inputName.text != "" && inputPhone.text != "" && inputAddress.text != "" && inputBeaty.text != "")
            {
                //                if (StoreController.Instance.currentSellNumber == 0)
                //                {
                //                    CommitController.Instance.OidExchangeReq(LoginModel.Instance.Uid, 1, inputName.text, inputPhone.text, inputAddress.text + Carriage.text, inputBeaty.text);
                //                }
                //                else
                if (Provincedropdown.value != 0 && CityDropDown.value != 0&& PinPaiDropDown.value != 0)
                {
                    CommitViewModel cvm = CommitViewModel.Instance;
                    cvm.Address = inputAddress.text;

                    Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", inputAddress.text, "test1"));

                    cvm.Name = inputName.text;
                    cvm.Beaty = inputBeaty.text;
                    cvm.Phone = inputPhone.text;
                    cvm.SelectPinpai = PinPaiDropDown.value;
                    cvm.Country = CountryDropDown.captionText.text;
                    cvm.Province = Provincedropdown.captionText.text;
                    cvm.City = CityDropDown.captionText.text;
                    cvm.SelectPinpai = cvm.brands[PinPaiDropDown.captionText.text].id;
                    if (CommitViewModel.Instance.Postage > 0)
                    {
                        PayOrderInterfaceMgr.Instance.payfor = PayFor.Exchange;

                        ViewMgr.Instance.Open(ViewNames.PayChooseView);
                    }
                    else
                    {
                        SystemMsgView.SystemFunction(Function.Tip, Info.Null);

                    }
                    //                Debug.LogError(string.Format("<color=#ff0000ff><---{0}-{1}----></color>", "test", "test1"));
                }
                else
                {
                    SystemMsgView.SystemFunction(Function.Tip, Info.ChooseFieldNotRight);

                }

            }
            else
            {
                SystemMsgView.SystemFunction(Function.Tip, Info.InputFieldNotNull);
            }
        }

        public override void OnOpen()
        {
            taos = CommitViewModel.Instance.taos;
            GlobalDispatcher.Instance.AddListener(GlobalEvent.OnRegionsConfigLoadDone, DataSet);
            CommitController.Instance.GetDispatcher().AddListener(CommitController.CommitControllerEvent.OnPostageCallback, OnReflashPostage);
            //奖品
            initPinPai();

            LoadReagionsConfig.Instance.InitConfig();
            ResourceMgr.Instance.LoadResource("Prefab/ExchangeItem", ((resource, b) =>
            {

                Exchangeitem = (GameObject)resource.UnityObj;

                if (StoreController.Instance.currentSellNumber != 0)
                {
                    BaseObject obj = Farm_Game_StoreInfoModel.Instance.GetData(StoreController.Instance.currentSellID);
                    CommitViewModel.Instance.taos.Clear();
                    LoadPayObj(obj, StoreController.Instance.currentSellNumber);

                }
                else
                {
                    //                    Dictionary<int, Oil> oils = Farm_Game_StoreInfoModel.storage.Oils;
                    //                    foreach (Oil o in oils.Values)
                    //                    {
                    //                        if (o.OilType == 3)
                    //                        {
                    //                           
                    //                            LoadPayObj(o);
                    //
                    //                        }
                    //                    }
                    Dictionary<int, Elixir> elixir = Farm_Game_StoreInfoModel.storage.Elixirs;
                    CommitViewModel.Instance.taos.Clear();
                    foreach (Elixir e in elixir.Values)
                    {
                        LoadPayObj(e);
                    }

                }
                Carriage.text = (CommitViewModel.Instance.Postage <= 0 ? 10 : CommitViewModel.Instance.Postage) + "元";

                OnValueChange(false);

            }));
            base.OnOpen();

        }

        void initPinPai()
        {

            List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();
            Dropdown.OptionData op0 = new Dropdown.OptionData();
            op0.text = "请选择品牌";
            list.Add(op0);
            foreach (var s in CommitViewModel.Instance.brands)
            {
                if (s.Value.enabled == true)
                {
                    Dropdown.OptionData op = new Dropdown.OptionData();
                    op.text = s.Value.name;

                    list.Add(op);
                }
            }

            PinPaiDropDown.AddOptions(list);

        }
        bool OnReflashPostage(int id, object o)
        {
            Carriage.text = CommitViewModel.Instance.Postage + "元";

            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", CommitViewModel.Instance.Postage, "test1"));

            return false;
        }
        private void LoadPayObj(BaseObject id)
        {

            BaseAtrribute ba = LoadObjctDateConfig.Instance.GetAtrribute(id.ID);
            GameObject go = GameObject.Instantiate(Exchangeitem);
            Text t = go.transform.Find("Item Label").GetComponent<Text>();
            Text c = go.transform.Find("count").GetComponent<Text>();
            Toggle toggle = go.GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(OnValueChange);
            ToggleAndObj tao = new ToggleAndObj(id.ID, toggle, id.ObjectNum);
            taos.Add(tao);

            t.text = ba.ExchangedName;
            c.text = "×" + id.ObjectNum;
            go.transform.SetParent(ExchangeGrid.transform, false);

        }
        //加载东西进入购买付款物体grid
        //这个是点击单个物体的时候
        private void LoadPayObj(BaseObject id, int instanceCurrentSellNumber)
        {

            BaseAtrribute ba = LoadObjctDateConfig.Instance.GetAtrribute(id.ID);
            GameObject go = GameObject.Instantiate(Exchangeitem);
            Text t = go.transform.Find("Item Label").GetComponent<Text>();
            Text c = go.transform.Find("count").GetComponent<Text>();
            Toggle toggle = go.GetComponent<Toggle>();
            //被点击

            toggle.onValueChanged.AddListener(OnValueChange);
            ToggleAndObj tao = new ToggleAndObj(id.ID, toggle, instanceCurrentSellNumber);
            taos.Add(tao);

            t.text = ba.ExchangedName;
            c.text = "×" + instanceCurrentSellNumber;

            go.transform.SetParent(ExchangeGrid.transform, false);

        }
        //按钮被点击需要重新计算邮费
        private void OnValueChange(bool ispress)
        {
            CommitController.Instance.SendPostageReq(taos);
            for (int i = 0; i < taos.Count; i++)
            {
                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", taos[i].Id, "test1"));

            }
        }



        public bool DataSet(int id, object a)
        {
            regions = LoadReagionsConfig.Instance.regionsInfo;
            Dropdown.OptionData op0 = new Dropdown.OptionData();
            op0.text = "请选择省份";
            ProvincelList.Add(op0);
            foreach (var province in regions)
            {
                Dropdown.OptionData op = new Dropdown.OptionData();
                op.text = province.Key;
                ProvincelList.Add(op);

            }
            Provincedropdown.AddOptions(ProvincelList);
            OnProvinceChange(0);
            if (PlayerSave.HasKey("province")) Provincedropdown.value = PlayerSave.GetInt("province");
            if (PlayerSave.HasKey("city")) CityDropDown.value = PlayerSave.GetInt("city");
            if (PlayerSave.HasKey("country")) CountryDropDown.value = PlayerSave.GetInt("country");

            if (PlayerSave.HasKey("pinpai")) PinPaiDropDown.value = PlayerSave.GetInt("pinpai");
            return true;
        }
        /// <summary>
        /// 省的下拉列表改变时
        /// </summary>
        /// <param name="index"></param>
        private void OnProvinceChange(int index)
        {
            Dropdown.OptionData data = ProvincelList[index];
            if (regions.ContainsKey(data.text))
            {
                Dictionary<string, List<string>> citys = regions[data.text];

                CityDropDown.options.Clear();
                CitylList.Clear();
                Dropdown.OptionData op0 = new Dropdown.OptionData();
                op0.text = "请选择城市";
                CitylList.Add(op0);
                foreach (var country in citys.Keys)
                {
                    Dropdown.OptionData op = new Dropdown.OptionData();
                    op.text = country;
                    CitylList.Add(op);

                }
                CityDropDown.AddOptions(CitylList);
                CityDropDown.value = 0;
                OnCityChange(0);
            }

        }
        /// <summary>
        /// 市的下拉列表改变时
        /// </summary>
        /// <param name="index"></param>
        private void OnCityChange(int index)
        {
            if (regions.ContainsKey(ProvincelList[Provincedropdown.value].text) &&
                regions[ProvincelList[Provincedropdown.value].text].ContainsKey(CitylList[index].text))
            {
                List<string> countrys = regions[ProvincelList[Provincedropdown.value].text][CitylList[index].text];
                CountryDropDown.options.Clear();
                CountryoplList.Clear();
                Dropdown.OptionData op0 = new Dropdown.OptionData();
                op0.text = "请选择县级";
                CountryoplList.Add(op0);
                for (int i = 0; i < countrys.Count; i++)
                {
                    Dropdown.OptionData op = new Dropdown.OptionData();
                    op.text = countrys[i];
                    CountryoplList.Add(op);

                }
                CountryDropDown.AddOptions(CountryoplList);
            }


        }


    }
}
