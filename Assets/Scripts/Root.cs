using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Root : MonoBehaviour {
    public Button b1, b2;
	// Use this for initialization
	void Start () {
        //myBag=(Bag)PanelMgr.instance.OpenPanel<Bag>("",10);

        GameObject.Find("Canvas/Panel/GetRedBtn").GetComponent<Button>().onClick.AddListener(AddRed);
        GameObject.Find("Canvas/Panel/GetBlueBtn").GetComponent<Button>().onClick.AddListener(AddBlue);     
    }
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    PanelMgr.instance.OpenPanel<Bag>("");
        //}
        //if (Input.GetKeyDown(KeyCode.C))
        //    PanelMgr.instance.ClosePanel("Bag");
	}

    public void AddRed()
    {
        Goods red_vial = new Consumable();
        red_vial.maxNum = 5;
        red_vial.name = "Blood";
        red_vial.itemSprite = Resources.Load<Sprite>("blood");
        red_vial.description = "这是一个喝了可以加血的药";
        BagMgr.instance.AddGoods(red_vial);
    }

    public void AddBlue()
    {
        Goods blue_vial = new Consumable();
        blue_vial.maxNum =3;
        blue_vial.name = "Magic";
        blue_vial.itemSprite = Resources.Load<Sprite>("magic");
        blue_vial.description = "这是一个喝了可以回蓝的药";
        BagMgr.instance.AddGoods(blue_vial);
    }

    int i = 0;
    public void AddEquipment()
    {
        Equipment eq = new Equipment();
        eq.maxNum = 1;
        eq.name = "E" + i;
        eq.itemSprite = Resources.Load<Sprite>("Equipment/Equipment_" + i);
        eq.description = name + "\n属性：100\n力量：23\n敏捷：15\n智慧\n20";
        BagMgr.instance.AddGoods(eq);
        i++;
    }
}
