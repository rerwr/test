using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    class CommonFunctionBarView : BaseSubView
    {
        private Button Store;
        private Button Factory;
        private Button Shop;
        private Button Setting;

        private Button Info;
        private Button Share;
        private Button Scan;

        private Button More;
        private Button Exchange;
        private Button Help;

        private bool isClicked = false;
        public CommonFunctionBarView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {

        }

        public override void BuildUIContent()
        {
            base.BuildUIContent();

            Store = TargetGo.transform.Find("Store").GetComponent<Button>();
            Factory = TargetGo.transform.Find("factory").GetComponent<Button>();
            Shop = TargetGo.transform.Find("Shop").GetComponent<Button>();
            Setting = TargetGo.transform.Find("more/Setting").GetComponent<Button>();
            Info = TargetGo.transform.Find("more/info").GetComponent<Button>();
            Share = TargetGo.transform.Find("more/share").GetComponent<Button>();
            Scan = TargetGo.transform.Find("more/scan").GetComponent<Button>();
            More = TargetGo.transform.Find("more").GetComponent<Button>();
            Exchange = TargetGo.transform.Find("Exchange").GetComponent<Button>();
            Help = TargetGo.transform.Find("Help").GetComponent<Button>();


            Store.onClick.AddListener(OnClickStore);
            Factory.onClick.AddListener(OnClickFactory);
            Shop.onClick.AddListener(OnClickShop);
            Setting.onClick.AddListener(OnClickSetting);
            Info.onClick.AddListener(OnClickInfo);
            Share.onClick.AddListener(OnClickShare);
            Scan.onClick.AddListener(OnClickScan);
            More.onClick.AddListener(OnClickMore);
            Exchange.onClick.AddListener(OnClickExchange);
            Help.onClick.AddListener(OnClickHelp);

        }
        void OnClickHelp()
        {
            ViewMgr.Instance.Open(ViewNames.HelpView);
        }
        void OnClickStore()
        {
            if (!ViewMgr.Instance.isOpen(ViewNames.StoreView))
            {
                ViewMgr.Instance.Open(ViewNames.StoreView);
            }

        }

        void OnClickFactory()
        {

            if (!ViewMgr.Instance.isOpen(ViewNames.FactoryView))
            {
                ViewMgr.Instance.Open(ViewNames.FactoryView);
            }
        }

        void OnClickShop()
        {
            if (!ViewMgr.Instance.isOpen(ViewNames.ShopView))
            {
                ViewMgr.Instance.Open(ViewNames.ShopView);
                
            }

        }

        void OnClickSetting()
        {

            if (!ViewMgr.Instance.isOpen(ViewNames.SettingView))
            {
            ViewMgr.Instance.Open(ViewNames.SettingView);

            }
        }

        void OnClickShare()
        {

            if (!ViewMgr.Instance.isOpen(ViewNames.ShareView))
            {
                ViewMgr.Instance.Open(ViewNames.ShareView);

            }

        }

        void OnClickInfo()
        {
           

            ViewMgr.Instance.Open(ViewNames.MsgListView);
        }

        void OnClickScan()
        {

            //            SceneManager.LoadScene("QRScanScene");
            Scene scene = SceneManager.GetSceneAt(0);
            
            for (int i = 0; i < scene.GetRootGameObjects().Length; i++)
            {
                if (scene.GetRootGameObjects()[i].name == "Panel2D" || scene.GetRootGameObjects()[i].name == "Canvas")
                {
                    Transform t = scene.GetRootGameObjects()[i].transform;
                    t.gameObject.SetActive(false);
                }

            }
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
           
        }

        void OnClickMore()
        {
            if (!isClicked)
            {
                foreach (Transform child in More.transform)
                {
                    child.gameObject.SetActive(true);
                }
                isClicked = true;
            }
            else
            {
                foreach (Transform child in More.transform)
                {
                    child.gameObject.SetActive(false);
                }

                isClicked = false;
            }
        }

        void OnClickExchange()
        {
            StoreItemView.Show();
            //没有高级精油的话
            if (!Farm_Game_StoreInfoModel.Instance.ContainType(ObjectType.SeniorOil))
            {
//                MTRunner.Instance.StartRunner(wait());
            }
        }

        IEnumerator wait()
        {
            yield return 0.3f;
            SystemMsgView.SystemFunction(Function.Tip, Game.Info.SeniorOilNotEnough);

        }
    }
}
