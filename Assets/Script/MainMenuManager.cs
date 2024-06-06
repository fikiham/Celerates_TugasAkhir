using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Button LoadGameButton;

    private void Update()
    {
        if (PlayerPrefs.GetInt("HaveSaved") == 99)
        {
            LoadGameButton.interactable = true;
        }
        else
        {
            LoadGameButton.interactable = false;
        }
    }

      public void PlayClickSound()
    {
        SoundManager.Instance.PlaySound("Click");
    }

    public void StartGame(bool newGame)
    {
        GameController.NewGame = newGame;
        StartCoroutine(DelayedLoadScene(GameController.LatestMap, 0.5f)); // delay 0.5 detik
    }

    private IEnumerator DelayedLoadScene(int sceneIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadingScreenUI.Instance.LoadScene(sceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("APPLICATION QUIT");
    }
}
