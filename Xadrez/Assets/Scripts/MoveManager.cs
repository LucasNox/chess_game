using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Threading;
using UnityEngine.SceneManagement;

public class MoveManager : MonoBehaviour
{
    //Matriz de GameObject que guarda as peças. Onde é null, não há peça
    private GameObject[,] board = new GameObject[8, 8];

    //Prefab das peças vermelhas
    public GameObject Red_King;
    public GameObject Red_Queen;
    public GameObject Red_Bishop;
    public GameObject Red_Horse;
    public GameObject Red_Tower;
    public GameObject Red_Pawn;

    //Prefab das peças azuis
    public GameObject Blue_King;
    public GameObject Blue_Queen;
    public GameObject Blue_Bishop;
    public GameObject Blue_Horse;
    public GameObject Blue_Tower;
    public GameObject Blue_Pawn;

    //Prefab das células onde se pode comer uma peça ou apenas se mover, além de celula selecionada
    public GameObject KillCell;
    public GameObject MoveCell;
    public GameObject SelectedCell;

    //Lista de peças vermelhas, azuis, e lista de possiveis movimentações da peça selecionada para se movimentar, além da lista das celulas marcadas pra movimento
    private LinkedList<GameObject> red_piece = new LinkedList<GameObject>();
    private LinkedList<GameObject> blue_piece = new LinkedList<GameObject>();
    private LinkedList<GameObject> cell_highlights = new LinkedList<GameObject>();
    private LinkedList<Vector3Int> possible_moves;

    //Peça selecionada para se movimentar
    private GameObject selected_piece = null;

    //Grid para movimentar peças
    private Grid move_grid;

    //Jogador dono do turno, inicia-se sempre o azul
    private PieceConfig.Color player_turn = PieceConfig.Color.blue;

    //Texto de quem é o turno no momento
    private GameObject red_text;
    private GameObject blue_text;

    //Jogador deste client
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        /*Inicializando jogador*/
        player = GameObject.Find("Player Data");
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
                    board[i, j] = piece;
                    if(piece.GetComponent<PieceConfig>().piece_color == PieceConfig.Color.blue)
                    {
                        blue_piece.AddLast(piece);
                    }
                    else
                    {
                        red_piece.AddLast(piece);
                    }
                }
            }
        }

        if(player.GetComponent<PlayerData>().team == PieceConfig.Color.red)
            waitForPlay(player.GetComponent<PlayerData>());
    }

    // Update is called once per frame
    void Update()
    {
        if(player_turn != player.GetComponent<PlayerData>().team && oponent_move != null)
        {
            selected_piece = board[oponent_move.xpos1, oponent_move.ypos1];
            swapPiecePositionInt(new Vector3Int(oponent_move.xpos1, oponent_move.ypos1, 0), new Vector3Int(oponent_move.xpos2, oponent_move.ypos2, 0));
            checkVictory();
            selected_piece.transform.position = move_grid.GetCellCenterWorld(new Vector3Int(oponent_move.xpos2, oponent_move.ypos2, 0));
            selected_piece = null;
            oponent_move = null;
            changeTurn();
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cell_clicked = move_grid.WorldToCell(mousePos);
            if (cell_clicked.x >= 8 || cell_clicked.x < 0 || cell_clicked.y >= 8 || cell_clicked.y < 0)
                return;
            if (selected_piece == null)
            {
                if(player_turn == player.GetComponent<PlayerData>().team)
                {
                    selected_piece = board[cell_clicked.x, cell_clicked.y];
                    possible_moves = selected_piece.GetComponent<PieceConfig>().getMovesAvailable(cell_clicked, board);
                    createCellHighlight(cell_clicked);
                }
            }
            else
            {
                if (selected_piece.Equals(board[cell_clicked.x, cell_clicked.y]))
                {
                    selected_piece = null;
                    destroyCellHighlight();
                }
                else
                {
                    bool contains = false;
                    foreach(Vector3Int pos in possible_moves)
                    {
                        if(pos.x == cell_clicked.x && pos.y == cell_clicked.y)
                        {
                            contains = true;
                            break;
                        }
                    }
                    if(contains)
                    {
                        player.GetComponent<PlayerData>().sendBytes(ObjectToByteArray(new Movement(move_grid.WorldToCell(selected_piece.transform.position), cell_clicked)));
                        swapPiecePosition(selected_piece.transform.position, mousePos);
                        checkVictory();
                        selected_piece.transform.position = move_grid.GetCellCenterWorld(cell_clicked);
                        if (selected_piece.GetComponent<PawnMove>() != null)
                            selected_piece.GetComponent<PawnMove>().useFirstMove();
                        selected_piece = null;
                        destroyCellHighlight();
                        changeTurn();
                        waitForPlay(player.GetComponent<PlayerData>());
                    }
                }
            }
        }
    }

    private void checkVictory()
    {
        bool red = false;
        bool blue = false;
        foreach(GameObject piece in red_piece)
        {
            if (piece.GetComponent<KingMove>() != null)
                red = true;
        }
        foreach (GameObject piece in blue_piece)
        {
            if (piece.GetComponent<KingMove>() != null)
                blue = true;
        }
        if(!red)
        {
            player.GetComponent<PlayerData>().closeSocket();
            Destroy(player);
            SceneManager.LoadScene("BlueVictory");
        }
        else if (!blue)
        {
            player.GetComponent<PlayerData>().closeSocket();
            Destroy(player);
            SceneManager.LoadScene("RedVictory");
        }
    }

    private void swapPiecePositionInt(Vector3Int pos1, Vector3Int pos2)
    {
        if (board[pos2.x, pos2.y] != null)
        {
            if (board[pos2.x, pos2.y].GetComponent<PieceConfig>().piece_color == PieceConfig.Color.blue)
                blue_piece.Remove(board[pos2.x, pos2.y]);
            else
                red_piece.Remove(board[pos2.x, pos2.y]);
            Destroy(board[pos2.x, pos2.y]);
        }
        board[pos2.x, pos2.y] = board[pos1.x, pos1.y];
        board[pos1.x, pos1.y] = null;
    }

    Movement oponent_move = null;

    private void waitForPlay(PlayerData player_data)
    {
        new Thread(() =>
        {
            oponent_move = (Movement)ByteArrayToObject(player_data.receiveBytes());
        }).Start();
    }

    private void changeTurn()
    {
        if(player_turn == PieceConfig.Color.blue)
        {
            red_text.SetActive(true);
            blue_text.SetActive(false);
            player_turn = PieceConfig.Color.red;
        }
        else
        {
            red_text.SetActive(false);
            blue_text.SetActive(true);
            player_turn = PieceConfig.Color.blue;
        }
    }

    private void swapPiecePosition(Vector3 pos1, Vector3 pos2)
    {
        Vector3Int cell1 = move_grid.WorldToCell(pos1);
        Vector3Int cell2 = move_grid.WorldToCell(pos2);
        if (board[cell2.x, cell2.y] != null)
        {
            if (board[cell2.x, cell2.y].GetComponent<PieceConfig>().piece_color == PieceConfig.Color.blue)
                blue_piece.Remove(board[cell2.x, cell2.y]);
            else
                red_piece.Remove(board[cell2.x, cell2.y]);
            Destroy(board[cell2.x, cell2.y]);
        }
        board[cell2.x, cell2.y] = board[cell1.x, cell1.y];
        board[cell1.x, cell1.y] = null;
    }

    private void createCellHighlight(Vector3Int selected_pos)
    {
        GameObject cell;
        foreach(Vector3Int move in possible_moves)
        {
            if(move.z == 0)
            {
                cell = Instantiate(MoveCell);
            }
            else
            {
                cell = Instantiate(KillCell);
            }
            cell_highlights.AddLast(cell);
            cell.transform.position = move_grid.GetCellCenterWorld(move);
        }
        cell = Instantiate(SelectedCell);
        cell_highlights.AddLast(cell);
        cell.transform.position = move_grid.GetCellCenterWorld(selected_pos);
    }

    private void destroyCellHighlight()
    {
        foreach(GameObject cell in cell_highlights)
        {
            Destroy(cell);
        }
        cell_highlights.Clear();
    }

    private byte[] ObjectToByteArray(System.Object obj)
    {
        if (obj == null)
            return null;

        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        bf.Serialize(ms, obj);

        return ms.ToArray();
    }

    private System.Object ByteArrayToObject(byte[] arrBytes)
    {
        MemoryStream memStream = new MemoryStream();
        BinaryFormatter binForm = new BinaryFormatter();
        memStream.Write(arrBytes, 0, arrBytes.Length);
        memStream.Seek(0, SeekOrigin.Begin);
        System.Object obj = (System.Object)binForm.Deserialize(memStream);

        return obj;
    }
}
