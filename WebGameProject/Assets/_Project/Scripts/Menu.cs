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
            //doesn't work unless project is built
            Debug.Log("Quit");
        }


    }
}
