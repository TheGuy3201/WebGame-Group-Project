using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

namespace Terminus
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject pauseMenuUI;
        public static bool GameIsPaused = false;
        public Transform playerTransform;
        
        private void OnPause(InputAction.CallbackContext context)
        {
            if (context.performed) // Ensures it only triggers once per press
            {
                if (GameIsPaused)
                    Resume();
                else
                    Pause();
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
            float x = playerTransform.position.x;
            float y = playerTransform.position.y;
            float z = playerTransform.position.z;
            PlayerPrefs.SetFloat(Constants.player_pos_x, x);
            PlayerPrefs.SetFloat(Constants.player_pos_y, y);
            PlayerPrefs.SetFloat(Constants.player_pos_z, z);
            PlayerPrefs.Save();
            Debug.Log($"x:{x}, y:{y}, z:{z}");
            Debug.Log("Game Saved");
        }



    }
}