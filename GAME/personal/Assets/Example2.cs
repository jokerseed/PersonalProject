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
        Debug.Log("example2");

        //Destroy(sp, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine("move");
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
            Debug.Log("move");
            var x = sp.transform.position.x + 0.05f;
            sp.transform.position = new Vector3(x, 0, 0);
            yield return null;
        }
    }
}

