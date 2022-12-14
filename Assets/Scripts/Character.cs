using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
public class Character : MonoBehaviour
{

    //PassarDeFase
    public int Passei = 0;

    public int coraccao = 1;


    //Invunerabilidade 
    public float intervaloDano;
    public int piscando;


    //Morte De Espinho
    public int Morte;

    //Animator
    private Animator animator;


    //Pega o Rifidibody2D do Character
    public Rigidbody2D Corpo;

    //Recebe a velocidade do Character
    public float velocidade;
    public float Vel_Player = 40f;

    //Pega o Componente Transform do Character
    public Transform ImagemCharacter;


    //Quantidades de pulo que o meu Character realizou
    public int qtd_pulo = 0;
    public float ForcaPulo;

    //Controlar quando posso pular novamente
    private float meuTempoPulo = 0;

    //Booleana que me diz se posso pular
    public bool Pulo_ativo = true;

    //Vida Do Personagem
    public int vida = 5;
    [SerializeField] private float TimerDano;
    [SerializeField] private float TimerDanoINC;
    public bool PodeDano;


    //Moeda
    public int Moeda = 0;
    public TextMeshProUGUI Moeda_Texto;

    //Chance do Jogo
    private int chances = 3;
    private TextMeshProUGUI Vida_Texto;


    //Barra De Hp
    public Sprite[] Barra_Hp;
    public GameObject ImageBarra_HP;

    //Disparo da bala
    public GameObject bala;
    private float meuTempoTiro = 0;
    private bool PodeAtirar = true;

    //Direção Da Bala
    private bool FlipX = false;

    //Faz a Bala Virar
    public Transform balaOrigem;

    //SpriterRenderer
    private SpriteRenderer spriteRenderer;


    //Variavel com a posição inicial
    public Vector3 PosInicial;

    //Dash
    private Rigidbody2D rb;


    //Controla o Jogo
    private GerenciadorJogo GJ;


    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(8, 9, false);
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        //Recebe a informação do GameObject
        GJ = GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorJogo>();


        //Determino a posição no inicio do jogo
        PosInicial = new Vector3(-16.8f, -5.37f, transform.position.z);

        //Mudo a Posição do Meu Character
        transform.position = PosInicial;

        Corpo = GetComponent<Rigidbody2D>();
        Moeda_Texto = GameObject.FindGameObjectWithTag("Moeda_Text").GetComponent<TextMeshProUGUI>();
        Vida_Texto = GameObject.FindGameObjectWithTag("Vida_Text").GetComponent<TextMeshProUGUI>();
        Vida_Texto.text = chances.ToString();

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GJ.EstadoDoJogo() == true)
        {
            Mover();
            Pular();
            Atirar();
            VerificarVida();
            Pause();
            
        }
    }
    //=================================================================





    //=================================================================
    //VIDA
    //Ativa somente a vida necessaria
    void VerificarVida()
    {
        if (PodeDano == false)
        {
            if (Time.time >= TimerDano)
            {
                Debug.Log("Olá");

                animator.SetBool("Damage", false);
            }
        }

        ImageBarra_HP.GetComponent<Image>().sprite = Barra_Hp[vida];


        //So morro se minha vida for menor ou igual a Zero
        if (vida <= 0)
        {
            Morrer();
        }


    }
    //=================================================================





    //=================================================================
    //Movimentação
    void Mover()
    {
        velocidade = Input.GetAxisRaw("Horizontal") * Vel_Player;
        Corpo.velocity = new Vector2(velocidade, Corpo.velocity.y);

        animator.SetFloat("Speed", Mathf.Abs(velocidade));


        if (velocidade > 0)
        {
            ImagemCharacter.transform.localScale = new Vector2(1, 1);
            FlipX = false;

        }
        else if (velocidade < 0)
        {
            ImagemCharacter.transform.localScale = new Vector2(-1, 1);
            FlipX = true;
        }

    }
    //=================================================================








    //=================================================================
    //Pulo
    void Pular()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Pulo_ativo == true || Input.GetKeyDown(KeyCode.S) && Pulo_ativo == true || Input.GetKeyDown(KeyCode.UpArrow) && Pulo_ativo == true || Input.GetButton("btA") && Pulo_ativo == true)
        {
            animator.SetBool("IsJumping", true);
            animator.SetBool("Gun", false);
            Pulo_ativo = false;
            qtd_pulo++;

            if (qtd_pulo <= 1)
            {
                AcaoPulo();
            }

        }

        if (Pulo_ativo == false)
        {
            TemporizadorPulo();
        }
    }

    /* public void OnLanding()
     {
         animator.SetBool("IsJumping", false);
     */


    //Força do Pulo
    void AcaoPulo()
    {
        //Zera velocidade de querda para o pulo
        Corpo.velocity = new Vector2(velocidade, 0);


        //adicionar força ao pulo
        Corpo.AddForce(transform.up * ForcaPulo);
    }


    //Controla o Tempo Para Pular Novamente
    void TemporizadorPulo()
    {
        meuTempoPulo += Time.deltaTime;
        if (meuTempoPulo > 0.5f)
        {
            Pulo_ativo = true;
            meuTempoPulo = 0;
        }
    }
    //=================================================================





    //=================================================================
    //Dash
   /* 
    * void Dash(float velocidade)
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            rb.velocity = Vector2.zero;
            rb.velocity += new Vector2(1, velocidade).normalized * 30;
            Debug.Log("Dash");
        }
        
    }
   */




    //=================================================================





    //=================================================================
    //Trigger
    //Detectar o chão
    //Colisões Invisivel
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Tilemap")
        {
            qtd_pulo = 0;
            animator.SetBool("IsJumping", false);
            Pulo_ativo = true;
            meuTempoPulo = 0;
        }

        //Colisão com Moedas
        if (other.gameObject.tag == "Moeda")
        {
            Destroy(other.gameObject);
            Moeda++;
            Moeda_Texto.text = Moeda.ToString();
            Debug.Log("peguei");
        }

        //Colisão com Vida
        if (other.gameObject.tag == "Vida")
        {
            Destroy(other.gameObject);
            if (vida < 5)
            {
                vida += coraccao;
            }
           
            Debug.Log("Vida");
        }

        if (other.gameObject.tag == "Checkpoint")
        {
            PosInicial = other.gameObject.transform.position;
            
        }

        if (other.gameObject.tag == "Morte_Imediata")
        {
            if (vida > 0)
            {
                //Tirar toda a vida
                vida -= 20;
            }
        }

        if(other.gameObject.tag == "PassaDeFase")
        {
            Passei++;
            GJ.Fase(Passei);
        }

        if(other.gameObject.tag == "FimDeJogo")
        {
            GJ.PlayerFimDeJogo();
        }

        if(other.gameObject.tag == "Espinho")
        {
            Morte++;
            Debug.Log(Morte);
            /*(if(Morte >= 2)
            {
                Debug.Log("Não deveria morrer");
                Morrer();
            }
            else
            if(Morte >= 1)
            {
                Debug.Log("Morreu certo");
                Morrer();
            }*/
            
        }

        if (other.gameObject.tag == "PassagemSecreta")
        {
            other.gameObject.GetComponent<TilemapRenderer>().enabled = false;
        }

    }



    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "PassagemSecreta")
        {
            other.gameObject.GetComponent<TilemapRenderer>().enabled = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {

    }
    
    //=================================================================




    //=================================================================
    //Dano
    //Colisões fisicas
    private void OnCollisionEnter2D(Collision2D other)
    {


        if (other.gameObject.CompareTag("Inimigo"))
        {
            Debug.Log("encostou");

            if (PodeDano == true)
            {
                LevarDano(other.gameObject.GetComponent<InfoEnemys>().DanoInimigo);
                animator.SetBool("Damage", true);
                TimerDano = Time.time + TimerDanoINC;
            }          
        }
        if (other.gameObject.CompareTag("Espinho")) { 
            Morte++;
            Morrer();
        }

    }

    

    void LevarDano(int Dano)
    {
        PodeDano = false;
        vida -= Dano;
        StartCoroutine(FeedbackDano());

        if (vida<=0)
        {
            vida = 0;
        }
    }

    IEnumerator FeedbackDano()
    {
        Physics2D.IgnoreLayerCollision(8, 9,true);

        for(int y = 0; y < piscando; y++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(intervaloDano);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(intervaloDano);
            Debug.Log(y);
            Debug.Log("Não tomo Dano");
        }

        Physics2D.IgnoreLayerCollision(8, 9, false);
        PodeDano = true;
        Debug.Log("Pode Dano");
    }
    //=================================================================






    //=================================================================
    public void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButton("btPause"))
        {
            GJ.PausarJogo();
        }
    }
    //=================================================================









    //=================================================================
    //Atira Bala
    void Atirar()
    {
        if (PodeAtirar == true)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetButton("btX"))
            {
                animator.SetBool("Gun", true);
                PodeAtirar = false;
                Disparo();
            }
        }
        else
        {
            TemporizadorTiro();
        }

    }


    void Disparo()
    {
        if (FlipX == false)
        {
            //Posição Que a Bala sai
            //Vector3 PontoDisparo = new Vector3(transform.position.x + 0.3f, transform.position.y, transform.position.z);
            GameObject BalaDisparada = Instantiate(bala, balaOrigem.position, Quaternion.identity);
            BalaDisparada.GetComponent<Tiro>().DirecaoBala(0.03f);
            BalaDisparada.GetComponent<Tiro>().MudaspiteDireita();
            //Destruir bala
            Destroy(BalaDisparada, 2f);

        }

        if (FlipX == true)
        {
            //Posição Que a Bala sai
            //Vector3 PontoDisparo = new Vector3(transform.position.x - 0.3f, transform.position.y, transform.position.z);
            GameObject BalaDisparada = Instantiate(bala, balaOrigem.position, Quaternion.identity);
            BalaDisparada.GetComponent<Tiro>().DirecaoBala(-0.03f);
            BalaDisparada.GetComponent<Tiro>().MudaspiteEsquerda();

            //Destruir bala
            Destroy(BalaDisparada, 2f);

        }

    }

    //Temporizado de Disparo
    void TemporizadorTiro()
    {
        meuTempoTiro += Time.deltaTime;
        if (meuTempoTiro > 0.5f)
        {
            PodeAtirar = true;
            meuTempoTiro = 0;
            animator.SetBool("Gun", false);
        }
    }

    //Morte
    void Morrer()
    {
        chances--;
        Vida_Texto.text = chances.ToString();


        //Só reinicia quando acaba as chances
        if (chances <= 0)
        {
            GJ.PlayerMorreu();
        }
        else
        {
            Inicializar();
        }

    }


    void Inicializar()
    {
        //PontoInicial
        transform.position = PosInicial;


        //Recuperar Vida
        vida = 5;
        ImageBarra_HP.GetComponent<Image>().sprite = Barra_Hp[vida];
    }



}

