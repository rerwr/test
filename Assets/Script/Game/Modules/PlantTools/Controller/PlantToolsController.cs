using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using Game;
using System;

public class PlantToolsController  : BaseController<PlantToolsController>
{
	protected override Type GetEventType()
	{
		return typeof(PlantToolsEvent);
	}

	public override void InitController()
	{

	}

    /// <summary>
    /// 此处可删减
    /// </summary>
    /// <param name="ToolsName"></param>
	public void PlantToolstoEvent(string ToolsName)
	{
//		Debug.Log ("名字"+ToolsType.C_fandi);
//		Sprite sp = Resources.Load<Sprite>("UI/"+ToolsName);
//		mousePic.GetComponent<SpriteRenderer>().sprite = sp;
//		GetDispatcher().Dispatch(GlobalEvent.OnPlantTools,ToolsName);
		GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnPlantTools,ToolsName);
//		ToolsState = ToolsName;
//		switch (ToolsName) {
//		case "C_fandi":
//			
//			break;
//		case "C_bozhong":
//
//			break;
//		case "C_chucao":
//
//			break;
//		case "C_chuchong":
//
//			break;
//		case "C_shifei":
//
//			break;
//		case "C_yijian":
//
//			break;
//		case "C_zhaoshui":
//
//			break;
//		case "C_caiji":
//
//			break;
//		}

	}

	public class PlantToolsEvent : EventDispatcher.BaseEvent
	{
		public static readonly int OnPlantToolsSucc = ++id;
		public static readonly int PlantToolsViewRefresh = ++id;
	}
}

