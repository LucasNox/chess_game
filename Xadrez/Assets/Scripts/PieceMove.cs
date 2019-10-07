﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PieceMove : MonoBehaviour
{
    public abstract LinkedList<Vector3Int> GetMovePositions(Vector3Int pos, GameObject[,] board_state);

    public LinkedList<Vector3Int> EliminateWrongPos(LinkedList<Vector3Int> pos_list, PieceConfig.Color color, GameObject[,] board)
    {
        List<Vector3Int> aux_list = new List<Vector3Int>(pos_list);

        foreach (Vector3Int pos in aux_list)
        {
            if (pos.x >= 8 || pos.x < 0 || pos.y >= 8 || pos.y < 0)
            {
                pos_list.Remove(pos);
            }
        }

        aux_list = new List<Vector3Int>(pos_list);

        foreach (Vector3Int pos in aux_list)
        {
            switch (pos.z)
            {
                case 0:
                    if (!OpposingTeam(board[pos.x,pos.y], color))
                        pos_list.Remove(pos);
                    else
                    {
                        if (board[pos.x, pos.y] == null)
                            pos_list.Find(pos).Value = new Vector3Int(pos.x, pos.y, 0);
                        else
                            pos_list.Find(pos).Value = new Vector3Int(pos.x, pos.y, 1);
                    }
                    continue;
                case 1:
                    if (board[pos.x, pos.y] != null)
                        pos_list.Remove(pos);
                    else
                    {
                        pos_list.Find(pos).Value = new Vector3Int(pos.x, pos.y, 0);
                    }
                    continue;
                case 2:
                    if (board[pos.x, pos.y] == null || !OpposingTeam(board[pos.x, pos.y], color))
                        pos_list.Remove(pos);
                    else
                    {
                        pos_list.Find(pos).Value = new Vector3Int(pos.x, pos.y, 1);
                    }
                    continue;
            }
        }

        return pos_list;
    }

    private bool OpposingTeam(GameObject piece, PieceConfig.Color color)
    {
        if (piece == null)
            return true;
        PieceConfig.Color piece_color = piece.GetComponent<PieceConfig>().piece_color;
        if (piece_color != color)
            return true;
        else
            return false;
    }
}
