using System;
using System.Collections.Generic;

using System.Reflection;
using UnityEngine;
//using Debugger = LuaInterface.Debugger;

namespace Framework
{
    public abstract class BaseViewController
    {
        public static BaseViewController Create(string viewname,GameObject mainGo)
        {
            string typename = "Game." +viewname + "Controller";
            //Debugger.Log(typename);
            Assembly ab=Assembly.GetExecutingAssembly();
            for (int i = 0; i < ab.GetManifestResourceNames().Length; i++)
            {

                Debug.Log(string.Format("<color=#ffffffff><---{0}-{1}----></color>", ab.GetManifestResourceNames()[i], "test1"));

            }
            BaseViewController vc = (BaseViewController)Assembly.GetExecutingAssembly().CreateInstance(typename);

            vc.MainGO = mainGo;
            return  vc;
        }
        public bool IsOpen { private set; get; }
        protected GameObject MainGO { set; get; }
        public List<BaseSubView> Viewlist = new List<BaseSubView>();

        /// <summary>
        /// 填充Viewlist
        /// </summary>
        public virtual void Build()
        {
            if (Viewlist != null)
            {
                for (int i = 0; i < Viewlist.Count; i++)
                {
                    Viewlist[i].BuildSubViews();
                }
            }
        }

        public void OnBuild()
        {
            for (int i = 0; i < Viewlist.Count; i++)
            {
                Viewlist[i].BuildUIContent();
            }
        }
        public void Open()
        {
            IsOpen = true;
            MainGO.SetActive(true);
        }

        public void OnOpen()
        {
            for (int i = 0; i < Viewlist.Count; i++)
            {
                Viewlist[i].OnOpen();
            }
        }
        public void Close()
        {
            IsOpen = false;
            MainGO.SetActive(false);
        }
        public virtual void OnClose()
        {
            for (int i = 0; i < Viewlist.Count; i++)
            {
                Viewlist[i].OnClose();
            }
        }
        public void Destroy()
        {
            GameObject.Destroy(MainGO);
            MainGO = null;
        }
        public virtual void OnDestroy()
        {
            for (int i = 0; i < Viewlist.Count; i++)
            {
                Viewlist[i].OnDestroy();
            }
        }
    }
}