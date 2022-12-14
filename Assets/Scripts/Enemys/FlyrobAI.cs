using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FlyrobAI : InfoEnemys
{

    //Invunerabilidade 
    public float intervaloDano;
    public int piscando;

    //SpriterRenderer
    private SpriteRenderer spriteRenderer;

    //Vida Inimigo
    [SerializeField] private float TimerDano;
    [SerializeField] private float TimerDanoINC;
    public bool PodeDano;

    //Animator
    public Animator animator;


    //Variaveis de Movimentação
    public Transform target;
    bool Seguir = false;
    public float speed;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    //Controla o Jogo
    private GerenciadorJogo GJ;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        //Recebe a informação do GameObject
        GJ = GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorJogo>();

        animator = GetComponentInChildren<Animator>();
        target = GJ.Player.transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        
        InvokeRepeating("UpdatePath", 0f, .5f);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GJ.EstadoDoJogo() == true)
        {
            if (Seguir == true)
            {
                Movimentação();
                animator.SetBool("Attack", true);
            }
            else
            {
                animator.SetBool("Attack", false);
            }
        }
        
    }

    void UpdatePath()
    {
            if (seeker.IsDone())
                seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void Movimentação()
    {
        if (path == null)
        {
            Debug.Log("oi");
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        animator.SetFloat("Speed", Mathf.Abs(speed));

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("chão");
            Seguir = false;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("chão");
            Seguir = true;
        }

    }


    public void Dano()
    {
        Debug.Log("LeveiDano");
        animator.SetTrigger("Damage");
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
