using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Connector : MonoBehaviour
{
    private bool connected = false;
    // Start is called before the first frame update
    void Start()
    {
        PlayerData player_data = GameObject.Find("Player Data").GetComponent<PlayerData>();
        new Thread(() =>
        {
            if(player_data.team == PieceConfig.Color.blue)
            {
                Debug.Log("entrando thread azul");
                connected = player_data.startGame();
                Debug.Log("saindo thread azul");
            }
            else
            {
                Debug.Log("entrando thread vermelha");
                connected = player_data.enterGame();
                Debug.Log("saindo thread vermelha");
            }
        }).Start();
    }

    // Update is called once per frame
    void Update()
    {
        if(connected)
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}
