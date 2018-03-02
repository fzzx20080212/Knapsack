using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class UILayer : MonoBehaviour,IDropHandler {



    //丢弃物品相关操作，需要拖动到Canvas的事件中
    public void OnDrop(PointerEventData eventData)
    {
        BagGrid beginGrid = eventData.pointerDrag.GetComponent<BagGrid>();
        TipsPanel.instance.Show(beginGrid.myGoods.name, beginGrid.myGoods.putNum, beginGrid.ThrowGoods);

    }
  

}
