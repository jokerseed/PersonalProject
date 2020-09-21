using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleFive : MonoBehaviour
{
    public GameObject gm;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 v1 = Vector2.one;
        Vector2 v2 = Vector2.one;
        var cos = Vector2.Dot(v1.normalized, v2.normalized);
        Debug.Log(cos);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
