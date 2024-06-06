using UnityEngine;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
         // Pastikan volumeSlider tidak null sebelum menggunakannya
        if (volumeSlider != null)
        {
            SetMusicVolume();
        }
        else
        {
            Debug.LogError("Slider tidak ditemukan!");
        }
    }

  public void SetMusicVolume()
    {
        if (volumeSlider != null)
        {
            float volume = volumeSlider.value;
            soundManager.SetMusicVolume(volume);
        }
    }
}
