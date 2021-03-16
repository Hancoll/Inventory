using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public ItemData data;
    public int count = 1;

    public Item(ItemData data) => this.data = data;

    public Item(ItemData data, int count) : this(data) => this.count = count;
}
