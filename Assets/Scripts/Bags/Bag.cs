using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Bag {

    public GameObject content;
    //背包格子池
    public List<BagGrid> gridsList;
    //物品详细信息文本框
    Transform descPanel;
    float descWidth;
    Text t_desc;
    public GoodsSort bagSort;

    //是否已经渲染过
    public bool hasRender = false;
    //背包中拥有的所有物品
    public List<Goods> goodsList=new List<Goods>();


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
    //否则返回空格子
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
    /// 关联格子中的物品
    /// </summary>
    /// <param name="goods"></param>
    /// <returns>被关联的格子</returns>
    public BagGrid RelGoods(Goods goods)
    {
        BagGrid emptyGrid = GetEmptyGrid();
        if (emptyGrid != null)
            emptyGrid.RelGoods(goods);
        else
            Debug.Log(goods.goodsSort + "背包已满");
        return emptyGrid;
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

