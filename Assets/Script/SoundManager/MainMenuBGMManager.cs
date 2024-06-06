using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBGMManager : MonoBehaviour
{
    private void Start()
    {
        // Mulai memutar BGM saat scene "MainMenu" dimuat
        SoundManager.Instance.PlayBGM("BGMMenu");

        // Subscribe ke event scene loaded untuk menghentikan BGM ketika scene lain dimuat
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unsubscribe dari event scene loaded ketika objek dihancurkan
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "MainMenu")
        {
            // Hentikan BGM saat scene selain "MainMenu" dimuat
            SoundManager.Instance.StopBGM();
        }
    }
}
