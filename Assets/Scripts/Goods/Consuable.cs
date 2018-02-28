using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

/// <summary>
/// 消耗品类
/// </summary>
public  class Consumable : Goods
{
    public Consumable() : base()
    {
        goodsSort = GoodsSort.Comsumables;
    }
    public override Goods Clone()
    {
        Consumable consumable = new Consumable();
        consumable.maxNum =maxNum;
        consumable.name = name;
        consumable.itemSprite = itemSprite;
        consumable.description = description;
        consumable.putNum = putNum;
        return consumable;
    }

    public override bool UseEffect()
    {
        string methodName = name + "Method";
        MethodInfo methodInfo = GoodsMethod.instance.GetType().GetMethod(methodName);
        if (methodInfo == null)
            return false;
        methodInfo.Invoke(GoodsMethod.instance, new Object[] { });
        return true;
    }
}
