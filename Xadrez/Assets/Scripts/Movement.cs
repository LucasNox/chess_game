using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializableAttribute]
public class Movement
{
    public int xpos1;
    public int ypos1;
    public int xpos2;
    public int ypos2;

    public Movement(Vector3Int pos1, Vector3Int pos2)
    {
        this.xpos1 = pos1.x;
        this.ypos1 = pos1.y;
        this.xpos2 = pos2.x;
        this.ypos2 = pos2.y;
    }
}
