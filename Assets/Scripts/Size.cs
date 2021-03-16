using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Size
{
    [Min(1)] public int width;
    [Min(1)] public int height;

    public Size(int width, int height)
    {
        this.width = width;
        this.height = height;
    }
}
