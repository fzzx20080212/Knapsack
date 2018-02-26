using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueVial : Goods
{

    public BlueVial() : base()
    {
        maxNum = 10;
        name = "蓝瓶";
        canUse = true;
        itemSprite = Resources.Load<Sprite>("magic");
        description = "这是一个喝了可以加蓝的药";
    }

    public override void UseEffect()
    {
        base.UseEffect();
    }
}
