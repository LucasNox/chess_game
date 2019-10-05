﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseMove : PieceMove
{
    public override LinkedList<Vector3Int> GetMovePositions(Vector3Int pos, GameObject[,] board_state)
    {
        LinkedList<Vector3Int> pos_list = new LinkedList<Vector3Int>();

        pos_list.AddLast(new Vector3Int(pos.x + 2, pos.y + 1, 0));
        pos_list.AddLast(new Vector3Int(pos.x + 2, pos.y - 1, 0));
        pos_list.AddLast(new Vector3Int(pos.x - 2, pos.y + 1, 0));
        pos_list.AddLast(new Vector3Int(pos.x - 2, pos.y - 1, 0));

        pos_list.AddLast(new Vector3Int(pos.x + 1, pos.y + 2, 0));
        pos_list.AddLast(new Vector3Int(pos.x - 1, pos.y + 2, 0));
        pos_list.AddLast(new Vector3Int(pos.x + 1, pos.y - 2, 0));
        pos_list.AddLast(new Vector3Int(pos.x - 1, pos.y - 2, 0));

        return EliminateWrongPos(pos_list, this.GetComponent<PieceConfig>().piece_color, board_state);
    }
}
