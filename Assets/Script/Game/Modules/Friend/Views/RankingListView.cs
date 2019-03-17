using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    class RankingListView:BaseSubView
    {
        private Button CloseBtn;
        private Button SearchBtn;
        private Button NextPageBtn;
        private Button LastPageBtn;
        private InputField SearchInput;
        private Transform RankList;
        private ScrollRect SR;
        private Transform LoadingImage;
        private GameObject RankItem;
        private Text _Text;

        private bool isFrist=true;
        private bool isReqingRank = false;
        private bool isSearching=false;
        private int rank = 1;

        private int MaxPage = 0;

        public RankingListView(GameObject targetGo, BaseViewController viewController) : base(targetGo, viewController)
        {
        }

        public override void BuildUIContent()
        {
            base.BuildUIContent();
            CloseBtn = TargetGo.transform.Find("CloseBtn").GetComponent<Button>();
            CloseBtn.onClick.AddListener(OnCloseBtnClick);
            SearchBtn = TargetGo.transform.Find("Search/Button").GetComponent<Button>();
            SearchBtn.onClick.AddListener(OnClickSearchBtn);
            NextPageBtn = TargetGo.transform.Find("NextPageBtn").GetComponent<Button>();
            NextPageBtn.onClick.AddListener(delegate() { this.RefreshRank(1); });
            LastPageBtn = TargetGo.transform.Find("LastPageBtn").GetComponent<Button>();
            LastPageBtn.onClick.AddListener(delegate () { this.RefreshRank(-1); });
            LastPageBtn.enabled = false;
            LastPageBtn.image.color = Color.gray;

            SearchInput = TargetGo.transform.Find("Search/InputField").GetComponent<InputField>();
            RankList = TargetGo.transform.Find("Scroll View/Viewport/Content");
            SR = TargetGo.transform.Find("Scroll View").GetComponent<ScrollRect>();
          
            //SR.onValueChanged.AddListener(RefreshRank);
            LoadingImage = TargetGo.transform.Find("LoadingImage");
            LoadingImage.gameObject.SetActive(false);

            _Text = TargetGo.transform.Find("Text").GetComponent<Text>();
            ResourceMgr.Instance.LoadResource("Prefab/RankListItem", OnLoadItem);
        }

        public override void OnOpen()
        {
            base.OnOpen();
            FriendsListView.RankingListView.SetActive(false);
            FriendsInfoController.Instance.GetDispatcher().AddListener(FriendsInfoEvent.OnReqRankingList, LoadRankList);
            FriendsInfoController.Instance.GetDispatcher().AddListener(FriendsInfoEvent.OnReqSearchList, SearchCallBack);
        }

        //点击关闭排行榜按钮
        private void OnCloseBtnClick()
        {
            TargetGo.transform.gameObject.SetActive(false);
        }

        //点击搜索好友按钮
        private void OnClickSearchBtn()
        {
            isSearching = true;
            string searchName = SearchInput.text;
            NextPageBtn.gameObject.SetActive(false);
            LastPageBtn.gameObject.SetActive(false);
            _Text.gameObject.SetActive(false);
            FriendsInfoController.Instance.Search(searchName);
        }
        //搜索结果回调
        private bool SearchCallBack(int eventId,object arg)
        {
            for (int i=0;i<RankList.childCount;i++)
            {
                Transform ch = RankList.GetChild(i);
                GameObject.Destroy(ch.gameObject);
            }

            Dictionary<int,PlayerInfo> searchList=SearchInfoModel.Instance.SearchList;
            foreach (PlayerInfo player in searchList.Values)
            {
                GameObject go = GameObject.Instantiate(RankItem) as GameObject;
                RankItem rankItem = go.AddComponent<RankItem>();
                rankItem.SetDate(player);

                go.transform.SetParent(RankList);
                go.transform.localScale = new Vector3(1, 1, 1);
            }

            return false;
        }

        //滑动刷新下一页排行榜
        private void RefreshRank(Vector2 v)
        {
            //if (isReqingRank&& v.y > -0.001f)
            //{
            //    isReqingRank = false;
            //}
            //Debug.LogError(v + "***" + isReqingRank + "***" + isSearching + "***" + -1.5f/ SR.content.childCount + "***" + SR.content.childCount);
            //if (!isReqingRank&&v.y < -1.5f / SR.content.childCount)
            //{
            //    if (!isSearching)
            //    {
            //        isReqingRank = true;
            //        LoadingImage.gameObject.SetActive(true);
            //        FriendsInfoController.Instance.RankListReq(LoginModel.Instance.Uid, ++FriendsInfoController.Instance.currentPage);
            //    }
            //}
            //if (isReqingRank && v.y > -0.01f)
            //{
            //    isReqingRank = false;
            //}
            //else
            //{
            //    LoadingImage.gameObject.SetActive(false);
            //}
        }

        //请求排行榜列表
        private void RefreshRank(int x)
        {
            int cp = FriendsInfoController.Instance.currentPage;
            cp = cp + x;
            if (cp <= 1)
            {
                LastPageBtn.enabled = false;
                LastPageBtn.image.color = Color.gray;
            }
            else
            {
                LastPageBtn.enabled = true;
                LastPageBtn.image.color = Color.white;
            }
            
            if (MaxPage != 0 && cp >= MaxPage)
            {
                NextPageBtn.enabled = false;
                NextPageBtn.image.color = Color.gray;
            }
            else
            {
                NextPageBtn.enabled = true;
                NextPageBtn.image.color = Color.white;
            }
            isReqingRank = true;
            LoadingImage.gameObject.SetActive(true);
            FriendsInfoController.Instance.RankListReq(LoginModel.Instance.Uid, cp);
            FriendsInfoController.Instance.currentPage = cp;
        }

        //加载item的回调
        private void OnLoadItem(Resource res,bool succ)
        {
            if (succ)
            {
                RankItem = res.UnityObj as GameObject;
                if (!isFrist)
                {
                    LoadRankList(0, null);
                }
            }
        }

        //加载排行榜列表
        private bool LoadRankList(int eventId,object arg)
        {
            if (RankItem == null)
            {
                isFrist = false;
                return false;
            }

            if (!LastPageBtn.gameObject.activeInHierarchy)
            {
                LastPageBtn.gameObject.SetActive(true);
                NextPageBtn.gameObject.SetActive(true);
                _Text.gameObject.SetActive(true);
            }

            Dictionary<int, PlayerInfo> rankList = RankInfoModel.Instance.RankList;
            if (rankList.Count == 0)
            {
                FriendsInfoController.Instance.currentPage--;
                MaxPage = FriendsInfoController.Instance.currentPage;
            }
            else
            {
                for (int i = 0; i < RankList.childCount; i++)
                {
                    if (RankList.GetChild(i) != null)
                    {
                        Transform tr = RankList.GetChild(i);
                        Image img = tr.Find("Icon").GetComponent<Image>();
                        img.material.SetTexture(0, null);
                        GameObject.Destroy(tr.gameObject);
                    }
                }
                Resources.UnloadUnusedAssets();
                System.GC.Collect();
                int min=0, max=0;

                foreach (PlayerInfo player in rankList.Values)
                {
                    if (min == 0) min = player.Rank;
                    if (player.Rank < min) min = player.Rank;
                    if (player.Rank > max) max = player.Rank;

                    GameObject go = GameObject.Instantiate(RankItem) as GameObject;
                    RankItem rankItem = go.AddComponent<RankItem>();
                    rankItem.SetDate(player);

                    go.transform.SetParent(RankList);
                    go.transform.localScale = new Vector3(1, 1, 1);
                    for (int i = 0; i < RankList.childCount - 1; i++)
                    {
                        if (RankList.GetChild(i).GetComponent<RankItem>().rank > rankItem.rank)
                        {
                            go.transform.SetSiblingIndex(i);
                            break;
                        }
                    }
                }
                _Text.text = FriendsInfoController.Instance.currentPage + "/"+10;
            }
            RankInfoModel.Instance.RankList.Clear();
            LoadingImage.gameObject.SetActive(false);
            return false;
        }

        public override void OnClose()
        {
            base.OnClose();

            for (int i = 0; i < RankList.childCount; i++)
            {
                Transform tr = RankList.GetChild(i);
                Image img=tr.Find("Icon").GetComponent<Image>();
                img.material.SetTexture(0,null);
                GameObject.Destroy(tr.gameObject);
            }
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
            isSearching = false;
            isReqingRank = false;
            FriendsInfoController.Instance.GetDispatcher().RemoveListener(FriendsInfoEvent.OnReqRankingList, LoadRankList);
            FriendsInfoController.Instance.GetDispatcher().RemoveListener(FriendsInfoEvent.OnReqSearchList, SearchCallBack);
        }
    }
}
