using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiro : MonoBehaviour
{
    public int DanoTiro = 1;
    public float TiroSpeed = 7;
    public float VelocidadeTiro = 0;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D mybody2D;
    private float direcaoTiro;

    //Controla o Jogo
    private GerenciadorJogo GJ;

    // Start is called before the first frame update
    void Start()
    {  
        //Recebe a informação do GameObject
        GJ = GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorJogo>();
        mybody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GJ.EstadoDoJogo() == true)
        {
            MoverBala();
        }
    }

    void MoverBala()
    {
        //Movimentação
        mybody2D.velocity = new Vector2(TiroSpeed * direcaoTiro,mybody2D.velocity.y);
    }


    //Direção Da Bala
    public void DirecaoBala(float direcao)
    {
        VelocidadeTiro = direcao;
    }

    public void MudaspiteDireita()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = false;
        direcaoTiro = 1;
    }

    public void MudaspiteEsquerda()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = true;
        direcaoTiro = -1;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Inimigo")
        {
            //Outro Objeto
            

            //Esse Objeto (Bala)
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Tilemap" || other.gameObject.tag == "Parede")
        {
            Destroy(this.gameObject);
        }
    }



}
