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

    public void StartGame(bool newGame)
    {
        GameController.NewGame = newGame;
        LoadingScreenUI.Instance.LoadScene(GameController.LatestMap);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("APPLICATION QUIT");
    }
}
