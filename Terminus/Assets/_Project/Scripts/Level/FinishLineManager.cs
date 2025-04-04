using UnityEngine;
using WebGame397;

namespace Terminus
{
    public class FinishLineManager : MonoBehaviour
    {
        public GameOver_Manager GameOverScreen;

        void OnTriggerEnter(Collider collision)
        {
            if (collision.CompareTag("Player"))
            {
                Debug.Log("Player entered the finish line!");
                GameEvents.ZoneEntered("FinalRoom");
                GameOverScreen.Setup("You Made It to The End");
            }
        }

    }
}
