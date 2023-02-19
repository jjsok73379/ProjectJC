using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadManager : Singleton<LoadManager>
{
    public Slider slider;
    AsyncOperation operation;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadCoroutine());
    }

    IEnumerator LoadCoroutine()
    {
        operation = SceneManager.LoadSceneAsync("Forest");
        operation.allowSceneActivation = false;

        float timer = 0.0f;
        while (!operation.isDone)
        {
            yield return null;

            timer += Time.deltaTime;
            if (operation.progress < 0.9f)
            {
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
                    operation.allowSceneActivation = true;
                }
            }
        }
    }
}
