using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    [SerializeField]
    private Image img;
    [SerializeField]
    private GameObject go;

    // Start is called before the first frame update
    void Start()
    {
        //Image img = Transform.FindObjectOfType<Image>();
        img.sprite = ResourceManager.LoadFromFile() as Sprite;
        go.SetActive(false);

        Debug.Log("游戏启动");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
