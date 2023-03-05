using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    bool SoundActivated = false;
    public GameObject SoundWindow;

    // Start is called before the first frame update
    void Start()
    {
        SoundWindow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TryOpenSoundWindow()
    {
        SoundActivated = !SoundActivated;
        SoundWindow.SetActive(SoundActivated);
    }
}
