
using Framework;
using Game;
using UnityEngine;

namespace Game
{
    

public class HelpView : IntroduceView
{


    public HelpView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
    {
    }
    public override void OnOpen()
    {
//        base.OnOpen();
     
//        if (GameTextDataMgr.Instance.TextDatas.ContainsKey(7) && !string.IsNullOrEmpty(GameTextDataMgr.Instance.TextDatas[7].content))
//        {
//            Text.text = GameTextDataMgr.Instance.TextDatas[7].content;
//        }
    }
    public override void OnClickCloseBtn()
    {
        ViewMgr.Instance.Close(ViewNames.HelpView);

    }
}
}
