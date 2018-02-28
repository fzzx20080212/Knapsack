﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagMgr : MonoBehaviour {

    GameObject bagSkin;
    //背包行数,列数
    int rows = 10, column = 5;
    //背包格子预制体
    GameObject gridPrefab;
    //四个背包的字典
    public Dictionary<GoodsSort, Bag> bagDict = new Dictionary<GoodsSort, Bag>();
    //背包名称
    string[] bagSonName = new string[] { "AllBag", "EquipmentBag", "ConsuableBag", "OthersBag" };

    //背包切换键
    Button ABtn, EBtn, CBtn, OBtn,ClickBtn;

    //单例
    public static BagMgr instance;
    // Use this for initialization
    void Start () {
        instance = this;
        gridPrefab = Resources.Load("Grid") as GameObject;
        bagSkin = GameObject.Find("Canvas/Panel/Bag");

        #region 背包切换按钮操作
        //背包切换按钮添加监听
        ABtn = bagSkin.transform.Find("ABtn").GetComponent<Button>();
        ClickBtn = ABtn;
        ABtn.GetComponent<Image>().color= new Color(1, 1, 0,1);
        ABtn.onClick.AddListener(delegate ()
        {
            ClickBtn.GetComponent<Image>().color = new Color(1, 1, 1);
            ABtn.GetComponent<Image>().color = new Color(1, 1, 0, 1);
            ClickBtn = ABtn;
            OpenBag(GoodsSort.Undefined);
        });
        EBtn = bagSkin.transform.Find("EBtn").GetComponent<Button>();
        EBtn.onClick.AddListener(delegate ()
        {
            ClickBtn.GetComponent<Image>().color = new Color(1, 1, 1);
            EBtn.GetComponent<Image>().color = new Color(1, 1, 0, 1);
            ClickBtn = EBtn;
            OpenBag(GoodsSort.Equipment);
        });
        CBtn = bagSkin.transform.Find("CBtn").GetComponent<Button>();
        CBtn.onClick.AddListener(delegate ()
        {
            ClickBtn.GetComponent<Image>().color = new Color(1, 1, 1);
            CBtn.GetComponent<Image>().color = new Color(1, 1, 0, 1);
            ClickBtn = CBtn;
            OpenBag(GoodsSort.Comsumables);
        });
        OBtn = bagSkin.transform.Find("OBtn").GetComponent<Button>();
        OBtn.onClick.AddListener(delegate ()
        {
            ClickBtn.GetComponent<Image>().color = new Color(1, 1, 1);
            OBtn.GetComponent<Image>().color = new Color(1, 1, 0, 1);
            ClickBtn = OBtn;
            OpenBag(GoodsSort.Others);
        });

        #endregion

        Init();
        
        StartCoroutine(DoAfterRender());

    }

    //第一次渲染结束
    IEnumerator DoAfterRender()
    {
   
        yield return new WaitForFixedUpdate();
        OpenBag(GoodsSort.Undefined);
        
    }

    //子背包第一次渲染
    IEnumerator BagDoAfterRender(GameObject content)
    {
        yield return new WaitForEndOfFrame();
        Transform temp = content.transform.Find("Viewport/Content");
        temp.GetComponent<ContentSizeFitter>().enabled = false;
        temp.GetComponent<GridLayoutGroup>().enabled = false;
    }
    //初始化背包面板
    void Init()
    {
        for(int n=0;n<bagSonName.Length;n++)
        {
            //加载预制体
            GameObject bagSon = GameObject.Find("Canvas/Panel/Bag/"+bagSonName[n]);
            Transform content = bagSon.transform.Find("Viewport/Content");
            List<BagGrid> gridsList = new List<BagGrid>();
            Bag bag = new Bag(bagSon,(GoodsSort)n);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    GameObject temp = Instantiate(gridPrefab);

                    BagGrid bagGrid = temp.AddComponent<BagGrid>();
                    bagGrid.myBag = bag;
                    bagGrid.id = j + i * column;
                    temp.transform.SetParent(content, false);
                    //BagGrid bagGrid = new BagGrid(temp);
                    gridsList.Add(bagGrid);

                }
            }
            bag.gridsList = gridsList;
            bagDict.Add((GoodsSort)n, bag);
        }

    }


 
	// Update is called once per frame
	void Update () {
   
	}

    //每获得一个物品，要同时给所有物品列表和对应类型的物品列表插入
    public void AddGoods(Goods goods)
    {

        bagDict[GoodsSort.Undefined].AddGoods(goods);
        //bagDict[goods.goodsSort].AddGoods(goods);

    }


    //打开某个背包
    void OpenBag(GoodsSort sort)
    {
        foreach(KeyValuePair<GoodsSort,Bag> bag in bagDict)
        {
            if (bag.Key == sort)
            {
                if (!bag.Value.hasRender)
                    StartCoroutine(BagDoAfterRender(bag.Value.content));
                bag.Value.OnShowing();
            }
            else
                bag.Value.OnClosing();
        }
    }
}