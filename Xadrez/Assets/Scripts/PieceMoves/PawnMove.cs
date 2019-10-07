using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnMove : PieceMove
{
    private bool first_move;

    public override LinkedList<Vector3Int> GetMovePositions(Vector3Int pos, GameObject[,] board_state)
    {
        LinkedList<Vector3Int> pos_list = new LinkedList<Vector3Int>();
        PieceConfig.Color color = this.GetComponent<PieceConfig>().piece_color;

        if(color == PieceConfig.Color.blue)
        {
            pos_list.AddLast(new Vector3Int(pos.x, pos.y + 1, 1));
            try
            {
                if (first_move && board_state[pos.x, pos.y + 1] == null)
                {
                    pos_list.AddLast(new Vector3Int(pos.x, pos.y + 2, 1));
                }
            }
            catch (System.Exception) { }
            pos_list.AddLast(new Vector3Int(pos.x + 1, pos.y + 1, 2));
            pos_list.AddLast(new Vector3Int(pos.x - 1, pos.y + 1, 2));
        }
        else
        {
            pos_list.AddLast(new Vector3Int(pos.x, pos.y - 1, 1));
            try
            {
                if(first_move && board_state[pos.x, pos.y - 1] == null)
                {
                    pos_list.AddLast(new Vector3Int(pos.x, pos.y - 2, 1));
                }
            }
            catch (System.Exception) { }
            pos_list.AddLast(new Vector3Int(pos.x + 1, pos.y - 1, 2));
            pos_list.AddLast(new Vector3Int(pos.x - 1, pos.y - 1, 2));
        }

        return EliminateWrongPos(pos_list, color, board_state);
    }

    public void useFirstMove()
    {
        first_move = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        first_move = true;    
    }
}
