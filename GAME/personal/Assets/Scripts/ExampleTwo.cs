using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ExampleTwo : MonoBehaviour
{
    public GameObject content;
    public GameObject content2;

    private int total = 0;
    // Start is called before the first frame update
    void Start()
    {

        Debug.Log(AssetDatabase.LoadAssetAtPath<Sprite>("Assets/64 flat icons/png/128px/Elements_Ice.png"));
        List<GameObject> l = new List<GameObject>();
        for (int i = 0; i < 10; i++)
        {
            GameObject g = new GameObject();
            Image img = g.AddComponent<Image>();
            img.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/64 flat icons/png/128px/Elements_Ice.png");
            l.Add(g);
            g.transform.SetParent(content.transform);
        }
        List<GameObject> l2 = new List<GameObject>();
        for (int i = 0; i < 10; i++)
        {
            GameObject g = new GameObject();
            Image img = g.AddComponent<Image>();
            img.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/64 flat icons/png/128px/Elements_Ice.png");
            l2.Add(g);


            RectTransform r = g.GetRectTransform();
            r.sizeDelta = new Vector2(100, (i + 1) * 10 + 10);
            total += (i + 1) * 10 + 10;
            r.pivot = new Vector2(0.5f, 1);
            r.anchorMin = new Vector2(0.5f, 1f);
            r.anchorMax = new Vector2(0.5f, 1f);

            r.anchoredPosition = new Vector2(0, -total);
            Debug.Log(total);
            g.transform.SetParent(content2.transform, false);
        }
        content2.GetRectTransform().sizeDelta = new Vector2(184, total);
    }

    // Update is called once per frame
    void Update()
    {

    }
}


public static class TranformExtensions
{
    public static RectTransform GetRectTransform(this GameObject go)
    {
        return go.transform as RectTransform;
    }
}