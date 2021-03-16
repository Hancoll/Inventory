using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField] private Vector2 defaultSize;
    [SerializeField] private Image itemImageComponent;
    [SerializeField] private Text countText;

    public void SetView(Item item)
    {
        if (item.data.StackCapacity > 1) countText.text = item.count.ToString();
        else countText.text = null;
        itemImageComponent.overrideSprite = item.data.Image;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(defaultSize.x * item.data.Size.width, defaultSize.y * item.data.Size.height);
    }
}
