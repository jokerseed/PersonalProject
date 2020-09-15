using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AnimTest : MonoBehaviour
{
    [SerializeField]
    private Animator m_Animator;

    private AudioSource m_Source;

    private Button btn;
    private ScrollRect sr;
    

    private void Start()
    {
        m_Source = gameObject.AddComponent<AudioSource>();

        var clip  =AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/scavengers_music.aif");
        m_Source.clip = clip;
        m_Source.Play();
    }


    private void OnGUI()
    {

        if (GUILayout.Button("PlayAnim"))
        {
            m_Animator.SetTrigger("myAnim");
        }

        if (GUILayout.Button("PlayAnim2"))
        {
            m_Animator.Play("Img");
        }

        if (GUILayout.Button("PlayAnim3"))
        {
            m_Animator.Play(Animator.StringToHash("Img"));
        }

        if (GUILayout.Button("PlayAnim4"))
        {
            m_Animator.Play(Animator.StringToHash("Img"), -1, 0.5f);
        }
    }
}
