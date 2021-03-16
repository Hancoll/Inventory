using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ItemData : ScriptableObject
{
    [SerializeField] private new string name;
    [SerializeField] private Sprite image;
    [SerializeField] [Min(1)] private int stackCapacity;//Вместимость стака
    [SerializeField] private Size size;

    public string Name => name;
    public Sprite Image => image;
    public int StackCapacity => stackCapacity;
    public Size Size => size;
}
