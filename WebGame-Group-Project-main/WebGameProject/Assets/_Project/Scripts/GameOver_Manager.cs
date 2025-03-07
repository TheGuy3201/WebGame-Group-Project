using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Terminus
{
    public class GameOver_Manager : MonoBehaviour
    {
        public PauseMenu StopSwitch;

        public TMP_Text messageText;
        public void Setup(string text)
        {
            gameObject.SetActive(true);
            messageText.text = text;

            StopSwitch.Pause();
            StopSwitch.pauseMenuUI.SetActive(false);
        }
    }
}
