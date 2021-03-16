using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CellScript : MonoBehaviour, IPointerClickHandler
{
    public event System.Action<System.Action> OnCellClick;
    public event System.Action OnItemDrop;

    public void OnDrop() => OnItemDrop?.Invoke();

    public void OnPointerClick(PointerEventData eventData)
    {
        CellScript inventoryCell = eventData.pointerEnter?.GetComponent<CellScript>();

        System.Action dropDelegate;
        dropDelegate = () => inventoryCell.OnDrop();

        OnCellClick?.Invoke(dropDelegate);
    }
}
