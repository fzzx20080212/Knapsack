using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Goods {

    public Equipment() : base()
    {
        goodsSort = GoodsSort.Equipment;
    }
    public override Goods Clone()
    {
        Consumable consumable = new Consumable();
        consumable.maxNum = maxNum;
        consumable.name = name;
        consumable.itemSprite = itemSprite;
        consumable.description = description;
        consumable.putNum = putNum;
        return consumable;
    }
}
