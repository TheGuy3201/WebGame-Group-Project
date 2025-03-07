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
            SceneManager.LoadScene(0);
        }
        public void QuitGame()
        {
            Application.Quit();
            //doesn't work unless project is built
            Debug.Log("Quit");
        }
        public void InventoryMenu()
        {
            //ineventory later
            Debug.Log("Inventory");
        }
        public void ReloadCheckpoint()
        {
            //Also Later
            Debug.Log("Checkpoint");
        }
    }
}

