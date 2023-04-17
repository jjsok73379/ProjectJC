using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyUI : MonoBehaviour
{
    public static DontDestroyUI Inst;

    public GameObject QuestPost;

    private void Awake()
    {
        if (Inst != null)
        {
            Destroy(gameObject);
            return;
        }
        Inst = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        QuestPost.SetActive(false);
    }
}
