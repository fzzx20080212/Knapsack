using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsPanel : MonoBehaviour {

    GameObject myPanel;
    Text showText;
    Button confirmBtn, cancelBtn,addBtn,delBtn;
    InputField numInput;
    public delegate void CallBack(int num);
    int curNum,maxNum;
    CallBack myCallBack;
    public static TipsPanel instance;
    
	// Use this for initialization
	void Start () {
        instance = this;

        myPanel = GameObject.Find("Canvas/Tips/BagTips");
        showText = myPanel.transform.Find("Panel/Text").GetComponent<Text>();
        confirmBtn = myPanel.transform.Find("Panel/ConfirmBtn").GetComponent<Button>();
        cancelBtn = myPanel.transform.Find("Panel/CancelBtn").GetComponent<Button>();
        addBtn = myPanel.transform.Find("Panel/AddBtn").GetComponent<Button>();
        delBtn = myPanel.transform.Find("Panel/DelBtn").GetComponent<Button>();
        numInput = myPanel.transform.Find("Panel/NumInput").GetComponent<InputField>();
        myPanel.SetActive(false);

        addBtn.onClick.AddListener(ClickAdd);
        delBtn.onClick.AddListener(ClickDelete);
        confirmBtn.onClick.AddListener(ClickConfirm);
        cancelBtn.onClick.AddListener(ClickCancel);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Show(string name,int count,CallBack confirm)
    {
        showText.text = "丢弃<color=#FFFFFF>" + name + "</color>";
        curNum = count;
        maxNum = count;
        numInput.text = count.ToString();
        myCallBack = confirm;
        if (!myPanel.activeSelf)
            myPanel.SetActive(true);
    }

    public void Close()
    {
        if (myPanel.activeSelf)
            myPanel.SetActive(false);
    }

    void ClickAdd()
    {
        if (++curNum > maxNum)
            curNum--;
        numInput.text = curNum.ToString();
    }

    void ClickDelete()
    {
        if (--curNum <= 0)
            curNum++;
        numInput.text = curNum.ToString();
    }

    void ClickConfirm()
    {
        myCallBack(curNum);
        Close();
    }

    void ClickCancel()
    {
        Close();
    }
}
