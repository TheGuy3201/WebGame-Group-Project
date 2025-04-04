using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Terminus
{
    public class AchievementSystem : MonoBehaviour
    {
        [SerializeField] GameObject achievement;
        [SerializeField] GameObject achievementTXT;
        
        private void OnEnable()
        {
            GameEvents.OnZoneEntered += HandleZoneEntered;
        }

        private void OnDisable()
        {
            GameEvents.OnZoneEntered -= HandleZoneEntered;
        }

        private void HandleZoneEntered(string zoneName)
        {
            switch (zoneName)
            {
                case "FinalRoom":
                    UnlockAchievement("You Beat the Game!!");
                    break;
                case "Hulk":
                    UnlockAchievement("Bro's a Hulk!!");
                    break;
                default:
                    Debug.Log("Entered zone: " + zoneName);
                    break;
            }
        }

        private void UnlockAchievement(string message)
        {
            Debug.Log(message);
            //Set message before revealing
            message = "Achievement Unlocked: " + message;
            achievementTXT?.GetComponent<TMP_Text>()?.SetText(message);

            //Reveal Achievement
            achievement.SetActive(true);

            //Delayed Hiding of the Achievement
            StartCoroutine(DelayedDeactivate());

            IEnumerator DelayedDeactivate()
            {
                yield return new WaitForSeconds(4);
                achievement?.SetActive(false);
            }

        }
    }

}
