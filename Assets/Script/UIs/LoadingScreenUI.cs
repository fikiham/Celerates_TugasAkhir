using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenUI : MonoBehaviour
{
    public static LoadingScreenUI Instance;
    [SerializeField] TMP_Text loadingText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void LoadScene(int i)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(LoadingScene(i));
    }

    IEnumerator LoadingScene(int i)
    {
        AsyncOperation loadLevel = SceneManager.LoadSceneAsync(i);
        string loading = "Loading...";
        int index = 6;

        while (!loadLevel.isDone)
        {
            if (index > 9)
                index = 6;
            loadingText.text = loading[..index];
            index++;
            yield return new WaitForSeconds(.1f);
        }
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
