using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Terminus
{
    public class MainMenu : MonoBehaviour
    {


        public void OnPlayButton()
        {
            SceneManager.LoadScene(1);
        }

        public void OnQuitButton()
        {
            Application.Quit();
            // Doesn't work unless project is built
            Debug.Log("Quit");
        }

        public void OnOptionsButton()
        {
            SceneManager.LoadScene(2);
        }

        public void OnBackButton()
        {
            SceneManager.LoadScene(0);
        }

        public void OnLoadButton()
        {
            bool has_save=PlayerPrefs.HasKey(Constants.player_pos_x)&PlayerPrefs.HasKey(Constants.player_pos_y)&PlayerPrefs.HasKey(Constants.player_pos_z);
            if (has_save)
            {
                PlayerPrefs.SetInt(Constants.usedload, 1);
                SceneManager.LoadScene(1);
            }


        }







    }
}