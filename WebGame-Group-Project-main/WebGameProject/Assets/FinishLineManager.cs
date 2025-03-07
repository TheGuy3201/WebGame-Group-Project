using UnityEngine;
using WebGame397;

namespace Terminus
{
    public class FinishLineManager : MonoBehaviour
    {
        public GameOver_Manager GameOverScreen;
        
        
        void OnTriggerEnter(Collider collision)
        {
            GameOverScreen.Setup("You Made It to The End");
        }


    }
}
