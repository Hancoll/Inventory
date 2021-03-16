using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private ItemUI takenItem;
    bool itemIsTaken;

    private void OnEnable()
    {
        inventoryManager.TakeItemEvent += TakeItem;
        inventoryManager.DropItemEvent += DropItem;
    }

    private void OnDisable()
    {
        inventoryManager.TakeItemEvent -= TakeItem;
        inventoryManager.DropItemEvent -= DropItem;
    }

    private void Update()
    {
        if(itemIsTaken)
            takenItem.gameObject.transform.position = Input.mousePosition;
    }

    public void TakeItem(Item item)
    {
        takenItem.SetView(item);
        takenItem.gameObject.SetActive(true);

        itemIsTaken = true;
    }

    public void DropItem()
    { 
        takenItem.gameObject.SetActive(false);

        itemIsTaken = false;
    }
}
