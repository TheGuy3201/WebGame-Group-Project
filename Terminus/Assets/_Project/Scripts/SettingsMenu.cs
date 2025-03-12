using UnityEngine;
using UnityEngine.Audio;

namespace Terminus
{
    public class SettingsMenu : MonoBehaviour
    {
        public AudioMixer mainMixer;
        private string PP_volume_key="volume",PP_fullscreen_key="isFullScreen",PP_quality_key="qualityIndex";
        

        private void Start()
        {
            // Load saved settings
            LoadSettings();
            
        }

        public void SetVolume(float volume)
        {
            mainMixer.SetFloat(PP_volume_key, volume);
            PlayerPrefs.SetFloat(PP_volume_key, volume);
        }

        public void SetFullScreen(bool isFullScreen)
        {
            Screen.fullScreen = isFullScreen;
            PlayerPrefs.SetInt(PP_fullscreen_key, isFullScreen ? 1 : 0);
        }

        public void SetQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
            PlayerPrefs.SetInt(PP_quality_key, qualityIndex);
        }

        private void LoadSettings()
        {
            // Load volume
            if (PlayerPrefs.HasKey(PP_volume_key))
            {
                float volume = PlayerPrefs.GetFloat(PP_volume_key);
                mainMixer.SetFloat(PP_volume_key, volume);
            }else
            {
                
            }

            // Load fullscreen setting
            if (PlayerPrefs.HasKey(PP_fullscreen_key))
            {
                bool isFullScreen = PlayerPrefs.GetInt(PP_fullscreen_key) == 1;
                Screen.fullScreen = isFullScreen;
            }

            // Load quality setting
            if (PlayerPrefs.HasKey(PP_quality_key))
            {
                int qualityIndex = PlayerPrefs.GetInt(PP_quality_key);
                QualitySettings.SetQualityLevel(qualityIndex);
            }
        }
    }
}

