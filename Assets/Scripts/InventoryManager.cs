using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    //ДЛЯ ТЕСТА
    [SerializeField] private ItemData item_0;
    [SerializeField] private ItemData item_1;
    [SerializeField] private ItemData item_2;

    public event System.Action<Item> TakeItemEvent;
    public event System.Action DropItemEvent;
    [SerializeField] private ItemsContainer mainInventory;
    [SerializeField] private ItemsContainer inventory;
    private Item takenItem;


    private void Start()
    {
        mainInventory.InitializeInventory(OnClick, OnItemDrop);
        inventory.InitializeInventory(OnClick, OnItemDrop);

        //ДЛЯ ТЕСТА
        mainInventory.AddItem(new Item(item_0));
        mainInventory.AddItem(new Item(item_1));
        mainInventory.AddItem(new Item(item_2, 12));
        mainInventory.AddItem(new Item(item_2, 14));
    }

    private void OnClick(int cellPositionX, int cellPositionY, ItemsContainer inventory, System.Action dropDelegate)
    {
        if (takenItem == null)
        {
            if(RemoveItem(cellPositionX, cellPositionY, inventory, out Item item))
                TakeItem(item);
        }

        else
            dropDelegate?.Invoke();
    }

    private void OnItemDrop(int cellPositionX, int cellPositionY, ItemsContainer inventory)
    {
        if (takenItem != null)
        {
            Item nextTakenItem = null;

            if (inventory.GetItemInCells(cellPositionX, cellPositionY, takenItem.data.Size, out int itemPositionX, out int itemPositionY, out ItemsContainer itemInventory))
                RemoveItem(itemPositionX, itemPositionY, itemInventory, out nextTakenItem);

            //Складывание предмета в 1 стак
            if (nextTakenItem != null && takenItem.data == nextTakenItem.data)
            {
                ItemData data = takenItem.data;

                if (data.StackCapacity > 1)
                {
                    if(takenItem.count < data.StackCapacity && nextTakenItem.count < data.StackCapacity)
                    {
                        if (takenItem.count + nextTakenItem.count <= data.StackCapacity)
                        {
                            takenItem.count = takenItem.count + nextTakenItem.count;
                            nextTakenItem = null;
                        }

                        else
                        {
                            int remainder = takenItem.count + nextTakenItem.count - data.StackCapacity;

                            takenItem.count = data.StackCapacity;
                            nextTakenItem.count = remainder;
                        }
                    }
                }
            }

            if (inventory.AddItem(cellPositionX, cellPositionY, takenItem))
            {
                if (nextTakenItem != null)
                {
                    TakeItem(nextTakenItem);
                }

                else
                {
                    takenItem = null;
                    DropItemEvent?.Invoke();
                }
            }
        }
    }

    private bool RemoveItem(int cellPositionX, int cellPositionY, ItemsContainer inventory, out Item item)
    {
        CellContent cellContent = inventory.GetCellContent(cellPositionX, cellPositionY);
        item = cellContent.item;

        if (item != null)
        {
            inventory.RemoveItem(cellContent.itemPositionX, cellContent.itemPositionY, item.data.Size);
            return true;
        }

        return false;
    }

    private void TakeItem(Item item)
    {
        TakeItemEvent?.Invoke(item);

        takenItem = item;
    }
}
