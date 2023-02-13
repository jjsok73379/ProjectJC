using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    string NPC_Typing;
    [SerializeField]
    TMP_Text Communication;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Typing());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Typing()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        for (int i = 0; i <= NPC_Typing.Length; ++i)
        {
            Communication.text = NPC_Typing.Substring(0, i);
            yield return new WaitForSecondsRealtime(0.2f);
        }
    }
}
