using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsMethod  {

    public static GoodsMethod instance = new GoodsMethod();

    private GoodsMethod()
    {

    }
    public void BloodMethod()
    {
        Debug.Log("加血");
    }
    public void MagicMethod()
    {
        Debug.Log("加蓝");
    }

}
