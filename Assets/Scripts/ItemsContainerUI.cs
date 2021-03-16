using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemsContainer))]
public class ItemsContainerUI : MonoBehaviour
{
    [SerializeField] private Transform itemsUIContainer;
    [SerializeField] private GameObject itemUIPrefab;
    private GameObject[,] itemsUI;
    private ItemsContainer inventory;

    private void Awake() => inventory = GetComponent<ItemsContainer>();

    private void OnEnable()
    {
        inventory.InitializeInventoryEvent += InitializeItemsContainerUI;
        inventory.AddItemEvent += AddItemView;
        inventory.RemoveItemEvent += RemoveItemView;
    }

    private void OnDisable()
    {
        inventory.InitializeInventoryEvent -= InitializeItemsContainerUI;
        inventory.AddItemEvent -= AddItemView;
        inventory.RemoveItemEvent -= RemoveItemView;
    }

    public void InitializeItemsContainerUI(Size size) => itemsUI = new GameObject[size.width, size.height];

    public void AddItemView(int cellPositionX, int cellPositionY, Item item)
    {
        GameObject itemView = Instantiate(itemUIPrefab, itemsUIContainer);
        RectTransform transform = itemView.GetComponent<RectTransform>();
        transform.anchoredPosition = new Vector2(cellPositionX * 120, cellPositionY * -120);
        itemView.GetComponent<ItemUI>().SetView(item);

        itemsUI[cellPositionX, cellPositionY] = itemView;
    }

    public void RemoveItemView(int positionX, int positionY)
    {
        Destroy(itemsUI[positionX, positionY]);
        itemsUI[positionX, positionY] = null;
    }
}
