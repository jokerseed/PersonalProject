using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Example2 : MonoBehaviour
{
    public GameObject sp;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("example2");

        //Debug.Log("magnitude:" + sp.transform.position.magnitude + "--" + sp.transform.position / sp.transform.position.magnitude);

        //Destroy(sp, 5f);

        var a = new Vector3(3, 0, 0);
        var b = new Vector3(0, 4, 0);
        var c = Vector3.Cross(a, b);
        //Debug.Log("面积:" + c.magnitude / 2);
    }

    // Update is called once per frame
    void Update()
    {
        //StartCoroutine("move");
    }

    /*
     * 暂停游戏
     */

    /// <summary>
    /// 暂停游戏
    /// </summary>
    void pause()
    {
        Time.timeScale = 0;
    }
    /*
     * 回复游戏
     */
    void resume()
    {
        Time.timeScale = 1;
    }

    IEnumerator move()
    {
        while (true)
        {
            var x = sp.transform.position.x + 0.01f;
            sp.transform.position = new Vector3(x, 0, 0);
            //yield return null;//下一帧运行的点 
            yield return new WaitForSeconds(1f);//间隔固定时间运行
        }
    }
}

