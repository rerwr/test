using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class TimeBarView:BaseSubView
    {
        private Slider slider;
        private Text name;
        private Text Num;
        private Button fertilizer;
        private Text sliderText;
        private Plant plant;
        public TimeBarView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {

        }

        public override void BuildUIContent()
        {
            base.BuildUIContent();
            slider = TargetGo.transform.Find("Slider").GetComponent<Slider>();
            name = TargetGo.transform.Find("Name").GetComponent<Text>();
            Num = TargetGo.transform.Find("Image/Num").GetComponent<Text>();
            fertilizer = TargetGo.transform.Find("Image").GetComponent<Button>();
            fertilizer.onClick.AddListener(Fertilizer);
            sliderText = TargetGo.transform.Find("Slider/Text").GetComponent<Text>();

        }

        private void Fertilizer()
        {
            if (Farm_Game_StoreInfoModel.storage.Fertilizers[701].ObjectNum<=0)
            {
                return;
            }
            SystemMsgView.SystemFunction(Function.OpenDialog, Info.Fertilizer,"",(() =>
            {
                FieldsController.ProtocalAction=ProtocalAction.Fertitlize;
                FarmUnit.SeletedFarmID = plant.FarmID;
                FieldsController.Instance.SendWFActionReq(plant.FarmID,GameAction.Fertitlize,701);
            }));
        }
        public override void OnOpen()
        {
            GlobalDispatcher.Instance.AddListener(GlobalEvent.OnFarmUnitClick, ReflashUI);
            GlobalDispatcher.Instance.AddListener(GlobalEvent.OnFarmUnitClick, Init);
            GlobalDispatcher.Instance.AddListener(GlobalEvent.OnOneSecond, ReflashOpenUI);
     
            base.OnOpen();
        }
        private bool Init(int eid, object arg)
        {
            if (plant!=null&&plant.Go)
            {
                Vector3 posW = plant.Go.transform.position;
                Vector3 pos = Camera.main.WorldToScreenPoint(new Vector3(posW.x, posW.y + 0.8f, posW.z));

                TargetGo.transform.position = new Vector3(pos.x, pos.y, pos.z);
            }
         
            return false;
        }
        public override void OnClose()
        {
//            GlobalDispatcher.Instance.RemoveListener(GlobalEvent.OnFarmUnitClick, ReflashUI);
//            GlobalDispatcher.Instance.RemoveListener(GlobalEvent.OnFarmUnitClick, Init);
//            GlobalDispatcher.Instance.RemoveListener(GlobalEvent.OnOneSecond, ReflashOpenUI);

            base.OnClose();
            TargetGo.transform.position = new Vector3(10000, 10000, 0);
        }
        private bool ReflashOpenUI(int eid, object arg)
        {
            if (plant!=null&&!plant.Renderer)
            {
                ViewMgr.Instance.Close(ViewNames.TimeBarView);
            }
            if (plant!=null)
            {
                if (!FriendFarmManager.Instance.isVisiting&&plant.CurrentType==2)
                {
                    fertilizer.gameObject.SetActive(true);
                }
                else
                {
                    fertilizer.gameObject.SetActive(false);
                }

//                if (GameStarter.Instance.isDebug)
//                {
//                    fertilizer.gameObject.SetActive(true);
//
//                }
                slider.value = (float)plant.percentage;
                if (FriendFarmManager.Instance.isVisiting&&plant.IsSteal==1)
                {
                    name.text =string.Format("{0} {1}", plant.Name , "(已偷取)").Replace('\n', ' ');
                   
                }
                else
                {
                    name.text = plant.Name;

                }
                Num.text = "×" +( Farm_Game_StoreInfoModel.storage.Fertilizers.ContainsKey(701)?  Farm_Game_StoreInfoModel.storage.Fertilizers[701].ObjectNum:0);
                long hour = (long)plant.RemainTime / 3600;
                int min = (int)(plant.RemainTime % 3600) / 60;
                int second = (int)(plant.RemainTime % 3600) % 60;

//                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}-{2}---></color>", hour, min, second));
                //显示倒计时
                sliderText.text = string.Format("({0})：{1}", plant.TypeName, (hour>=10?hour.ToString():("0"+(int)hour))
                    +":"+(min>=10?min.ToString():"0"+min) + ":"+ (second >= 10?second.ToString():"0"+second));
                if (!FriendFarmManager.Instance.isVisiting)
                {
                    if (hour == 0 && min == 0 && second == 0)
                    {
                        if (ViewMgr.Instance.isOpen(ViewNames.TimeBarView))
                        {
                            ViewMgr.Instance.Close(ViewNames.TimeBarView);
                        }
                    }
                }
               
            }
          
            return false;
        }

      

        private bool ReflashUI(int eid, object arg)
        {
            plant=(Plant) arg;
//            BaseAtrribute ba=LoadObjctDateConfig.Instance.GetAtrribute(plant.FarmID);
//            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>",ba.Name, "test1"));

            slider.value = (float)plant.percentage;
            ReflashOpenUI(0, null); 
            return false;
        }
    }
}