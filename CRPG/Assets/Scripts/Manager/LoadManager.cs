using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour
{
    public static LoadManager Inst;
    public static int nextScene;
    public Slider slider;
    [SerializeField]
    GameObject TouchToScreen;
    [SerializeField]
    GameObject Loading;


    private void Awake()
    {
        Inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        TouchToScreen.SetActive(false);
        Loading.SetActive(false);
        StartCoroutine(LoadCoroutine());
    }

    public static void LoadScene(int index)
    {
        nextScene = index;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadCoroutine()
    {
        AsyncOperation operation;
        operation = SceneManager.LoadSceneAsync(nextScene);
        operation.allowSceneActivation = false;

        float timer = 0.0f;
        while (!operation.isDone)
        {
            yield return null;

            timer += Time.deltaTime;
            if (operation.progress < 0.9f)
            {
                Loading.SetActive(true);
                TouchToScreen.SetActive(false);
                slider.value = Mathf.Lerp(operation.progress, 1.0f, timer);
                if (slider.value >= operation.progress)
                {
                    timer = 0.0f;
                }
            }
            else
            {
                slider.value = Mathf.Lerp(slider.value, 1.0f, timer);
                if (slider.value >= 0.99f)
                {
                    CursorManager.Inst.SetActiveCursorType(CursorManager.CursorType.Arrow);
                    Loading.SetActive(false);
                    TouchToScreen.SetActive(true);
                    if (Input.GetMouseButtonDown(0))
                    {
                        operation.allowSceneActivation = true;
                    }
                }
            }
        }
    }
}
