using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BagGrid:MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler,IBeginDragHandler,IDragHandler,IDropHandler,IEndDragHandler
{
    //背包格子
    GameObject myGrid;
    //格子存放的货物
    Goods myGoods;
    //格子的Image和Text组件
    Vector2 imagePos;
    Image image;
    Text text;

    Bag myBag;
    Camera cam;
    public void Start()
    {
        myGrid = this.gameObject;
        image = myGrid.transform.Find("Image").GetComponent<Image>();
        imagePos = image.transform.localPosition;
        text = image.transform.Find("count").GetComponent<Text>();
        myBag = Bag.instance;
        cam = Camera.main;

    }


    public void UseGoods()
    {
        if (myGoods == null)
            return;
        if (!myGoods.canUse)
            return;
        myGoods.UseEffect();
        if (--myGoods.putNum == 0)
        {
            RemoveGoods();
            return;
        }
        text.text = myGoods.putNum.ToString();
    }

    //移除物品
    private void RemoveGoods()
    {
        myGoods = null;
        image.sprite = null;
        text.text = "";
    }

    /// <summary>
    /// 放置物品
    /// </summary>
    /// <param name="goods"></param>
    public void SetGoods(Goods goods)
    {
        if (myGoods==null)
        {
            myGoods = goods;
            image.sprite = goods.ItemSprite;
        }
        else
            myGoods.putNum+=goods.putNum;
        text.text = myGoods.putNum.ToString();
    }

    //检查是否可以增加物品
    public bool canPut(Goods goods)
    {
        if (myGoods == null)
            return true;
        if (!isFull() && isSame(goods))
            return true;
        return false;
    }

    // 检测格子是否装满物体
    private bool isFull()
    {
        if (myGoods.maxNum > myGoods.putNum)
            return false;
        return true;
    }

    //增加的物品是否和当前物品相同
    private bool isSame(Goods goods)
    {
        return goods.name == myGoods.name;
    }


    //交换两个格子的物品
    public void SwapGrid(BagGrid beginGrid)
    {
        if (myGoods == null)
        {
            SetGoods(beginGrid.myGoods);
            beginGrid.RemoveGoods();
        }        
        else
        {
            Goods tempGoods = beginGrid.myGoods;
            beginGrid.RemoveGoods();
            beginGrid.SetGoods(myGoods);
            RemoveGoods();
            SetGoods(tempGoods);
        }
    }

    #region 事件监听
    public int id;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2)
        {
            UseGoods();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (myGoods == null)
            return;
        myBag.ShowDesc(myGoods.GetDescription(), eventData.pointerEnter.transform.position);
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        if (myGoods == null)
            return;
        myBag.CloseDesc();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (myGoods != null)
        {
            if (Bag.instance.content.GetComponent<GridLayoutGroup>().enabled)
            {
                Bag.instance.content.GetComponent<ContentSizeFitter>().enabled = false;
                Bag.instance.content.GetComponent<GridLayoutGroup>().enabled = false;

            }

            image.transform.position = eventData.position;
            //设置当前格子层级最高，否则会造成遮挡
            myGrid.transform.SetAsLastSibling();
        }
           
    }

    public void OnDrop(PointerEventData eventData)
    {
        
        BagGrid beginGrid = eventData.pointerDrag.GetComponent<BagGrid>();
        
        if (beginGrid.myGoods == null)
            return;
        SwapGrid(beginGrid);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.transform.localPosition = imagePos;
    }


    #endregion
}
