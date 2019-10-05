using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMove : PieceMove
{
    public override LinkedList<Vector3Int> GetMovePositions(Vector3Int pos, GameObject[,] board_state)
    {
        LinkedList<Vector3Int> pos_list = new LinkedList<Vector3Int>();

        for (int i = 0; i < 7; i++)
        {
            try
            {
                if (board_state[pos.x + i, pos.y] != null)
                {
                    pos_list.AddLast(new Vector3Int(pos.x + i, pos.y, 0));
                    break;
                }
                pos_list.AddLast(new Vector3Int(pos.x + i, pos.y, 0));
            }
            catch (System.Exception) { }
        }
        for (int i = 0; i < 7; i++)
        {
            try
            {
                if (board_state[pos.x, pos.y - i] != null)
                {
                    pos_list.AddLast(new Vector3Int(pos.x, pos.y - i, 0));
                    break;
                }
                pos_list.AddLast(new Vector3Int(pos.x, pos.y - i, 0));
            }
            catch (System.Exception) { }
        }
        for (int i = 0; i < 7; i++)
        {
            try
            {
                if (board_state[pos.x, pos.y + i] != null)
                {
                    pos_list.AddLast(new Vector3Int(pos.x, pos.y + i, 0));
                    break;
                }
                pos_list.AddLast(new Vector3Int(pos.x, pos.y + i, 0));
            }
            catch (System.Exception) { }
        }
        for (int i = 0; i < 7; i++)
        {
            try
            {
                if (board_state[pos.x - i, pos.y] != null)
                {
                    pos_list.AddLast(new Vector3Int(pos.x - i, pos.y, 0));
                    break;
                }
                pos_list.AddLast(new Vector3Int(pos.x - i, pos.y, 0));
            }
            catch (System.Exception) { }
        }

        return EliminateWrongPos(pos_list, this.GetComponent<PieceConfig>().piece_color, board_state);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
