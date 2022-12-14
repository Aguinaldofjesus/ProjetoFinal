using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorJogo : MonoBehaviour
{
    //Verifica se o Jogo está ligado ou não
    public bool GameLigado = false;


    //Chama Tela de GameOver
    public GameObject TelaGameOver;
    public GameObject TelaPause;
    public GameObject Player;

    //Chama Tela de Fim De jogo
    public GameObject TelaFimDeJogo;

   

    // Start is called before the first frame update
    void Start()
    {
        //LigarJogo();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameLigado == true)
        {
            Time.timeScale = 1;
        }

        else
        {
            Time.timeScale = 0;
        }
    }


    public bool EstadoDoJogo()
    {
        return GameLigado;
    }

    public void LigarJogo()
    {
        GameLigado = true;
    }

    public void PlayerMorreu()
    {
        //Chama Tela De Game Over
        TelaGameOver.SetActive(true);
        GameLigado = false;
    }

    //Reinicia o jogo
    public void Reiniciar()
    {
        SceneManager.LoadScene(0);
    }


    //Passa De Fase
    //A Int "Passei" fica no Player
    public void Fase(int Passei)
    {
        SceneManager.LoadScene(Passei);
    }


    //Pausa o Jogo
    public void PausarJogo()
    {
        GameLigado = false;
        TelaPause.SetActive(true);
    }

    public void PlayerFimDeJogo()
    {
        //Chama Tela De Fim De Jogo
        TelaFimDeJogo.SetActive(true);
        GameLigado = false;
    }

    public void SairJogo()
    {
        Application.Quit();
    }
}
