using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Bag : PanelBase {

    public Transform content;
    //背包行数,列数
    int rows=10,column=3;
    //背包格子预制体
    GameObject gridPrefab;
    //背包格子池
    List<BagGrid> gridsArray;
    //物品详细信息文本框
    Transform descPanel;
    float descWidth;
    Text t_desc;

    public static Bag instance;
    public void Start()
    {
        instance = this;
    }



    public override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "Bag";
        layer = PanelkLayer.Panel;
        //if (args.Length > 0)
        //    rows =(int) args[0];
        gridsArray = new List<BagGrid>();
        
    }


    public override void OnShowing()
    {
        base.OnShowing();
        //加载预制体
        content = skin.transform.Find("Scroll View/Viewport/Content");

        gridPrefab = Resources.Load("Grid")as GameObject;
        descPanel = GameObject.Find("Canvas").transform.Find("Tips/DescPanel");
        descPanel.gameObject.SetActive(false);
        t_desc = descPanel.Find("Text").GetComponent<Text>();
        descWidth = descPanel.GetComponent<RectTransform>().sizeDelta.x;
        for(int i=0;i<rows;i++)
        {
            for(int j = 0; j < column; j++)
            {
                GameObject temp = Instantiate(gridPrefab);

                BagGrid bagGrid=temp.AddComponent<BagGrid>();
                temp.transform.SetParent(content, false);
                //BagGrid bagGrid = new BagGrid(temp);
                gridsArray.Add( bagGrid);

            }
        }

    }


    public override void OnClosing()
    {
        base.OnClosing();
        
    }

    /// <summary>
    /// 增加物品
    /// </summary>
    public void AddGoods(Goods goods)
    {
        foreach (BagGrid grid in gridsArray)
        {
            if (grid.canPut(goods))
            {
                grid.SetGoods(goods);
                break;
            }
   
            
        }
    }

    public void ShowDesc(string desc,Vector2 pos)
    {
        if (desc.Length > 0)
        {
            descPanel.gameObject.SetActive(true);
            descPanel.transform.position = pos+new Vector2(-descWidth/2-20,0);
            t_desc.text = desc;
        }
    }

    public void CloseDesc()
    {
        if (descPanel.gameObject.activeSelf)
            descPanel.gameObject.SetActive(false);
    }
}
