using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsContainer : MonoBehaviour
{
    public event System.Action<int, int, Item> AddItemEvent;
    public event System.Action<int, int> RemoveItemEvent;
    public event System.Action<Size> InitializeInventoryEvent;
    [SerializeField] private Transform cellScriptsContainer;
    [SerializeField] private Size inventorySize;
    private CellContent[,] cells;

    #region Public
    public bool AddItem(Item item)
    {
        if (GetFreePosition(item.data.Size, out int cellPositionX, out int cellPositionY))
            AddItem(cellPositionX, cellPositionY, item);

        return false;
    }

    public bool AddItem(int cellPositionX, int cellPositionY, Item item)
    {
        if (GetCells(cellPositionX, cellPositionY, item.data.Size, out CellContent[] cells))
        {
            foreach (CellContent cell in cells)
                if (cell.item != null)
                    return false;

            for (int i = 0; i < cells.Length; i++)
                cells[i].SetCellContent(item, cellPositionX, cellPositionY);

            AddItemEvent?.Invoke(cellPositionX, cellPositionY, item);

            return true;
        }

        return false;
    }

    public bool RemoveItem(int cellPositionX, int cellPositionY, Size size)
    {
        if (GetCells(cellPositionX, cellPositionY, size, out CellContent[] cells))
        {
            for (int i = 0; i < cells.Length; i++)
                cells[i].Clear();

            RemoveItemEvent?.Invoke(cellPositionX, cellPositionY);

            return true;
        }

        return false;
    }

    public CellContent GetCellContent(int cellPositionX, int cellPositionY) => cells[cellPositionX, cellPositionY];

    public bool GetItemInCells(int cellPositionX, int cellPositionY, Size size, out int itemPositionX, out int itemPositionY, out ItemsContainer itemInventory)
    {
        Item itemInCell = null;
        itemPositionX = -1;
        itemPositionY = -1;
        itemInventory = this;

        if (GetCells(cellPositionX, cellPositionY, size, out CellContent[] cells))
        {
            for (int i = 0; i < cells.Length; i++)
            {
                if (cells[i].item != null)
                {
                    if (itemInCell == null)
                    {
                        itemInCell = cells[i].item;
                        itemPositionX = cells[i].itemPositionX;
                        itemPositionY = cells[i].itemPositionY;
                    }

                    else if (cells[i].item != itemInCell)
                        return false;
                }
            }

            if (itemInCell != null)
                return true;
        }

        return false;
    }

    public void InitializeInventory(System.Action<int, int, ItemsContainer, System.Action> OnClick, System.Action<int, int, ItemsContainer> OnItemDrop)
    {
        cells = new CellContent[inventorySize.width, inventorySize.height];
        CellScript[] inventoryCells = cellScriptsContainer.GetComponentsInChildren<CellScript>();

        for (int y = 0; y < inventorySize.height; y++)
        {
            for (int x = 0; x < inventorySize.width; x++)
            {
                int indexX = x;
                int indexY = y;

                cells[indexX, indexY] = new CellContent();

                inventoryCells[y * inventorySize.width + x].OnCellClick += (drop) => OnClick(indexX, indexY, this, drop);
                inventoryCells[y * inventorySize.width + x].OnItemDrop += () => OnItemDrop(indexX, indexY, this);
            }
        }

        InitializeInventoryEvent?.Invoke(inventorySize);
    }

    #endregion

    #region Private

    /// <summary>
    /// Поиск свободного места.
    /// </summary>
    private bool GetFreePosition(Size size, out int cellPositionX, out int cellPositionY)
    {
        cellPositionX = -1;
        cellPositionY = -1;

        for(int y = 0; y < cells.GetLength(1); y++)

            for (int x = 0; x < cells.GetLength(0); x++)

                if (CheckCellsForFree(x, y, size))
                {
                    cellPositionX = x;
                    cellPositionY = y;

                    return true;
                }

        return false;
    }

    private bool CheckCellsForFree(int cellPositionX, int cellPositionY, Size size)
    {
        int xPosition = cellPositionX + size.width;
        int yPosition = cellPositionY + size.height;

        if (xPosition <= inventorySize.width && xPosition >= 0 && yPosition <= inventorySize.height && yPosition >= 0)
        {
            int cellIndex = 0;

            for (int x = cellPositionX; x < cellPositionX + size.width; x++)

                for (int y = cellPositionY; y < cellPositionY + size.height; y++, cellIndex++)

                    if (cells[x, y].item != null)
                        return false;

            return true;
        }

        return false;
    }

    private bool GetCells(int cellPositionX, int cellPositionY, Size size, out CellContent[] cells)
    {
        cells = new CellContent[size.width * size.height];

        int xPosition = cellPositionX + size.width;
        int yPosition = cellPositionY + size.height;

        if (xPosition <= inventorySize.width && xPosition >= 0 && yPosition <= inventorySize.height && yPosition >= 0)
        {
            int cellIndex = 0;

            for(int x = cellPositionX; x < cellPositionX + size.width; x++)

                for (int y = cellPositionY; y < cellPositionY + size.height; y++, cellIndex++)
                    cells[cellIndex] = this.cells[x, y];

            return true;
        }

        return false;
    }

    #endregion
}

