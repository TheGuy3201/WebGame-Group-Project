using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Terminus
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject pauseMenuUI;
        public static bool GameIsPaused = false;
        public Transform playerTransform;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }

        public void Resume()
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            GameIsPaused = false;
        }

        public void Pause()
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            GameIsPaused = true;
        }

        public void LoadMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }

        public void QuitGame()
        {
            Application.Quit();
            // Doesn't work unless project is built
            Debug.Log("Quit");
        }

        public void Saving()
        {
            // Save the player's position
            PlayerPrefs.SetFloat("PlayerPosX", playerTransform.position.x);
            PlayerPrefs.SetFloat("PlayerPosY", playerTransform.position.y);
            PlayerPrefs.SetFloat("PlayerPosZ", playerTransform.position.z);
            PlayerPrefs.Save();
            Debug.Log("Game Saved");
        }

        private void Start()
        {
            // Load the player's position
            if (PlayerPrefs.HasKey("PlayerPosX") && PlayerPrefs.HasKey("PlayerPosY") && PlayerPrefs.HasKey("PlayerPosZ"))
            {
                float x = PlayerPrefs.GetFloat("PlayerPosX");
                float y = PlayerPrefs.GetFloat("PlayerPosY");
                float z = PlayerPrefs.GetFloat("PlayerPosZ");
                playerTransform.position = new Vector3(x, y, z);
                Debug.Log("Game Loaded");
            }
        }
    }
}