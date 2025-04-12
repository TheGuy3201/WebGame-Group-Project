using UnityEngine;
using UnityEngine.UI;
using WebGame397;

namespace Terminus
{
    public class ItemController : MonoBehaviour
    {
        public Item item;

        public void RemoveItem()
        {
            InventoryManager.Instance.Remove(item);
            Destroy(gameObject);
        }

        public void AddItem(Item newItem)
        {
            item = newItem;
        }

        public void UseItem()
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Damage(-15);
            RemoveItem();
        }
    }
}
