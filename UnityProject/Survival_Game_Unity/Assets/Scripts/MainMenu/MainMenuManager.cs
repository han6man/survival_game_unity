using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingBar;

    public void LoadScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsynchronously(sceneId));
    }

    IEnumerator LoadSceneAsynchronously(int sceneId)
    {
        Time.timeScale = 1;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            loadingBar.value = operation.progress;
            yield return null;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
