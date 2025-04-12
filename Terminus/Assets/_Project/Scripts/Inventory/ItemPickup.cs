using UnityEngine;
using WebGame397;

namespace Terminus
{
    public class ItemPickup : MonoBehaviour
    {
        public Item Item;

        public void Pickup()
        {
            InventoryManager.Instance.Add(Item);
            Destroy(gameObject);
        }

        public void UseMedkit()
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Damage(-15);
            GameObject.Find("GameManager").GetComponent<InventoryManager>().Remove(Item);
            

        }
    }
}
