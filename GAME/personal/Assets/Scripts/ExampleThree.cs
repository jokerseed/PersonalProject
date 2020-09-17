using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExampleThree : MonoBehaviour
{
    public Canvas c;
    public Button b;
    public GameObject img;

    private GameObject db;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        //b.onClick.AddListener(clickHandle);

        //db = GUIDefaultControls.CreateDoubleButton(new DefaultControls.Resources());
        //db.transform.SetParent(c.transform);
        //db.GetComponent<DoubleButton>().onDoubleClick.AddListener(clickHandle);
        //db.GetComponent<DoubleButton>().onDoubleClick.AddListener(clickHandle2);

        img.AddComponent<dragCom>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void clickHandle()
    {
        Debug.Log("点击事件");
    }

    void clickHandle2()
    {
        Debug.Log("点击事件2");
    }
}

/*
 * 滑动组件
 */
class dragCom : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    Vector2 v;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("开始拖动");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("拖动中");
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(gameObject.GetRectTransform(), eventData.position, eventData.pressEventCamera, out v))
        {
            gameObject.GetRectTransform().anchoredPosition = v;
        }
        Debug.Log(v.ToString());
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("结束拖动");
    }
}

public class GUIDefaultControls
{
    public static GameObject CreateDoubleButton(DefaultControls.Resources resources)
    {
        GameObject dcButton = DefaultControls.CreateButton(resources);
        dcButton.name = "DoubleClickButton";
        dcButton.transform.Find("Text").GetComponent<Text>().text = "双击按钮";
        UnityEngine.Object.DestroyImmediate(dcButton.GetComponent<Button>());
        dcButton.AddComponent<DoubleButton>();
        return dcButton;
    }
}

/*
 * 双击按钮
 */
public class DoubleButton : Button
{
    [Serializable]
    public class DoubleClickedEvent : UnityEvent { }
    [SerializeField]
    private DoubleClickedEvent m_onDoubleClick = new DoubleClickedEvent();

    //这个是双击成功后激活的事件
    public DoubleClickedEvent onDoubleClick
    {
        get { return m_onDoubleClick; }
        set { m_onDoubleClick = value; }
    }
    private DateTime m_firstTime;
    private DateTime m_secondTime;
    private void Press()
    {
        if (null != onDoubleClick)
            onDoubleClick.Invoke();
        resetTime();
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        // 按下按钮时对两次的时间进行记录
        if (m_firstTime.Equals(default(DateTime)))
            m_firstTime = DateTime.Now;
        else
            m_secondTime = DateTime.Now;
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        // 在第二次鼠标抬起的时候进行时间的触发,时差小于400ms触发
        if (!m_firstTime.Equals(default(DateTime)) && !m_secondTime.Equals(default(DateTime)))
        {
            var intervalTime = m_secondTime - m_firstTime;
            float milliSeconds = intervalTime.Seconds * 1000 + intervalTime.Milliseconds;
            if (milliSeconds < 400)
                Press();
            else
                resetTime();
        }
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        resetTime();
    }
    private void resetTime()
    {
        m_firstTime = default(DateTime);
        m_secondTime = default(DateTime);
    }
}

/*
 * 长按按钮
 */
public class LongClickButton : Button
{
    [Serializable]
    public class LongClickEvent : UnityEvent { }
    [SerializeField]
    private LongClickEvent m_onLongClick = null;
    public LongClickEvent onLongClick
    {
        get
        {
            return m_onLongClick;
        }
        set
        {
            m_onLongClick = value;
        }
    }
    private DateTime m_firstTime = default(DateTime);
    private DateTime m_secondTime = default(DateTime);
    private void Press()
    {
        if (null != onLongClick)
            onLongClick.Invoke();
        resetTime();
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (m_firstTime.Equals(default(DateTime)))
            m_firstTime = DateTime.Now;
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        // 在鼠标抬起的时候进行事件触发，时差大于600ms触发
        if (!m_firstTime.Equals(default(DateTime)))
            m_secondTime = DateTime.Now;
        if (!m_firstTime.Equals(default(DateTime)) && !m_secondTime.Equals(default(DateTime)))
        {
            var intervalTime = m_secondTime - m_firstTime;
            int milliSeconds = intervalTime.Seconds * 1000 + intervalTime.Milliseconds;
            if (milliSeconds > 600) Press();
            else resetTime();
        }
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        resetTime();
    }
    private void resetTime()
    {
        m_firstTime = default(DateTime);
        m_secondTime = default(DateTime);
    }
}
