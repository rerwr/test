using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using Framework;
using Game;
using Google.ProtocolBuffers;
//using UnityEditor;
using UnityEngine;

public class ShopModel : BaseModel<ShopModel>
{
    public int cellsNum = 100;
    public Dictionary<int,Seed> seeds=new Dictionary<int, Seed>();
    public Dictionary<int,DogFood> DogFoods=new Dictionary<int, DogFood>();
    public Dictionary<int,Fertilizer> Fertilizers=new Dictionary<int, Fertilizer>();
    public Dictionary<int, Formula> Formulas = new Dictionary<int, Formula>();

    //测试用果实
    public Dictionary<int, Result> Results = new Dictionary<int, Result>();

    public override void InitModel()
    {
        
    }

    public  void SetData(Farm_Game_ShopInfo_Anw GenerateAnw)
    {
        DataSettingManager.SetAnwData(out seeds,out DogFoods,out Fertilizers, out Results, out Formulas,GenerateAnw);
        
    }



}