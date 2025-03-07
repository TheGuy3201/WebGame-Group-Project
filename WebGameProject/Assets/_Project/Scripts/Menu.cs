using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Terminus
{
    public class Menu : MonoBehaviour
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
            // Load the game scene
            StartCoroutine(LoadGameSceneAndSetPlayerPosition());
        }

        private IEnumerator LoadGameSceneAndSetPlayerPosition()
        {
            // Load the game scene asynchronously
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            // Find the player object and set its position
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null && PlayerPrefs.HasKey("PlayerPosX") && PlayerPrefs.HasKey("PlayerPosY") && PlayerPrefs.HasKey("PlayerPosZ"))
            {
                float x = PlayerPrefs.GetFloat("PlayerPosX");
                float y = PlayerPrefs.GetFloat("PlayerPosY");
                float z = PlayerPrefs.GetFloat("PlayerPosZ");
                player.transform.position = new Vector3(x, y, z);
                Debug.Log("Player position loaded");
            }
        }
    }
}

