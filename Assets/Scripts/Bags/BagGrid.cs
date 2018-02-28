﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Reflection;

public class BagGrid : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler
{
    //背包格子
    GameObject myGrid;
    //格子存放的货物
    public Goods myGoods;
    //格子的Image和Text组件
    Vector2 imagePos;
    Image image;
    Text text;
    //格子Id
    public int id;
    public Bag myBag;
    public void Start()
    {
        myGrid = this.gameObject;
        image = myGrid.transform.Find("Image").GetComponent<Image>();
        imagePos = image.transform.localPosition;
        text = image.transform.Find("count").GetComponent<Text>();

    }

    //使用物品
    public void UseGoods()
    {
        if (myGoods == null)
            return;

        string methodName = myGoods.name + "Method";
        MethodInfo methodInfo = GoodsMethod.instance.GetType().GetMethod(methodName);
        if (methodInfo == null)
            return;
        methodInfo.Invoke(GoodsMethod.instance,new Object[] { });
        myGoods.putNum--;
        BagGrid sortGrid = myGoods.sortGrid;
        UpdateGrid();
        sortGrid.UpdateGrid();

    }
    //更新格子
    public void UpdateGrid()
    {
        if (myGoods.putNum == 0)
        {
            if (myGoods != null)
                myGoods = null;
            image.sprite = null;
            myBag.CloseDesc();
            text.text = "";
        }
        else
            text.text = (myGoods.putNum == 1) ? "" : myGoods.putNum.ToString();
        
    }
    //移除物品
    public void RemoveGoods()
    {
        myGoods = null;
        image.sprite = null;
        text.text = "";
        myBag.CloseDesc();
    }


    #region 添加物品
    /// <summary>
    /// 放置物品
    /// </summary>
    /// <param name="goods"></param>
    public void SetGoods(Goods goods)
    {
        if (myGoods == null)
        {
            myGoods = goods;
            //myGoods.gridId = id;
            image.sprite = goods.itemSprite;
            myBag.goodsList.Add(myGoods);
        }
        else
        {
            myGoods.putNum += goods.putNum;
            myGoods.sortGrid.UpdateGrid();
        }
        text.text = myGoods.putNum == 1 ? "" : myGoods.putNum.ToString();

    }


    // 检测格子是否将会装满物体
    public bool willFull(int goodsNum)
    {
        if (myGoods.maxNum >= myGoods.putNum + goodsNum)
            return false;
        return true;
    }


    //增加的物品是否和当前物品相同
    public bool isSame(Goods goods)
    {
        if (myGoods != null)
            return goods.name == myGoods.name;
        return false;
    }


    //所有栏物品与对应背包内的物品进行引用传递进行关联,这样能保证，在增加和减少对应物品数量时能只单操作物品数据，而不用考虑所在物品所属背包
    public void RelGoods(Goods goods)
    {
        myGoods = goods;
        //myGoods.gridId = id;
        image.sprite = myGoods.itemSprite;
        myBag.goodsList.Add(myGoods);
        text.text = myGoods.putNum == 1 ? "" : myGoods.putNum.ToString();

    }
    #endregion

    //交换两个格子的物品
    public void SwapGrid(BagGrid beginGrid)
    {
        if (myGoods == null)
        {
            SetGoods(beginGrid.myGoods);
            if (myBag.bagSort == GoodsSort.Undefined)
                myGoods.mainGrid = this;
            else
                myGoods.sortGrid = this;
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
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2)
        {
            myGoods.mainGrid.UseGoods();
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