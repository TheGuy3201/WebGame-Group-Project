using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
namespace Terminus
{
    public class SaveSystem : MonoBehaviour
    {
        public void SaveData()
        {
            PlayerPrefs.SetInt("QualitySettingPreference", QualitySettings.GetQualityLevel());
            PlayerPrefs.SetFloat("VolumeSettingPreference", 0.5f);
            PlayerPrefs.SetInt("FullScreenSettingPreference", 1);
        }
        public void LoadData() 
        {
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("QualitySettingPreference"));
            //mainMixer.SetFloat("volume", PlayerPrefs.GetFloat("VolumeSettingPreference"));
            Screen.fullScreen = PlayerPrefs.GetInt("FullScreenSettingPreference") == 1 ? true : false;

        }
    }

}

