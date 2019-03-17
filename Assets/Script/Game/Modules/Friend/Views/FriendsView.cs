using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class FriendsView : BaseSubView
    {
        private Button RankBtn;

        private Transform FriendsList;

        private GameObject friendItem;
        private ScrollRect sr;
        public FriendsView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
        }

        public override void BuildUIContent()
        {
            base.BuildUIContent();
            RankBtn = TargetGo.transform.Find("RankBtn").GetComponent<Button>();
            RankBtn.onClick.AddListener(OnRankBtnClick);
            FriendsList = TargetGo.transform.Find("Scroll View/Viewport/Content");
            sr = TargetGo.transform.Find("Scroll View").GetComponent<ScrollRect>();
               sr .onValueChanged.AddListener(num);
       
//            TargetGo.transform.Find("Scroll View").GetComponent<ScrollRect>().OnEndDrag(new PointerEventData());
            ResourceMgr.Instance.LoadResource("Prefab/FriendItem", OnLoadItem);
        }

        private void num(Vector2 arg0)
        {

            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", arg0.ToString(), "test1"));

            if (arg0.x >= 0.99f)
            {
                if (FriendsInfoModel.Instance.pages.Count> FriendsInfoModel.Instance .currentLoadpage)
                {
                    ReflashItem(++FriendsInfoModel.Instance.currentLoadpage);

                }
            }


        }


        public override void OnOpen()
        {
            base.OnOpen();
            LoadingImageManager.Instance.StartLoading(TargetGo.transform, TargetGo.transform.position);
            FriendsInfoController.Instance.GetDispatcher().AddListener(FriendsInfoEvent.OnReqFriendsInfo, FriendSlist);

            FriendsInfoController.Instance.SendFriendsInfoReq(0);

        
        }

        private bool FriendSlist(int id,object arg)
        {

            MTRunner.Instance.StartRunner(Wait(0.3f));


            return true;
        }


        IEnumerator Wait(float time)
        {
            yield return time;
            Dictionary<int, PlayerInfo> players = FriendsInfoModel.Instance.playerInfos;
            int index = 0;
            List<Dictionary<int, PlayerInfo>> pages = FriendsInfoModel.Instance.pages;
            int count = 0;
            Dictionary<int, PlayerInfo> info = new Dictionary<int, PlayerInfo>();
            if (players.Count>=5)
            {

                foreach (PlayerInfo p in players.Values)
                {

                    //                if (!info.ContainsKey(p.UserGameId))
                    {
                        info.Add(p.UserGameId, p);

                        index++;
                        count++;
                        if (count%players.Values.Count==0)
                        {
                            pages.Add(info);

                        }
                        if (index >= 5)
                        {
                            //每5个增加一页
                            pages.Add(info);
                      
                            index = 0;
                            info = new Dictionary<int, PlayerInfo>();

                        }
                    }
                }
            }
            else
            {
                foreach (PlayerInfo p in players.Values)
                {
                    
                        info.Add(p.UserGameId, p);
                    
                }
                pages.Add(info);

            }

            LoadFriendList(0, null);

        }
        private void OnLoadItem(Resource res, bool succ)
        {
            if (succ)
            {

                friendItem = res.UnityObj as GameObject;
//                LoadFriendList(0, null);
            }
        }

        //加载好友列表
        private bool LoadFriendList(int eventId, object arg)
        {

            if (friendItem == null) return false;

//            for (int i = 0; i < FriendsList.childCount; i++)
//            {
//                GameObject.Destroy(FriendsList.GetChild(i).transform.gameObject);
//            }
//            if (FriendsInfoModel.Instance.pages.Count >= 2)
//            {
//
//                ReflashItem(1);
//                ReflashItem(2);
//                FriendsInfoModel.Instance.currentLoadpage =1;
//            }
//            else
            {
                ReflashItem(0);
                FriendsInfoModel.Instance.currentLoadpage = 0;

            }


            LoadingImageManager.Instance.ReduceLoadingItem();

            return true;
        }

        /// <summary>
        /// 加载一页5个好友
        /// </summary>
        /// <param name="page"></param>
        void ReflashItem(int page)
        {
            List<Dictionary<int, PlayerInfo>> instancePages = FriendsInfoModel.Instance.pages;

            Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", page, "test1"));

            if (instancePages.Count>0)
            {
                Transform parent=FriendsList.transform.Find("page" + page);
                //刷新朋友页
                if (parent != null)
                {
                    for (int i = 0; i < parent.childCount; i++)
                    {
                        for (int j = 0; j < instancePages.Count; j++)
                        {
                            if (instancePages[page].ContainsKey(int.Parse(parent.transform.GetChild(i).name)))
                            {
                                parent.transform.GetChild(i).GetComponent<FriendItem>().SetData(instancePages[page][int.Parse(parent.transform.GetChild(i).name)]);
                            }
                        }
                      
                    }
                }
                else
                {
                  
                    if (FriendsInfoModel.Instance.pages.Count>page&& !FriendsInfoModel.Instance.hasLoadPage.ContainsKey(page))
                    {
                        GameObject contentPage = new GameObject("page" + page);
                        contentPage.transform.SetParent(FriendsList);
                        List<FriendItem> friends = new List<FriendItem>();
                        HorizontalLayoutGroup  hlg= contentPage.AddComponent<HorizontalLayoutGroup>();
//                        contentPage.AddComponent<ContentSizeFitter>();
                
//                        hlg.childForceExpandHeight = false;
//                        hlg.childForceExpandWidth = false;
                        hlg.spacing = 10;
//                        hlg.childControlWidth = false;
//                        hlg.childControlHeight = false;
                        contentPage.transform.localScale=new Vector3(1,1,1);
//                        contentPage.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(517f * FriendsInfoModel.Instance.pages[page].Count / 5, 100);

                      
                        foreach (var friItemInfo in FriendsInfoModel.Instance.pages[page])
                        {
                            GameObject go = GameObject.Instantiate(friendItem) as GameObject;
                            FriendItem fitem = go.AddComponent<FriendItem>();
                            fitem.SetData(friItemInfo.Value);
//                            go.AddComponent<LayoutElement>();

                            go.name = fitem.playerID.ToString();
                            friends.Add(fitem);
                            go.transform.SetParent(contentPage.transform,false);
                          
                        }
                        //每页对应的好友
                        FriendsInfoModel.Instance.hasLoadPage.Add(page, friends);
                    }
                   

                }

            }
        }
        //点击排行榜按钮
        private void OnRankBtnClick()
        {
            FriendsInfoController.Instance.currentPage = 1;
            FriendsInfoController.Instance.RankListReq(LoginModel.Instance.Uid, 1);
            FriendsListView.RankingListView.SetActive(true);
        }

        public override void OnClose()
        {
            base.OnClose();
            FriendsInfoModel.Instance.pages.Clear();

            FriendsInfoController.Instance.GetDispatcher().RemoveListener(FriendsInfoEvent.OnReqFriendsInfo, LoadFriendList);
        }
    }
}
