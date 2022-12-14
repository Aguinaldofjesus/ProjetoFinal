using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icebob : InfoEnemys
{
    //Invunerabilidade 
    public float intervaloDano;
    public int piscando;

    //Pega o Componente Transform do Icebob
    public Transform ImagemIcebob;

    //SpriterRenderer
    private SpriteRenderer spriteRenderer;

    //Velocidade Do Inimigo
    public float velocidade = 2f;


    //Controla o Jogo
    private GerenciadorJogo GJ;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        //Recebe a informação do GameObject
        GJ = GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorJogo>();


        ImagemIcebob = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GJ.EstadoDoJogo() == true)
        {
            Andar();
        }
    }

    void Andar()
    {
        transform.position = new Vector3(transform.position.x + velocidade * Time.deltaTime, transform.position.y,transform.position.z);
    }


    //Colisão para fazer o inimigo bate na parede e volta
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Parede") || other.gameObject.CompareTag("Player")|| other.gameObject.CompareTag("Tilemap"))
        {
            velocidade = velocidade * -1;
            ImagemIcebob.transform.localScale = new Vector2(transform.localScale.x * -1, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Buracos"))
        {
            Debug.Log("chão");
            velocidade = velocidade * -1;
            ImagemIcebob.transform.localScale = new Vector2(transform.localScale.x * -1, 1);
        }
    }


    public void Dano()
    {
        Debug.Log("LeveiDano");
        StartCoroutine(FeedbackDano());
    }


    IEnumerator FeedbackDano()
    {

        for (int y = 0; y < piscando; y++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(intervaloDano);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(intervaloDano);
            Debug.Log("inimigo tomou dano");

        }

    }
}
