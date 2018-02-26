using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum PanelkLayer
{
    Panel,
    Tips,
}

public class PanelMgr:MonoBehaviour
{
    void Start()
    {

    }

    public static PanelMgr instance;
    private GameObject canvas;
    //存放已打开过的面板
    public Dictionary<string, PanelBase> dict;
    //层级
    private Dictionary<PanelkLayer, Transform> layerDict;

    public void Awake()
    {
        instance = this;
        InitLayer();
        dict = new Dictionary<string, PanelBase>();
       
    }

    //初始化面板
    private void InitLayer()
    {
        canvas = GameObject.Find("Canvas");
        if (canvas == null)
            Debug.LogError("canvas is null");
        layerDict = new Dictionary<PanelkLayer, Transform>();
        foreach (PanelkLayer p in Enum.GetValues(typeof(PanelkLayer)))
        {
            string name = p.ToString();
            Transform transform = canvas.transform.Find(name);
            layerDict.Add(p, transform);
        }

    }

    public PanelBase OpenPanel<T>(string skinPath,params object[] args)where T : PanelBase
    {
        //已经打开
        string name = typeof(T).ToString();
        if (dict.ContainsKey(name))
        {
            //return null;
            dict[name].skin.SetActive(true);
            return dict[name];
        }


        //面板脚本
        PanelBase panel = canvas.AddComponent<T>() ;
        panel.Init(args);
        dict.Add(name, panel);
        //加载界面
        skinPath = skinPath == "" ? panel.skinPath : skinPath;
        GameObject skin = (GameObject)Resources.Load(skinPath);
        if (skin == null)
            Debug.LogError(skinPath + "is null");
        panel.skin = Instantiate(skin);
        panel.skin.transform.SetParent(layerDict[panel.layer], false);
        panel.OnShowing();
        panel.OnShowed();

        return panel;

    }

    public void ClosePanel(string name)
    {
        PanelBase panel = dict[name];
        if (panel == null)
            return;
        panel.skin.SetActive(false);
        panel.OnClosing();
        //dict.Remove(name);
        panel.OnClosed();
        //GameObject.Destroy(panel.skin);
        //Component.Destroy(panel);
    }
}

