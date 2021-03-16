using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellContent
{
    public Item item;
    public int itemPositionX;
    public int itemPositionY;

    public void SetCellContent(Item item, int itemPositionX, int itemPositionY)
    {
        this.item = item;
        this.itemPositionX = itemPositionX;
        this.itemPositionY = itemPositionY;
    }

    public void Clear()
    {
        item = null;
        itemPositionX = -1;
        itemPositionY = -1;
    }
}
