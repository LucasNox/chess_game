using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] GameObject brilhoAzul = null;
    [SerializeField] GameObject criarBut = null;
    [SerializeField] GameObject entrarBut = null;
    [SerializeField] GameObject brilhoVermelho = null;
    [SerializeField] GameObject entrarIP = null;
    [SerializeField] GameObject entrarIpBut = null;
    [SerializeField] GameObject voltarBut = null;

    public void insertIPPainel()
    {
        brilhoAzul.SetActive(false);
        criarBut.SetActive(false);
        entrarBut.SetActive(false);

        brilhoAzul.SetActive(true);
        entrarIP.SetActive(true);
        entrarIpBut.SetActive(true);
        voltarBut.SetActive(true);
    }

    public void initialPainel()
    {
        brilhoAzul.SetActive(true);
        criarBut.SetActive(true);
        entrarBut.SetActive(true);

        brilhoVermelho.SetActive(false);
        entrarIP.SetActive(false);
        entrarIpBut.SetActive(false);
        voltarBut.SetActive(false);
    }

    public void initiateGame()
    {
        GameObject player = GameObject.Find("Player Data");
        player.GetComponent<PlayerData>().team = PieceConfig.Color.blue;
        DontDestroyOnLoad(player);

        SceneManager.LoadScene("WaitingScene");
    }

    public void enterGame()
    {
        GameObject player = GameObject.Find("Player Data");
        player.GetComponent<PlayerData>().team = PieceConfig.Color.red;
        player.GetComponent<PlayerData>().IP = entrarIP.GetComponent<InputField>().text;
        DontDestroyOnLoad(player);

        SceneManager.LoadScene("WaitingScene");
    }
}
