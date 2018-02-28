using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GoodsSort
{
    Equipment=1,
    Comsumables=2,    //消耗品
    Others=3,
    Undefined=0,
}
public abstract class Goods {
    public Sprite itemSprite;
    //此类物品的单个格子上限
    public int maxNum=1;
    public string name;
    public string description = "";
    //已放置的物品数量，和格子id
    public int putNum = 1;

    //该物品存放的格子,种类背包，以及主背包
    public BagGrid sortGrid,mainGrid;
    //物品种类
    public GoodsSort goodsSort=GoodsSort.Undefined;
   
    public Goods()
    {

    }

    public virtual bool UseEffect()
    {
        return false;
    }

    //获得该物品的排列权重
    public int GetWeight()
    {
        return (int)goodsSort * 1000;
    }

    public abstract Goods Clone();
    /// <summary>
    /// 获得物品描述
    /// </summary>
    /// <returns></returns>
    public string GetDescription()
    {
        return description;
    }
    
}

