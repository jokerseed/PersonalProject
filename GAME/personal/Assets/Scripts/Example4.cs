using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Example4 : MonoBehaviour, IEvent
{
    public InputField inp;

    public void Message()
    {
        Debug.Log("全局事件");
    }

    // Start is called before the first frame update
    void Start()
    {
        ExecuteEvents.Execute<IEvent>(gameObject, null, (x, y) => x.Message());

        //rect transform中有些不会在帧末尾初始化
        Canvas.ForceUpdateCanvases();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void click()
    {
        //Debug.Log("点击事件");
        string txt = inp.GetComponent<InputField>().text;
        Debug.Log("输入框中的文本:" + txt);
    }

    public void OnSliderChange()
    {
        Debug.Log("slider--changed");
    }
}
