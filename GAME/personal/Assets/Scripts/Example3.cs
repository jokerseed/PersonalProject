using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 事件函数
 */
public class Example3 : MonoBehaviour
{
    /*
     * 始终在任何 Start 函数之前并在实例化预制件之后调用此函数。（如果游戏对象在启动期间处于非活动状态，则在激活之后才会调用 Awake。）
     */
    private void Awake()
    {
        //Debug.Log("awake-------");
    }

    /**
     * （仅在对象处于激活状态时调用）在启用对象后立即调用此函数
     */
    private void OnEnable()
    {
        //Debug.Log("enable-------");
    }

    /*
     * 在第一次帧更新之前
     */
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnApplicationPause(bool pause)
    {
        //Debug.Log("OnApplicationPause----"+pause);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        
    }

    private void LateUpdate()
    {
        //Destroy(gameObject);
    }

    private void OnDestroy()
    {
        //Debug.Log("destory---------");
    }

    private void OnDisable()
    {
        //Debug.Log("disable----------");
    }

    private void OnApplicationQuit()
    {
        //Debug.Log("applicationQuit---------");
    }
}
