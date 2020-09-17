using System;
using UnityEngine;

public class PageHome : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Go1;
    [SerializeField]
    private GameObject m_Go2;
    [SerializeField]
    private GameObject m_Go3;
    [SerializeField]
    private GameObject m_Go4;
    [SerializeField]
    private GameObject m_Go5;
    [SerializeField]
    private GameObject m_Go6;

    public ExampleThree THREE;

    // Start is called before the first frame update
    void Start()
    {
        THREE.OnUpdate += OnMainUpdate; //() => {};
        THREE.OnUpdate += OnMainUpdate2; //() => {};
    }

    private void OnMainUpdate()
    {
        Debug.Log("tHREE MAIN");
    }

    private void OnMainUpdate2()
    {
        Debug.Log("tHREE MAIN222222");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        THREE.OnUpdate -= OnMainUpdate;
        THREE.OnUpdate -= OnMainUpdate2;
    }
}
