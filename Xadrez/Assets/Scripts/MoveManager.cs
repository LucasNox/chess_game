using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MoveManager : MonoBehaviour
{
    private GameObject[,] board = new GameObject[8, 8];

    public GameObject Red_King;
    public GameObject Red_Queen;
    public GameObject Red_Bishop;
    public GameObject Red_Horse;
    public GameObject Red_Tower;
    public GameObject Red_Pawn;

    public GameObject Blue_King;
    public GameObject Blue_Queen;
    public GameObject Blue_Bishop;
    public GameObject Blue_Horse;
    public GameObject Blue_Tower;
    public GameObject Blue_Pawn;

    private LinkedList<GameObject> red_piece = new LinkedList<GameObject>();
    private LinkedList<GameObject> blue_piece = new LinkedList<GameObject>();

    private GameObject selected_piece = null;

    private Grid move_grid;

    private int player_turn = 0;
    GameObject red_text;
    GameObject blue_text;

    // Start is called before the first frame update
    void Start()
    {
        /*Inicializando variáveis de objetos necessários*/
        move_grid = GameObject.Find("MoveGrid").GetComponent<Grid>();
        red_text = GameObject.Find("RedText");
        blue_text = GameObject.Find("BlueText");
        red_text.SetActive(false);
        /*Inicializando tabuleiro com null*/
        for (int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                board[i, j] = null;
            }
        }

        red_piece.AddLast(Red_King);
        red_piece.AddLast(Red_Queen);
        red_piece.AddLast(Red_Bishop);
        red_piece.AddLast(Red_Horse);
        red_piece.AddLast(Red_Tower);
        red_piece.AddLast(Red_Pawn);

        blue_piece.AddLast(Blue_King);
        blue_piece.AddLast(Blue_Queen);
        blue_piece.AddLast(Blue_Bishop);
        blue_piece.AddLast(Blue_Horse);
        blue_piece.AddLast(Blue_Tower);
        blue_piece.AddLast(Blue_Pawn);

        /*Setando as peças em suas posições iniciais*/
        board[0, 0] = Blue_Tower;
        board[1, 0] = Blue_Horse;
        board[2, 0] = Blue_Bishop;
        board[3, 0] = Blue_Queen;
        board[4, 0] = Blue_King;
        board[5, 0] = Blue_Bishop;
        board[6, 0] = Blue_Horse;
        board[7, 0] = Blue_Tower;
        board[0, 1] = Blue_Pawn;
        board[1, 1] = Blue_Pawn;
        board[2, 1] = Blue_Pawn;
        board[3, 1] = Blue_Pawn;
        board[4, 1] = Blue_Pawn;
        board[5, 1] = Blue_Pawn;
        board[6, 1] = Blue_Pawn;
        board[7, 1] = Blue_Pawn;

        board[0, 7] = Red_Tower;
        board[1, 7] = Red_Horse;
        board[2, 7] = Red_Bishop;
        board[3, 7] = Red_Queen;
        board[4, 7] = Red_King;
        board[5, 7] = Red_Bishop;
        board[6, 7] = Red_Horse;
        board[7, 7] = Red_Tower;
        board[0, 6] = Red_Pawn;
        board[1, 6] = Red_Pawn;
        board[2, 6] = Red_Pawn;
        board[3, 6] = Red_Pawn;
        board[4, 6] = Red_Pawn;
        board[5, 6] = Red_Pawn;
        board[6, 6] = Red_Pawn;
        board[7, 6] = Red_Pawn;

        /*Instanciando as peças e movendo-as para suas posições*/
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                if(board[i, j] != null)
                {
                    GameObject piece = Instantiate(board[i, j]);
                    Vector3 piece_pos = move_grid.GetCellCenterWorld(new Vector3Int(i, j, 0));
                    piece.transform.position = piece_pos;
                    Debug.Log(board[i,j].transform.position);
                    board[i, j] = piece;
                    //Debug.Log(piece.transform.position);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cell_clicked = move_grid.WorldToCell(mousePos);
            Debug.Log(cell_clicked);
            if(selected_piece == null)
            {
                if(player_turn == 0)
                {
                    Debug.Log("ta entrando aqui");
                    /*if(blue_piece.Contains(PrefabUtility.GetPrefabAssetType(board[cell_clicked.x, cell_clicked.y])))
                    {
                        selected_piece = board[cell_clicked.x, cell_clicked.y];
                        Debug.Log("peça azul selecionada");
                    }*/
                }
                else
                {
                    if (red_piece.Contains(board[cell_clicked.x, cell_clicked.y]))
                    {
                        selected_piece = board[cell_clicked.x, cell_clicked.y];
                        Debug.Log("peça vermelha selecionada");
                    }
                }
            }
            else
            {
                selected_piece.transform.position = move_grid.GetCellCenterWorld(cell_clicked);
                selected_piece = null;
                changeTurn();
            }
        }
    }

    private void changeTurn()
    {
        if(player_turn == 0)
        {
            red_text.SetActive(true);
            blue_text.SetActive(false);
            player_turn = 1;
        }
        else
        {
            red_text.SetActive(false);
            blue_text.SetActive(true);
            player_turn = 0;
        }
    }
}
