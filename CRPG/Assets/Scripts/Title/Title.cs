using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    [SerializeField]
    Button LoadBtn;
    [SerializeField]
    Color UnActive;
    [SerializeField]
    GameObject NoTouch;

    private void Start()
    {
        if (SaveAndLoad.Inst.IsSaved)
        {
            NoTouch.SetActive(false);
            LoadBtn.interactable = true;
            LoadBtn.gameObject.GetComponent<Image>().color = Color.white;
        }
        else
        {
            NoTouch.SetActive(true);
            LoadBtn.interactable = false;
            LoadBtn.gameObject.GetComponent<Image>().color = UnActive;
        }
    }

    public void ClickStart()
    {
        LoadManager.LoadScene(1);
    }

    public void ClickLoad()
    {
        DataManager.Inst.IsLoad = true;
        LoadManager.LoadScene(SaveAndLoad.Inst.saveData.SceneNum);
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
