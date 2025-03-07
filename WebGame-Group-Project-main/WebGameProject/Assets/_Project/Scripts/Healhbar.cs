using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using WebGame397;

namespace Terminus
{
    public class Healhbar : MonoBehaviour
    {
        [SerializeField] PlayerController controller;
        private Slider healthBar;

        private void Start()
        {
            healthBar = GetComponent<Slider>();
            controller.OnDamage += ControllerOnDamage;

            SetUp();
        }

        private void SetUp()
        {
            healthBar.maxValue = controller.health;
            healthBar.value = controller.health;
        }

        private void ControllerOnDamage(object sender, EventArgs e)
        {
            healthBar.value = controller.health;
        }
    }
}
