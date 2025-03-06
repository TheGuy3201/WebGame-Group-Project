using UnityEngine;
using UnityEngine.Audio;

namespace Terminus
{
    public class SettingsMenu : MonoBehaviour
    {
        public AudioMixer mainMixer;

        private void Start()
        {
            // Load saved settings
            LoadSettings();
        }

        public void SetVolume(float volume)
        {
            mainMixer.SetFloat("volume", volume);
            PlayerPrefs.SetFloat("volume", volume);
        }

        public void SetFullScreen(bool isFullScreen)
        {
            Screen.fullScreen = isFullScreen;
            PlayerPrefs.SetInt("isFullScreen", isFullScreen ? 1 : 0);
        }

        public void SetQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
            PlayerPrefs.SetInt("qualityIndex", qualityIndex);
        }

        private void LoadSettings()
        {
            // Load volume
            if (PlayerPrefs.HasKey("volume"))
            {
                float volume = PlayerPrefs.GetFloat("volume");
                mainMixer.SetFloat("volume", volume);
            }

            // Load fullscreen setting
            if (PlayerPrefs.HasKey("isFullScreen"))
            {
                bool isFullScreen = PlayerPrefs.GetInt("isFullScreen") == 1;
                Screen.fullScreen = isFullScreen;
            }

            // Load quality setting
            if (PlayerPrefs.HasKey("qualityIndex"))
            {
                int qualityIndex = PlayerPrefs.GetInt("qualityIndex");
                QualitySettings.SetQualityLevel(qualityIndex);
            }
        }
    }
}

