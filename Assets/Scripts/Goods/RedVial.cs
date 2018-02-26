using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedVial : Goods {

	public RedVial():base()
    {
        maxNum = 5;
        name = "血瓶";
        canUse = true;
        itemSprite = Resources.Load<Sprite>("blood");
        description = "这是一个喝了可以加血的药";
    }

    public override void UseEffect()
    {
        base.UseEffect();
    }
}
