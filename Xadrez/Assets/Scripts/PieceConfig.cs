using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceConfig : MonoBehaviour
{
    public enum Color { blue, red };

    public Color piece_color;
    [SerializeField] PieceMove[] moves = new PieceMove[2];

    public LinkedList<Vector3Int> getMovesAvailable(Vector3Int pos, GameObject[,] board)
    {
        LinkedList<Vector3Int> possible_moves = new LinkedList<Vector3Int>();
        LinkedList<Vector3Int> aux_moves;
        foreach(PieceMove move_set in moves)
        {
            aux_moves = move_set.GetMovePositions(pos, board);
            foreach(Vector3Int move in aux_moves)
            {
                possible_moves.AddLast(move);
            }
        }
        return possible_moves;
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
