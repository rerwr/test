using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    enum Function
    {
        CloseDialog,//点击确定关闭当前面板

        Quit,//点击确定退出面板
        OpenDialog,//点击确定打开其他面板，关闭当前面板
        Tip,//无确定按钮
        GetDialog,//获得物品对话框
        
    }
    class SystemMsgView:BaseSubView
    {
        private static Text text;
        private static Button btn;
        private static Button Cancelbtn;
        private static GameObject gridLayout;
        private static Function fun;
        private static  string content;
        private static string viewNames;
        private static Action click;
        private static float time=1.5f;
        private static List<DeltaStoreUnit> ids=new List<DeltaStoreUnit>();
        public SystemMsgView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {

        }

        public override void BuildSubViews()
        {
            base.BuildSubViews();
            text=TargetGo.transform.Find("Content/Content").GetComponent<Text>();
            btn = TargetGo.transform.Find("Btn").GetComponent<Button>();
            Cancelbtn = TargetGo.transform.Find("Cancel").GetComponent<Button>();
            gridLayout = TargetGo.transform.Find("Container").gameObject;
            text.text = content;

        }

        public override void OnOpen()
        {
            base.OnOpen();
            text.text = content;
            if (fun == Function.CloseDialog)
            {
                btn.onClick.AddListener(ClosePanel);
            }
            else if (fun == Function.Quit)
            {
                btn.onClick.AddListener(OnQuit);
            }
            else if (fun == Function.Tip)
            {
                //                btn.gameObject.SetActive(false);
                btn.onClick.AddListener(ClosePanel);

                MTRunner.Instance.StartRunner(wait());
            }
            else if(fun==Function.OpenDialog)
            {
                Cancelbtn.gameObject.SetActive(true);
                Cancelbtn.onClick.AddListener(ClosePanel);
                Vector3 pos=btn.GetComponent<RectTransform>().position;
                btn.GetComponent<RectTransform>().anchoredPosition = new Vector3(-85.35f, -86f, pos.z);
                btn.onClick.AddListener((() =>
                    {
                         ClosePanel();
                   
                        if (viewNames!=null)
                        {
                             Open(viewNames);
                        }
                        if (click!=null)
                        {
                            click();

                        }

                    }
                ));
            }else if (fun==Function.GetDialog)
            {
                btn.onClick.AddListener(ClosePanel);

                MTRunner.Instance.StartRunner(wait());
                ResourceMgr.Instance.LoadResource("Prefab/GetItem",((resource, b) =>
                {
                    GameObject go=resource.UnityObj as GameObject;

                    for (int i = 0; i < ids.Count; i++)
                    {
                        GameObject item=GameObject.Instantiate(go);
                        GetItem getItem = item.AddComponent<GetItem>();
                        getItem.SetData(ids[i]);
                        item.transform.SetParent(gridLayout.transform);
                    }

                    
                }));

            }
        }
        public static void SystemFunction(Function fun1,List<DeltaStoreUnit> id, float t = 5)
        {
            fun = fun1;
            content = "恭喜您获得：";
            ids = id;
            time = t;
            
            ViewMgr.Instance.Open(ViewNames.SystemMsgView);
            
        }

        public static void SystemFunction(Function fun1, string content1, float t = 1.8f)
        {
            fun = fun1;
            content = content1;
            
            time = t;
           
            ViewMgr.Instance.Open(ViewNames.SystemMsgView);
        

        }
        public static void SystemFunction(Function fun1,string content1, string ViewNames1,Action action)
        {
            fun = fun1;
            content = content1;
            viewNames = ViewNames1;
            click = action;
            
            if (text)
            {
                text.text = content1;
               
            }

            ViewMgr.Instance.Open(ViewNames.SystemMsgView);

        }

        IEnumerator wait()
        {
            yield return time;
            if (ViewMgr.Instance.isOpen(ViewNames.SystemMsgView))
            {
                ViewMgr.Instance.Close(ViewNames.SystemMsgView);
                for (int i = 0; i < gridLayout.transform.childCount; i++)
                {
                    gridLayout.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
          
        }
        
        private static void Open(string name)
        {
            if (name!="")
            {
                ViewMgr.Instance.Open(name);
                ClosePanel();
            }
          
        }

        private static void ClosePanel()
        {
            ViewMgr.Instance.Close(ViewNames.SystemMsgView);
        }
        private  void OnQuit()
        {

          ViewMgr.Instance.Open(ViewNames.LoginView);
            SocketMgr.Instance._isneed2loginview = false;
            SocketMgr.Instance.Disconnect();
          ViewMgr.Instance.Close(ViewNames.SystemMsgView);
        }
    }
}
