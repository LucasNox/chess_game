using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingMove : PieceMove
{
    public override LinkedList<Vector3Int> GetMovePositions(Vector3Int pos, GameObject[,] board_state)
    {
        LinkedList<Vector3Int> pos_list = new LinkedList<Vector3Int>();

        pos_list.AddLast(new Vector3Int(pos.x - 1, pos.y + 1, 0));
        pos_list.AddLast(new Vector3Int(pos.x, pos.y + 1, 0));
        pos_list.AddLast(new Vector3Int(pos.x + 1, pos.y + 1, 0));

        pos_list.AddLast(new Vector3Int(pos.x - 1, pos.y, 0));
        pos_list.AddLast(new Vector3Int(pos.x + 1, pos.y, 0));

        pos_list.AddLast(new Vector3Int(pos.x - 1, pos.y - 1, 0));
        pos_list.AddLast(new Vector3Int(pos.x, pos.y - 1, 0));
        pos_list.AddLast(new Vector3Int(pos.x + 1, pos.y - 1, 0));

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
