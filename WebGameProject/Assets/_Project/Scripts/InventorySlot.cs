using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;
using UnityEngine.UI;

namespace Terminus
{
    public class InventorySlot : MonoBehaviour
    {
        public Image icon;
        public void ClearSlot() 
        { 
            icon.enabled = false;
        }
        public void DrawSlot(InventoryItem item)
        {
            if(item == null)
            {
                ClearSlot();
                return;
            }
            icon.enabled = true;

            


        }
    }
}
