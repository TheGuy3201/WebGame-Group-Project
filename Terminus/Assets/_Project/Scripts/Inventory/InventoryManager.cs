using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Terminus
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance;
        public List<Item> Items = new List<Item>();

        public Transform ItemContent;
        public GameObject InventoryItem;

        private void Awake()
        {
            Instance = this;
        }

        public void Add(Item item)
        {
            Items.Add(item);
        }

        public void Remove(Item item)
        {
            Items.Remove(item);
        }

        public void ListItems()
        {
            //Deletes content before opening
            foreach(Transform item in ItemContent)
            {
                Destroy(item.gameObject);
            }

            foreach (var item in Items)
            {
                GameObject obj = Instantiate(InventoryItem, ItemContent);
                TMP_Text itemName = obj.transform.Find("ItemName").GetComponent<TMP_Text>();
                Image itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();

                itemName.text = item.itemName;
                itemIcon.sprite = item.icon;
            }
        }
    }
}
