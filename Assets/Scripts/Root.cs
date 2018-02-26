using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Root : MonoBehaviour {

    Bag myBag;
    public Button b1, b2;
	// Use this for initialization
	void Start () {
        myBag=(Bag)PanelMgr.instance.OpenPanel<Bag>("",10);
        b1.onClick.AddListener(AddRed);
        b2.onClick.AddListener(AddBlue);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.V))
        {
            PanelMgr.instance.OpenPanel<Bag>("");
        }
        if (Input.GetKeyDown(KeyCode.C))
            PanelMgr.instance.ClosePanel("Bag");
	}

    public void AddRed()
    {
        Sprite sprite = Resources.Load<Sprite>("blood");
        Goods red_vial = new RedVial();
        myBag.AddGoods(red_vial);
    }

    public void AddBlue()
    {
        Sprite sprite = Resources.Load<Sprite>("magic");
        Goods blue_vial = new BlueVial();
        myBag.AddGoods(blue_vial);
    }
}
