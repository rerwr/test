using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework;

namespace Game
{
    public class FactoryView : BaseSubView
    {
        private Button CloseBtn;
        private Button QuesBtn;
        private Toggle primaryToggle;

        public FactoryView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
        }

        public override void BuildSubViews()
        {
            subViews = new List<BaseSubView>();
            subViews.Add(new FactoryPrimaryView(TargetGo.transform.Find("Factory/PrimaryFc").gameObject,ViewController));
            subViews.Add(new FactorySeniorView(TargetGo.transform.Find("Factory/SeniorFc").gameObject, ViewController));
            subViews.Add(new FactorySuccView(TargetGo.transform.Find("ProduceSuccView").gameObject, ViewController));
            subViews.Add(new FactoryGoodsView(TargetGo.transform.Find("Factory/GoodsFc").gameObject, ViewController));

            base.BuildSubViews();
        }

        public override void BuildUIContent()
        {
            base.BuildUIContent();

            CloseBtn = TargetGo.transform.Find("CloseBtn").GetComponent<Button>();
            CloseBtn.onClick.AddListener(OnClickCloseBtn);
            QuesBtn = TargetGo.transform.Find("QuesBtn").GetComponent<Button>();
            QuesBtn.onClick.AddListener(OnClickQuesBtn);
            primaryToggle= TargetGo.transform.Find("Factory/ToggleGroup/Primary").GetComponent<Toggle>();
        }

        public override void OnOpen()
        {

            base.OnOpen();
            primaryToggle.isOn = true;
            MusicManager.Instance.Playsfx(AudioNames.OpenFactory);
        }
        private void OnClickQuesBtn()
        {
           SystemMsgView.SystemFunction(Function.CloseDialog, "<size=22>合成规则：\r\n1.初级工厂中100个果实数量可以合成1个初级精油。\r\n\n2.高级工厂中2个初级精油可以合成1个半成品的精油\r\n\n3.当有40个半成品精油时，系统将自动合1个高级精油\r\n\n4.实物工厂中高级精油可以配合神秘配方等材料合成用于兑换的实物</size>" );
        }
        private void OnClickCloseBtn() {
            ViewMgr.Instance.Close(ViewNames.FactoryView);

            MusicManager.Instance.Playsfx(AudioNames.CloseBtn);
        }
    }
}
