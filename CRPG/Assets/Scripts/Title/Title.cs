using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string SceneName = "LoadingScene";
    SaveData saveData;

    public void ClickStart()
    {
        LoadManager.LoadScene(1);
    }

    public void ClickLoad()
    {
        LoadManager.LoadScene(saveData.SceneNum);
    }

    public void ClickSetting()
    {
        OpenManager.Inst.TryOpenSoundWindow();
    }

    public void ClickExit()
    {
        Application.Quit();
    }
}
