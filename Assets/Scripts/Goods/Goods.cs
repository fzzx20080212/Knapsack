using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Goods  {
    public Sprite itemSprite;
    //此类物品的单个格子上限
    public int maxNum=1;
    public string name;
    public bool canUse = false;
    protected string description = "";
    //已放置的物品数量
    public int putNum = 1;
    public Sprite ItemSprite
    {
        get { return itemSprite; }
    }
    public Goods()
    {
    }

    public Goods Clone()
    {
        Goods goods = new Goods();
        return goods;
    }
	
    //物品使用效果
    public virtual void UseEffect()
    {
        Debug.Log("你使用了" + name);
    }

    /// <summary>
    /// 获得物品描述
    /// </summary>
    /// <returns></returns>
    public string GetDescription()
    {
        return description;
    }
    
}
