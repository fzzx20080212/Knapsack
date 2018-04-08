using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class BagGrid : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IDropHandler, IEndDragHandler,IBeginDragHandler
{
    //背包格子
    GameObject myGrid;
    //格子存放的货物
    public Goods myGoods;
    //格子的Image和Text组件
    Vector2 imagePos;
    Image image;
    Text text;
    float gridWidth;
    //格子Id
    public Bag myBag;
    public void Start()
    {
        myGrid = this.gameObject;
        image = myGrid.transform.Find("Image").GetComponent<Image>();
        imagePos = image.transform.localPosition;
        gridWidth = myGrid.GetComponent<RectTransform>().sizeDelta.x;
        text = image.transform.Find("count").GetComponent<Text>();

    }

    void Update()
    {
        
    }
    //使用物品
    public void UseGoods()
    {
        if (myGoods == null)
            return;

        if (!myGoods.UseEffect())
            return;
        myGoods.putNum--;
        BagGrid sortGrid = myGoods.sortGrid;
        RefreshGrid();
        sortGrid.RefreshGrid();

    }
 

    /// <summary>
    /// 刷新格子
    /// </summary>
    public void RefreshGrid()
    {
        if(myGoods==null)
        {
            image.sprite = null;
            myBag.CloseDesc();
            text.text = "";
            return;
        }
        if (myGoods.putNum == 0)
        {
            image.sprite = null;
            myBag.CloseDesc();
            text.text = "";
            myGoods = null;
        }
        else
        {
            image.sprite = myGoods.itemSprite;
            text.text = (myGoods.putNum == 1) ? "" : myGoods.putNum.ToString();  
        }
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
        }
        else
        {
            myGoods.putNum += goods.putNum;
            myGoods.sortGrid.RefreshGrid();
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



    #endregion

    //交换两个格子的物品
    private void SwapGrid(BagGrid beginGrid)
    {
        Goods temp = beginGrid.myGoods;
        beginGrid.myGoods = myGoods;
        myGoods = temp;
        if (myBag.bagSort == GoodsSort.Undefined)
        {
            if (beginGrid.myGoods != null)
            {
                beginGrid.myGoods.mainGrid = temp.mainGrid;
            }
            myGoods.mainGrid.RefreshGrid();
            myGoods.mainGrid = this;
            myGoods.mainGrid.RefreshGrid();
        }
        else
        {
            if (beginGrid.myGoods != null)
            {
                beginGrid.myGoods.sortGrid = temp.sortGrid;
            }
            myGoods.sortGrid.RefreshGrid();
            myGoods.sortGrid = this;
            myGoods.sortGrid.RefreshGrid();
        }

    }

    //当两个格子物品种类相同时，叠加物品
    private void OverlayGoods(BagGrid beginGrid)
    {
        if (myGoods.putNum >= myGoods.maxNum)
            return;
        int sum = myGoods.putNum + beginGrid.myGoods.putNum;
        if (sum <= myGoods.maxNum)
        {
            myGoods.putNum = sum;
            beginGrid.myGoods.putNum=0;
        }
        else
        {
            beginGrid.myGoods.putNum -= myGoods.maxNum - myGoods.putNum;
            myGoods.putNum = myGoods.maxNum;
            
        }
        myGoods.mainGrid.RefreshGrid();
        myGoods.sortGrid.RefreshGrid();
        BagGrid tempGrid = beginGrid.myGoods.sortGrid;
        beginGrid.myGoods.mainGrid.RefreshGrid();
        tempGrid.RefreshGrid();
    }


    //丢弃物品
    public void ThrowGoods(int num)
    {
        myGoods.putNum -=num;
        if (myGoods.putNum < 0)
            myGoods.putNum = 0;
        BagGrid temp = myGoods.sortGrid;
        myGoods.mainGrid.RefreshGrid();

        temp.RefreshGrid();
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
        if (myGoods == null||!myBag.canShowDesc)
            return;
        myBag.ShowDesc(myGoods.GetDescription(), eventData.pointerEnter.transform.position-new Vector3(gridWidth/2,0,0));
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        if (myGoods == null)
            return;
        myBag.CloseDesc();
    }

  

    public void OnDrag(PointerEventData eventData)
    {
        if (myGoods != null)
        {
            //image.transform.position = eventData.position;
            //设置当前格子层级最高，否则会造成遮挡
            //myGrid.transform.SetAsLastSibling();
            BagMgr.instance.dragImage.transform.position = eventData.position;
        }

    }

    public void OnDrop(PointerEventData eventData)
    {

        BagGrid beginGrid = eventData.pointerDrag.GetComponent<BagGrid>();

        if (beginGrid.myGoods == null)
            return;
        if (myGoods!=null&&beginGrid.myGoods.name == myGoods.name)
            OverlayGoods(beginGrid);
        else
            SwapGrid(beginGrid);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //image.transform.localPosition = imagePos;
        image.gameObject.SetActive(true);
        myBag.canShowDesc = true;
        BagMgr.instance.dragImage.gameObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        myBag.canShowDesc = false;
        myBag.CloseDesc();
        BagMgr.instance.dragImage.gameObject.SetActive(true);
        image.gameObject.SetActive(false);
        BagMgr.instance.dragImage.sprite = myGoods.itemSprite;
        BagMgr.instance.dragImage.transform.Find("Text").GetComponent<Text>().text = myGoods.putNum.ToString();
    }


    #endregion
}
