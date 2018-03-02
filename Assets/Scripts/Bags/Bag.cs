using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
public class Bag {

    public GameObject content;
    //背包格子池
    public List<BagGrid> gridsList;
    //物品详细信息文本框
    Transform descPanel;
    float descWidth;
    Text t_desc;
    //能否显示信息框（物品拖动时不显示信息框）
    public bool canShowDesc=true;
    public GoodsSort bagSort;

    //是否已经渲染过
    public bool hasRender = false;
  


    public Bag(GameObject bagSon,GoodsSort sort)
    {
        content = bagSon;
        bagSort = sort;
        //加载预制体
        descPanel = GameObject.Find("Canvas").transform.Find("Tips/DescPanel");
        descPanel.gameObject.SetActive(false);
        t_desc = descPanel.Find("Text").GetComponent<Text>();
        descWidth = descPanel.GetComponent<RectTransform>().sizeDelta.x;
    }

 
    public void OnShowing()
    {
        hasRender = true;
        if(!content.activeSelf)
            content.SetActive(true);
    }



    public  void OnClosing()
    { 
        if(content.activeSelf)
            content.SetActive(false);
    }

    /// <summary>
    /// 增加物品
    /// </summary>
    public void AddGoods(Goods goods)
    {
        BagGrid sameGrid = GetSameGrid(goods);
        if (sameGrid != null)
        {
            sameGrid.SetGoods(goods);
            return;
        }
        BagGrid emptyGrid = GetEmptyGrid();
        if (emptyGrid != null)
        {
            BagGrid grid=BagMgr.instance.bagDict[goods.goodsSort].RelGoods(goods);
            emptyGrid.SetGoods(goods);
            goods.sortGrid = grid;
            goods.mainGrid = emptyGrid;
            return;
        }
        Debug.Log("背包已满，无法获得物品");
    }

    //返回具有相同物品且未到上限的格子
    public BagGrid GetSameGrid(Goods goods)
    {
        //是否有相同物品
        foreach(BagGrid grid in gridsList)
        {
            if (grid.isSame(goods)&& !grid.willFull(goods.putNum))
                return grid;
        }
        return null;
    }
    //否则返回首个空格子
    private BagGrid GetEmptyGrid()
    {
        foreach (BagGrid grid in gridsList)
        {
            if (grid.myGoods == null)
                return grid;
        }
        return null;
    }

    /// <summary>
    /// 找到对应物品类背包中存放该物体的格子
    /// </summary>
    /// <param name="goods"></param>
    /// <returns>被关联的格子</returns>
    public BagGrid RelGoods(Goods goods)
    {
        BagGrid emptyGrid = GetEmptyGrid();
        if (emptyGrid != null)
        {
            emptyGrid.myGoods = goods;
            emptyGrid.RefreshGrid();
        }

        else
            Debug.Log(goods.goodsSort + "背包已满");
        return emptyGrid;
    }


    private Comparison<Goods> comparison = new Comparison<Goods>((Goods x, Goods y)=>
    {
        if (x.GetWeight() > y.GetWeight())
            return -1;
        return 1;
    });

    /// <summary>
    /// 整理背包
    /// </summary>
    public void SortBag()
    {
        List<Goods> temp = new List<Goods>();
        foreach(BagGrid grid in gridsList)
        {
            if(grid.myGoods!=null)
                temp.Add(grid.myGoods);
        }
        temp.Sort(comparison);
        int i = 0;
        for (; i < temp.Count; i++)
        {
            gridsList[i].myGoods = temp[i];
            if (bagSort == GoodsSort.Undefined)
                gridsList[i].myGoods.mainGrid = gridsList[i];
            else
                gridsList[i].myGoods.sortGrid = gridsList[i];
            gridsList[i].RefreshGrid();

        }
        for (; i < gridsList.Count; i++)
        {
            gridsList[i].myGoods = null;
            gridsList[i].RefreshGrid();
        }

    }

    #region 物品信息面板
    //展示物品信息
    public void ShowDesc(string desc, Vector2 pos)
    {
        if (desc.Length > 0)
        {
            descPanel.gameObject.SetActive(true);
            descPanel.transform.position = pos + new Vector2(-descWidth / 2 - 20, 0);
            t_desc.text = desc;
        }
    }
    //关闭物品信息面板
    public void CloseDesc()
    {
        if (descPanel.gameObject.activeSelf)
            descPanel.gameObject.SetActive(false);
    }


    #endregion
}

